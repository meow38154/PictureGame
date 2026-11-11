using System;
using CoreSystem.Effect;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MapSystem.Stage
{
    public class NextStageAnim : MonoBehaviour
    {
        [SerializeField] private AssetNameSO nextAssetName;
        [SerializeField] private EventChannelSO nextStageEvent;
        
        [SerializeField] private Image prevScreen;
        [SerializeField] private Image nextScreen;

        [SerializeField] private GameObject root;
        
        private string _sceneName;
        private Sprite _prevSprite;
        private Sprite _nextSprite;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>(true);
        }

        private void OnEnable()
        {
            nextStageEvent.AddListener<StringEvent>(HandlePlayAnim);
        }

        private void OnDisable()
        {
            nextStageEvent.RemoveListener<StringEvent>(HandlePlayAnim);
        }

        private async void HandlePlayAnim(StringEvent obj)
        {
            _sceneName = obj.Value;

            Rect rect = new Rect(0, 0, Screen.width, Screen.height);

            Texture2D prevTexture = await Utility.GetCurrentSceneScreenAsync(rect);
            if (prevTexture != null)
            {
                if (_prevSprite != null)
                {
                    Destroy(_prevSprite.texture);
                    Destroy(_prevSprite);
                }

                _prevSprite = Sprite.Create(prevTexture, new Rect(0, 0, prevTexture.width, prevTexture.height), new Vector2(0.5f, 0.5f));
                prevScreen.sprite = _prevSprite;
            }

            Texture2D nextTexture = await Utility.GetSceneFirstScreenAsync(_sceneName, rect);
            if (nextTexture != null)
            {
                if (_nextSprite != null)
                {
                    Destroy(_nextSprite.texture);
                    Destroy(_nextSprite);
                }

                _nextSprite = Sprite.Create(nextTexture, new Rect(0, 0, nextTexture.width, nextTexture.height), new Vector2(0.5f, 0.5f));
                nextScreen.sprite = _nextSprite;
            }

            root.gameObject.SetActive(true);
            _animator.Play(nextAssetName.AssetHash, 0, 0f);
        }

        private void OnDestroy()
        {
            if (_prevSprite != null)
            {
                Destroy(_prevSprite.texture);
                Destroy(_prevSprite);
            }

            if (_nextSprite != null)
            {
                Destroy(_nextSprite.texture);
                Destroy(_nextSprite);
            }
        }
    }
}