using Common;
using CoreSystem.Effect;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace CoreSystem.Manager
{
    public class SceneChangeManager : MonoSingleton<SceneChangeManager>
    {
        [SerializeField] private AssetNameSO sceneStartAnim;
        [SerializeField] private AssetNameSO sceneEndAnim;
        
        private bool _isLoading;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void ChangeScene(string sceneName)
        {
            StartCoroutine(SceneLoadAnim(sceneName));
        }

        private IEnumerator SceneLoadAnim(string sceneName)
        {
            _animator.Play(sceneStartAnim.AssetHash);
            yield return new WaitForSeconds(GetAnimatorClipTime());
            ChangeSceneAsync(sceneName);
            _animator.Play(sceneEndAnim.AssetHash);
        }

        private float GetAnimatorClipTime()
        {
            return _animator.GetCurrentAnimatorStateInfo(0).length;
        }

        public AsyncOperation ChangeSceneAsync(string sceneName)
        {
            return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}