
using System.Collections.Generic;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DevLib.ModuleSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Command.UndoableObjects
{
    public class UndoableObject : ModuleOwner
    {
        [SerializeField] private EventChannelSO commandInvokeEvent;

        private readonly Stack<UndoSaveData> _undoStack = new();
        private readonly Stack<UndoSaveData> _redoStack = new();

        protected override void Awake()
        {
            base.Awake();

            _undoStack.Push(new UndoSaveData
            {
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale,
                Parent = transform.parent,
                CustomData = CaptureCustomData()
            });
            
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
                case CommandActionType.Redo:
                    Redo();
                    break;
                case CommandActionType.Execute:
                    StackPile();
                    break;
                case CommandActionType.Peek:
                    Undo();
                    Redo();
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

        private UndoSaveData CaptureState()
        {
            return new UndoSaveData
            {
                Position = transform.localPosition,
                Rotation = transform.localRotation,
                Scale = transform.localScale,
                Parent = transform.parent,
                CustomData = CaptureCustomData()
            };
        }

        private void RestoreState(UndoSaveData data)
        {
            transform.SetParent(data.Parent, false);
            transform.localPosition = data.Position;
            transform.localRotation = data.Rotation;
            transform.localScale = data.Scale;

            RestoreCustomData(data.CustomData);
        }

        private void StackPile()
        {
            _undoStack.Push(CaptureState());
            _redoStack.Clear();
        }

        private void Undo()
        {
            if (_undoStack.Count <= 0) return;

            _redoStack.Push(CaptureState());

            UndoSaveData data = _undoStack.Count == 1 ? _undoStack.Peek() : _undoStack.Pop();

            RestoreState(data);
        }

        private void Redo()
        {
            if (_redoStack.Count <= 0) return;

            _undoStack.Push(CaptureState());

            UndoSaveData data = _redoStack.Pop();

            RestoreState(data);
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
