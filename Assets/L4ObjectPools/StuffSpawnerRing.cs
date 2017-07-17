using UnityEngine;

namespace L4ObjectPools
{
    public class StuffSpawnerRing : MonoBehaviour
    {
        [SerializeField] private int _numberOfSpawners;
        [SerializeField] private float _radius;
        [SerializeField] private int _tiltAngle;
        [SerializeField] private StuffSpawner _spawnerPrefab;
        [SerializeField] private Material[] _stuffMaterials;

        private void Awake()
        {
            for (var i = 0; i < _numberOfSpawners; ++i)
            {
                CreateSpawner(i);
            }
        }

        private void CreateSpawner(int index)
        {
            Transform spawner = Instantiate(_spawnerPrefab, transform).transform;
            spawner.localRotation = Quaternion.Euler(0, index * 360f / _numberOfSpawners, 0);
            spawner.Translate(0, 0, _radius);
            spawner.Rotate(_tiltAngle, 0, 0);
            spawner.GetComponent<StuffSpawner>().StuffMaterial = _stuffMaterials[index % _stuffMaterials.Length];
        }
    }
}