using UnityEngine;

namespace L3FramesPerSecond
{
    public class NucleonSpawner : MonoBehaviour
    {
        [SerializeField] private float _timeBetweenSpawns;
        [SerializeField] private float _spawnDistance;
        [SerializeField] private Nucleon[] _nucleons;
        private float _timeSinceLastSpawn;

        private void FixedUpdate()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            if (_timeSinceLastSpawn >= _timeBetweenSpawns)
            {
                _timeSinceLastSpawn -= _timeBetweenSpawns;
                SpawnNucleon();
            }
        }

        private void SpawnNucleon()
        {
            Nucleon prefab = _nucleons[Random.Range(0, _nucleons.Length)];
            var nucleon = Instantiate<Nucleon>(prefab);
            nucleon.transform.localPosition = Random.onUnitSphere * _spawnDistance;
        }
    }
}