using System.Collections.Generic;
using UnityEngine;

namespace T2MeshBasics.S2RoundedCube
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class RoundedCube : MonoBehaviour
    {
        public int XSize, YSize, ZSize;
        public int Roundness;

        private Mesh mesh;
        private Vector3[] vertices;
        private Vector3[] normals;
        private Color32[] cubeUv;

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
            int doubleRoundness = Roundness * 2;
            AddBoxCollider(XSize, YSize - doubleRoundness, ZSize - doubleRoundness);
            AddBoxCollider(XSize - doubleRoundness, YSize, ZSize - doubleRoundness);
            AddBoxCollider(XSize - doubleRoundness, YSize - doubleRoundness, ZSize);

            var min = new Vector3(Roundness, Roundness, Roundness);
            var mid = new Vector3(XSize / 2f, YSize / 2f, ZSize / 2f);
            Vector3 max = new Vector3(XSize, YSize, ZSize) - min;

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
            c.radius = Roundness;
            c.height = c.center[direction] * 2;
        }

        private void AddBoxCollider(int x, int y, int z)
        {
            var box = gameObject.AddComponent<BoxCollider>();
            box.size = new Vector3(x, y, z);
        }

        private void CreateTriangles()
        {
            var trianglesZ = new int[(XSize * YSize) * 12];
            var trianglesX = new int[(YSize * ZSize) * 12];
            var trianglesY = new int[(ZSize * XSize) * 12];
            int ring = (XSize + ZSize) * 2;
            int tZ = 0, tX = 0, tY = 0, v = 0;
            for (var y = 0; y < YSize; y++, v++)
            {
                for (var q = 0; q < XSize; q++, v++)
                {
                    tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 0; q < ZSize; q++, v++)
                {
                    tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 0; q < XSize; q++, v++)
                {
                    tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
                }
                for (var q = 1; q < ZSize; q++, v++)
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
            int v = ring * YSize;
            for (var x = 1; x < XSize; x++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
            }
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

            int vMin = ring * (YSize + 1) - 1;
            int vMid = vMin + 1;
            int vMax = v + 2;
            for (var z = 2; z < ZSize; z++, vMin--, vMax++, vMid++)
            {
                t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + XSize - 1);
                for (var x = 2; x < XSize; x++, vMid++)
                {
                    t = SetQuad(triangles, t, vMid, vMid + 1, vMid + XSize - 1, vMid + XSize);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid + XSize - 1, vMax + 1);
            }

            int vTop = vMin - 2;
            t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
            for (var x = 2; x < XSize; x++, vMid++, vTop--)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
            }
            t = SetQuad(triangles, t, vMid, vMax, vTop, vTop - 1);

            return t;
        }

        private void CreateBottomFace(IList<int> triangles, int t, int ring)
        {
            var v = 1;
            int vMid = vertices.Length - (XSize - 1) * (ZSize - 1);
            t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
            for (var x = 2; x < XSize; x++, v++, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
            }
            t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

            int vMin = ring - 2;
            ++vMid;
            int vMax = v + 3;
            for (var z = 2; z < ZSize; z++, vMin--, vMax++, vMid++)
            {
                t = SetQuad(triangles, t, vMin, vMid, vMin + 1, vMid - XSize + 1);
                for (var x = 2; x < XSize; x++, vMid++)
                {
                    t = SetQuad(triangles, t, vMid, vMid + 1, vMid - XSize + 1, vMid - XSize + 2);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid - XSize + 1, vMax - 1);
            }

            int vTop = vMin - 1;
            vMid -= XSize - 1;
            t = SetQuad(triangles, t, vMin, vTop, vMin + 1, vMid);
            for (var x = 2; x < XSize; x++, vMid++, vTop--)
            {
                t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
            }
            SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
        }

        private void CreateVertices()
        {
            const int cornerVertices = 8;
            int edgeVertices = (XSize + YSize + ZSize - 3) * 4;
            int faceVertices = 2 * ((XSize - 1) * (YSize - 1) +
                                    (YSize - 1) * (ZSize - 1) +
                                    (ZSize - 1) * (XSize - 1));
            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            normals = new Vector3[vertices.Length];
            cubeUv = new Color32[vertices.Length];

            var vi = 0;
            for (var y = 0; y <= YSize; y++)
            {
                for (var x = 0; x < XSize; x++)
                {
                    SetVertex(vi++, x, y, 0);
                }
                for (var z = 0; z < ZSize; z++)
                {
                    SetVertex(vi++, XSize, y, z);
                }
                for (int x = XSize; x > 0; --x)
                {
                    SetVertex(vi++, x, y, ZSize);
                }
                for (int z = ZSize; z > 0; --z)
                {
                    SetVertex(vi++, 0, y, z);
                }
            }

            for (var z = 1; z < ZSize; z++)
            {
                for (var x = 1; x < XSize; x++)
                {
                    SetVertex(vi++, x, YSize, z);
                }
            }
            for (var z = 1; z < ZSize; z++)
            {
                for (var x = 1; x < XSize; x++)
                {
                    SetVertex(vi++, x, 0, z);
                }
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.colors32 = cubeUv;
        }

        private void SetVertex(int i, int x, int y, int z)
        {
            Vector3 inner = vertices[i] = new Vector3(x, y, z);

            if (x < Roundness)
            {
                inner.x = Roundness;
            }
            else if (x > XSize - Roundness)
            {
                inner.x = XSize - Roundness;
            }
            if (y < Roundness)
            {
                inner.y = Roundness;
            }
            else if (y > YSize - Roundness)
            {
                inner.y = YSize - Roundness;
            }
            if (z < Roundness)
            {
                inner.z = Roundness;
            }
            else if (z > ZSize - Roundness)
            {
                inner.z = ZSize - Roundness;
            }

            normals[i] = (vertices[i] - inner).normalized;
            vertices[i] = inner + normals[i] * Roundness;
            cubeUv[i] = new Color32((byte) x, (byte) y, (byte) z, 0);
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