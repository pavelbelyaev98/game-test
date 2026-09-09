#if UNITY_EDITOR
using System.Collections;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    // Validation-only density field. Uses the production mesher, colliders and
    // approved terrain material; never saved into MainGame or a player profile.
    public static class CrouchTerrainFixture
    {
        public static IEnumerator Prepare(TerrainVolume terrain)
        {
            var grid = terrain.Capture();
            var samples = grid.Density.ToArray();
            for (int z = 0; z <= grid.Size.z; z++)
            for (int y = 0; y <= grid.Size.y; y++)
            for (int x = 0; x <= grid.Size.x; x++)
            {
                Vector3 w = terrain.transform.TransformPoint(new Vector3(x, y, z) * grid.CellSize);
                float floor = -4f;
                if (Mathf.Abs(w.x + 6f) <= 0.5f && Mathf.Abs(w.z) <= 8f) floor = -2f;
                if (Mathf.Abs(w.x + 2f) <= 0.875f && w.z >= -8f && w.z <= 4f)
                    floor = -4f + Mathf.Clamp((w.z + 6f) * 0.5f, 0f, 3f);
                if (Mathf.Abs(w.x - 2f) <= 0.875f && Mathf.Abs(w.z) <= 8f)
                    floor = -3f + Mathf.Clamp(Mathf.Floor((w.z + 6f) / 1.25f), 0f, 5f) * 0.2f;
                float density = floor - w.y;
                if (w.x >= 4f)
                {
                    density = -w.y;
                    float high = BoxInterior(w, new Vector3(5, -3, -8), new Vector3(7, -0.7f, -2));
                    float corner = BoxInterior(w, new Vector3(5, -3, -4), new Vector3(10, -0.7f, -2));
                    float low = BoxInterior(w, new Vector3(5, -3, -2.25f), new Vector3(7, -1.7f, 4));
                    density = Mathf.Min(density, -Mathf.Max(high, Mathf.Max(corner, low)));
                }
                samples[x + (grid.Size.x + 1) * (y + (grid.Size.y + 1) * z)] = Mathf.Clamp(density, -0.25f, 0.25f);
            }
            grid.Density = DensitySnapshot.CopyFrom(samples);
            grid.Revision++;
            grid.LowestCarvedY = 0;
            yield return terrain.Restore(grid, terrain.ExcavationSeed);
        }

        private static float BoxInterior(Vector3 p, Vector3 min, Vector3 max) =>
            Mathf.Min(Mathf.Min(p.x - min.x, max.x - p.x), Mathf.Min(Mathf.Min(p.y - min.y, max.y - p.y), Mathf.Min(p.z - min.z, max.z - p.z)));

        public static void Place(FpsPlayer player, Vector3 position, float crouch = 0)
        {
            var snapshot = new WorldSnapshot();
            player.Capture(snapshot);
            snapshot.PlayerPosition = position;
            snapshot.PlayerRotation = Quaternion.identity;
            snapshot.Pitch = 0;
            snapshot.VerticalSpeed = 0;
            snapshot.CrouchAmount = crouch;
            player.Restore(snapshot);
        }

        public static void Advance(FpsPlayer player, float seconds, FpsInputFrame frame, int fps)
        {
            int frames = Mathf.CeilToInt(seconds * fps);
            for (int i = 0; i < frames; i++) player.Tick(frame, seconds / frames);
        }
    }
}
#endif
