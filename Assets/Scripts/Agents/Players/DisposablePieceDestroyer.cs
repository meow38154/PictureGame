using System;
using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;
using IDisposable = MapSystem.IDisposable;

namespace Agents.Players
{
    public class DisposablePieceDestroyer : Module
    {
        private IPartFollower _partFollower;
        private IGroundChecker _groundChecker;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            _partFollower = owner.GetModule<IPartFollower>();
            _groundChecker = owner.GetModule<IGroundChecker>();

            _partFollower.OnChangeGroundPiece += CheckPiece;
            _groundChecker.OnGroundExit += HandleExitPiece;
        }

        private void OnDestroy()
        {
            _partFollower.OnChangeGroundPiece -= CheckPiece;
        }
        
        private void HandleExitPiece(Collider2D pieceCollider)
        {
            if (!pieceCollider.gameObject.TryGetComponent<IMapPiece>(out var piece))
                return;
            
            CheckPiece(piece);
        }
        
        private void CheckPiece(IMapPiece piece)
        {
            if (piece is not IDisposable disposablePiece)
                return;
            
            disposablePiece.Dispose();
            piece.SetPieceActive(false); // 임시
        }
    }
}