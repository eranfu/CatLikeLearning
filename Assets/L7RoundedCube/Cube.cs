using UnityEngine;

namespace L7RoundedCube
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Cube : MonoBehaviour
    {
        public int xSize, ySize, zSize;

        private Mesh _mesh;
        private Vector3[] _vertices;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = "Procedural Cube";

            var cornerVertices = 8;
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            int faceVertices = 2 * ((xSize - 1) * (ySize - 1) +
                                    (ySize - 1) * (zSize - 1) +
                                    (zSize - 1) * (xSize - 1));
            _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
            var vi = 0;
            for (var x = 0; x <= xSize; x++)
            {
                for (var y = 0; y <= ySize; y++)
                {
                    for (var z = 0; z <= zSize; z++)
                    {
                        if (z != 0 && z != zSize) continue;
                        _vertices[vi++] = new Vector3(x, y, z);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (_vertices == null) return;
            Gizmos.color = Color.black;
            foreach (Vector3 vertex in _vertices)
            {
                Gizmos.DrawSphere(vertex, 0.1f);
            }
        }
    }
}