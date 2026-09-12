using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;

namespace SomethingDownThere
{
    // Authored clumps, instanced in small spatial batches. Grass is derived from
    // the original surface and current soil; no grass state belongs in a save.
    [DisallowMultipleComponent, RequireComponent(typeof(TerrainVolume))]
    public sealed class SurfaceGrassRenderer : MonoBehaviour
    {
        [SerializeField] private Mesh nearMesh;
        [SerializeField] private Mesh farMesh;
        [SerializeField] private Material material;
        [SerializeField, Min(1)] private float patchSize = 2f;
        [SerializeField, Range(4, 20)] private int cellsPerPatch = 8;
        private const int Seed = 127;
        private sealed class Patch
        {
            public Bounds Bounds;
            public Vector3[] Roots;
            public Matrix4x4[] Candidates, Near;
            public int NearCount;
            public bool Dirty = true;
        }
        private TerrainVolume terrain;
        private readonly List<Patch> patches = new List<Patch>();
        private readonly Plane[] planes = new Plane[6];
        // The shader uses uniform scaling, so the documented 1023-instance limit
        // applies. Compact already-culled patches into reusable submission arrays.
        private readonly Matrix4x4[] nearBatch = new Matrix4x4[1023], farBatch = new Matrix4x4[1023];
        public int PatchCount => patches.Count;
        public int SupportedClumps { get; private set; }
        public int LastRebuiltPatches { get; private set; }
        public double LastRebuildMilliseconds { get; private set; }
        public double LastSubmissionMilliseconds { get; private set; }
        public int LastDrawCalls { get; private set; }
        public int LastTriangles { get; private set; }
        public int LastVisibleClumps { get; private set; }
        public int LastFarClumps { get; private set; }
        public ulong PlacementHash { get; private set; }

        private void OnEnable()
        {
            terrain = GetComponent<TerrainVolume>();
            terrain.Changed += SoilChanged;
            RenderPipelineManager.beginCameraRendering += RenderCamera;
        }

        private void OnDisable()
        {
            if (terrain != null) terrain.Changed -= SoilChanged;
            RenderPipelineManager.beginCameraRendering -= RenderCamera;
            patches.Clear();
            SupportedClumps = LastRebuiltPatches = LastDrawCalls = LastTriangles = LastVisibleClumps = 0;
            PlacementHash = 0;
        }

