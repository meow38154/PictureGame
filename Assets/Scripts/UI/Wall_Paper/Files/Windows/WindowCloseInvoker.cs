using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    public class WindowCloseInvoker : MonoBehaviour
    {
        [SerializeField] private EventChannelSO windowCloseRegisterEvent;

        public void Close()
        {
            if (windowCloseRegisterEvent == null) return;
            
            windowCloseRegisterEvent.RaiseEvent(CreateEvents.AbstractWindowEvent.InitData(GetComponentInParent<AbstractWindows>()));
        }
    }
}