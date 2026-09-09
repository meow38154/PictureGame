using System;
using DevLib.AnimatorSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoreSystem.Manager
{
    public class SceneChangeManager : MonoBehaviour
    {
        [SerializeField] private string defaultSceneName;
        [SerializeField] private HashDataSO sceneEnter;
        [SerializeField] private HashDataSO sceneExit;
        
        public static SceneChangeManager Instance { get; private set; }

        private ISceneTransition _transition;

        private void Awake()
        {
            ManagerInit();
            _transition = GetComponent<ISceneTransition>();
        }

        private async void Start()
        {
            await SceneChangeAsync(defaultSceneName);
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

        public async Awaitable SceneChangeAsync(string sceneName)
        {
            bool transitionIsNull = _transition != null;
            
            if (transitionIsNull)
                await _transition.PlayAsync(sceneExit);

            await SceneManager.LoadSceneAsync(sceneName);
            
            if (transitionIsNull)
                await _transition.PlayAsync(sceneEnter);
        }
        
        public async Awaitable SceneChangeAsync(int sceneNum)
        {
            bool transitionIsNull = _transition != null;
            
            if (transitionIsNull)
                await _transition.PlayAsync(sceneExit);

            await SceneManager.LoadSceneAsync(sceneNum);
            
            if (transitionIsNull)
                await _transition.PlayAsync(sceneEnter);
        }
    }
}