using UnityEngine;

namespace L4ObjectPools
{
    [RequireComponent(typeof(Rigidbody))]
    public class Stuff : MonoBehaviour
    {
        private Rigidbody _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }


    }
}