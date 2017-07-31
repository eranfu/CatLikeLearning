using System.Collections;
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
            StartCoroutine(Generate());
        }

        private IEnumerator Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Cube";
            yield return CreateVertices();
        }

        private IEnumerator CreateVertices()
        {
            var wait = new WaitForSeconds(0.05f);
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
                    yield return wait;
                }
                for (var z = 0; z < zSize; z++)
                {
                    vertices[vi++] = new Vector3(xSize, y, z);
                    yield return wait;
                }
                for (int x = xSize; x > 0; --x)
                {
                    vertices[vi++] = new Vector3(x, y, zSize);
                    yield return wait;
                }
                for (int z = zSize; z > 0; --z)
                {
                    vertices[vi++] = new Vector3(0, y, z);
                    yield return wait;
                }
            }

            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    vertices[vi++] = new Vector3(x, ySize, z);
                    yield return wait;
                }
            }
            for (var z = 1; z < zSize; z++)
            {
                for (var x = 1; x < xSize; x++)
                {
                    vertices[vi++] = new Vector3(x, 0, z);
                    yield return wait;
                }
            }
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