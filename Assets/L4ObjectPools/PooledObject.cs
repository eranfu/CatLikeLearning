using System;
using UnityEngine;

namespace L4ObjectPools
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { private get; set; }

        [NonSerialized] private ObjectPool poolInstanceForPrefab;

        public T GetPooledInstance<T>() where T : PooledObject
        {
            if (poolInstanceForPrefab == null)
            {
                poolInstanceForPrefab = ObjectPool.GeneratePool(this);
            }
            return (T) poolInstanceForPrefab.GetObject();
        }

        public void ReturnToPool()
        {
            if (Pool)
            {
                Pool.AddObject(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}