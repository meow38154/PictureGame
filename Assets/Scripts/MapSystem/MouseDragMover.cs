using System;
using Agents.Players;
using UnityEngine;

namespace MapSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MouseDragMover : MonoBehaviour
    {
        [SerializeField] private int tileSize = 2;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private Collider2D clickCollider;
        [SerializeField] private PlayerInputSo playerInputSo;

        private Rigidbody2D _rigidbody;

        private Vector2 _targetPosition;
        private Vector2 _previousPosition;
        private Vector2 _grabOffset;

        private bool _isDragging;

        public event Action<Vector2> OnMoved;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _targetPosition = _rigidbody.position;
            _previousPosition = _rigidbody.position;

            _rigidbody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void OnEnable()
        {
            SubscribeInputEvents();
        }

        private void OnDisable()
        {
            UnsubscribeInputEvents();
        }

        private void Update()
        {
            if (!_isDragging)
                return;

            UpdateTargetPosition();
        }

        private void FixedUpdate()
        {
            NotifyMovement();

            if (!_isDragging)
                return;

            MoveToTarget();
        }

        private void SubscribeInputEvents()
        {
            playerInputSo.OnMouseLeftButtonPressed += HandleMouseDown;
            playerInputSo.OnMouseLeftButtonReleased += HandleMouseUp;
        }

        private void UnsubscribeInputEvents()
        {
            playerInputSo.OnMouseLeftButtonPressed -= HandleMouseDown;
            playerInputSo.OnMouseLeftButtonReleased -= HandleMouseUp;
        }

        private void HandleMouseDown()
        {
            if (!IsMouseOverCollider())
                return;

            StartDragging();
        }

        private void HandleMouseUp()
        {
            if (!_isDragging)
                return;

            StopDragging();
        }

        private void StartDragging()
        {
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.gravityScale = 0f;
            _rigidbody.freezeRotation = true;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;

            Vector2 mousePosition = GetMouseWorldPosition();

            _grabOffset = _rigidbody.position - mousePosition;

            _previousPosition = _rigidbody.position;
            _targetPosition = mousePosition + _grabOffset;

            _isDragging = true;
        }

        private void StopDragging()
        {
            _isDragging = false;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;

            CorrectionPosition();

            _previousPosition = _rigidbody.position;

            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }

        private void UpdateTargetPosition()
        {
            _targetPosition = GetMouseWorldPosition() + _grabOffset;
        }

        private void MoveToTarget()
        {
            Vector2 nextPosition = Vector2.Lerp(
                _rigidbody.position,
                _targetPosition,
                moveSpeed * Time.fixedDeltaTime
            );

            _rigidbody.MovePosition(nextPosition);
        }

        private void NotifyMovement()
        {
            Vector2 currentPosition = _rigidbody.position;
            Vector2 delta = currentPosition - _previousPosition;

            _previousPosition = currentPosition;

            if (delta.sqrMagnitude <= Mathf.Epsilon)
                return;

            OnMoved?.Invoke(delta);
        }

        private bool IsMouseOverCollider()
        {
            if (clickCollider == null)
                return false;

            return clickCollider.OverlapPoint(
                GetMouseWorldPosition()
            );
        }

        private Vector2 GetMouseWorldPosition()
        {
            return Utility.GetMouseWorldPosition();
        }

        private void CorrectionPosition()
        {
            Vector2 previousPosition = _rigidbody.position;

            Vector2 correctedPosition = new Vector2(
                Mathf.Round(previousPosition.x / tileSize) * tileSize,
                Mathf.Round(previousPosition.y / tileSize) * tileSize
            );

            _rigidbody.position = correctedPosition;

            Vector2 delta = correctedPosition - previousPosition;

            if (delta.sqrMagnitude > Mathf.Epsilon)
                OnMoved?.Invoke(delta);
        }
    }
}