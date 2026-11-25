using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace Feedbacks
{
    public class SceneChangeEvent : MonoBehaviour
    {
        [SerializeField] private EventChannelSO sceneChangeEvent;
        
        public void ChangeScener(string sceneName)
        {
            if (sceneChangeEvent == null) return;
            
            sceneChangeEvent.RaiseEvent(CreateEvents.StringEvent.InitData(sceneName));
        }
    }
}