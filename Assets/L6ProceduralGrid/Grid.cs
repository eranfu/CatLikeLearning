using System;
using UnityEngine;

namespace L6ProceduralGrid
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Grid : MonoBehaviour
    {
        public int width, height;
        private Vector3[] vertices;
        private Mesh mesh;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Grid";

            vertices = new Vector3[(width + 1) * (height + 1)];
            var uv = new Vector2[vertices.Length];
            var tangents = new Vector4[vertices.Length];
            var tangent = new Vector4(1, 0, 0, -1);
            for (int i = 0, row = 0; row <= height; row++)
            {
                for (var column = 0; column <= width; ++column, ++i)
                {
                    vertices[i] = new Vector3(column, row);
                    uv[i] = new Vector2(column / (float) width, row / (float) height);
                    tangents[i] = tangent;
                }
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.tangents = tangents;

            var triangles = new int[width * height * 6];
            for (int ti = 0, vi = 0, y = 0; y < height; ++y, ++vi)
            {
                for (var x = 0; x < width; ti += 6, ++vi, ++x)
                {
                    triangles[ti] = vi;
                    triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 5] = vi + width + 2;
                }
            }
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            if (vertices == null) return;
            Gizmos.color = Color.black;
            foreach (Vector3 vertex in vertices)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.1f);
            }
        }
    }
}