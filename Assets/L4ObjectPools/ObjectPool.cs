using UnityEngine;

namespace L4ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private PooledObject _prefab;

        public PooledObject GetObject()
        {
            PooledObject obj = Instantiate(_prefab, transform);
            obj.Pool = this;
            return obj;
        }

        public void AddObject(PooledObject o)
        {
            Destroy(o.gameObject);
        }

        public static ObjectPool InstanceOfPrefab(PooledObject prefab)
        {
            ObjectPool[] objectPools = FindObjectsOfType<ObjectPool>();
            foreach (ObjectPool pool in objectPools)
            {
                if (pool.name == prefab.name + "Pool")
                    return pool;
            }
            var objectPool = new GameObject(prefab.name + "Pool").AddComponent<ObjectPool>();
            objectPool._prefab = prefab;
            return objectPool;
        }
    }
}