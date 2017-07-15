using System.Collections;
using UnityEngine;

namespace L2ConstructingAFractal
{
    public class Fractal : MonoBehaviour
    {
        private static readonly Vector3[] childDirections =
        {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.forward,
            Vector3.back,
        };

        private static readonly Quaternion[] childOrientations =
        {
            Quaternion.identity,
            Quaternion.Euler(0, 0, -90),
            Quaternion.Euler(0, 0, 90),
            Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(-90, 0, 0)
        };

        public Mesh mesh;
        public Material material;
        public int maxDepth;
        public float childScale;
        private int _depth;

        private void Start()
        {
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
            gameObject.AddComponent<MeshRenderer>().material = material;
            if (_depth < maxDepth)
            {
                StartCoroutine(CreateChildren());
            }
        }

        private IEnumerator CreateChildren()
        {
            for (var i = 0; i < childDirections.Length; ++i)
            {
                yield return new WaitForSeconds(0.5f);
                new GameObject("Fractal Child").AddComponent<Fractal>()
                    .Initialize(this, i);
            }
        }

        private void Initialize(Fractal parent, int i)
        {
            mesh = parent.mesh;
            material = parent.material;
            maxDepth = parent.maxDepth;
            _depth = parent._depth + 1;
            childScale = parent.childScale;

            transform.parent = parent.transform;
            transform.localScale = Vector3.one * childScale;
            transform.localPosition = childDirections[i] * (0.5f + 0.5f * childScale);
            transform.localRotation = childOrientations[i];
        }
    }
}