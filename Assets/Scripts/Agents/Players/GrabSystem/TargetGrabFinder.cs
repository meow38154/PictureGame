using CoreSystem.GrabSystem;
using CoreSystem.Triggers;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class TargetGrabFinder : MonoBehaviour, IGrabFinder
    {
        [SerializeField] private float clickPointRadius = 0.5f;
        [SerializeField] private LayerMask grabbableLayerMask;
        [SerializeField] private ColliderTrigger trigger;
        
        public bool TryFindObject(out IGrabbable grabbable)
        {
            Vector2 mousePos = Utility.GetMouseWorldPosition();
            
            Collider2D col = Physics2D.OverlapCircle(mousePos, clickPointRadius, grabbableLayerMask);

            if (col == null)
            {
                grabbable = null;
                return false;
            }

            grabbable = col.GetComponent<IGrabbable>();
            
            return grabbable != null;
        }
    }
}