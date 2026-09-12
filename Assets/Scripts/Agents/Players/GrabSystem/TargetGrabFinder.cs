using CoreSystem.GrabSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class TargetGrabFinder : MonoBehaviour, IGrabFinder
    {
        [SerializeField] private float clickPointRadius = 0.5f;
        [SerializeField] private LayerMask grabbableLayerMask;
        [SerializeField] private Transform grabIntersectionPointTrm;
        [SerializeField] private float grabIntersection = 1f;

        private readonly Collider2D[] _results = new Collider2D[8];

        public bool TryFindObject(out IGrabbable grabbable)
        {
            grabbable = null;

            Vector2 mousePos = Utility.GetMouseWorldPosition();

            int count = Physics2D.OverlapCircle(
                mousePos,
                clickPointRadius,
                new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = grabbableLayerMask
                },
                _results
            );

            float closestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider2D col = _results[i];

                if (col == null)
                    continue;

                IGrabbable target = col.GetComponentInParent<IGrabbable>();

                if (target == null)
                    continue;

                Vector2 closestPoint = col.ClosestPoint(grabIntersectionPointTrm.position);
                float distance = Vector2.Distance(grabIntersectionPointTrm.position, closestPoint);

                if (distance > grabIntersection)
                    continue;

                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                grabbable = target;
            }

            return grabbable != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blueViolet;
            Gizmos.DrawWireSphere(grabIntersectionPointTrm.position, grabIntersection);
        }
    }
}