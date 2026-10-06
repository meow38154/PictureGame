using System;
using System.Collections.Generic;
using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerPartDetector : Module, IPartDetector
    {
        [SerializeField] private Transform footTrm;
        [SerializeField] private LayerMask partLayer;

        private readonly Dictionary<IMapPart, HashSet<Collider2D>> _partColliders = new();

        private bool _isDirty;

        public event Action<IMapPart> OnPartChangeEvent;

        public IMapPart CurrentPart { get; private set; }

        private void Update()
        {
            if (!_isDirty)
                return;

            _isDirty = false;
            UpdateCurrentPart();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("음");
            
            IMapPart part = GetMapPart(other);
            
            Debug.Log(other.gameObject.name);
            
            if (part == null)
                return;

            AddPartCollider(part, other);
            _isDirty = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            IMapPart part = GetMapPart(other);

            if (part == null)
                return;

            RemovePartCollider(part, other);
            _isDirty = true;
        }

        private void AddPartCollider(IMapPart part, Collider2D collider)
        {
            if (!_partColliders.TryGetValue(part, out HashSet<Collider2D> colliders))
            {
                colliders = new HashSet<Collider2D>();
                _partColliders.Add(part, colliders);
            }

            colliders.Add(collider);
        }

        private void RemovePartCollider(IMapPart part, Collider2D collider)
        {
            if (!_partColliders.TryGetValue(part, out HashSet<Collider2D> colliders))
                return;

            colliders.Remove(collider);

            if (colliders.Count == 0)
                _partColliders.Remove(part);
        }

        private IMapPart GetMapPart(Collider2D col)
        {
            MapPart part = col.GetComponentInParent<MapPart>();

            if (part == null)
                return null;

            if (!IsPartLayer(col.gameObject.layer))
                return null;

            return part;
        }

        private bool IsPartLayer(int layer)
        {
            return (partLayer.value & (1 << layer)) != 0;
        }

        private void UpdateCurrentPart()
        {
            IMapPart newPart = FindClosestPart();

            if (ReferenceEquals(CurrentPart, newPart))
                return;

            CurrentPart = newPart;
            OnPartChangeEvent?.Invoke(CurrentPart);
        }

        private IMapPart FindClosestPart()
        {
            IMapPart closestPart = null;
            float closestDistance = float.MaxValue;

            Vector2 footPosition = footTrm.position;

            foreach (IMapPart part in _partColliders.Keys)
            {
                if (part == null || part.Collider == null)
                    continue;

                Vector2 closestPoint = part.Collider.ClosestPoint(footPosition);

                float distance = Vector2.SqrMagnitude(
                    footPosition - closestPoint
                );

                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                closestPart = part;
            }

            return closestPart;
        }
    }
}