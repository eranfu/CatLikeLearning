using UnityEngine;

namespace L3FramesPerSecond
{
    public class NucleonSpawner : MonoBehaviour
    {
        [SerializeField] private float timeBetweenSpawns;
        [SerializeField] private float spawnDistance;
        [SerializeField] private Nucleon[] nucleons;
        private float timeSinceLastSpawn;

        private void FixedUpdate()
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn >= timeBetweenSpawns)
            {
                timeSinceLastSpawn -= timeBetweenSpawns;
                SpawnNucleon();
            }
        }

        private void SpawnNucleon()
        {
            Nucleon prefab = nucleons[Random.Range(0, nucleons.Length)];
            var nucleon = Instantiate<Nucleon>(prefab);
            nucleon.transform.localPosition = Random.onUnitSphere * spawnDistance;
        }
    }
}