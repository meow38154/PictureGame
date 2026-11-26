using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    public interface IMapPiece
    {
        event Action OnPlayerLand;
        event Action OnPlayerTakeOff;

        bool OnPlayer { get; set; }

        bool Fixed { get; }
        Transform Transform { get; }

        bool IsDeployed { get; }
        bool IsDragging { get; }
        bool IsActive { get; }

        Vector2Int GridPosition { get; }
        IReadOnlyList<Vector2Int> OccupiedCells { get; }

        void Initialize(IMapGrid mapGrid);

        bool CanDeployment(Vector2Int pos, bool ignorePlayer = false);
        bool TryDeployment(Vector2Int pos);
        void RemoveDeployment();

        void SetDragging(bool dragging);
        void SetPieceActive(bool active);

        void SetPreview(bool valid);
        void ResetPreview();

        void AllFreeze();
        void AllUnFreeze();
    }
}