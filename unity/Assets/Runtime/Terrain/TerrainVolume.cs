using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SomethingDownThere
{
    [DisallowMultipleComponent]
    public sealed class TerrainVolume : MonoBehaviour, IDigTarget
    {
        [SerializeField] private Vector3Int dimensions = new Vector3Int(48, 24, 48);
        [SerializeField, Min(0.1f)] private float cellSize = 0.5f;
        [SerializeField, Range(2, 16)] private int chunkSize = 8;
        [SerializeField, Min(0.1f)] private float digRadius = 1.1f;
        [SerializeField] private Material soilMaterial;
        [SerializeField] private GameObject untouchedPreview;

        private sealed class Chunk
        {
            public Mesh Mesh;
            public MeshCollider Collider;
            public MeshRenderer Renderer;
        }
        private readonly Dictionary<Vector3Int, Chunk> chunks = new Dictionary<Vector3Int, Chunk>();
        private ExcavationGrid grid;
        private Transform chunkRoot;
        public Vector3Int Dimensions => dimensions;
        public float CellSize => cellSize;
        public int RemainingCells => grid?.RemainingCells ?? dimensions.x * dimensions.y * dimensions.z;
        public int Revision => grid?.Revision ?? 0;
        public int ChunkCount => chunks.Count;
        public int LastRebuiltChunkCount { get; private set; }
        public double LastDigMilliseconds { get; private set; }
        public bool CanDig => isActiveAndEnabled && grid != null && grid.RemainingCells > 0;
        public string DigPrompt => "LMB Dig soil";
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
            if (cellsPerChunk < 2 || cellsPerChunk > 16) throw new ArgumentOutOfRangeException(nameof(cellsPerChunk));
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
                chunk.Renderer.sharedMaterial = soilMaterial;
                chunks.Add(key, chunk);
                Rebuild(key, chunk);
            }
        }

        public bool IsSolid(Vector3 worldPoint) => grid != null && grid.IsSolid(transform.InverseTransformPoint(worldPoint));

        public bool TryDig(RaycastHit hit)
        {
            LastRebuiltChunkCount = 0;
            LastDigMilliseconds = 0;
            if (!CanDig || hit.collider == null || !hit.collider.enabled || hit.collider.transform.parent != chunkRoot)
                return false;
            Vector3 point = transform.InverseTransformPoint(hit.point)
                - transform.InverseTransformDirection(hit.normal) * (cellSize * 0.25f);
            // Stale hits must not continue removing remote soil around an already emptied face.
            if (!grid.IsSolid(point)) return false;
            var timer = Stopwatch.StartNew();
            if (!grid.RemoveSphere(point, digRadius, out BoundsInt changed)) return false;
            Vector3Int first = Vector3Int.Max(Vector3Int.zero, changed.min - Vector3Int.one);
            Vector3Int last = Vector3Int.Min(dimensions - Vector3Int.one, changed.max);
            for (int z = first.z / chunkSize; z <= last.z / chunkSize; z++)
            for (int y = first.y / chunkSize; y <= last.y / chunkSize; y++)
            for (int x = first.x / chunkSize; x <= last.x / chunkSize; x++)
            {
                var key = new Vector3Int(x, y, z);
                Rebuild(key, chunks[key]);
                LastRebuiltChunkCount++;
            }
            timer.Stop();
            LastDigMilliseconds = timer.Elapsed.TotalMilliseconds;
            return true;
        }

        private void Rebuild(Vector3Int key, Chunk chunk)
        {
            // Detach before mutating so PhysX cannot keep the previous cooked surface.
            chunk.Collider.sharedMesh = null;
            TerrainChunkMesh.Rebuild(chunk.Mesh, grid, key * chunkSize, chunkSize);
            bool visible = chunk.Mesh.vertexCount > 0;
            chunk.Renderer.enabled = visible;
            chunk.Collider.enabled = visible;
            if (visible) chunk.Collider.sharedMesh = chunk.Mesh;
        }

        private void OnDestroy()
        {
            foreach (var chunk in chunks.Values)
                if (Application.isPlaying) Destroy(chunk.Mesh); else DestroyImmediate(chunk.Mesh);
            chunks.Clear();
        }
    }
}
