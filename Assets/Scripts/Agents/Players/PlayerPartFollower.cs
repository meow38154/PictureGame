using DevLib.ModuleSystem;
using MapSystem;
using UnityEngine;

namespace Agents.Players
{
    public class PlayerPartFollower : Module
    {
        [SerializeField] private PlayerPartDetector partDetector;
        [SerializeField] private Rigidbody2D playerRb;

        private MouseDragMover _currentMover;

        private void OnEnable()
        {
            partDetector.OnPartChangeEvent += HandlePartChanged;
        }

        private void OnDisable()
        {
            partDetector.OnPartChangeEvent -= HandlePartChanged;
            UnsubscribeCurrentMover();
        }

        private void HandlePartChanged(IMapPart part)
        {
            UnsubscribeCurrentMover();

            if (part == null)
                return;

            _currentMover = part.Collider.GetComponentInParent<MouseDragMover>();

            if (_currentMover == null)
                return;

            _currentMover.OnMoved += HandlePartMoved;
        }

        private void HandlePartMoved(Vector2 delta)
        {
            playerRb.position += delta;
        }

        private void UnsubscribeCurrentMover()
        {
            if (_currentMover == null)
                return;

            _currentMover.OnMoved -= HandlePartMoved;
            _currentMover = null;
        }
    }
}