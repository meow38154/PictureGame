using System.Collections;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace MapSystem.Stage
{
    public class GoBackSelectorInvoker : MonoBehaviour
    {
        [SerializeField] private EventChannelSO goBackSelectorEvent;
        [SerializeField] private EventChannelSO reflectScreenCaptureEvent;

        public void EventInvoke()
        {
            if (!gameObject.activeInHierarchy) return;
            
            StartCoroutine(Clear());
        }

        private IEnumerator Clear()
        {
            reflectScreenCaptureEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
            yield return new WaitForSeconds(1.5f);
            goBackSelectorEvent.RaiseEvent(CreateEvents.BoolEvent.InitData(true));
        }
    }
}