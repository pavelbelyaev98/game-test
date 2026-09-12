using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SomethingDownThere
{
    [DisallowMultipleComponent]
    public sealed class TerrainVolume : MonoBehaviour, IDigTarget
    {
        [SerializeField] private Vector3Int dimensions = new Vector3Int(192, 96, 192);
        [SerializeField, Min(0.1f)] private float cellSize = 0.125f;
        [SerializeField, Range(2, 24)] private int chunkSize = 16;
        [SerializeField, Min(0.1f)] private float digRadius = 0.345807f;
        [SerializeField, Range(0f, 0.15f)] private float scoopVariation = 0.12f;
        [SerializeField, Range(0f, 0.08f)] private float scoopDepthVariation = 0.05f;
        [SerializeField] private int excavationSeed = 2718;
        [SerializeField] private Material soilMaterial;
        [SerializeField] private GameObject untouchedPreview;

        private sealed class Chunk
        {
            public Mesh Mesh;
            public MeshCollider Collider;
            public MeshRenderer Renderer;
            public readonly TerrainChunkMesh.DensityCache DensityCache = new TerrainChunkMesh.DensityCache();
            public Action BeforeMeshWrite;
            public void DetachCollider() => Collider.sharedMesh = null;
        }
        private readonly Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
        private ExcavationGrid grid;
        private readonly TerrainChunkMesh.Workspace meshing = new TerrainChunkMesh.Workspace();
        private Transform chunkRoot;
        public Vector3Int Dimensions => dimensions;
        public float CellSize => cellSize;
        public float RemovedVolume => grid?.RemovedVolume ?? 0;
        public float LastRemovedVolume => grid?.LastRemovedVolume ?? 0;
        public float LastDetachedVolume => grid?.LastDetachedVolume ?? 0;
        public int LastDetachedSamples => grid?.LastDetachedSamples ?? 0;
        public int LastSupportVisitedSamples => grid?.LastSupportVisitedSamples ?? 0;
        public int LastRemnantSamples => grid?.LastRemnantSamples ?? 0;
        public float LastRemnantVolume => grid?.LastRemnantVolume ?? 0;
        public int LastRemnantCheckedSamples => grid?.LastRemnantCheckedSamples ?? 0;
        public float SurfaceHeight => transform.TransformPoint(Vector3.up * dimensions.y * cellSize).y;
        // Kept for existing session diagnostics; volume in m3 is the smooth terrain metric.
        public int RemainingCells => dimensions.x * dimensions.y * dimensions.z
            - Mathf.RoundToInt(RemovedVolume / (cellSize * cellSize * cellSize));
        public int Revision => grid?.Revision ?? 0;
        public long StateRevision { get; private set; }
        public int ExcavationSeed => excavationSeed;
        public int ChunkCount => chunks.Count;
        public int LastRebuiltChunkCount { get; private set; }
        public double LastDigMilliseconds { get; private set; }
        public double LastGridMilliseconds { get; private set; }
        public double LastMeshMilliseconds { get; private set; }
        public double LastDiscoveryMilliseconds { get; private set; }
        public long SnapshotCopiedBytes => grid?.SnapshotCopiedBytes ?? 0;
        public event Action<Bounds> Changed;
        public bool CanDig => isActiveAndEnabled && grid != null;
        public bool IsRestoring { get; private set; }
        public string DigPrompt => "";
        public float DigRadius
        {
            get => digRadius;
            set
            {
                if (!ExcavationGrid.Finite(value) || value < cellSize || value > 4f)
                    throw new ArgumentOutOfRangeException(nameof(value));
                digRadius = value;
            }
        }

        // Author before activation. Runtime state is initialized once, never on a checkpoint/enable.
        public void Configure(Vector3Int size, float metersPerCell, int cellsPerChunk, float radius,
            Material material, GameObject preview = null)
        {
            if (grid != null) throw new InvalidOperationException("Cannot reconfigure an excavation session.");
            if (cellsPerChunk < 2 || cellsPerChunk > 24) throw new ArgumentOutOfRangeException(nameof(cellsPerChunk));
            // Validate dimensions without keeping an independently mutable state instance.
            new ExcavationGrid(size, metersPerCell);
            dimensions = size;
            cellSize = metersPerCell;
            chunkSize = cellsPerChunk;
            DigRadius = radius;
            soilMaterial = material;
            untouchedPreview = preview;
        }

        private void Awake() => InitializeSession();

        public void InitializeSession()
        {
            if (grid != null) return;
            if ((transform.lossyScale - Vector3.one).sqrMagnitude > 0.0001f)
                throw new InvalidOperationException("TerrainVolume requires unit scale; configure its dimensions instead.");
            grid = new ExcavationGrid(dimensions, cellSize);
            if (untouchedPreview != null) untouchedPreview.SetActive(false);
            chunkRoot = new GameObject("Chunks").transform;
            chunkRoot.SetParent(transform, false);
            for (int z = 0; z < dimensions.z; z += chunkSize)
            for (int y = 0; y < dimensions.y; y += chunkSize)
            for (int x = 0; x < dimensions.x; x += chunkSize)
            {
                var key = new Vector3Int(x / chunkSize, y / chunkSize, z / chunkSize);
                var root = new GameObject($"Chunk {key.x},{key.y},{key.z}", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                root.layer = gameObject.layer;
                root.transform.SetParent(chunkRoot, false);
                var chunk = new Chunk
                {
                    Mesh = new Mesh { name = root.name },
                    Collider = root.GetComponent<MeshCollider>(),
                    Renderer = root.GetComponent<MeshRenderer>()
                };
                root.GetComponent<MeshFilter>().sharedMesh = chunk.Mesh;
                chunk.BeforeMeshWrite = chunk.DetachCollider;
                chunk.Renderer.sharedMaterial = soilMaterial;
                chunks.Add(key, chunk);
                Rebuild(key, chunk);
            }
        }

        public bool IsSolid(Vector3 worldPoint) => grid != null && grid.IsSolid(transform.InverseTransformPoint(worldPoint));
        public float SignedDensity(Vector3 worldPoint) => grid != null ? grid.Sample(transform.InverseTransformPoint(worldPoint)) : 0;

        public GridSnapshot Capture() => grid.Capture();

        public System.Collections.IEnumerator Restore(GridSnapshot snapshot, int seed)
        {
            IsRestoring = true;
            try
            {
                grid.Restore(snapshot);
                excavationSeed = seed;
                foreach (var chunk in chunks.Values) chunk.Collider.enabled = false;
                var slice = Stopwatch.StartNew();
                foreach (var pair in chunks)
                {
                    Rebuild(pair.Key, pair.Value);
                    if (slice.Elapsed.TotalMilliseconds < 8) continue;
                    yield return null;
                    slice.Restart();
                }
                Physics.SyncTransforms();
                NotifyChanged(new BoundsInt(Vector3Int.zero, dimensions));
            }
            finally { IsRestoring = false; }
        }

        public bool TryDig(RaycastHit hit) => TryDig(hit, digRadius);

        public bool TryDig(RaycastHit hit, float radius)
        {
            LastRebuiltChunkCount = 0;
            LastDigMilliseconds = 0;
            if (!CanDig || hit.collider == null || !hit.collider.enabled || hit.collider.transform.parent != chunkRoot)
                return false;
            if (!ExcavationGrid.Finite(radius) || radius < cellSize || radius > 4f) return false;
            Vector3 surface = transform.InverseTransformPoint(hit.point);
            // The net approximates the isosurface within a cell. Accept that tolerance,
            // but reject a cached hit into the air left by a previous scoop.
            if (grid.Sample(surface) < -cellSize * 0.75f) return false;
            // Removing an island can leave zero-density surface samples in empty air.
            // Recheck the actual mesh too, so its old hit cannot carve nearby soil.
            float hitTolerance = cellSize * 0.75f;
            if (!hit.collider.Raycast(new Ray(hit.point + hit.normal * hitTolerance, -hit.normal),
                out _, hitTolerance * 2)) return false;
            int seed = unchecked(excavationSeed + grid.Revision * 486187739);
            uint depthHash = unchecked((uint)seed * 747796405u + 2891336453u);
            depthHash = unchecked(((depthHash >> (int)((depthHash >> 28) + 4)) ^ depthHash) * 277803737u);
            depthHash = (depthHash >> 22) ^ depthHash;
            float depthOffset = ((depthHash >> 8) * (1f / 16777216f) * 2 - 1) * scoopDepthVariation;
            Vector3 normal = transform.InverseTransformDirection(hit.normal).normalized;
            Vector3 point = surface - normal * (radius * (0.12f + depthOffset));
            var timer = Stopwatch.StartNew();
            if (!grid.RemoveScoop(point, radius, normal, seed, scoopVariation, out BoundsInt changed)) return false;
            LastGridMilliseconds = timer.Elapsed.TotalMilliseconds;
            // The grid expands this region to include any detached components, even
            // beyond the brush/chunk. Rebuild visible surfaces and collision together.
            // Two cells cover vertex topology plus finite-difference normals at seams.
            Vector3Int first = Vector3Int.Max(Vector3Int.zero, changed.min - Vector3Int.one * 2);
            Vector3Int last = Vector3Int.Min(dimensions - Vector3Int.one, changed.max + Vector3Int.one * 2);
            for (int z = first.z / chunkSize; z <= last.z / chunkSize; z++)
            for (int y = first.y / chunkSize; y <= last.y / chunkSize; y++)
            for (int x = first.x / chunkSize; x <= last.x / chunkSize; x++)
            {
                var key = new Vector3Int(x, y, z);
                if (Rebuild(key, chunks[key])) LastRebuiltChunkCount++;
            }
            LastMeshMilliseconds = timer.Elapsed.TotalMilliseconds - LastGridMilliseconds;
            NotifyChanged(changed);
            timer.Stop();
            LastDigMilliseconds = timer.Elapsed.TotalMilliseconds;
            LastDiscoveryMilliseconds = LastDigMilliseconds - LastGridMilliseconds - LastMeshMilliseconds;
            return true;
        }

        // Only the explicitly confirmed admin reset uses this. Reuse chunk objects
        // and mesh buffers so reset cannot leave old colliders or accumulate resources.
        public void ResetExcavation()
        {
            if (grid == null) return;
            grid.Reset();
            foreach (var pair in chunks) Rebuild(pair.Key, pair.Value);
            LastRebuiltChunkCount = 0;
            LastDigMilliseconds = 0;
            NotifyChanged(new BoundsInt(Vector3Int.zero, dimensions));
        }

        private void NotifyChanged(BoundsInt samples)
        {
            StateRevision++;
            if (Changed == null) return;
            // Include the interpolation halo and detached soil beyond the scoop.
            Vector3 min = ((Vector3)samples.min - Vector3.one * 2) * cellSize;
            Vector3 max = ((Vector3)samples.max + Vector3.one * 2) * cellSize;
            var world = new Bounds(transform.TransformPoint(min), Vector3.zero);
            for (int i = 0; i < 8; i++)
                world.Encapsulate(transform.TransformPoint(new Vector3((i & 1) == 0 ? min.x : max.x,
                    (i & 2) == 0 ? min.y : max.y, (i & 4) == 0 ? min.z : max.z)));
            Changed.Invoke(world);
        }

        private bool Rebuild(Vector3Int key, Chunk chunk)
        {
            // Detach before mutating so PhysX cannot keep the previous cooked surface.
            bool changed = TerrainChunkMesh.Rebuild(chunk.Mesh, grid, key * chunkSize, chunkSize, meshing,
                chunk.DensityCache, chunk.BeforeMeshWrite);
            bool visible = chunk.Mesh.GetIndexCount(0) > 0;
            chunk.Renderer.enabled = visible;
            chunk.Collider.enabled = visible;
            if (changed && visible) chunk.Collider.sharedMesh = chunk.Mesh;
            return changed;
        }

        private void OnDestroy()
        {
            foreach (var chunk in chunks.Values)
                if (Application.isPlaying) Destroy(chunk.Mesh); else DestroyImmediate(chunk.Mesh);
            chunks.Clear();
        }
    }
}
