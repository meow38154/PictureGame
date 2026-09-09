using CoreSystem.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private ColliderTrigger colliderTrigger;
        [SerializeField] public UnityEvent onKeyDown;

        private bool _end;
        
        private void Update()
        {
            if (!colliderTrigger.IsSearch || !Keyboard.current.eKey.wasPressedThisFrame || _end) return;
            onKeyDown?.Invoke();
            _end = true;
        }
    }
}