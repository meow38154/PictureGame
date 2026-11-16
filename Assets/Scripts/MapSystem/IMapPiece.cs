using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    public interface IMapPiece
    {
        bool Fixed { get; }
        Transform Transform { get; }
        public IReadOnlyList<Vector2Int> OccupiedCells { get; }
        bool IsDeployed { get; }
        bool IsDragging { get; }
        bool IsActive { get; }
        Vector2Int GridPosition { get; }
        void SetDragging(bool dragging);
        void SetDeployed(Vector2Int pos);
        void SetPreview(bool valid);
        void ResetPreview();
        void ClearDeployed();
        void SetPieceActive(bool active);
        void AllFreeze();
        void AllUnFreeze();
    }
}