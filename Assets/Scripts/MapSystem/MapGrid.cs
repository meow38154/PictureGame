using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapSystem
{
    /// <summary>
    /// 맵의 전체적인 셀 그리드를 담당하는 클래스입니다.
    /// </summary>
    
    public class MapGrid : MonoBehaviour
    {
        //채워진 셀 위치 (중복 방지를 위해 HashSet 사용)
        private HashSet<Vector2Int> _occupiedCells = new();

        //배치
        public void Deployment(Vector2Int pos, MapPieceSo mapPieceSo)
        {
            if (!CanDeployment(pos,  mapPieceSo)) return;
            
            foreach (var pieceSell in mapPieceSo.OccupiedCells)
            {
                _occupiedCells.Add(pieceSell + pos);
            }
        }

        //삭제
        public void RemoveDeployment(Vector2Int pos, MapPieceSo mapPieceSo)
        {
            foreach (var pieceSell in mapPieceSo.OccupiedCells)
            {
                if (!_occupiedCells.Contains(pieceSell + pos)) continue; 
                _occupiedCells.Remove(pieceSell + pos);
            }
        }
        
        //배치 가능한지 확인
        private bool CanDeployment(Vector2Int pos, MapPieceSo mapPieceSo)
        {
            return mapPieceSo.OccupiedCells.All(sellPos => !_occupiedCells.Contains(sellPos + pos));
        }
    }
}