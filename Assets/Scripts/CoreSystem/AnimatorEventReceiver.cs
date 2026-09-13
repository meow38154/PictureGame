using DevLib.AnimatorSystem;
using UnityEngine;

namespace CoreSystem
{
    public class AnimatorEventReceiver : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Play(HashDataSO hashDataSo)
        {
            animator.Play(hashDataSo.HashValue, 0, 0);
        }
    }
}