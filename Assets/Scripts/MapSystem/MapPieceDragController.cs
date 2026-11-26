using System;
using Agents.Players;
using Command;
using Common.Attributes;
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
        [ReadOnly, SerializeField] private Camera targetCamera;

        [Header("Detection")]
        [SerializeField] private LayerMask pieceLayer;

        [Header("Grid Settings")]
        [SerializeField, Min(1)] private int sellSize = 2;

        [Header("Placement")]
        [SerializeField, Min(0)] private int searchRadius = 5;

        [Header("So")] 
        [SerializeField] private EventChannelSO playerFreezeControllerEvent;
        [SerializeField] private EventChannelSO movementControllerRegisteredEvent;
        [SerializeField] private EventChannelSO commandInvokeEvent;
        [SerializeField] private PlayerInputSO inputSo;

        private IMapPiece _selectedPiece;

        private Vector3 _originalWorldPosition;
        private Vector2Int _originalGridPosition;
        private Vector2Int _grabCellOffset;
        private Vector2Int _targetPosition;
        private Vector2Int _lastValidPosition;

        private bool _wasDeployed;
        private bool _canPlace;

        private IControlMovement _controlMovement;

        private IMapGrid mapGrid;

        private void Awake()
        {
            targetCamera = Camera.main;
            mapGrid = mapGridObject.GetComponent<IMapGrid>();
        }

        private void Start()
        {
            EndDrag();
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
            if (_controlMovement == null || _controlMovement.PlayerIsStop()) return;
            if (_selectedPiece != null) return;

            Vector3 mouseWorld = GetMouseWorldPosition();

            Collider2D hit = Physics2D.OverlapPoint(mouseWorld, pieceLayer);

            if (hit == null) return;

            IMapPiece piece = hit.GetComponentInParent<IMapPiece>();

            if (piece?.OccupiedCells == null || piece.Fixed || !piece.IsActive) return;

            _selectedPiece = piece;

            _originalWorldPosition = piece.Transform.position;
            _originalGridPosition = piece.GridPosition;
            _wasDeployed = piece.IsDeployed;

            Vector2Int mouseCell = mapGrid.WorldToCell(mouseWorld);

            Vector2Int pieceCell = mapGrid.WorldToCell(piece.Transform.position);

            _grabCellOffset = mouseCell - pieceCell;

            _selectedPiece.RemoveDeployment();

            _lastValidPosition = pieceCell;
            _targetPosition = _lastValidPosition;

            // 드래그 시작
            _selectedPiece.SetDragging(true);
            _selectedPiece.AllFreeze();

            playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(false));

            UpdateDrag();
        }

        private void UpdateDrag()
        {
            Vector2Int mouseCell = mapGrid.WorldToCell(GetMouseWorldPosition());

            Vector2Int targetCell = mouseCell - _grabCellOffset;

            Vector2Int snappedPos = SnapToGrid(targetCell);

            _canPlace = TryFindNearestPosition(snappedPos, out _targetPosition);

            if (!_canPlace) _targetPosition = _lastValidPosition;
            else _lastValidPosition = _targetPosition;

            Vector3 worldPos = mapGrid.CellToWorld(_targetPosition);

            worldPos.z = _originalWorldPosition.z;

            _selectedPiece.Transform.position = worldPos;
            _selectedPiece.SetPreview(_canPlace);
        }

        private void EndDrag()
        {
            if (_selectedPiece == null)
            {
                playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
                return;
            }

            bool canPlace = _canPlace && _selectedPiece.CanDeployment(_targetPosition);

            bool willBeDeployed = canPlace || _wasDeployed;

            Vector2Int finalGridPosition = canPlace ? _targetPosition : _originalGridPosition;

            Vector3 finalWorldPosition = canPlace ? mapGrid.CellToWorld(finalGridPosition) : _originalWorldPosition;

            finalWorldPosition.z = _originalWorldPosition.z;

            _selectedPiece.Transform.position = _originalWorldPosition;

            bool hasChanged = finalWorldPosition != _originalWorldPosition || willBeDeployed != _wasDeployed;

            if (hasChanged)
            {
                ICommand command = new MoveMapPieceCommand(_selectedPiece, mapGrid, _originalGridPosition, finalGridPosition, 
                    _originalWorldPosition, finalWorldPosition, _wasDeployed, willBeDeployed);

                CommandInvokeEventStruct eventData = new()
                {
                    Action = CommandActionType.Execute,
                    Command = command
                };

                commandInvokeEvent.RaiseEvent(CreateEvents.CommandInvokeEvent.InitData(eventData));
            }
            else if (_wasDeployed)
            {
                if (!_selectedPiece.TryDeployment(_originalGridPosition))
                    Debug.LogError("기존 파츠 배치 복구에 실패했습니다.");
            }

            _selectedPiece.SetDragging(false);
            _selectedPiece.ResetPreview();
            _selectedPiece.AllUnFreeze();

            playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));

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
    result = _lastValidPosition;

    if (_selectedPiece == null) return false;

    int size = Mathf.Max(1, sellSize);

    while (result != targetPos)
    {
        Vector2Int direction = targetPos - result;

        int moveX = Mathf.Clamp(direction.x, -size, size);
        int moveY = Mathf.Clamp(direction.y, -size, size);

        Vector2Int horizontal = result + new Vector2Int(moveX, 0);
        Vector2Int vertical = result + new Vector2Int(0, moveY);
        Vector2Int diagonal = result + new Vector2Int(moveX, moveY);

        bool canMoveX = moveX != 0 && CanMovePath(result, horizontal);
        bool canMoveY = moveY != 0 && CanMovePath(result, vertical);

        if (canMoveX && canMoveY)
        {
            if (CanMovePath(horizontal, diagonal) &&
                CanMovePath(vertical, diagonal))
            {
                result = diagonal;
                continue;
            }
        }

        if (canMoveX && canMoveY)
        {
            result = Mathf.Abs(direction.x) >= Mathf.Abs(direction.y)
                ? horizontal
                : vertical;

            continue;
        }

        if (canMoveX)
        {
            result = horizontal;
            continue;
        }

        if (canMoveY)
        {
            result = vertical;
            continue;
        }

        break;
    }

    return _selectedPiece.CanDeployment(result);
}

