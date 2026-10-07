using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    /// <summary>
    /// 맵 파츠 데이터를 저장하는 So입니다.
    /// </summary>
    
    [CreateAssetMenu(fileName = "base MapPiece So", menuName = "Map/Piece", order = 0)]
    public class MapPieceSo : ScriptableObject
    {
        [SerializeField] private GameObject mapPrefab;

        //에셋에 저장, 수정은 불가
        [SerializeField, HideInInspector] private List<Vector2Int> occupiedCells = new();
        public IReadOnlyList<Vector2Int> OccupiedCells => occupiedCells;
        
        private void OnValidate()
        {
            //리스트 초기화
            occupiedCells.Clear();
            
            Tilemap tilemap = mapPrefab.GetComponentInChildren<Tilemap>();
            
            if (tilemap == null) return;

            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                //그 부분에 타일 없으면 넘기기
                if (!tilemap.HasTile(pos))
                    continue;
                
                occupiedCells.Add(new Vector2Int(pos.x, pos.y));
            }
        }
    }
}