using System;
using Common;
using CoreSystem.Effect;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using CoreSystem.Events;
using DevLib.EventChannelSystem;

namespace CoreSystem.Manager
{
    public class SceneChangeManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO sceneChangeEvent;
        [SerializeField] private AssetNameSO sceneStartAnim;
        [SerializeField] private AssetNameSO sceneEndAnim;
        
        private bool _isLoading;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            sceneChangeEvent.AddListener<StringEvent>(HandleChangeScene);
        }

        private void HandleChangeScene(StringEvent stringEvent)
        {
            StartCoroutine(SceneLoadAnim(stringEvent.Value));
        }

        private void OnDisable()
        {
            sceneChangeEvent.RemoveListener<StringEvent>(HandleChangeScene);
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