private bool CanMovePath(Vector2Int startPos, Vector2Int targetPos)
{
    Vector2Int direction = targetPos - startPos;

    int steps = Mathf.Max(Mathf.Abs(direction.x), Mathf.Abs(direction.y));

    for (int i = 1; i <= steps; i++)
    {
        float t = (float)i / steps;

        Vector2Int candidate = new Vector2Int(
            Mathf.RoundToInt(Mathf.Lerp(startPos.x, targetPos.x, t)),
            Mathf.RoundToInt(Mathf.Lerp(startPos.y, targetPos.y, t)));

        if (!_selectedPiece.CanDeployment(candidate)) return false;
    }

    return true;
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

            if (_wasDeployed)
            {
                if (!_selectedPiece.TryDeployment(_originalGridPosition))
                    Debug.LogError("기존 파츠 배치 복구에 실패했습니다.");
            }

            _selectedPiece.SetDragging(false);
            _selectedPiece.ResetPreview();
            _selectedPiece.AllUnFreeze();

            playerFreezeControllerEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));

            _selectedPiece = null;
        }

        private void HandleSetControlMovement(ControlMovementEvent obj)
        {
            _controlMovement = obj.Value;
        }

        private void OnValidate()
        {
            if (mapGridObject == null) return;

            if (mapGridObject.TryGetComponent(out IMapGrid _)) return;

            Debug.LogError("할당한 오브젝트에 IMapGrid가 없습니다.");
            mapGridObject = null;
        }
    }
}