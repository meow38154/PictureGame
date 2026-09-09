using CoreSystem.Manager;
using DevLib.BattleSystem.Feedback;
using UnityEngine;

namespace Feedbacks
{
    public class SceneChangeFeedback : AbstractFeedback
    {
        [SerializeField] private string sceneName;
        
        public async override void PlayFeedback()
        {
            await SceneChangeManager.Instance.SceneChangeAsync(sceneName);
        }
    }
}