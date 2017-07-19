using UnityEngine;

namespace L4ObjectPools
{
    public class StuffSpawner : MonoBehaviour
    {
        [SerializeField] private FloatRange _timeBetweenSpawns;
        [SerializeField] private FloatRange _scale;
        [SerializeField] private float _velocity;
        [SerializeField] private FloatRange _randomVelocity;
        [SerializeField] private FloatRange _angulerVelocity;
        [SerializeField] private Stuff[] _stuffPrefabs;
        public Material StuffMaterial { private get; set; }
        private float _timeSinceLastSpawn;

        private void FixedUpdate()
        {
            _timeSinceLastSpawn += Time.deltaTime;
            float currentTimeBetween = _timeBetweenSpawns.RandomInRange;
            if (_timeSinceLastSpawn < currentTimeBetween) return;
            _timeSinceLastSpawn -= currentTimeBetween;
            SpawnStuff();
        }

        private void SpawnStuff()
        {
            Stuff prefab = _stuffPrefabs[Random.Range(0, _stuffPrefabs.Length)];
            var spawn = prefab.GetPooledInstance<Stuff>();
            spawn.transform.localPosition = transform.position;
            spawn.transform.localScale = Vector3.one * _scale.RandomInRange;
            spawn.transform.localRotation = Random.rotation;
            spawn.Body.AddForce(transform.up * _velocity + Random.onUnitSphere * _randomVelocity.RandomInRange,
                ForceMode.VelocityChange);
            spawn.Body.angularVelocity = Random.onUnitSphere * _angulerVelocity.RandomInRange;
            spawn.SetMaterial(StuffMaterial);
        }
    }
}