using System;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerMovement : Module, IControlMovement
    {
        [SerializeField] private EventChannelSO movementControllerRegisteredEvent;
        [SerializeField] private EventChannelSO playerFreezeControllerEvent;
        [SerializeField] private float playerMoveSpeed;
        
        private int _currentDirection = 1;
        private float _movementDirectionX;
        private Rigidbody2D _playerRb;
        private IAgentFreezer _freezer;
        private IRenderer _renderer;
        private Tween _flipTween;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _freezer = _owner.GetModule<IAgentFreezer>();
            _renderer = _owner.GetModule<IRenderer>();
            _playerRb = _owner.GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            movementControllerRegisteredEvent.RaiseEvent(CreateEvents.ControlMovementEvent.InitData(this));
        }

        public void SetMovementDirectionX(float movementXInput)
        {
            _movementDirectionX = movementXInput;
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }
        
        public void UpdateFacingDirection(float movementXKey)
        {
            if (movementXKey == 0)
                return;

            int direction = movementXKey > 0 ? 1 : -1;

            if (_currentDirection == direction) return;

            _currentDirection = direction;

            Transform rendererTrm = _renderer.Animator.transform;

            float targetY = direction > 0 ? 0f : 180f;

            _flipTween?.Kill();

            _flipTween = rendererTrm.DORotate(new Vector3(0f, targetY, 0f), 0.15f).SetEase(Ease.OutQuad);
        }
        
        private void MovePlayer() 
        {
            if (_playerRb == null) return;
            
            _playerRb.linearVelocityX = _movementDirectionX * playerMoveSpeed;
        }

        private void OnEnable()
        {
            playerFreezeControllerEvent.AddListener<BoolEvent>(HandleFreezeSetting);
        }

        private void OnDisable()
        {
            playerFreezeControllerEvent.RemoveListener<BoolEvent>(HandleFreezeSetting);
        }

        private void HandleFreezeSetting(BoolEvent obj)
        {
            if (!obj.Value) _freezer.Freeze();
            else _freezer.UnFreeze();
        }


        public bool PlayerIsStop()
        {
            return _playerRb.linearVelocity.sqrMagnitude > 0.1f;
        }
    }
}