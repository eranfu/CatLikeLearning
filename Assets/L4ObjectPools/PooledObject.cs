using System;
using UnityEngine;

namespace L4ObjectPools
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { private get; set; }

        [NonSerialized] private ObjectPool _poolInstanceForPrefab;

        public T GetPooledInstance<T>() where T : PooledObject
        {
            if (_poolInstanceForPrefab == null)
            {
                _poolInstanceForPrefab = ObjectPool.GeneratePool(this);
            }
            return (T) _poolInstanceForPrefab.GetObject();
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