using System.Collections.Generic;
using UnityEngine;

namespace C1Basics.S5ObjectPools
{
    public class ObjectPool : MonoBehaviour
    {
        private PooledObject prefab;
        private readonly List<PooledObject> availableObjects = new List<PooledObject>();

        public PooledObject GetObject()
        {
            PooledObject obj;
            int lastIndex = availableObjects.Count - 1;
            if (lastIndex >= 0)
            {
                obj = availableObjects[lastIndex];
                availableObjects.RemoveAt(lastIndex);
                obj.gameObject.SetActive(true);
            }
            else
            {
                obj = Instantiate(prefab, transform);
                obj.Pool = this;
            }
            return obj;
        }

        public void AddObject(PooledObject o)
        {
            availableObjects.Add(o);
            o.gameObject.SetActive(false);
        }

        public static ObjectPool GeneratePool(PooledObject prefab)
        {
            var pool = new GameObject(prefab.name + "Pool").AddComponent<ObjectPool>();
            pool.prefab = prefab;
            DontDestroyOnLoad(pool);
            return pool;
        }
    }
}