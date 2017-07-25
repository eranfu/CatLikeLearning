using UnityEngine;

namespace L4ObjectPools
{
    public class StuffSpawner : MonoBehaviour
    {
        [SerializeField] private FloatRange timeBetweenSpawns;
        [SerializeField] private FloatRange scale;
        [SerializeField] private float velocity;
        [SerializeField] private FloatRange randomVelocity;
        [SerializeField] private FloatRange angulerVelocity;
        [SerializeField] private Stuff[] stuffPrefabs;
        public Material StuffMaterial { private get; set; }
        private float timeSinceLastSpawn;

        private void FixedUpdate()
        {
            timeSinceLastSpawn += Time.deltaTime;
            float currentTimeBetween = timeBetweenSpawns.RandomInRange;
            if (timeSinceLastSpawn < currentTimeBetween) return;
            timeSinceLastSpawn -= currentTimeBetween;
            SpawnStuff();
        }

        private void SpawnStuff()
        {
            Stuff prefab = stuffPrefabs[Random.Range(0, stuffPrefabs.Length)];
            var spawn = prefab.GetPooledInstance<Stuff>();
            spawn.transform.localPosition = transform.position;
            spawn.transform.localScale = Vector3.one * scale.RandomInRange;
            spawn.transform.localRotation = Random.rotation;
            spawn.Body.AddForce(transform.up * velocity + Random.onUnitSphere * randomVelocity.RandomInRange,
                ForceMode.VelocityChange);
            spawn.Body.angularVelocity = Random.onUnitSphere * angulerVelocity.RandomInRange;
            spawn.SetMaterial(StuffMaterial);
        }
    }
}