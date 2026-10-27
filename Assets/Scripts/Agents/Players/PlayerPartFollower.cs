
using System;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerPartFollower : Module, IPartFollower
    {
        [SerializeField] private EventChannelSO eventChannelSo;

        private bool _canCurrentPartChange = true;

        private IGroundChecker _groundChecker;
        private Rigidbody2D _playerRb;
        private IMapPiece _currentPart;
        private Transform _originalParent;
        
        public IMapPiece CurrentPart => _currentPart;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _playerRb = owner.GetComponent<Rigidbody2D>();
            _groundChecker = owner.GetModule<IGroundChecker>();
            _originalParent = owner.transform.parent;

            Debug.Assert(_playerRb != null);
            Debug.Assert(_groundChecker != null);
        }

        private void FixedUpdate()
        {
            if (_playerRb == null || _groundChecker == null)
                return;

            if (!_canCurrentPartChange)
                return;

            if (_currentPart != null && _currentPart.IsDragging)
                return;

            if (_playerRb.linearVelocityY > 0.05f)
            {
                SetCurrentPart(null);
                return;
            }

            SetCurrentPart(FindGroundPart());
        }

        private IMapPiece FindGroundPart()
        {
            Collider2D col = _groundChecker.CurrentGround;

            return col != null
                ? col.GetComponentInParent<IMapPiece>()
                : null;
        }

        private void SetCurrentPart(IMapPiece nextPart)
        {
            if (_currentPart == nextPart)
                return;

            _currentPart = nextPart;

            Transform parent = _currentPart != null
                ? _currentPart.Transform
                : _originalParent;

            _owner.transform.SetParent(parent, true);
        }

        private void OnEnable()
        {
            if (eventChannelSo != null)
                eventChannelSo.AddListener<BoolEvent>(
                    HandleChangeCanCurrentPart);
        }

        private void OnDisable()
        {
            if (eventChannelSo != null)
                eventChannelSo.RemoveListener<BoolEvent>(
                    HandleChangeCanCurrentPart);
            
            _currentPart = null;
        }

        private void HandleChangeCanCurrentPart(BoolEvent evt)
        {
            _canCurrentPartChange = evt.Value;
        }
    }
}
