using UnityEngine;

namespace L4ObjectPools
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        public T GetPooledInstance<T>() where T : PooledObject
        {
            throw new System.NotImplementedException();
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