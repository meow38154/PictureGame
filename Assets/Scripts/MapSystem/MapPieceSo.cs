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
        [SerializeField] private Tilemap mapPrefabTilemap;

        //에셋에 저장, 수정은 불가
        [SerializeField] private List<Vector2Int> occupiedCells = new();
        public IReadOnlyList<Vector2Int> OccupiedCells => occupiedCells;
        
        private void OnValidate()
        {
            TileDataRegister();
        }

        public void TileDataRegister()
        {
            //리스트 초기화
            occupiedCells.Clear();

            if (mapPrefabTilemap == null) return;
            
            if (mapPrefabTilemap == null) return;

            foreach (var pos in mapPrefabTilemap.cellBounds.allPositionsWithin)
            {
                //그 부분에 타일 없으면 넘기기
                if (!mapPrefabTilemap.HasTile(pos)) continue;
                
                occupiedCells.Add(new Vector2Int(pos.x, pos.y));
            }
        }
    }
}