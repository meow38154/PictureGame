using System;
using Common;
using CoreSystem.Triggers;
using UnityEngine;

namespace CoreSystem.Events
{
    public class GlitchDisableTriggerEvent : MonoBehaviour
    {
        private void OnValidate()
        {
            gameObject.name = "GlitchDisableTriggerEvent";
        }

        public void Play(Transform objectToDisable)
        {
            var colliderTriggers = objectToDisable.GetComponentsInChildren<ColliderTrigger>();

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