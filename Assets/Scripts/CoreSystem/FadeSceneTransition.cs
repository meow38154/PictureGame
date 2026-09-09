using DevLib.AnimatorSystem;
using UnityEngine;

namespace CoreSystem
{
    public class FadeSceneTransition : MonoBehaviour, ISceneTransition
    {
        private const float SCENECHANGE_DELAY = 0.2f;
        
        [SerializeField] private Animator animator;
        
        public async Awaitable PlayAsync(HashDataSO hash)
        {
            animator.Play(hash.HashValue, 0, 0);
            float animPlaytime = animator.GetCurrentAnimatorStateInfo(0).length + SCENECHANGE_DELAY;

            await Awaitable.WaitForSecondsAsync(animPlaytime);
        }
    }
}