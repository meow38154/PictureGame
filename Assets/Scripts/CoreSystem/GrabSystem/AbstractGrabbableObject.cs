using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace CoreSystem.GrabSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstractGrabbableObject : MonoBehaviour, IGrabbable
    {
        [SerializeField] private EventChannelSO grabEventChannelSo;
        public bool IsGrabbing { get; private set; }
        public Transform Transform => transform;
        
        private Collider2D _collider;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        public virtual void OnGrabbed()
        {
            grabEventChannelSo.RaiseEvent(CreateEvents.GrabbableObjectEvent.InitData(this));
            _collider.enabled = false;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            IsGrabbing = true;
        }

        public virtual void OnReleased()
        {
            _collider.enabled = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            IsGrabbing = false;
        }
    }
}

