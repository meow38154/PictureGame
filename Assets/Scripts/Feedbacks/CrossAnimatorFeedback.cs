using DevLib.AnimatorSystem;
using DevLib.BattleSystem.Feedback;
using UnityEngine;

namespace Feedbacks
{
    public class CrossAnimatorFeedback : AbstractFeedback
    {
        [SerializeField] private HashDataSO animParam;
        [SerializeField] private float crossTime = 0.1f;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInParent<Animator>();
        }

        public override void PlayFeedback()
        {
            if (_animator == null)
                return;

            if (!_animator.enabled)
                return;

            if (!_animator.gameObject.activeInHierarchy)
                return;

            if (_animator.runtimeAnimatorController == null)
                return;

            _animator.CrossFadeInFixedTime(
                animParam.HashValue,
                crossTime
            );
        }
    }
}