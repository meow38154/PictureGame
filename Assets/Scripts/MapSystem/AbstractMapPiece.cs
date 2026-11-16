using System.Collections.Generic;
using Agents;
using Common.Attributes;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    /// <summary>
    /// 
    /// </summary>
   
    public abstract class AbstractMapPiece : MonoBehaviour, IMapPiece
    {
        private readonly Color NotCanDeploymentColor = new Color(1f, 0.3f, 0.3f, 0.65f);
        private readonly Color CanDeploymentColor = new Color(0.4f, 1f, 0.6f, 0.65f);
        private readonly Color FixedPieceColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private const float DISABLE_COLOR_ALPHA = 0.5f;

        [SerializeField] private MapPieceSo data;
        [SerializeField] private string pieceName;
        
        [field: SerializeField] public bool Fixed { get; private set; }
        public Transform Transform => transform;
        

        public bool IsDeployed { get; private set; }
        public bool IsDragging { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Vector2Int GridPosition { get; private set; }
        public IReadOnlyList<Vector2Int> OccupiedCells { get; private set; }

        private Tilemap _tilemaps;
        private Color _originalColors;
        private Collider2D _collider;

        private bool _active;

        protected virtual void Awake()
        {
            SetDeployed(Vector2Int.RoundToInt(transform.position));
            _tilemaps = GetComponentInChildren<Tilemap>();
            _collider = GetComponent<Collider2D>();

            OccupiedCells = data.OccupiedCells;
            
            _originalColors = _tilemaps.color;
        }

        public void SetDragging(bool dragging)
        {
            IsDragging = dragging;
        }

        public void SetDeployed(Vector2Int pos)
        {
            GridPosition = pos;
            IsDeployed = true;
        }

        public void ClearDeployed()
        {
            IsDeployed = false;
        }

        public void SetPreview(bool valid)
        {
            Color color = valid ? CanDeploymentColor : NotCanDeploymentColor;

            _tilemaps.color = color;
        }

        public void ResetPreview()
        { 
            _tilemaps.color = _originalColors;
        }

        private void OnDisable()
        {
            IsDragging = false;
        }

        private void OnValidate()
        {
            _tilemaps = GetComponentInChildren<Tilemap>();
            
            _tilemaps.color =  Fixed ? FixedPieceColor : Color.white;
        }

        public void SetPieceActive(bool active)
        {
            if (_tilemaps == null) return;

            Color originalColor = _tilemaps.color;
            _tilemaps.color = new Color(originalColor.r, originalColor.g, originalColor.b, active ? 1f : DISABLE_COLOR_ALPHA);
            _collider.enabled = active;
            IsActive = active;
        }

        public void AllFreeze()
        {
            var freezies = GetComponentsInChildren<IAgentFreezer>();

            foreach (IAgentFreezer freeze in freezies)
            {
                freeze.Freeze();
            }
        }

        public void AllUnFreeze()
        {
            var freezies = GetComponentsInChildren<IAgentFreezer>();

            foreach (IAgentFreezer freeze in freezies)
            {
                freeze.UnFreeze();
            }
        }

        [ContextMenu("Update File")]
        public void PrefabDataRefresh()
        {
            if (data == null || _tilemaps == null) return;
            
            data.TileDataRegister(_tilemaps);
        }
        
        [ContextMenu("Create File")]
        public void CreateMapPieceSo()
        {
#if UNITY_EDITOR
            _tilemaps = GetComponentInChildren<Tilemap>();

            if (_tilemaps == null) return;

            MapPieceSo mapPieceSo = ScriptableObject.CreateInstance<MapPieceSo>();

            mapPieceSo.TileDataRegister(_tilemaps);

            string folderPath = $"Assets/GameModules/Piece/{SceneManager.GetActiveScene().name}";

            System.IO.Directory.CreateDirectory(folderPath);

            string path = AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{(string.IsNullOrEmpty(pieceName) ? gameObject.name : pieceName)}.asset");

            AssetDatabase.CreateAsset(mapPieceSo, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Undo.RecordObject(this, "Assign MapPieceSo");

            data = mapPieceSo;

            EditorUtility.SetDirty(this);

            Debug.Log($"MapPieceSo Created: {path}");
#endif
        }
    }
}