using Agents.Players;
using Command.UndoableObjects;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace CoreSystem.GrabSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class AbstractGrabbableObject : UndoableObject, IGrabbable
    {
        [SerializeField] private EventChannelSO grabEventChannelSo;
        public bool IsGrabbing { get; private set; }
        public Transform Transform => transform;
        
        private Collider2D _collider;
        private Rigidbody2D _rb;

        private IPartFollower _partFollower;
        
        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _partFollower = GetModule<IPartFollower>();
        }

        public virtual void OnGrabbed()
        {
            _partFollower.IsActive = false;
            grabEventChannelSo.RaiseEvent(CreateEvents.GrabbableObjectEvent.InitData(this));
            _collider.enabled = false;
            ChangeAllRigidbodyType(RigidbodyType2D.Kinematic);
            IsGrabbing = true;
        }

        public virtual void OnReleased()
        {
            _partFollower.IsActive = true;
            _collider.enabled = true; 
            ChangeAllRigidbodyType(RigidbodyType2D.Dynamic);
            IsGrabbing = false;
        }

        private void ChangeAllRigidbodyType(RigidbodyType2D type)
        {
            _rb.bodyType = type;
        }
    }
}

