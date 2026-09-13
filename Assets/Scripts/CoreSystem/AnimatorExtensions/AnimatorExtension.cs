using DevLib.AnimatorSystem;
using UnityEngine;

namespace CoreSystem.AnimatorExtensions
{
    public static class AnimatorExtension
    {
        public static void Play(this Animator animator, HashDataSO hashDataSo)
        {
            animator.Play(hashDataSo.HashValue);
        }
    }
}