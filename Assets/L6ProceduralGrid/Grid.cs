using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace L6ProceduralGrid
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Grid : MonoBehaviour
    {
        public int width, height;
        private Vector3[] _vertices;
        private Mesh _mesh;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = "Procedural Grid";

            _vertices = new Vector3[(width + 1) * (height + 1)];
            var uv = new Vector2[_vertices.Length];
            var tangents = new Vector4[_vertices.Length];
            var tangent = new Vector4(1, 0, 0, -1);
            for (int i = 0, row = 0; row <= height; row++)
            {
                for (var column = 0; column <= width; ++column, ++i)
                {
                    _vertices[i] = new Vector3(column, row);
                    uv[i] = new Vector2(column / (float) width, row / (float) height);
                    tangents[i] = tangent;
                }
            }
            _mesh.vertices = _vertices;
            _mesh.uv = uv;
            _mesh.tangents = tangents;

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
            _mesh.triangles = triangles;
            _mesh.RecalculateNormals();
        }

        private void OnDrawGizmos()
        {
            if (_vertices == null) return;
            Gizmos.color = Color.black;
            foreach (Vector3 vertex in _vertices)
            {
                Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.1f);
            }
        }
    }
}