using CoreSystem.GrabSystem;
using DG.Tweening;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public class PlayerGrabMover : MonoBehaviour, IGrabMover
    {
        [SerializeField] private float moveDuration = 0.3f;
        [SerializeField] private Ease  ease = Ease.InOutCubic;
        
        public void MoveGrabbable(IGrabbable grabbable, Vector2 position)
        {
            grabbable.Rigidbody.DOMove(position, moveDuration).SetEase(ease);
        }
    }
}