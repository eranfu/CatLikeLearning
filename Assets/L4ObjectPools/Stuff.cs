using UnityEngine;
using UnityEngine.SceneManagement;

namespace L4ObjectPools
{
    [RequireComponent(typeof(Rigidbody))]
    public class Stuff : PooledObject
    {
        public Rigidbody Body { get; private set; }
        private MeshRenderer[] _meshRenderers;

        private void Awake()
        {
            Body = GetComponent<Rigidbody>();
            _meshRenderers = GetComponentsInChildren<MeshRenderer>();
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                ReturnToPool();
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("KillZone"))
            {
                ReturnToPool();
            }
        }

        public void SetMaterial(Material m)
        {
            foreach (MeshRenderer meshRenderer in _meshRenderers)
            {
                meshRenderer.material = m;
            }
        }
    }
}