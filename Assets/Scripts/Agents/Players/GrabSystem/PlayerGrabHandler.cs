using System;
using System.Collections.Generic;
using CoreSystem.GrabSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerGrabHandler : MonoBehaviour, IPlayerGrabHandler
    {
        private const int MaxSeparationIterations = 16;
        private const float SeparationPadding = 0.001f;

        [SerializeField] private LayerMask ignoredPlayerLayers = 1 << 3;
        [SerializeField] private PlayerJumper jumper; //개선 예정

        private IGrabbable _current;

        private Transform _grabAnchor;
        private Transform _grabProxy;
        private Transform _originalParent;
        private RigidbodyType2D _originalBodyType;
        private readonly List<IgnoredCollision> _ignoredCollisions = new();
        private Vector3 _originalLocalScale;
        
        private readonly struct IgnoredCollision
        {
            public readonly Collider2D GrabbedCollider;
            public readonly Collider2D PlayerCollider;
            public readonly bool WasIgnored;

            public IgnoredCollision(Collider2D grabbedCollider, Collider2D playerCollider, bool wasIgnored)
            {
                GrabbedCollider = grabbedCollider;
                PlayerCollider = playerCollider;
                WasIgnored = wasIgnored;
            }
        }

        public bool IsGrabbing => _current != null;

        public void Grab(IGrabbable target, Transform handTrm)
        {
            if (target == null || IsGrabbing)
                return;
            target.OnGrabbed();

            _grabAnchor = handTrm != null ? handTrm : transform;
            _grabProxy = new GameObject("Grab Proxy").transform;
            UpdateGrabProxy();
            _current = target;

            _originalParent = target.Transform.parent;
            _originalLocalScale = target.Transform.localScale;
            _originalBodyType = target.Rigidbody.bodyType;

            IgnorePlayerCollisions(target);

            target.Rigidbody.linearVelocity = Vector2.zero;
            target.Rigidbody.angularVelocity = 0f;
            target.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            
            target.Transform.SetParent(_grabProxy, true);

            jumper.CanJump = false;
            
        } 

        public void Release()
        {
            if (_current == null)
                return;

            IGrabbable released = _current;
            _current = null;

            released.Transform.DOKill();
            Vector3 releasePosition = released.Transform.position;
            float releaseAngle = released.Transform.eulerAngles.z;

            // SetParent(worldPositionStays: true) decomposes the world matrix using the
            // hand's scale. If the player flips on this frame, that decomposition can
            // bake a transient negative or non-uniform scale into the released object.
            released.Transform.SetParent(_originalParent, false);
            released.Transform.localScale = _originalLocalScale;
            released.Transform.SetPositionAndRotation(
                releasePosition,
                Quaternion.Euler(0f, 0f, releaseAngle)
            );
            Destroy(_grabProxy.gameObject);
            _grabProxy = null;
            _grabAnchor = null;

            MoveOutsidePlayerColliders(released);
            RestorePlayerCollisions();
            released.Rigidbody.bodyType = _originalBodyType;
            jumper.CanJump = true;
            
            released.OnReleased();
        }
        
        private void LateUpdate()
        {
            if (_current != null)
                UpdateGrabProxy();
        }

        private void UpdateGrabProxy()
        {
            // Follow the hand without inheriting its animated X/Y rotation or scale.
            // Parenting directly to the hand makes a 2D sprite appear squashed while
            // the player's 3D direction-flip tween is between 0 and 180 degrees.
            _grabProxy.SetPositionAndRotation(
                _grabAnchor.position,
                Quaternion.Euler(0f, 0f, _grabAnchor.eulerAngles.z)
            );
        }

        private void IgnorePlayerCollisions(IGrabbable target)
        {
            _ignoredCollisions.Clear();

            Collider2D[] grabbedColliders = target.Transform.GetComponentsInChildren<Collider2D>(true);
            Collider2D[] playerColliders = transform.root.GetComponentsInChildren<Collider2D>(true);

            foreach (Collider2D grabbedCollider in grabbedColliders)
            {
                foreach (Collider2D playerCollider in playerColliders)
                {
                    if (grabbedCollider == playerCollider ||
                        (ignoredPlayerLayers.value & (1 << playerCollider.gameObject.layer)) == 0)
                        continue;

                    bool wasIgnored = Physics2D.GetIgnoreCollision(grabbedCollider, playerCollider);
                    _ignoredCollisions.Add(new IgnoredCollision(grabbedCollider, playerCollider, wasIgnored));
                    Physics2D.IgnoreCollision(grabbedCollider, playerCollider, true);
                }
            }
        }

        private void MoveOutsidePlayerColliders(IGrabbable released)
        {
            Physics2D.SyncTransforms();

            for (int iteration = 0; iteration < MaxSeparationIterations; iteration++)
            {
                ColliderDistance2D deepestOverlap = default;
                bool foundOverlap = false;

                foreach (IgnoredCollision collision in _ignoredCollisions)
                {
                    if (collision.WasIgnored ||
                        collision.GrabbedCollider == null || collision.PlayerCollider == null ||
                        !collision.GrabbedCollider.enabled || !collision.PlayerCollider.enabled ||
                        !collision.GrabbedCollider.gameObject.activeInHierarchy ||
                        !collision.PlayerCollider.gameObject.activeInHierarchy)
                        continue;

                    ColliderDistance2D distance = collision.GrabbedCollider.Distance(collision.PlayerCollider);
                    if (!distance.isOverlapped || foundOverlap && distance.distance >= deepestOverlap.distance)
                        continue;

                    deepestOverlap = distance;
                    foundOverlap = true;
                }

                if (!foundOverlap)
                    return;
                
                Vector2 separation = deepestOverlap.normal * (deepestOverlap.distance - SeparationPadding);
                released.Transform.position += (Vector3)separation;
                Physics2D.SyncTransforms();
            }
        }

        private void RestorePlayerCollisions()
        {
            foreach (IgnoredCollision collision in _ignoredCollisions)
            {
                if (collision.GrabbedCollider == null || collision.PlayerCollider == null)
                    continue;

                Physics2D.IgnoreCollision(
                    collision.GrabbedCollider,
                    collision.PlayerCollider,
                    collision.WasIgnored
                );
            }

            _ignoredCollisions.Clear();
        }
    }
}