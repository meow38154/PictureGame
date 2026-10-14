using System.Collections.Generic;
using UnityEngine;

namespace MapSystem
{
    /// <summary>
    /// 맵의 셀 점유 상태를 관리합니다.
    /// </summary>
    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private Grid grid;
        
        private readonly HashSet<Vector2Int> _occupiedCells = new();

        private void Awake()
        {
            if (grid == null) grid = GetComponentInParent<Grid>();

            if (grid == null) Debug.LogError("Grid가 할당되지 않았습니다.", this);
        }

        private void Start()
        {
            InitializeMapPieces();
        }

        private void InitializeMapPieces()
        {
            _occupiedCells.Clear();

            MapPiece[] pieces = GetComponentsInChildren<MapPiece>(true);

            foreach (MapPiece piece in pieces)
            {
                if (piece.Data == null) continue;

                Vector2Int gridPos = WorldToCell(piece.transform.position);
                
                if (TryDeployment(gridPos, piece.Data))
                {
                    piece.SetDeployed(gridPos);
                }
                else
                {
                    Debug.LogWarning($"초기 배치 실패: {piece.name}, 위치: {gridPos}", piece);
                }
            }
        }

        public Vector2Int WorldToCell(Vector3 worldPos)
        {
            Vector3Int pos = grid.WorldToCell(worldPos);
            return new Vector2Int(pos.x, pos.y);
        }

        public Vector3 CellToWorld(Vector2Int pos)
        {
            return grid.CellToWorld(new Vector3Int(pos.x, pos.y, 0));
        }

        // 배치 가능 여부
        public bool CanDeployment(Vector2Int pos, MapPieceSo data)
        {
            if (data == null || data.OccupiedCells.Count == 0) return false;

            foreach (Vector2Int cell in data.OccupiedCells)
            {
                if (_occupiedCells.Contains(cell + pos)) return false;
            }

            return true;
        }

        // 배치
        public bool TryDeployment(Vector2Int pos, MapPieceSo data)
        {
            if (!CanDeployment(pos, data)) return false;

            foreach (Vector2Int cell in data.OccupiedCells) _occupiedCells.Add(cell + pos);

            return true;
        }

        // 제거
        public void RemoveDeployment(Vector2Int pos, MapPieceSo data)
        {
            if (data == null) return;

            foreach (Vector2Int cell in data.OccupiedCells) _occupiedCells.Remove(cell + pos);
        }
    }
}