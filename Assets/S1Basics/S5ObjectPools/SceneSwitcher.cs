using UnityEngine;
using UnityEngine.SceneManagement;

namespace S1Basics.S5ObjectPools
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void SwitchScene()
        {
            int nextIndex = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextIndex);
        }
    }
}