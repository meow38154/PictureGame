using UnityEngine;

namespace MapSystem
{
    public class DisposableMapPiece : AbstractMapPiece
    {
        protected override void Awake()
        {
            base.Awake();
            OnPlayerTakeOff += HandlePieceDisable;
        }

        private void HandlePieceDisable()
        {
            if (!IsActive || !IsDeployed || IsDragging) return;
            
            SetPieceActive(false);
            transform.position += Vector3.down * -10000f;
        }

        private void OnDestroy()
        {
            OnPlayerTakeOff -= HandlePieceDisable;
        }
    }
}