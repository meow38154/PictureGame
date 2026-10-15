using CoreSystem.Triggers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] public UnityEvent onKeyDown;
        private ColliderTrigger colliderTrigger;

        private bool _end;

        private void Awake()
        {
            colliderTrigger = GetComponentInChildren<ColliderTrigger>();
            Debug.Assert(colliderTrigger != null, "ColliderTrigger is missing");
        }

        private void Update()
        {
            if (!colliderTrigger.IsSearch || !Keyboard.current.eKey.wasPressedThisFrame || _end) return;
            onKeyDown?.Invoke();
            _end = true;
        }
    }
}