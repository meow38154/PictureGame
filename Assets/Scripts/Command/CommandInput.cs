using Agents.Players;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace Command
{
    public class CommandInput : MonoBehaviour
    {
        [SerializeField] private EventChannelSO commandInvokeEvent;
        [SerializeField] private PlayerInputSO playerInputSO;

        private bool _isCtrlDown;
        
        private void Awake()
        {
            playerInputSO.OnLCtrlKeyPressed += HandleCtrlKeyPressed;
            playerInputSO.OnLCtrlKeyReleased += HandleCtrlKeyReleased;

            playerInputSO.OnZKeyPressed += HandleZKeyPressed;
            playerInputSO.OnYKeyPressed += HandleYKeyPressed;
        }

        private void HandleCtrlKeyPressed() => _isCtrlDown = true;

        private void HandleCtrlKeyReleased() => _isCtrlDown = false;

        private void HandleZKeyPressed()
        {
            if (_isCtrlDown) RaiseCommand(CommandActionType.Undo);
        }

        private void HandleYKeyPressed()
        {
            if (_isCtrlDown) RaiseCommand(CommandActionType.Redo);
        }

        private void OnDestroy()
        {
            playerInputSO.OnLCtrlKeyPressed -= HandleCtrlKeyPressed;
            playerInputSO.OnLCtrlKeyReleased -= HandleCtrlKeyReleased;
            
            playerInputSO.OnZKeyPressed -= HandleZKeyPressed;
            playerInputSO.OnYKeyPressed -= HandleYKeyPressed;
        }

        private void RaiseCommand(CommandActionType action)
        {
            CommandInvokeEventStruct eventData = new()
            {
                Action = action,
                Command = null
            };

            commandInvokeEvent.RaiseEvent(CreateEvents.CommandInvokeEvent.InitData(eventData));
        }
    }
}