using UnityEngine;

namespace L4ObjectPools
{
    public class StuffSpawner : MonoBehaviour
    {
        [SerializeField] private Stuff[] _stuffPrefabs;
        [SerializeField] private float _timeBetweenSpawns;
        private float _timeSinceLastSpawn;

        private void FixedUpdate()
        {
            _timeSinceLastSpawn += Time.unscaledDeltaTime;
            if (!(_timeSinceLastSpawn >= _timeBetweenSpawns)) return;
            _timeSinceLastSpawn -= _timeBetweenSpawns;
            SpawnStuff();
        }

        private void SpawnStuff()
        {
            Stuff prefab = _stuffPrefabs[Random.Range(0, _stuffPrefabs.Length)];
            Stuff spawn = Instantiate(prefab);
            spawn.transform.localPosition = transform.position;
        }
    }
}