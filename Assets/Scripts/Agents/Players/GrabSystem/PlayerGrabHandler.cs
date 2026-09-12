using CoreSystem.GrabSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerGrabHandler : MonoBehaviour, IPlayerGrabHandler
    {
        private IGrabbable _current;

        private Transform _originalParent;
        private RigidbodyType2D _originalBodyType;

        public bool IsGrabbing => _current != null;

        public void Grab(IGrabbable target, Transform handTrm)
        {
            if (target == null || IsGrabbing)
                return;

            Transform grabParent = handTrm != null ? handTrm : transform;
            _current = target;

            _originalParent = target.Transform.parent;
            _originalBodyType = target.Rigidbody.bodyType;

            target.Rigidbody.linearVelocity = Vector2.zero;
            target.Rigidbody.angularVelocity = 0f;
            target.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            
            target.Transform.SetParent(grabParent, true); 
            
            target.OnGrabbed();
        } 

        public void Release()
        {
            if (_current == null)
                return;

            IGrabbable released = _current;
            _current = null;

            released.Transform.DOKill();
            released.Transform.SetParent(_originalParent, true);
            
            released.Rigidbody.bodyType = _originalBodyType;
            released.OnReleased();
        }
    }
}