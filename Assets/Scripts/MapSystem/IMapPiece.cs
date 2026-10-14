using UnityEngine;

namespace MapSystem
{
    public interface IMapPiece
    {
        MapPieceSo Data { get; }
        bool Fixed { get; }
        Transform Transform { get; }
        bool IsDeployed { get; }
        bool IsDragging { get; }
        Vector2Int GridPosition { get; }
        void SetDragging(bool dragging);
        void SetDeployed(Vector2Int pos);
        void SetPreview(bool valid);
        void ResetPreview();
        void ClearDeployed();
        void SetPieceActive(bool active);
    }
}