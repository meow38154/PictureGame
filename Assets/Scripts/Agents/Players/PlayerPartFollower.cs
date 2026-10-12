
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerPartFollower : Module
    {
        //이거 받아서 true면 작동 안되게 하기
        [SerializeField] private EventChannelSO eventChannelSo;
        [Header("Detection")]
        [SerializeField] private LayerMask mapPieceLayer;
        
        private IGroundChecker  _groundChecker;

        private Rigidbody2D _playerRb;
        private Collider2D _playerCollider;
        private MapPiece _currentPart;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _playerRb = owner.GetComponent<Rigidbody2D>();
            _playerCollider = owner.GetComponent<Collider2D>();
            _groundChecker = owner.GetModule<IGroundChecker>();

            Debug.Assert(_playerRb != null, "PlayerPartFollower: Rigidbody2D가 없습니다.");
            Debug.Assert(_playerCollider != null, "PlayerPartFollower: Collider2D가 없습니다.");
            Debug.Assert(_groundChecker != null, "PlayerPartFollower: GroundChecker 없습니다.");
        }

        private void FixedUpdate()
        {
            if (_playerRb == null || _playerCollider == null)
                return;
            
            if (_currentPart != null && _currentPart.IsDragging) return;
            
            if (_playerRb.linearVelocityY > 0.05f)
            {
                SetCurrentPart(null); return;
            }

            SetCurrentPart(FindGroundPart());
        }

        private MapPiece FindGroundPart()
        {
            Collider2D col = _groundChecker.CurrentGround;

            if (col == null) return null;
            
            return col.TryGetComponent(out _currentPart) ? _currentPart : null;
        }

        private void SetCurrentPart(MapPiece nextPart)
        {
            if (_currentPart == nextPart) return;

            if (_currentPart != null)
            {
                _currentPart.OnMoved -= HandlePartMoved;
            }

            _currentPart = nextPart;

            if (_currentPart != null)
            {
                _currentPart.OnMoved += HandlePartMoved;
            }
        }

        private void HandlePartMoved(Vector2 delta)
        {
            Debug.Log(delta);
            
            if (_playerRb == null) return;
            
            transform.position += (Vector3)delta;
        }

        private void OnDisable()
        {
            SetCurrentPart(null);
        }

        private void OnDestroy()
        {
            SetCurrentPart(null);
        }
    }
}
