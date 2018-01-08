using UnityEngine;
using UnityEngine.SceneManagement;

namespace C1Basics.S5ObjectPools
{
    [RequireComponent(typeof(Rigidbody))]
    public class Stuff : PooledObject
    {
        public Rigidbody Body { get; private set; }
        private MeshRenderer[] meshRenderers;

        private void Awake()
        {
            Body = GetComponent<Rigidbody>();
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
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
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = m;
            }
        }
    }
}