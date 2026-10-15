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

        private void Awake()
        {
            colliderTrigger = GetComponentInChildren<ColliderTrigger>();
            Debug.Assert(colliderTrigger != null, "ColliderTrigger is missing");
        }

        private void Update()
        {
            if (!colliderTrigger.IsSearch || !Keyboard.current.eKey.wasPressedThisFrame) return;
            onKeyDown?.Invoke();
        }
    }
}