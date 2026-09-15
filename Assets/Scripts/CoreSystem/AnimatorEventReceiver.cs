using DevLib.AnimatorSystem;
using UnityEngine;

namespace CoreSystem
{
    public class AnimatorEventReceiver : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Play(HashDataSO hashDataSo)
        {
            if (animator == null || !animator.gameObject.activeSelf) return;
            animator.Play(hashDataSo.HashValue, 0, 0);
        }
    }
}