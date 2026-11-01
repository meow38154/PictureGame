using UnityEngine;
using UnityEngine.UI;

namespace CoreSystem.Manager
{
    public class SceneChangeManager : MonoBehaviour
    {
        [SerializeField] private string defaultSceneName;
        public static SceneChangeManager Instance { get; private set; }

        private void Awake()
        {
            ManagerInit();
        }

        private void ManagerInit()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ChangeScene(string sceneName)
        {

        }
    }
}