using Command;
using CoreSystem.Effect;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Force_Undo
{
    [RequireComponent(typeof(Animator))]
    public class ForceUndoSystem : MonoBehaviour
    {
        [SerializeField] private EventChannelSO forceUndoEvent;
        [SerializeField] private EventChannelSO commandInvokeEvent;
        [SerializeField] private AssetNameSO animationName;
        
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            forceUndoEvent.AddListener<BoolEvent>(HandlePlayForceUndoAnim);
        }

        private void OnDisable()
        {
            forceUndoEvent.RemoveListener<BoolEvent>(HandlePlayForceUndoAnim);
        }

        private void HandlePlayForceUndoAnim(BoolEvent obj)
        {
            if (obj.Value)
            {
                _animator.Play(animationName.AssetHash,0,0);
            }
        }
        
        public void RaiseCommand()
        {
            CommandInvokeEventStruct eventData = new()
            {
                Action = CommandActionType.Undo,
                Command = null
            };

            commandInvokeEvent.RaiseEvent(CreateEvents.CommandInvokeEvent.InitData(eventData));
        }
    }
}