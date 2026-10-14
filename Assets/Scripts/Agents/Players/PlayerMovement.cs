using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerMovement : Module, IControlMovement
    {
        [field: SerializeField] public bool Freeze { get; private set; } = true;
        [SerializeField] private EventChannelSO movementControllerRegisteredEvent;
        [SerializeField] private EventChannelSO dragStateChangedEvent;
        
        [SerializeField] private float playerMoveSpeed;
        private Rigidbody2D _playerRb;
        
        private IRenderer _renderer;
        private float _movementDirectionX;
        private Tween _flipTween;
        private int _currentDirection = 1;
        private RigidbodyConstraints2D _originalConstraints;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _renderer = _owner.GetModule<IRenderer>();
            _playerRb = _owner.GetComponent<Rigidbody2D>();
            
            _originalConstraints = _playerRb.constraints;
        }

        private void Start()
        {
            movementControllerRegisteredEvent.RaiseEvent(new ControlMovementEvent().InitData(this));
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

            _flipTween = rendererTrm.DORotate(new Vector3(0f, targetY, 0f), 0.15f, RotateMode.Fast).SetEase(Ease.OutQuad);
        }
        
        private void MovePlayer() 
        {
            if (_playerRb == null) return;
            _playerRb.linearVelocityX = _movementDirectionX * playerMoveSpeed;
        }

        private void OnEnable()
        {
            dragStateChangedEvent.AddListener<BoolEvent>(HandleChangeCanMove);
        }

        private void OnDisable()
        {
            dragStateChangedEvent.RemoveListener<BoolEvent>(HandleChangeCanMove);
        }

        private void HandleChangeCanMove(BoolEvent obj)
        {
            Freeze = obj.Value;
            Debug.Log(Freeze);
            
            if (Freeze)
            {
                _playerRb.constraints = _originalConstraints;
            }
            else
            {
                _playerRb.linearVelocity = Vector2.zero;
                _playerRb.angularVelocity = 0f;
                _playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

        public bool PlayerIsStop()
        {
            return _playerRb.linearVelocity.sqrMagnitude > 0.1f;
        }
    }
}