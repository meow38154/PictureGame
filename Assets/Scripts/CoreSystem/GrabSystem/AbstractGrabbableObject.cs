using UnityEngine;

namespace CoreSystem.GrabSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstractGrabbableObject : MonoBehaviour, IGrabbable
    {
        [SerializeField] private LayerMask ignoredWhileGrabbing;
        [field: SerializeField] public Transform Pivot { get; private set; }
        
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody { get; private set; }
        private Collider2D _collider2D;
        
        private LayerMask _previouslyGrabbedLayerMask;
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            if (Pivot == null) Pivot = transform;
        }

        public virtual void OnGrabbed()
        {
            _previouslyGrabbedLayerMask = _collider2D.excludeLayers;
            _collider2D.excludeLayers = ignoredWhileGrabbing;
        }

        public virtual void OnReleased()
        {
            _collider2D.excludeLayers = _previouslyGrabbedLayerMask;
        }
    }
}