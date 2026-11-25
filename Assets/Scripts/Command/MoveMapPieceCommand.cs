using MapSystem;
using UnityEngine;

namespace Command
{
    public class MoveMapPieceCommand : ICommand
    {
        private readonly IMapPiece _piece;

        private readonly Vector2Int _originalGridPosition;
        private readonly Vector2Int _targetGridPosition;

        private readonly Vector3 _originalWorldPosition;
        private readonly Vector3 _targetWorldPosition;

        private readonly bool _wasDeployed;
        private readonly bool _willBeDeployed;

        public MoveMapPieceCommand(IMapPiece piece, IMapGrid mapGrid, Vector2Int originalGridPosition, Vector2Int targetGridPosition,
            Vector3 originalWorldPosition, Vector3 targetWorldPosition, bool wasDeployed, bool willBeDeployed)
        {
            _piece = piece;

            _originalGridPosition = originalGridPosition;
            _targetGridPosition = targetGridPosition;

            _originalWorldPosition = originalWorldPosition;
            _targetWorldPosition = targetWorldPosition;

            _wasDeployed = wasDeployed;
            _willBeDeployed = willBeDeployed;
        }

        public void Execute()
        {
            Apply(_targetGridPosition, _targetWorldPosition, _willBeDeployed);
        }

        public void Undo()
        {
            Apply(_originalGridPosition, _originalWorldPosition, _wasDeployed);
        }

        public void Redo()
        {
            Execute();
        }

        private void Apply(Vector2Int gridPosition, Vector3 worldPosition, bool deployed)
        {
            Vector3 previousWorldPosition = _piece.Transform.position;
            Vector2Int previousGridPosition = _piece.GridPosition;

            bool previousDeployed = _piece.IsDeployed;
            bool previousActive = _piece.IsActive;

            _piece.RemoveDeployment();
            _piece.SetPieceActive(true);

            _piece.Transform.position = worldPosition;

            if (deployed && !_piece.TryDeployment(gridPosition))
            {
                _piece.Transform.position = previousWorldPosition;
                _piece.SetPieceActive(previousActive);

                if (previousDeployed)
                    _piece.TryDeployment(previousGridPosition);

                Debug.LogError("파츠 배치에 실패했습니다.");
            }
        }
    }
}