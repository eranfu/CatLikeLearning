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

        [SerializeField] private Mesh[] _meshes;
        [SerializeField] private Material _material;
        [SerializeField] private int _maxDepth;
        [SerializeField] private float _childScale;
        [Range(0, 1)] [SerializeField] private float _spawnProbability;
        private int _depth;
        private Material[,] _materials;
        private float _rotationSpeed;
        [SerializeField] private float _maxRotationSpeed;
        [Range(0, 360)] [SerializeField] private int _maxTwist;

        private void InitializeMaterials()
        {
            _materials = new Material[_maxDepth + 1, 2];
            for (var i = 0; i <= _maxDepth; ++i)
            {
                var t = i / (_maxDepth - 1f);
                t *= t;
                _materials[i, 0] = new Material(_material)
                {
                    color = Color.Lerp(Color.white, Color.yellow, t)
                };
                _materials[i, 1] = new Material(_material)
                {
                    color = Color.Lerp(Color.white, Color.cyan, t)
                };
            }
            _materials[_maxDepth, 0].color = Color.magenta;
            _materials[_maxDepth, 1].color = Color.red;
        }

        private void Start()
        {
            if (_materials == null)
            {
                InitializeMaterials();
            }

            _rotationSpeed = Random.Range(-_maxRotationSpeed, _maxRotationSpeed);
            transform.Rotate(Random.Range(-_maxTwist, _maxTwist), 0, 0);
            gameObject.AddComponent<MeshFilter>().mesh = _meshes[Random.Range(0, _meshes.Length)];
            gameObject.AddComponent<MeshRenderer>().material = _materials[_depth, Random.Range(0, 2)];
            if (_depth < _maxDepth)
            {
                StartCoroutine(CreateChildren());
            }
        }

        private void Update()
        {
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0f);
        }

        private IEnumerator CreateChildren()
        {
            for (var i = 0; i < childDirections.Length; ++i)
            {
                if (Random.value > _spawnProbability) continue;
                yield return new WaitForSeconds(Random.Range(0, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>()
                    .Initialize(this, i);
            }
        }

        private void Initialize(Fractal parent, int i)
        {
            _meshes = parent._meshes;
            _materials = parent._materials;
            _maxDepth = parent._maxDepth;
            _depth = parent._depth + 1;
            _childScale = parent._childScale;
            _spawnProbability = parent._spawnProbability;
            _maxRotationSpeed = parent._maxRotationSpeed;
            _maxTwist = parent._maxTwist;

            transform.parent = parent.transform;
            transform.localScale = Vector3.one * _childScale;
            transform.localPosition = childDirections[i] * (0.5f + 0.5f * _childScale);
            transform.localRotation = childOrientations[i];
        }
    }
}