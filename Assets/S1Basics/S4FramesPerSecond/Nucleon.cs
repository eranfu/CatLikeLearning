using UnityEngine;

namespace S1Basics.S4FramesPerSecond
{
    [RequireComponent(typeof(Rigidbody))]
    public class Nucleon : MonoBehaviour
    {
        [SerializeField] private float attractionForce;
        private Rigidbody body;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            body.AddForce(transform.localPosition * -attractionForce);
        }
    }
}