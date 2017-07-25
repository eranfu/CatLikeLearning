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

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Cube";

            var cornerVertices = 8;
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            int faceVertices = 2 * ((xSize - 1) * (ySize - 1) +
                                    (ySize - 1) * (zSize - 1) +
                                    (zSize - 1) * (xSize - 1));
            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            var vi = 0;
            for (var x = 0; x <= xSize; x++)
            {
                for (var y = 0; y <= ySize; y++)
                {
                    for (var z = 0; z <= zSize; z++)
                    {
                        if (z != 0 && z != zSize) continue;
                        vertices[vi++] = new Vector3(x, y, z);
                    }
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