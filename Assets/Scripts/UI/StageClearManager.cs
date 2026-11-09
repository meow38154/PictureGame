using Common;
using CoreSystem.Events;
using CoreSystem.Manager;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI
{
    public class StageClearManager : MonoSingleton<StageClearManager>
    {
        [SerializeField] private EventChannelSO goBackSelectorEvent;
        [SerializeField] private GameObject canvas;

        private void Start()
        {
            goBackSelectorEvent.AddListener<BoolEvent>(HandleClearEvent);
        }

        private void OnDestroy()
        {
            goBackSelectorEvent.RemoveListener<BoolEvent>(HandleClearEvent);
        }

        public void HandleClearEvent(BoolEvent obj)
        {
            if (!obj.Value) return;
            canvas.SetActive(true);
            SceneChangeManager.Instance.ChangeScene("Stage Selector");
        }
    }
}