using UnityEngine;

namespace L4ObjectPools
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        public T GetPooledInstance<T>() where T : PooledObject
        {
            return (T) ObjectPool.InstanceOfPrefab(this).GetObject();
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