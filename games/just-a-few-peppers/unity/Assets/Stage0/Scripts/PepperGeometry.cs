using UnityEngine;

namespace Chushkopek.Stage0
{
    public static class PepperGeometry
    {
        public const float Length = .56f;
        const int Rows = 38;
        const int Columns = 30;

        public static Mesh Create(string name, int strip = -1)
        {
            var mesh = new Mesh { name = name };
            var indices = new int[Rows * Columns * 6];
            var uv = new Vector2[(Rows + 1) * (Columns + 1)];
            int cursor = 0;
            for (int row = 0; row <= Rows; row++)
            for (int col = 0; col <= Columns; col++)
            {
                int index = row * (Columns + 1) + col;
                uv[index] = new Vector2(col / (float)Columns, row / (float)Rows);
                if (row == Rows || col == Columns) continue;
                indices[cursor++] = index; indices[cursor++] = index + 1; indices[cursor++] = index + Columns + 1;
                indices[cursor++] = index + 1; indices[cursor++] = index + Columns + 2; indices[cursor++] = index + Columns + 1;
            }
            mesh.vertices = Vertices(strip, 0f);
            mesh.triangles = indices;
            mesh.uv = uv;
            mesh.RecalculateNormals(); mesh.RecalculateBounds();
            return mesh;
        }

        public static Vector3[] Vertices(int strip, float progress)
        {
            var vertices = new Vector3[(Rows + 1) * (Columns + 1)];
            WriteVertices(vertices, strip, progress);
            return vertices;
        }

        public static void WriteVertices(Vector3[] vertices, int strip, float progress)
        {
            float startAngle = strip < 0 ? 0f : strip == 0 ? -Mathf.PI / 2f : Mathf.PI / 2f;
            float span = strip < 0 ? Mathf.PI * 2f : Mathf.PI;
            for (int row = 0; row <= Rows; row++)
            {
                // s=0 is the stem shoulder, s=1 is the pointed tip.
                float s = row / (float)Rows;
                for (int col = 0; col <= Columns; col++)
                {
                    float angle = startAngle + span * col / Columns;
                    float radius = Radius(s, angle) + (strip >= 0 ? .0025f : 0f);
                    Vector3 radial = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
                    Vector3 point = radial * radius + new Vector3(.028f * s * s, Length * (1f - s), 0f);
                    if (strip >= 0 && s < progress)
                    {
                        float distance = (progress - s) * Length;
                        float curl = Mathf.Min(distance / .062f, Mathf.PI * 1.65f);
                        float boundaryRadius = Radius(progress, angle) + .003f;
                        float outward = (1f - Mathf.Cos(curl)) * .062f + Mathf.Max(0f, distance - .23f) * .4f;
                        point = radial * (boundaryRadius + outward);
                        float flatten = Mathf.SmoothStep(0f, 1f, distance / .09f);
                        Vector3 ribbon = new Vector3((strip == 0 ? 1f : -1f) * (boundaryRadius + outward), 0,
                            Mathf.Sin(angle) * Mathf.Lerp(radius, boundaryRadius, .5f));
                        point = Vector3.Lerp(point, ribbon, flatten);
                        point.y = Length * (1f - progress) + Mathf.Sin(curl) * .062f;
                        point.x += .028f * progress * progress;
                    }
                    vertices[row * (Columns + 1) + col] = point;
                }
            }
        }

        static float Radius(float s, float angle)
        {
            float shoulder = Mathf.Sin(Mathf.Clamp01(s / .16f) * Mathf.PI * .5f);
            return .145f * shoulder * Mathf.Pow(Mathf.Clamp01(1f - s), .68f)
                * (1f + .065f * Mathf.Cos(angle * 3f + s * 5f));
        }
    }
}
