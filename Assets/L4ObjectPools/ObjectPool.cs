using System.Collections.Generic;
using UnityEngine;

namespace L4ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        private PooledObject _prefab;
        private readonly List<PooledObject> _availableObjects = new List<PooledObject>();

        public PooledObject GetObject()
        {
            PooledObject obj;
            int lastIndex = _availableObjects.Count - 1;
            if (lastIndex >= 0)
            {
                obj = _availableObjects[lastIndex];
                _availableObjects.RemoveAt(lastIndex);
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(_prefab, transform);
                obj.Pool = this;
            }
            return obj;
        }

        public void AddObject(PooledObject o)
        {
            _availableObjects.Add(o);
            o.gameObject.SetActive(false);
        }

        public static ObjectPool GeneratePool(PooledObject prefab)
        {
            var pool = new GameObject(prefab.name + "Pool").AddComponent<ObjectPool>();
            pool._prefab = prefab;
            DontDestroyOnLoad(pool);
            return pool;
        }
    }
}