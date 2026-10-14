using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapSystem
{
    /// <summary>
    /// 
    /// </summary>
    
    public class MapPiece : MonoBehaviour
    {
        private static readonly Color NotCanDeploymentColor = new Color(1f, 0.3f, 0.3f, 0.65f);
        private static readonly Color CanDeploymentColor = new Color(0.4f, 1f, 0.6f, 0.65f);
        private static readonly Color FixedPieceColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        [field: SerializeField] public MapPieceSo Data { get; private set; }
        [field: SerializeField] public bool Fixed { get; private set; }

        //플레이어가 따라갈 수 있게 Vector2 보내는 용도
        public event Action<Vector2> OnMoved;

        public bool IsDeployed { get; private set; }
        public bool IsDragging { get; private set; }
        public Vector2Int GridPosition { get; private set; }

        private Tilemap _tilemaps;
        private Color _originalColors;

        private void Awake()
        {
            SetDeployed(Vector2Int.RoundToInt(transform.position));
            _tilemaps = GetComponentInChildren<Tilemap>();
            

            _originalColors = _tilemaps.color;
        }

        public void MoveTo(Vector3 position)
        {
            Vector2 delta = position - transform.position;

            transform.position = position;

            if (delta.sqrMagnitude > 0f) OnMoved?.Invoke(delta);
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
    }
}