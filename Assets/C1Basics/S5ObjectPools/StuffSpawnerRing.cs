using UnityEngine;

namespace C1Basics.S5ObjectPools
{
    public class StuffSpawnerRing : MonoBehaviour
    {
        [SerializeField] private int numberOfSpawners;
        [SerializeField] private float radius;
        [SerializeField] private int tiltAngle;
        [SerializeField] private StuffSpawner spawnerPrefab;
        [SerializeField] private Material[] stuffMaterials;

        private void Awake()
        {
            for (var i = 0; i < numberOfSpawners; ++i)
            {
                CreateSpawner(i);
            }
        }

        private void CreateSpawner(int index)
        {
            Transform spawner = Instantiate(spawnerPrefab, transform).transform;
            spawner.localRotation = Quaternion.Euler(0, index * 360f / numberOfSpawners, 0);
            spawner.Translate(0, 0, radius);
            spawner.Rotate(tiltAngle, 0, 0);
            spawner.GetComponent<StuffSpawner>().StuffMaterial = stuffMaterials[index % stuffMaterials.Length];
        }
    }
}