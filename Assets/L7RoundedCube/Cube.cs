using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L7RoundedCube
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Cube : MonoBehaviour
    {
        public int xSize, ySize, zSize;

        private Mesh mesh;
        private Vector3[] vertices;

        private void Awake()
        {
            Generate();
        }

        private static int SetQuad(IList<int> triangles, int t, int v00, int v10, int v01, int v11)
        {
            triangles[t] = v00;
            triangles[t + 1] = triangles[t + 4] = v01;
            triangles[t + 2] = triangles[t + 3] = v10;
            triangles[t + 5] = v11;
            return t + 6;
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Cube";
            CreateVertices();
            CreateTriangles();
        }

        private void CreateTriangles()
        {
            int quads = (xSize * ySize + ySize * zSize + zSize * xSize) * 2;
            var triangles = new int[quads * 6];
            int ring = (xSize + zSize) * 2;
            int t = 0, v = 0;
            for (var y = 0; y < ySize; y++, v++)
            {
                for (var q = 0; q < ring - 1; q++, v++)
                {
                    t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
                }
                t = SetQuad(triangles, t, v, v + 1 - ring, v + ring, v + 1);
            }

            t = CreateTopFace(triangles, t, ring);
            CreateBottomFace(triangles, t, ring);
            mesh.triangles = triangles;
        }

        private int CreateTopFace(int[] triangles, int t, int ring)
        {
            int v = ring * ySize;
            for (var x = 1; x < xSize; x++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
            }
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

            int vMin = ring * (ySize + 1) - 1;
            int vMid = vMin + 1;
            int vMax = v + 2;
            for (var z = 2; z < zSize; z++, vMin--, vMax++, vMid++)
            {
                t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
                for (var x = 2; x < xSize; x++, vMid++)
                {
                    t = SetQuad(triangles, t, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
            }

            int vTop = vMin - 2;
            t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
            for (var x = 2; x < xSize; x++, vMid++, vTop--)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
            }
            t = SetQuad(triangles, t, vMid, vMax, vTop, vTop - 1);

            return t;
        }

        private void CreateBottomFace(IList<int> triangles, int t, int ring)
        {
            var v = 1;
            int vMid = vertices.Length - (xSize - 1) * (zSize - 1);
            t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
            for (var x = 2; x < xSize; x++, v++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
            }
            t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

            int vMin = ring - 2;
            ++vMid;
            int vMax = v + 3;
            for (var z = 2; z < zSize; z++, vMin--, vMax++, vMid++)
            {
                t = SetQuad(triangles, t, vMin, vMid, vMin + 1, vMid - xSize + 1);
                for (var x = 2; x < xSize; x++, vMid++)
                {
                    t = SetQuad(triangles, t, vMid, vMid + 1, vMid - xSize + 1, vMid - xSize + 2);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid - xSize + 1, vMax - 1);
            }

            int vTop = vMin - 1;
            vMid -= xSize - 1;
            t = SetQuad(triangles, t, vMin, vTop, vMin + 1, vMid);
            for (var x = 2; x < xSize; x++, vMid++, vTop--)
            {
                t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
            }
            SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        }

        private void CreateVertices()
        {
            const int cornerVertices = 8;
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            int faceVertices = 2 * ((xSize - 1) * (ySize - 1) +
                                    (ySize - 1) * (zSize - 1) +
                                    (zSize - 1) * (xSize - 1));
            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            var vi = 0;
            for (var y = 0; y <= ySize; y++)
            {
                for (var x = 0; x < xSize; x++)
                {
                    vertices[vi++] = new Vector3(x, y, 0);
                }
                for (var z = 0; z < zSize; z++)
                {
                    vertices[vi++] = new Vector3(xSize, y, z);
                }
                for (int x = xSize; x > 0; --x)
                {
                    vertices[vi++] = new Vector3(x, y, zSize);
                }
                for (int z = zSize; z > 0; --z)
                {
                    vertices[vi++] = new Vector3(0, y, z);
                }
            }

            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    vertices[vi++] = new Vector3(x, ySize, z);
                }
            }
            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    vertices[vi++] = new Vector3(x, 0, z);
                }
            }

            mesh.vertices = vertices;
        }

        private void OnDrawGizmos()
        {
            if (vertices == null) return;
            Gizmos.color = Color.black;
            foreach (Vector3 vertex in vertices)
            {
                Gizmos.DrawSphere(vertex, 0.1f);
            }
        }
    }
}