        private void LateUpdate()
        {
            LastRebuiltPatches = 0;
            if (terrain == null || !terrain.CanDig || terrain.IsRestoring) return;
            if (nearMesh == null || material == null) return;
            var start = Stopwatch.GetTimestamp();
            if (patches.Count == 0) CreatePatches();
            foreach (var patch in patches)
            {
                if (!patch.Dirty) continue;
                SupportedClumps -= patch.NearCount;
                patch.NearCount = 0;
                for (int i = 0; i < patch.Roots.Length; i++)
                {
                    if (!RootSupported(patch.Roots[i], patch.Candidates[i].GetColumn(0).magnitude)) continue;
                    patch.Near[patch.NearCount++] = patch.Candidates[i];
                }
                SupportedClumps += patch.NearCount;
                patch.Dirty = false;
                LastRebuiltPatches++;
            }
            if (LastRebuiltPatches > 0) UpdatePlacementHash();
            LastRebuildMilliseconds = (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;
        }

        private void CreatePatches()
        {
            // At most 400 instances per draw, below Unity's conservative 511 limit.
            int cells = Mathf.Clamp(cellsPerPatch, 4, 20);
            float size = Mathf.Max(1, patchSize);
            var extent = (Vector3)terrain.Dimensions * terrain.CellSize;
            // A sphere around the authored mesh covers every seeded rotation and
            // scale. The shader's combined breeze/flutter stays below 1.5 amplitudes.
            float bladePadding = (nearMesh.bounds.center.magnitude + nearMesh.bounds.extents.magnitude) * 1.65f
                + material.GetFloat("_WindAmplitude") * 1.5f + .02f;
            var random = new System.Random(Seed);
            for (float z = 0; z < extent.z; z += size)
            for (float x = 0; x < extent.x; x += size)
            {
                var roots = new List<Vector3>(cells * cells);
                var matrices = new List<Matrix4x4>(cells * cells);
                for (int iz = 0; iz < cells; iz++)
                for (int ix = 0; ix < cells; ix++)
                {
                    float px = x + (ix + .5f + ((float)random.NextDouble() - .5f) * .8f) * size / cells;
                    float pz = z + (iz + .5f + ((float)random.NextDouble() - .5f) * .8f) * size / cells;
                    if (px > extent.x - .03f || pz > extent.z - .03f) continue;
                    var root = terrain.transform.TransformPoint(new Vector3(px, extent.y - .006f, pz));
                    float scaleRandom = (float)random.NextDouble();
                    float rotation = (float)random.NextDouble() * 360;
                    float keep = (float)random.NextDouble();
                    // Even whole-site growth, with seeded spacing and height
                    // variation. Camera distance only simplifies blade geometry.
                    if (keep >= .88f) continue;
                    float scale = Mathf.Lerp(.95f, 1.5f, scaleRandom);
                    roots.Add(root);
                    matrices.Add(Matrix4x4.TRS(root, terrain.transform.rotation *
                        Quaternion.Euler(0, rotation, 0), Vector3.one * scale));
                }
                var bounds = new Bounds(terrain.transform.TransformPoint(new Vector3(x, extent.y, z)), Vector3.zero);
                bounds.Encapsulate(terrain.transform.TransformPoint(new Vector3(Mathf.Min(x + size, extent.x), extent.y, z)));
                bounds.Encapsulate(terrain.transform.TransformPoint(new Vector3(x, extent.y, Mathf.Min(z + size, extent.z))));
                bounds.Encapsulate(terrain.transform.TransformPoint(new Vector3(Mathf.Min(x + size, extent.x), extent.y, Mathf.Min(z + size, extent.z))));
                bounds.Expand(bladePadding * 2);
                patches.Add(new Patch { Bounds = bounds, Roots = roots.ToArray(), Candidates = matrices.ToArray(),
                    Near = new Matrix4x4[roots.Count] });
            }
        }

        private bool RootSupported(Vector3 root, float scale)
        {
            // Test the clump footprint at the original turf, not a lower wall or
            // nearby floor. A conservative skirt prevents roots hovering at lips.
            Vector3 down = -terrain.transform.up * .018f;
            Vector3 a = terrain.transform.right * (.085f * scale), b = terrain.transform.forward * (.085f * scale);
            return terrain.IsSolid(root + down) && terrain.IsSolid(root + down + a)
                && terrain.IsSolid(root + down - a) && terrain.IsSolid(root + down + b)
                && terrain.IsSolid(root + down - b);
        }

        private void SoilChanged(Bounds changed)
        {
            changed.Expand(.25f);
            foreach (var patch in patches)
                if (patch.Bounds.Intersects(changed)) patch.Dirty = true;
        }

        private void UpdatePlacementHash()
        {
            ulong hash = 14695981039346656037UL;
            foreach (var patch in patches)
            for (int i = 0; i < patch.NearCount; i++)
            {
                Vector4 p = patch.Near[i].GetColumn(3);
                unchecked
                {
                    hash = (hash ^ (uint)Mathf.RoundToInt(p.x * 10000)) * 1099511628211UL;
                    hash = (hash ^ (uint)Mathf.RoundToInt(p.z * 10000)) * 1099511628211UL;
                }
            }
            PlacementHash = hash;
        }

        private void RenderCamera(ScriptableRenderContext context, Camera camera)
        {
            if (camera.cameraType != CameraType.Game && camera.cameraType != CameraType.SceneView) return;
            LastDrawCalls = LastTriangles = LastVisibleClumps = LastFarClumps = 0;
            if (terrain == null || terrain.IsRestoring || patches.Count == 0 || !SystemInfo.supportsInstancing) return;
            var start = Stopwatch.GetTimestamp();
            GeometryUtility.CalculateFrustumPlanes(camera, planes);
            int nearCount = 0, farCount = 0;
            Bounds nearBounds = default, farBounds = default;
            foreach (var patch in patches)
            {
                if (patch.NearCount == 0 || !GeometryUtility.TestPlanesAABB(planes, patch.Bounds)) continue;
                // The distant authored mesh preserves every root/blade/tip and
                // palette; only small intermediate bends are simplified. Never
                // change coverage or blade count when the camera approaches.
                bool distant = farMesh != null && patch.Bounds.SqrDistance(camera.transform.position) > 16f * 16f;
                if (distant) {
                    Append(camera, farMesh, patch, farBatch, ref farCount, ref farBounds);
                    LastFarClumps += patch.NearCount;
                }
                else Append(camera, nearMesh, patch, nearBatch, ref nearCount, ref nearBounds);
            }
            Submit(camera, nearMesh, nearBatch, nearCount, nearBounds);
            Submit(camera, farMesh, farBatch, farCount, farBounds);
            LastSubmissionMilliseconds = (Stopwatch.GetTimestamp() - start) * 1000.0 / Stopwatch.Frequency;
        }

        private void Append(Camera camera, Mesh mesh, Patch patch, Matrix4x4[] batch, ref int count, ref Bounds bounds)
        {
            int offset = 0;
            while (offset < patch.NearCount)
            {
                int length = Mathf.Min(batch.Length - count, patch.NearCount - offset);
                if (count == 0) bounds = patch.Bounds; else bounds.Encapsulate(patch.Bounds);
                Array.Copy(patch.Near, offset, batch, count, length);
                offset += length; count += length;
                if (count == batch.Length) { Submit(camera, mesh, batch, count, bounds); count = 0; }
            }
        }

        private void Submit(Camera camera, Mesh mesh, Matrix4x4[] batch, int count, Bounds bounds)
        {
            if (count == 0 || mesh == null) return;
            var parameters = new RenderParams(material) {
                camera = camera, worldBounds = bounds, layer = gameObject.layer,
                shadowCastingMode = ShadowCastingMode.Off, receiveShadows = true,
                lightProbeUsage = LightProbeUsage.Off, motionVectorMode = MotionVectorGenerationMode.Camera
            };
            Graphics.RenderMeshInstanced(parameters, mesh, 0, batch, count);
            LastDrawCalls++; LastVisibleClumps += count;
            LastTriangles += count * (int)(mesh.GetIndexCount(0) / 3);
        }
    }
}
