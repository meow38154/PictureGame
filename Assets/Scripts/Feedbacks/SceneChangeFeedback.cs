using CoreSystem.Manager;
using DevLib.BattleSystem.Feedback;
using UnityEngine;

namespace Feedbacks
{
    public class SceneChangeFeedback : AbstractFeedback
    {
        [SerializeField] private string sceneName;
        
        public override void PlayFeedback()
        {
            SceneChangeManager.Instance.ChangeScene(sceneName);
        }
    }
}