using System;
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
            Enter(other.gameObject.layer);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Exit(other.gameObject.layer);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Enter(other.gameObject.layer);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            Exit(other.gameObject.layer);
        }
        
        private void Enter(int layer)
        {
            if (!IsTargetLayer(layer))
                return;
            
            IsSearch = true;
            onEnter?.Invoke();
        }
        
        private void Exit(int layer)
        {
            if (!IsTargetLayer(layer))
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