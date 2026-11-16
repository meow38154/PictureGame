using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    public interface IMapGrid
    {
        Vector2Int WorldToCell(Vector3 worldPos);
        Vector3 CellToWorld(Vector2Int pos);
        bool CanDeployment(Vector2Int pos, IReadOnlyList<Vector2Int> data);
        bool TryDeployment(Vector2Int pos, IReadOnlyList<Vector2Int> data);
        void RemoveDeployment(Vector2Int pos, IReadOnlyList<Vector2Int> data);
    }
}