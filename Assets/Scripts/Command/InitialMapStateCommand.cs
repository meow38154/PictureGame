using System.Collections.Generic;
using MapSystem;
using UnityEngine;

namespace Command
{
    public class InitialMapStateCommand : ICommand
    {
        private struct MapPieceState
        {
            public IMapPiece Piece;

            public Vector3 WorldPosition;
            public Vector2Int GridPosition;

            public bool IsDeployed;
            public bool IsActive;
        }

        private readonly List<MapPieceState> _initialStates = new();

        public InitialMapStateCommand(IMapGrid mapGrid, IMapPiece[] pieces)
        {
            foreach (IMapPiece piece in pieces)
            {
                MapPieceState state = new()
                {
                    Piece = piece,

                    WorldPosition = piece.Transform.position,
                    GridPosition = piece.GridPosition,

                    IsDeployed = piece.IsDeployed,
                    IsActive = piece.IsActive
                };

                _initialStates.Add(state);
            }
        }

        public void Execute()
        {
            Restore();
        }

        public void Undo()
        {
            Restore();
        }

        public void Redo()
        {
            Restore();
        }

        private void Restore()
        {
            foreach (MapPieceState state in _initialStates)
            {
                state.Piece.RemoveDeployment();
            }

            foreach (MapPieceState state in _initialStates)
            {
                state.Piece.Transform.position = state.WorldPosition;

                state.Piece.SetDragging(false);
                state.Piece.SetPieceActive(state.IsActive);

                if (state.IsDeployed)
                {
                    if (!state.Piece.TryDeployment(state.GridPosition))
                        Debug.LogError($"초기 배치 복구 실패: {state.Piece.Transform.name}");
                }

                state.Piece.ResetPreview();
            }
        }
    }
}