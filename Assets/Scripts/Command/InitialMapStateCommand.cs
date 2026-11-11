using System.Collections.Generic;
using MapSystem;
using UnityEngine;

namespace Command
{
    public class InitialMapStateCommand : ICommand
    {
        private readonly IMapGrid _grid;
        private readonly List<PieceState> _states = new();

        private struct PieceState
        {
            public IMapPiece Piece;
            public Vector2Int GridPosition;
            public Vector3 WorldPosition;
            public bool IsDeployed;
        }

        public InitialMapStateCommand(IMapGrid grid, IMapPiece[] pieces)
        {
            _grid = grid;

            foreach (IMapPiece piece in pieces)
            {
                if (piece == null || piece.Data == null) continue;

                _states.Add(new PieceState
                {
                    Piece = piece,
                    GridPosition = piece.GridPosition,
                    WorldPosition = piece.Transform.position,
                    IsDeployed = piece.IsDeployed
                });
            }
        }

        public void Execute()
        {
        }

        public void Undo()
        {
            Debug.Log("이");
            
            foreach (PieceState state in _states)
            {
                if (state.Piece == null) continue;

                if (state.Piece.IsDeployed)
                {
                    _grid.RemoveDeployment(state.Piece.GridPosition, state.Piece.Data);
                    state.Piece.ClearDeployed();
                }
            }

            foreach (PieceState state in _states)
            {
                if (state.Piece == null) continue;

                state.Piece.Transform.position = state.WorldPosition;

                if (!state.IsDeployed) continue;

                if (_grid.TryDeployment(state.GridPosition, state.Piece.Data))
                    state.Piece.SetDeployed(state.GridPosition);
                else
                    Debug.LogWarning($"초기 배치 복원 실패: {state.Piece.Transform.gameObject.name}");
            }
        }
    }
}