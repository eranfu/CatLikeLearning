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
    }
}