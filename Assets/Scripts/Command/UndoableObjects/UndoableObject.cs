using System.Collections.Generic;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace Command.UndoableObjects
{
    public class UndoableObject : ModuleOwner
    {
        [SerializeField] private EventChannelSO commandInvokeEvent;

        private Stack<UndoSaveData> _undoStack = new();

        protected override void Awake()
        {
            base.Awake();
            commandInvokeEvent.AddListener<CommandInvokeEvent>(HandleUndoCommand);
        }

        private void HandleUndoCommand(CommandInvokeEvent obj)
        {
            CommandActionType actionType = obj.Value.Action;

            switch (actionType)
            {
                case CommandActionType.Undo:
                    Undo();
                    break;
                case CommandActionType.Execute:
                    StackPile();
                    break;
            }
        }

        private void OnDestroy()
        {
            commandInvokeEvent.RemoveListener<CommandInvokeEvent>(HandleUndoCommand);
        }

        protected virtual object CaptureCustomData()
        {
            return null;
        }

        protected virtual void RestoreCustomData(object data)
        {
        }

        private void StackPile()
        {
            _undoStack.Push(new UndoSaveData
            {
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale,
                Parent = transform.parent,
                CustomData = CaptureCustomData()
            });
        }

        private void Undo()
        {
            if (_undoStack.Count <= 0) return;

            UndoSaveData data = _undoStack.Pop();

            transform.SetParent(data.Parent, false);
            transform.localPosition = data.Position;
            transform.localRotation = data.Rotation;
            transform.localScale = data.Scale;

            RestoreCustomData(data.CustomData);
        }
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (commandInvokeEvent != null) return;
            
            //이쁘게 바꿀게요...
            commandInvokeEvent = AssetDatabase.LoadAssetAtPath<EventChannelSO>("Assets/GameModules/EventChannel/CommandInvokeEvent.asset");
        }
#endif
    }
}