using MapSystem;
using UnityEngine;

namespace Command
{
    public class MoveMapPieceCommand : ICommand
    {
        private readonly IMapPiece _piece;
        private readonly IMapGrid _grid;

        private readonly Vector2Int _beforeGridPosition;
        private readonly Vector2Int _afterGridPosition;

        private readonly Vector3 _beforeWorldPosition;
        private readonly Vector3 _afterWorldPosition;

        private readonly bool _wasDeployed;
        private readonly bool _willBeDeployed;

        public MoveMapPieceCommand(IMapPiece piece, IMapGrid grid, Vector2Int beforeGridPosition, Vector2Int afterGridPosition,
            Vector3 beforeWorldPosition, Vector3 afterWorldPosition, bool wasDeployed, bool willBeDeployed)
        {
            _piece = piece;
            _grid = grid;

            _beforeGridPosition = beforeGridPosition;
            _afterGridPosition = afterGridPosition;

            _beforeWorldPosition = beforeWorldPosition;
            _afterWorldPosition = afterWorldPosition;

            _wasDeployed = wasDeployed;
            _willBeDeployed = willBeDeployed;
        }

        public void Execute()
        {
            Apply(_afterGridPosition, _afterWorldPosition, _willBeDeployed);
        }

        public void Undo()
        {
            Apply(_beforeGridPosition, _beforeWorldPosition, _wasDeployed);
        }

        private void Apply(Vector2Int gridPosition, Vector3 worldPosition, bool shouldDeploy)
        {
            if (_piece.IsDeployed)
            {
                _grid.RemoveDeployment(_piece.GridPosition, _piece.Data);

                _piece.ClearDeployed();
            }

            if (shouldDeploy)
            {
                if (!_grid.TryDeployment(gridPosition, _piece.Data))
                {
                    Debug.LogWarning("MapPiece deployment failed.");
                    return;
                }
            }

            _piece.Transform.position = worldPosition;

            if (shouldDeploy)
            {
                _piece.SetDeployed(gridPosition);
            }
        }
    }
}