
using System;
using DevLib.ModuleSystem;
using UnityEngine;

namespace Agents
{
    public enum CheckerShape
    {
        Square,
        Circle
    }

    public class GroundChecker : Module, IGroundChecker
    {
        [Header("Base")]
        [SerializeField] private CheckerShape checkerShape;
        [SerializeField] private bool gizmosEnabled = true;
        [SerializeField] private Vector2 checkerOffset = Vector2.zero;
        [SerializeField] private LayerMask whatIsGround;

        [Header("Square")]
        [SerializeField] private Vector2 squareSize = Vector2.one;

        [Header("Circle")]
        [SerializeField] private float circleRadius = 1f;

        public Collider2D CurrentGround { get; private set; }

        public event Action<Collider2D> OnGroundEnter;
        public event Action<Collider2D> OnGroundExit;

        //땅 감지가 물리니까 Fixed로 실행
        private void FixedUpdate()
        {
            UpdateGround();
        }

        private void UpdateGround()
        {
            Collider2D nextGround = DetectGround();

            if (CurrentGround == nextGround) return;

            Collider2D previousGround = CurrentGround;
            CurrentGround = nextGround;

            if (previousGround != null) OnGroundExit?.Invoke(previousGround);

            if (nextGround != null) OnGroundEnter?.Invoke(nextGround);
        }

        public bool IsGroundChecking()
        {
            return DetectGround() != null;
        }

        private Collider2D DetectGround()
        {
            Vector2 position = (Vector2)transform.position + checkerOffset;

            return checkerShape switch
            {
                CheckerShape.Square => Physics2D.OverlapBox(position, squareSize, 0f, whatIsGround),
                CheckerShape.Circle => Physics2D.OverlapCircle(position, circleRadius, whatIsGround),

                _ => null
            };
        }

        private void OnDisable()
        {
            Collider2D previousGround = CurrentGround;
            CurrentGround = null;

            //비활성화 되면 Exit를 한 번 실행
            if (previousGround != null) OnGroundExit?.Invoke(previousGround);
        }

        private void OnDrawGizmos()
        {
            if (!gizmosEnabled) return;

            Vector2 position = (Vector2)transform.position + checkerOffset;

            Gizmos.color = Color.yellow;

            switch (checkerShape)
            {
                case CheckerShape.Square:
                    Gizmos.DrawWireCube(position, squareSize);
                    break;

                case CheckerShape.Circle:
                    Gizmos.DrawWireSphere(position, circleRadius);
                    break;
            }
        }
    }
}
