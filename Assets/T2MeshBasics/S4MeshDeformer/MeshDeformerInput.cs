using UnityEngine;

namespace T2MeshBasics.S4MeshDeformer
{
    public class MeshDeformerInput : MonoBehaviour
    {
        [SerializeField] private float force = 10f;
        [SerializeField] private float offset = 0.1f;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                var meshDeformer = hit.collider.GetComponent<MeshDeformer>();
                if (meshDeformer)
                {
                    Vector3 hitPoint = hit.point;
                    hitPoint += hit.normal * offset;
                    meshDeformer.AddDeformingForce(hitPoint, force);
                }
            }
        }
    }
}