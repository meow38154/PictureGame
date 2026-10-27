using DevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    public class WindowAnimation : Module, IWindowAnimation
    {
        private RectTransform _ownerRectTransform;
        private Tween _openScaleChangeTween;
        private Tween _openFadeTween;
        private CanvasGroup _canvasGroup;
        private Vector3 _originalScale;
        private Vector3 _targetScale;
        
        [SerializeField] private float startScaleMultiplier = 0.7f;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            
            _canvasGroup = owner.GetComponent<CanvasGroup>();
            _ownerRectTransform = owner.GetComponent<RectTransform>();
            
            if (_ownerRectTransform == null)
            {
                Debug.LogWarning("RectTransform not found");
                return;
            }

            _originalScale = _ownerRectTransform.localScale;

            _openScaleChangeTween?.Kill();
            _openFadeTween?.Kill();
            
            _targetScale = new Vector3(_originalScale.x * startScaleMultiplier, _originalScale.y * startScaleMultiplier, _originalScale.z);
        }

        public void OpenAnimationPlay(float duration)
        {
            _ownerRectTransform.localScale = _targetScale;

            _openScaleChangeTween = _ownerRectTransform.DOScale(_originalScale, duration);
            _openFadeTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, duration);
        }

        public void CloseAnimationPlay(float duration)
        {
            _ownerRectTransform.localScale = _originalScale;
            
            _openScaleChangeTween = _ownerRectTransform.DOScale(_targetScale, duration);
            _openFadeTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, duration);
        }

        private void OnDestroy()
        {
            _openScaleChangeTween?.Kill();
            _openFadeTween?.Kill();
        }
    }
}