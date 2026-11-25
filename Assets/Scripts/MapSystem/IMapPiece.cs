using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    public interface IMapPiece
    {
        bool Fixed { get; }
        bool IsDeployed { get; }
        bool IsDragging { get; }
        bool IsActive { get; }
        bool OnPlayer { get; set; }

        Transform Transform { get; }
        Vector2Int GridPosition { get; }
        IReadOnlyList<Vector2Int> OccupiedCells { get; }

        void Initialize(IMapGrid mapGrid);

        bool CanDeployment(Vector2Int pos);
        bool TryDeployment(Vector2Int pos);
        void RemoveDeployment();

        void SetDragging(bool dragging);
        void SetPreview(bool valid);
        void ResetPreview();

        void SetPieceActive(bool active);

        void AllFreeze();
        void AllUnFreeze();
    }
}