using UnityEngine;

namespace L3FramesPerSecond
{
    [RequireComponent(typeof(Rigidbody))]
    public class Nucleon : MonoBehaviour
    {
        [SerializeField] private float _attractionForce;
        private Rigidbody _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _body.AddForce(transform.localPosition * -_attractionForce);
        }
    }
}