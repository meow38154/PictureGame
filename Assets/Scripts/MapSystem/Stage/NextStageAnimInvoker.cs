using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace MapSystem.Stage
{
    public class NextStageAnimInvoker : MonoBehaviour
    {
        [SerializeField] private EventChannelSO nextStageEvent;

        public void InvokeNextStageAnim(string sceneName)
        {
            nextStageEvent.RaiseEvent(CreateEvents.StringEvent.InitData(sceneName));
        }
    }
}