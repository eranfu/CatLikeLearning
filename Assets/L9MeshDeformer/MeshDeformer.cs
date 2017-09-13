using UnityEngine;

namespace L9MeshDeformer
{
    [RequireComponent(typeof(MeshFilter))]
    public class MeshDeformer : MonoBehaviour
    {
        [SerializeField] private float springForce = 20;
        [SerializeField] private float damping = 5;

        private float uniformScale = 1;
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
            uniformScale = transform.lossyScale.x;
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                UpdateVertex(i);
            }
            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();
        }

        private void UpdateVertex(int i)
        {
            Vector3 displacement = displacedVertices[i] - originalVertices[i];
            displacement *= uniformScale;
            vertexVelocities[i] -= Time.deltaTime * springForce * displacement;
            vertexVelocities[i] *= 1 - damping * Time.deltaTime;
            displacedVertices[i] += Time.deltaTime / uniformScale * vertexVelocities[i];
        }

        public void AddDeformingForce(Vector3 point, float force)
        {
            for (int i = 0; i < displacedVertices.Length; i++)
            {
                AddForceToVertex(i, point, force);
            }
        }

        private void AddForceToVertex(int i, Vector3 point, float force)
        {
            point = transform.InverseTransformPoint(point);
            Vector3 pointToVertex = displacedVertices[i] - point;
            pointToVertex *= uniformScale;
            float attenuatedForce = force / (1 + pointToVertex.sqrMagnitude);
            float velocity = attenuatedForce * Time.deltaTime;
            vertexVelocities[i] += pointToVertex.normalized * velocity;
        }
    }
}