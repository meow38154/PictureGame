using System;
using System.Collections.Generic;
using Agents;
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

        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private MapPieceSo data;
        [SerializeField] private string pieceName;

        private bool _onPlayer;

        public event Action OnPlayerLand;
        public event Action OnPlayerTakeOff;

        public bool OnPlayer
        {
            get => _onPlayer;
            set
            {
                if (_onPlayer == value) return;

                _onPlayer = value;

                if (_onPlayer) OnPlayerLand?.Invoke();
                else OnPlayerTakeOff?.Invoke();
            }
        }

        [field: SerializeField] public bool Fixed { get; private set; }
        public Transform Transform => transform;


        public bool IsDeployed { get; private set; }
        public bool IsDragging { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Vector2Int GridPosition { get; private set; }
        public IReadOnlyList<Vector2Int> OccupiedCells { get; private set; }

        private IMapGrid _mapGrid;

        protected Tilemap Tilemaps;
        private Color _originalColors;
        private Collider2D _collider;

        protected virtual void Awake()
        {
            Tilemaps = GetComponentInChildren<Tilemap>();
            _collider = GetComponent<Collider2D>();

            OccupiedCells = data.OccupiedCells;

            _originalColors = Tilemaps.color;
        }

        public void Initialize(IMapGrid mapGrid)
        {
            _mapGrid = mapGrid;
            GridPosition = _mapGrid.WorldToCell(transform.position);
        }

        public bool CanDeployment(Vector2Int pos)
        {
            if (_mapGrid == null || !IsActive) return false;

            return _mapGrid.CanDeployment(pos, OccupiedCells);
        }

        public bool TryDeployment(Vector2Int pos)
        {
            if (_mapGrid == null || !IsActive) return false;

            if (IsDeployed) return GridPosition == pos;

            if (!_mapGrid.TryDeployment(pos, OccupiedCells)) return false;

            SetDeployed(pos);

            return true;
        }

        public void RemoveDeployment()
        {
            if (_mapGrid == null || !IsDeployed) return;

            _mapGrid.RemoveDeployment(GridPosition, OccupiedCells);

            ClearDeployed();
        }

        public void SetDragging(bool dragging)
        {
            IsDragging = dragging;

            if (_collider != null)
                _collider.enabled = IsActive && !IsDragging;

            ResetPreview();
        }

        private void SetDeployed(Vector2Int pos)
        {
            GridPosition = pos;
            IsDeployed = true;
        }

        private void ClearDeployed()
        {
            IsDeployed = false;
        }

        public void SetPreview(bool valid)
        {
            Color color = valid ? CanDeploymentColor : NotCanDeploymentColor;

            Tilemaps.color = color;
        }

        public void ResetPreview()
        {
            if (Tilemaps == null) return;

            Color color = _originalColors;

            if (!IsActive || IsDragging)
                color.a = DISABLE_COLOR_ALPHA;

            Tilemaps.color = color;
        }

        private void OnDisable()
        {
            RemoveDeployment();
            IsDragging = false;
        }

        private void OnValidate()
        {
            Tilemaps = GetComponentInChildren<Tilemap>();

            if (Tilemaps == null) return;

            Tilemaps.color = Fixed && defaultColor == Color.white ? FixedPieceColor : defaultColor;
        }

        public void SetPieceActive(bool active)
        {
            if (!active)
                RemoveDeployment();

            IsActive = active;

            if (_collider != null)
                _collider.enabled = IsActive && !IsDragging;

            ResetPreview();
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
            if (data == null || Tilemaps == null) return;

            data.TileDataRegister(Tilemaps);
        }

        [ContextMenu("Create File")]
        public void CreateMapPieceSo()
        {
#if UNITY_EDITOR
            Tilemaps = GetComponentInChildren<Tilemap>();

            if (Tilemaps == null) return;

            MapPieceSo mapPieceSo = ScriptableObject.CreateInstance<MapPieceSo>();

            mapPieceSo.TileDataRegister(Tilemaps);

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