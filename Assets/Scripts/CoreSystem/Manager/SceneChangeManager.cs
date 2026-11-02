using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreSystem.Manager
{
    public class SceneChangeManager : MonoBehaviour
    {
        public static SceneChangeManager Instance { get; private set; }

        private bool _isLoading;

        private void Awake()
        {
            ManagerInit();
            DontDestroyOnLoad(gameObject);
        }

        private void ManagerInit()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public AsyncOperation ChangeScene(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}