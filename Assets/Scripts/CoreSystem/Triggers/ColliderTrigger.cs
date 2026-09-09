using UnityEngine;
using UnityEngine.Events;

namespace CoreSystem.Triggers
{
    public class ColliderTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers;

        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onExit;
        
        public bool IsSearch { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsTargetLayer(other.gameObject.layer))
                return;
            
            IsSearch = true;
            onEnter?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!IsTargetLayer(other.gameObject.layer))
                return;
            
            IsSearch = false;
            onExit?.Invoke();
        }

        private bool IsTargetLayer(int layer)
        {
            return (targetLayers.value & (1 << layer)) != 0;
        }
    }
}