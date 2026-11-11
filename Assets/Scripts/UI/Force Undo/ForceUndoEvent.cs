using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Force_Undo
{
    public class ForceUndoEvent : MonoBehaviour
    {
        [SerializeField] private EventChannelSO forceUndoEvent;

        public void Play()
        {
            forceUndoEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
        }
    }
}