using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CoreSystem.Triggers
{
    public class UiTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool IsStay { get; private set; }
        
        public UnityEvent OnEnter;
        public UnityEvent OnExit;
        public UnityEvent OnClick;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter?.Invoke();
            IsStay = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit?.Invoke();
            IsStay = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}