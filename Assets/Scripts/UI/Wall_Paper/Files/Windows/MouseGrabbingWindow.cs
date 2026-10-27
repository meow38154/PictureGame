using CoreSystem.Events;
using DevLib.ModuleSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Wall_Paper.Files.Windows
{
    public class MouseGrabbingWindow : Module,
        IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private bool _isGrabbing;

        private RectTransform _ownerRectTransform;
        private Canvas _canvas;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _ownerRectTransform = _owner.GetComponent<RectTransform>();
            _canvas = _owner.GetComponentInParent<Canvas>().rootCanvas;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _isGrabbing = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isGrabbing) return;

            _ownerRectTransform.anchoredPosition +=
                eventData.delta / _canvas.scaleFactor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _isGrabbing = false;
        }

        private void OnDisable()
        {
            _isGrabbing = false;
        }
    }
}