using UnityEngine;

namespace L9MeshDeformer
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshDeformer : MonoBehaviour
    {
        private Mesh deformingMesh;
        private Vector3[] originalVertices, displacedVertices;
        private Vector3[] vertexVelocities;

        private void Start()
        {
            deformingMesh = GetComponent<MeshFilter>().mesh;
            originalVertices = deformingMesh.vertices;
            displacedVertices = new Vector3[originalVertices.Length];
            for (var i = 0; i < originalVertices.Length; i++)
            {
                displacedVertices[i] = originalVertices[i];
            }
            vertexVelocities = new Vector3[originalVertices.Length];
        }

        private void Update()
        {
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                updateVertex(i);
            }
            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();
        }

        private void updateVertex(int i)
        {
            displacedVertices[i] += vertexVelocities[i] * Time.deltaTime;
        }

        public void addDeformingForce(Vector3 point, float force)
        {
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                addForceToVertex(i, point, force);
            }
        }

        private void addForceToVertex(int i, Vector3 point, float force)
        {
            Vector3 pointToVertex = displacedVertices[i] - point;
            float attenuatedForce = force / (1 + pointToVertex.sqrMagnitude);
            float velocity = attenuatedForce * Time.deltaTime;
            vertexVelocities[i] += pointToVertex.normalized * velocity;
        }
    }
}