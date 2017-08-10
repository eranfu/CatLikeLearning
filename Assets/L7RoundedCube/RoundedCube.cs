using System;
using System.Collections.Generic;
using UnityEngine;

namespace L7RoundedCube
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class RoundedCube : MonoBehaviour
    {
        public int xSize, ySize, zSize;
        public int roundness;

        private Mesh mesh;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Color32[] cubeUV;

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
            CreateColliders();
        }

        private void CreateColliders()
        {
            int doubleRoundness = roundness * 2;
            AddBoxCollider(xSize, ySize - doubleRoundness, zSize - doubleRoundness);
            AddBoxCollider(xSize - doubleRoundness, ySize, zSize - doubleRoundness);
            AddBoxCollider(xSize - doubleRoundness, ySize - doubleRoundness, zSize);

            var min = new Vector3(roundness, roundness, roundness);
            var mid = new Vector3(xSize / 2f, ySize / 2f, zSize / 2f);
            Vector3 max = new Vector3(xSize, ySize, zSize) - min;

            AddCapsuleCollider(0, mid.x, min.y, min.z);
            AddCapsuleCollider(0, mid.x, min.y, max.z);
            AddCapsuleCollider(0, mid.x, max.y, min.z);
            AddCapsuleCollider(0, mid.x, max.y, max.z);

            AddCapsuleCollider(1, min.x, mid.y, min.z);
            AddCapsuleCollider(1, min.x, mid.y, max.z);
            AddCapsuleCollider(1, max.x, mid.y, min.z);
            AddCapsuleCollider(1, max.x, mid.y, max.z);

            AddCapsuleCollider(2, min.x, min.y, mid.z);
            AddCapsuleCollider(2, min.x, max.y, mid.z);
            AddCapsuleCollider(2, max.x, min.y, mid.z);
            AddCapsuleCollider(2, max.x, max.y, mid.z);
        }

        private void AddCapsuleCollider(int direction, float x, float y, float z)
        {
            var c = gameObject.AddComponent<CapsuleCollider>();
            c.center = new Vector3(x, y, z);
            c.direction = direction;
            c.radius = roundness;
            c.height = c.center[direction] * 2;
        }

        private void AddBoxCollider(int x, int y, int z)
        {
            var box = gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(x, y, z);
        }

        private void CreateTriangles()
        {
            var trianglesZ = new int[(xSize * ySize) * 12];
            var trianglesX = new int[(ySize * zSize) * 12];
            var trianglesY = new int[(zSize * xSize) * 12];
            int ring = (xSize + zSize) * 2;
            int tZ = 0, tX = 0, tY = 0, v = 0;
            for (var y = 0; y < ySize; y++, v++)
            {
                for (var q = 0; q < xSize; q++, v++)
                {
                    tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 0; q < zSize; q++, v++)
                {
                    tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 0; q < xSize; q++, v++)
                {
                    tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 1; q < zSize; q++, v++)
                {
                    tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
                }
                tX = SetQuad(trianglesX, tX, v, v + 1 - ring, v + ring, v + 1);
            }

            tY = CreateTopFace(trianglesY, tY, ring);
            CreateBottomFace(trianglesY, tY, ring);

            mesh.subMeshCount = 3;
            mesh.SetTriangles(trianglesZ, 0);
            mesh.SetTriangles(trianglesX, 1);
            mesh.SetTriangles(trianglesY, 2);
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
            normals = new Vector3[vertices.Length];
            cubeUV = new Color32[vertices.Length];

            var vi = 0;
            for (var y = 0; y <= ySize; y++)
            {
                for (var x = 0; x < xSize; x++)
                {
                    SetVertex(vi++, x, y, 0);
                }
                for (var z = 0; z < zSize; z++)
                {
                    SetVertex(vi++, xSize, y, z);
                }
                for (int x = xSize; x > 0; --x)
                {
                    SetVertex(vi++, x, y, zSize);
                }
                for (int z = zSize; z > 0; --z)
                {
                    SetVertex(vi++, 0, y, z);
                }
            }

            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    SetVertex(vi++, x, ySize, z);
                }
            }
            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    SetVertex(vi++, x, 0, z);
                }
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors32 = cubeUV;
        }

        private void SetVertex(int i, int x, int y, int z)
        {
            Vector3 inner = vertices[i] = new Vector3(x, y, z);

            if (x < roundness)
            {
                inner.x = roundness;
            }
            else if (x > xSize - roundness)
            {
                inner.x = xSize - roundness;
            }
            if (y < roundness)
            {
                inner.y = roundness;
            }
            else if (y > ySize - roundness)
            {
                inner.y = ySize - roundness;
            }
            if (z < roundness)
            {
                inner.z = roundness;
            }
            else if (z > zSize - roundness)
            {
                inner.z = zSize - roundness;
            }

            normals[i] = (vertices[i] - inner).normalized;
            vertices[i] = inner + normals[i] * roundness;
            cubeUV[i] = new Color32((byte) x, (byte) y, (byte) z, 0);
        }

        private void OnDrawGizmos()
        {
            if (vertices == null) return;
            Gizmos.color = Color.black;
            for (var i = 0; i < vertices.Length; i++)
            {
                Gizmos.color = Color.black;
                Vector3 vertex = vertices[i];
                Gizmos.DrawSphere(transform.TransformPoint(vertex), 0.1f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.TransformPoint(vertex), transform.TransformDirection(normals[i]));
            }
        }
    }
}