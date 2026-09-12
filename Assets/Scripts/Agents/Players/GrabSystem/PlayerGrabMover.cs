using CoreSystem.GrabSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerGrabMover : MonoBehaviour, IGrabMover
    {
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private Ease ease = Ease.InOutCubic;
        
        public void MoveGrabbable(IGrabbable grabbable, Vector2 position)
        {
            Transform target = grabbable.Transform;
            Transform parent = target.parent;

            if (parent == null)
                return;

            Vector3 targetPivotLocal = parent.InverseTransformPoint(position);
            
            Vector3 pivotOffset = grabbable.Pivot * grabbable.Transform.localScale;

            Vector3 destination = targetPivotLocal - pivotOffset;

            target.DOLocalRotate(Vector3.zero, moveDuration / 2).SetEase(ease);
            target.DOLocalMove(destination, moveDuration).SetEase(ease);
        }
    }
}