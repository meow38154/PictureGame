using DevLib.ModuleSystem;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace UI.Wall_Paper.Files.Windows
{
    public abstract class AbstractWindows : ModuleOwner, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        [Header("Animation Settings")]
        [SerializeField] private float animDuration = 0.5f;
        [Header("Resize Settings")]
        [SerializeField] private float borderWidth = 10f;
        [SerializeField] private Vector2 minSize = new(200f, 150f);
        [SerializeField] private Vector2 maxSize = new(1600f, 1000f);

        [System.Flags]
        public enum BorderDirection
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8
        }

        public BorderDirection CurrentBorder { get; private set; }

        private RectTransform _rect;
        private RectTransform _parentRect;

        private Vector2 _startMousePosition;
        private Vector2 _startSize;
        private Vector2 _startPosition;

        private IWindowAnimation _windowAnimation;

        protected override void Awake()
        {
            base.Awake();
            _windowAnimation = GetModule<IWindowAnimation>();
        }

        public void WindowOpen()
        {
            _windowAnimation.OpenAnimationPlay(animDuration);
        }

        public void WindowClose()
        {
            _windowAnimation.CloseAnimationPlay(animDuration);
            StartCoroutine(AnimEndDisable(animDuration));
        }

        private IEnumerator AnimEndDisable(float duration)
        {
            yield return new WaitForSeconds(duration);
            gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            
            transform.SetAsLastSibling();

            _rect ??= GetComponent<RectTransform>();
            _parentRect ??= _rect.parent as RectTransform;

            if (_parentRect == null) return;

            CurrentBorder = GetBorderDirection(eventData.position, eventData.pressEventCamera);

            if (CurrentBorder == BorderDirection.None) return;

            _startSize = _rect.rect.size;
            _startPosition = _rect.anchoredPosition;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, eventData.position, eventData.pressEventCamera, out _startMousePosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CurrentBorder == BorderDirection.None) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRect, eventData.position, eventData.pressEventCamera, out Vector2 mousePosition)) return;

            Vector2 delta = mousePosition - _startMousePosition;

            float left = 0f;
            float right = 0f;
            float top = 0f;
            float bottom = 0f;

            if (CurrentBorder.HasFlag(BorderDirection.Left))
            {
                float width = Mathf.Clamp(_startSize.x - delta.x, minSize.x, maxSize.x);

                left = _startSize.x - width;
            }
            else if (CurrentBorder.HasFlag(BorderDirection.Right))
            {
                float width = Mathf.Clamp(_startSize.x + delta.x, minSize.x, maxSize.x);

                right = width - _startSize.x;
            }

            if (CurrentBorder.HasFlag(BorderDirection.Top))
            {
                float height = Mathf.Clamp(_startSize.y + delta.y, minSize.y, maxSize.y);

                top = height - _startSize.y;
            }
            else if (CurrentBorder.HasFlag(BorderDirection.Bottom))
            {
                float height = Mathf.Clamp(_startSize.y - delta.y, minSize.y, maxSize.y);

                bottom = _startSize.y - height;
            }

            Vector2 size = _startSize + new Vector2(right - left, top - bottom);

            Vector2 position = _startPosition + new Vector2(left * (1f - _rect.pivot.x) + right * _rect.pivot.x, bottom * (1f - _rect.pivot.y) + top * _rect.pivot.y);

            _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);

            _rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

            _rect.anchoredPosition = position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CurrentBorder = BorderDirection.None;
        }

        private BorderDirection GetBorderDirection(Vector2 screenPosition, Camera cam)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, screenPosition, cam, out Vector2 point)) return BorderDirection.None;

            Rect rect = _rect.rect;

            if (!rect.Contains(point)) return BorderDirection.None;

            BorderDirection direction = BorderDirection.None;

            if (point.x <= rect.xMin + borderWidth) direction |= BorderDirection.Left;
            else if (point.x >= rect.xMax - borderWidth) direction |= BorderDirection.Right;

            if (point.y >= rect.yMax - borderWidth) direction |= BorderDirection.Top;
            else if (point.y <= rect.yMin + borderWidth) direction |= BorderDirection.Bottom;

            return direction;
        }
    }
}