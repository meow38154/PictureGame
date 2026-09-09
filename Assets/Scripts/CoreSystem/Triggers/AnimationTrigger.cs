using System;
using DevLib.ModuleSystem;

namespace CoreSystem.Triggers
{
    public class AnimationTrigger : Module
    {
        public event Action OnTriggerEvent;
        public event Action OnAnimationEndEvent;
        
        public void TriggerEvent()
        {
            OnTriggerEvent?.Invoke();
        }
        
        public void TriggerAnimationEnd()
        {
            OnAnimationEndEvent?.Invoke();
        }
    }
}