using CoreSystem.Events;
using CoreSystem.Manager;
using DevLib.BattleSystem.Feedback;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace Feedbacks
{
    public class SceneChangeFeedback : AbstractFeedback
    {
        [SerializeField] private string sceneName;
        [SerializeField] private EventChannelSO sceneChangeEvent;
        
        public override void PlayFeedback()
        {
            sceneChangeEvent.RaiseEvent(CreateEvents.StringEvent.InitData(sceneName));
        }
    }
}