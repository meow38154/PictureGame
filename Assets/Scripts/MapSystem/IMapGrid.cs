using UnityEngine;

namespace MapSystem
{
    public interface IMapGrid
    {
        Vector2Int WorldToCell(Vector3 worldPos);
        Vector3 CellToWorld(Vector2Int pos);
        bool CanDeployment(Vector2Int pos, MapPieceSo data);
        bool TryDeployment(Vector2Int pos, MapPieceSo data);
        void RemoveDeployment(Vector2Int pos, MapPieceSo data);
    }
}