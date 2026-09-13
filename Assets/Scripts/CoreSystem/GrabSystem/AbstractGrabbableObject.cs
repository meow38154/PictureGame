using System;
using UnityEngine;

namespace CoreSystem.GrabSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstractGrabbableObject : MonoBehaviour, IGrabbable
    {
        [SerializeField] private LayerMask ignoredWhileGrabbing;
        [field: SerializeField] public Vector2 Pivot { get; private set; }
        
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody { get; private set; }
        private Collider2D _collider2D;
        
        private LayerMask _previouslyGrabbedLayerMask;
        
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
        }

        public virtual void OnGrabbed()
        {
            _previouslyGrabbedLayerMask = _collider2D.excludeLayers;
            _collider2D.excludeLayers = ignoredWhileGrabbing;
            TrmCorrection();
        }

        public virtual void OnReleased()
        {
            _collider2D.excludeLayers = _previouslyGrabbedLayerMask;
            TrmCorrection();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.TransformPoint(Pivot), 0.1f);
        }

        private void TrmCorrection()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z);
        }
    }
}