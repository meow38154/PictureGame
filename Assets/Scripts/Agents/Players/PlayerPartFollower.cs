using System.Collections;
using System.Collections.Generic;
using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerPartFollower : Module, IPartFollower
    {
        [field: SerializeField] public bool IsActive { get; set; } = true;

        [SerializeField] private float partReleaseDelay = 0.1f;
        [SerializeField] private float partEnterDelay = 0.1f;

        private IGroundChecker _groundChecker;
        private Rigidbody2D _playerRb;
        private IMapPiece _currentPart;

        private Coroutine _enterCoroutine;

        private readonly Dictionary<IMapPiece, Coroutine> _releaseCoroutines = new();

        public IMapPiece CurrentPart => _currentPart;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);

            _playerRb = owner.GetComponent<Rigidbody2D>();
            _groundChecker = owner.GetModule<IGroundChecker>();

            Debug.Assert(_playerRb != null);
            Debug.Assert(_groundChecker != null);
        }

        private void FixedUpdate()
        {
            if (_playerRb == null || _groundChecker == null) return;

            if (_currentPart is { IsDragging: true }) return;

            if (_playerRb.linearVelocityY > 0.05f)
            {
                SetCurrentPart(null);
                return;
            }

            SetCurrentPart(FindGroundPart());
        }

        private IMapPiece FindGroundPart()
        {
            Collider2D col = _groundChecker.CurrentGround;

            return col != null ? col.GetComponentInParent<IMapPiece>() : null;
        }

        private void SetCurrentPart(IMapPiece nextPart)
        {
            if (_currentPart == nextPart || !IsActive) return;

            if (_enterCoroutine != null)
            {
                StopCoroutine(_enterCoroutine);
                _enterCoroutine = null;
            }

            if (_currentPart != null)
            {
                IMapPiece previousPart = _currentPart;

                if (_releaseCoroutines.TryGetValue(previousPart, out Coroutine coroutine)) StopCoroutine(coroutine);

                _releaseCoroutines[previousPart] = StartCoroutine(ReleaseOnPlayer(previousPart));
            }

            _currentPart = nextPart;

            bool isNull = _currentPart == null;

            if (!isNull)
            {
                if (_releaseCoroutines.TryGetValue(_currentPart, out Coroutine coroutine))
                {
                    StopCoroutine(coroutine);
                    _releaseCoroutines.Remove(_currentPart);
                }

                if (!_currentPart.OnPlayer) _enterCoroutine = StartCoroutine(EnterOnPlayer(_currentPart));
            }

            _owner.transform.SetParent(isNull ? null : _currentPart.Transform, true);
        }

        private IEnumerator EnterOnPlayer(IMapPiece part)
        {
            yield return new WaitForSeconds(partEnterDelay);

            if (part == _currentPart) part.OnPlayer = true;

            _enterCoroutine = null;
        }

        private IEnumerator ReleaseOnPlayer(IMapPiece part)
        {
            yield return new WaitForSeconds(partReleaseDelay);

            if (part != _currentPart) part.OnPlayer = false;

            _releaseCoroutines.Remove(part);
        }
    }
}