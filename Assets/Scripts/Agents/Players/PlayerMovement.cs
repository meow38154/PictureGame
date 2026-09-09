using DevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerMovement : Module, IControlMovement
    {
        [field: SerializeField] public bool CanControl { get; private set; } = true;
        
        [SerializeField] private float playerMoveSpeed;
        private Rigidbody2D _playerRb;
        
        private IRenderer _renderer;
        private float _movementDirectionX;
        private Tween _flipTween;
        private int _currentDirection = 1;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _renderer = _owner.GetModule<IRenderer>();
            _playerRb = _owner.GetComponent<Rigidbody2D>();
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

            if (_currentDirection == direction)
                return;

            _currentDirection = direction;

            Transform rendererTrm = _renderer.Animator.transform;

            float targetY = direction > 0 ? 0f : 180f;

            _flipTween?.Kill();

            _flipTween = rendererTrm
                .DORotate(
                    new Vector3(0f, targetY, 0f),
                    0.15f,
                    RotateMode.Fast)
                .SetEase(Ease.OutQuad);
        }
        
        private void MovePlayer() 
        {
            if (_playerRb == null) return;
            _playerRb.linearVelocityX = _movementDirectionX * playerMoveSpeed;
        }

        public void ChangeCanMove(bool canMove)
        {
            CanControl = canMove;
        }
    }
}