using CoreSystem.Effect;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerJumper : Module, IControlJumper
    {
        [SerializeField] private AssetNameSO sweatVfxAssetName;
        [SerializeField] private float jumpForce;
        [SerializeField, Range(0f, 1f)] private float jumpCutMultiplier = 0.5f;

        [field: SerializeField] public bool CanJump { get; set; } = true;
        [field: SerializeField] public float CoyoteTime { get; private set; } = 0.1f;

        public bool IsJumpFall { get; set; }

        private Rigidbody2D _playerRb;
        
        private IVfxModule _vfxModule;

        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _vfxModule = owner.GetModule<IVfxModule>();
            Debug.Assert(_vfxModule != null, "Player에는 IVfxModule 필요합니다.");
            _playerRb = _owner.GetComponent<Rigidbody2D>();
            Debug.Assert(_playerRb != null, "Player에는 Rigidbody2D가 필요합니다.");
        }

        public void Jump()
        {
            if (_playerRb == null) return;

            _playerRb.linearVelocityY = 0f;
            _playerRb.AddForceY(jumpForce, ForceMode2D.Impulse);
        }

        public void CancelJump()
        {
            if (_playerRb == null)
                return;

            if (_playerRb.linearVelocityY <= 0f)
                return;

            if (!CanJump)
                return;
            
            _playerRb.linearVelocityY *= jumpCutMultiplier;
        }

        public void NotJumpVfx()
        {
            _vfxModule.PlayVfx(sweatVfxAssetName.AssetHash);
        }
    }
}