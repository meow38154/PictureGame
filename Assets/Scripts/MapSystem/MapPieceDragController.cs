using Agents.Players;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MapSystem
{
    public class MapPieceDragController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject mapGridObject;
        [SerializeField] private Camera targetCamera;

        [Header("Detection")]
        [SerializeField] private LayerMask pieceLayer;

        [Header("Grid Settings")]
        [SerializeField, Min(1)] private int sellSize = 2;

        [Header("Placement")]
        [SerializeField, Min(0)] private int searchRadius = 5;

        [Header("So")] 
        [SerializeField] private EventChannelSO dragStateChangedEvent;
        [SerializeField] private EventChannelSO movementControllerRegisteredEvent;
        [SerializeField] private PlayerInputSo inputSo;
        
        private IMapPiece _selectedPiece;

        private Vector3 _originalWorldPosition;
        private Vector2Int _originalGridPosition;
        private Vector2Int _grabCellOffset;
        private Vector2Int _targetPosition;

        private bool _wasDeployed;
        private bool _canPlace;
        
        private IControlMovement _controlMovement;

        private IMapGrid mapGrid;
        
        private void Awake()
        {
            targetCamera = Camera.main;
            mapGrid = mapGridObject.GetComponent<IMapGrid>();
        }

        private void Update()
        {
            Mouse mouse = Mouse.current;

            if (mouse == null || targetCamera == null || mapGrid == null) return;

            if (mouse.leftButton.wasPressedThisFrame) BeginDrag();

            if (_selectedPiece == null) return;

            if (mouse.leftButton.isPressed) UpdateDrag();

            if (mouse.leftButton.wasReleasedThisFrame) EndDrag();
        }

        private Vector3 GetMouseWorldPosition()
        {
            return Utility.GetMouseWorldPosition();
        }

        private void BeginDrag()
        {
            if (_controlMovement.PlayerIsStop()) return;
            
            Vector3 mouseWorld = GetMouseWorldPosition();

            Collider2D hit = Physics2D.OverlapPoint(mouseWorld, pieceLayer);

            if (hit == null) return;
            
            IMapPiece piece = hit.GetComponentInParent<IMapPiece>();

            if (piece == null || piece.Data == null || piece.Fixed) return;

            inputSo.CanMovement = false;
            
            _selectedPiece = piece;

            _originalWorldPosition = piece.Transform.position;
            _originalGridPosition = piece.GridPosition;
            _wasDeployed = piece.IsDeployed;

            Vector2Int mouseCell = mapGrid.WorldToCell(mouseWorld);

            Vector2Int pieceCell = mapGrid.WorldToCell(piece.Transform.position);

            _grabCellOffset = mouseCell - pieceCell;

            if (_wasDeployed)
            {
                mapGrid.RemoveDeployment(_originalGridPosition, piece.Data);

                piece.ClearDeployed();
            }

            // 드래그 시작
            _selectedPiece.SetDragging(true);
            _selectedPiece.SetPieceActive(false);
            
            dragStateChangedEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(false));

            UpdateDrag();
        }

        private void UpdateDrag()
        {
            Vector2Int mouseCell = mapGrid.WorldToCell(GetMouseWorldPosition());

            Vector2Int targetCell = mouseCell - _grabCellOffset;

            Vector2Int snappedPos = SnapToGrid(targetCell);

            _canPlace = TryFindNearestPosition(snappedPos, out _targetPosition);

            if (!_canPlace) _targetPosition = snappedPos;

            Vector3 worldPos = mapGrid.CellToWorld(_targetPosition);

            worldPos.z = _originalWorldPosition.z;

            _selectedPiece.Transform.position = worldPos;
            _selectedPiece.SetPreview(_canPlace);
        }

        private void EndDrag()
        {
            if (_canPlace && mapGrid.TryDeployment(_targetPosition, _selectedPiece.Data))
            {
                _selectedPiece.SetDeployed(_targetPosition);
            }
            else
            {
                _selectedPiece.Transform.position = _originalWorldPosition;

                if (_wasDeployed && mapGrid.TryDeployment(_originalGridPosition, _selectedPiece.Data))
                {
                    _selectedPiece.SetDeployed(_originalGridPosition);
                }
            }

            _selectedPiece.ResetPreview();

            // 드래그 종료
            _selectedPiece.SetDragging(false);
            _selectedPiece.SetPieceActive(true);
            dragStateChangedEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
            
            inputSo.CanMovement = true;
            
            _selectedPiece = null;
        }

        private Vector2Int SnapToGrid(Vector2Int position)
        {
            int size = Mathf.Max(1, sellSize);

            return new Vector2Int(
                Mathf.RoundToInt((float)position.x / size) * size, 
                Mathf.RoundToInt((float)position.y / size) * size);
        }

        private bool TryFindNearestPosition(Vector2Int targetPos, out Vector2Int result)
        {
            result = targetPos;

            if (_selectedPiece == null) return false;
            
            MapPieceSo data = _selectedPiece.Data;

            if (mapGrid.CanDeployment(targetPos, data)) return true;

            int size = Mathf.Max(1, sellSize);

            float nearestDistance = float.MaxValue;
            bool found = false;

            for (int x = -searchRadius; x <= searchRadius; x++)
            {
                for (int y = -searchRadius; y <= searchRadius; y++)
                {
                    Vector2Int candidate = targetPos + new Vector2Int(x * size, y * size);

                    if (!mapGrid.CanDeployment(candidate, data)) continue;

                    float distance = (candidate - targetPos).sqrMagnitude;

                    if (distance >= nearestDistance) continue;

                    nearestDistance = distance;
                    result = candidate;
                    found = true;
                }
            }

            return found;
        }

        private void OnEnable()
        {
            movementControllerRegisteredEvent.AddListener<ControlMovementEvent>(HandleSetControlMovement);
        }

        private void OnDisable()
        {
            movementControllerRegisteredEvent.RemoveListener<ControlMovementEvent>(HandleSetControlMovement);
            
            if (_selectedPiece == null) return;

            _selectedPiece.Transform.position = _originalWorldPosition;
            _selectedPiece.ResetPreview();

            if (_wasDeployed)
            {
                if (mapGrid != null && mapGrid.TryDeployment(_originalGridPosition, _selectedPiece.Data))
                {
                    _selectedPiece.SetDeployed(_originalGridPosition);
                }
            }

            _selectedPiece.SetDragging(false);
            _selectedPiece = null;
        }
        
        private void HandleSetControlMovement(ControlMovementEvent obj)
        {
            _controlMovement = obj.Value;
        }

        private void OnValidate()
        {
            if (mapGridObject.TryGetComponent(out IMapGrid _)) return;
            
            Debug.LogError("할당한 오브젝트에 IMapGrid가 없습니다.");
            mapGridObject = null;
        }
    }
}
