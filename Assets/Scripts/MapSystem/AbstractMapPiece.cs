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
        private bool _wasOnPlayer;

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

        private Tilemap _tilemaps;
        private Color _originalColors;
        private Collider2D _collider;

        protected virtual void Awake()
        {
            _tilemaps = GetComponentInChildren<Tilemap>();
            _collider = GetComponent<Collider2D>();

            OccupiedCells = data.OccupiedCells;

            _originalColors = _tilemaps.color;
        }

        public void Initialize(IMapGrid mapGrid)
        {
            _mapGrid = mapGrid;
            GridPosition = _mapGrid.WorldToCell(transform.position);
        }

        public bool CanDeployment(Vector2Int pos, bool ignorePlayer = false)
        {
            if (_mapGrid == null || !IsActive) return false;

            return _mapGrid.CanDeployment(pos, OccupiedCells, ignorePlayer || (IsDragging ? _wasOnPlayer : OnPlayer));
        }

        public bool TryDeployment(Vector2Int pos)
        {
            if (_mapGrid == null || !IsActive) return false;

            if (IsDeployed) return GridPosition == pos;

            if (!_mapGrid.TryDeployment(pos, OccupiedCells, IsDragging ? _wasOnPlayer : OnPlayer)) return false;

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
            if (dragging && !IsDragging) _wasOnPlayer = OnPlayer;

            IsDragging = dragging;

            if (!IsDragging) _wasOnPlayer = false;

            if (_collider != null) _collider.enabled = IsActive && !IsDragging;

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

            _tilemaps.color = color;
        }

        public void ResetPreview()
        {
            if (_tilemaps == null) return;

            Color color = _originalColors;

            if (!IsActive || IsDragging) color.a = DISABLE_COLOR_ALPHA;

            _tilemaps.color = color;
        }

        private void OnDisable()
        {
            RemoveDeployment();
            IsDragging = false;
            _wasOnPlayer = false;
        }

        private void OnValidate()
        {
            _tilemaps = GetComponentInChildren<Tilemap>();

            if (_tilemaps == null) return;

            _tilemaps.color = Fixed && defaultColor == Color.white ? FixedPieceColor : defaultColor;
        }

        public void SetPieceActive(bool active)
        {
            if (!active)
                RemoveDeployment();

            IsActive = active;
            if (_collider != null) _collider.enabled = IsActive && !IsDragging;

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