using Common;
using CoreSystem.Triggers;
using UnityEngine;

namespace CoreSystem.Events
{
    public class GlitchDisableTriggerEvent : MonoBehaviour
    {
        [SerializeField] private Transform objectToDisableGlitch;

        public void Play()
        {
            var colliderTriggers = objectToDisableGlitch.GetComponentsInChildren<ColliderTrigger>();

            foreach (var colliderTrigger in colliderTriggers)
            {
                if (colliderTrigger.TryGetComponent(out Visuals _))
                {
                    colliderTrigger.IsActive = false;
                }
            }
        }
    }
}