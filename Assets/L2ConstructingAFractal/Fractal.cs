using System.Collections;
using UnityEngine;

namespace L2ConstructingAFractal
{
    public class Fractal : MonoBehaviour
    {
        private static readonly Vector3[] ChildDirections =
        {
            Vector3.up,
            Vector3.right,
            Vector3.left,
            Vector3.forward,
            Vector3.back,
        };

        private static readonly Quaternion[] ChildOrientations =
        {
            Quaternion.identity,
            Quaternion.Euler(0, 0, -90),
            Quaternion.Euler(0, 0, 90),
            Quaternion.Euler(90, 0, 0),
            Quaternion.Euler(-90, 0, 0)
        };

        [SerializeField] private Mesh[] meshes;
        [SerializeField] private Material material;
        [SerializeField] private int maxDepth;
        [SerializeField] private float childScale;
        [Range(0, 1)] [SerializeField] private float spawnProbability;
        private int depth;
        private Material[,] materials;
        private float rotationSpeed;
        [SerializeField] private float maxRotationSpeed;
        [Range(0, 360)] [SerializeField] private int maxTwist;

        private void InitializeMaterials()
        {
            materials = new Material[maxDepth + 1, 2];
            for (var i = 0; i <= maxDepth; ++i)
            {
                var t = i / (maxDepth - 1f);
                t *= t;
                materials[i, 0] = new Material(material)
                {
                    color = Color.Lerp(Color.white, Color.yellow, t)
                };
                materials[i, 1] = new Material(material)
                {
                    color = Color.Lerp(Color.white, Color.cyan, t)
                };
            }
            materials[maxDepth, 0].color = Color.magenta;
            materials[maxDepth, 1].color = Color.red;
        }

        private void Start()
        {
            if (materials == null)
            {
                InitializeMaterials();
            }

            rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
            transform.Rotate(Random.Range(-maxTwist, maxTwist), 0, 0);
            gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
            gameObject.AddComponent<MeshRenderer>().material = materials[depth, Random.Range(0, 2)];
            if (depth < maxDepth)
            {
                StartCoroutine(CreateChildren());
            }
        }

        private void Update()
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0f);
        }

        private IEnumerator CreateChildren()
        {
            for (var i = 0; i < ChildDirections.Length; ++i)
            {
                if (Random.value > spawnProbability) continue;
                yield return new WaitForSeconds(Random.Range(0, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>()
                    .Initialize(this, i);
            }
        }

        private void Initialize(Fractal parent, int i)
        {
            meshes = parent.meshes;
            materials = parent.materials;
            maxDepth = parent.maxDepth;
            depth = parent.depth + 1;
            childScale = parent.childScale;
            spawnProbability = parent.spawnProbability;
            maxRotationSpeed = parent.maxRotationSpeed;
            maxTwist = parent.maxTwist;

            transform.parent = parent.transform;
            transform.localScale = Vector3.one * childScale;
            transform.localPosition = ChildDirections[i] * (0.5f + 0.5f * childScale);
            transform.localRotation = ChildOrientations[i];
        }
    }
}