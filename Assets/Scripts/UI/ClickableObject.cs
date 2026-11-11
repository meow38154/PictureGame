using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class ClickableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEvent onClickEvent;
        [SerializeField] private float hoverScaleMultiplier = 0.9f;
        [SerializeField] private float clickScaleMultiplier = 0.5f;
        [SerializeField] private float animDuration = 0.3f;
        
        private RectTransform _rectTransform;
        private Vector2 _saveSize;
        private bool _isClick;
        
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _saveSize = _rectTransform.localScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClickEvent?.Invoke();
            _isClick = true;
            Sequence s = DOTween.Sequence();
            
            s.Append(_rectTransform.DOScale(_saveSize * clickScaleMultiplier, animDuration));
            s.Append(_rectTransform.DOScale(_saveSize, animDuration));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isClick) return;
            
            _rectTransform.DOScale(_saveSize * hoverScaleMultiplier, animDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isClick) return;
            
            _rectTransform.DOScale(_saveSize, animDuration);
        }
    }
}