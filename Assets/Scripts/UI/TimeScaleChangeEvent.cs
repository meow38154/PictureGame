using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI
{
    public class TimeScaleChangeEvent : MonoBehaviour
    {
        [SerializeField] private EventChannelSO timeScaleChangeEvent;
        
        public void Play(float timeScale)
        {
            timeScaleChangeEvent.RaiseEvent(CreateEvents.FloatEvent.InitData(timeScale));
        }
    }
}