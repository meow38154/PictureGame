using DevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    public class WindowOpenAnimation : Module
    {
        private RectTransform _ownerRectTransform;
        private Tween _openScaleChangeTween;
        private Tween _openFadeTween;
        private CanvasGroup _canvasGroup;
        private Vector3 _originalScale;
        

        [SerializeField] private float openDuration = 0.3f;
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

            _ownerRectTransform.localScale = new Vector3(_originalScale.x * startScaleMultiplier, _originalScale.y * startScaleMultiplier, _originalScale.z);

            _openScaleChangeTween = _ownerRectTransform.DOScale(_originalScale, openDuration);
            _openFadeTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, openDuration);
        }

        private void OnDestroy()
        {
            _openScaleChangeTween?.Kill();
            _openFadeTween?.Kill();
        }
    }
}