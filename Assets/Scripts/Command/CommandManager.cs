using System;
using System.Collections.Generic;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace Command
{
    public enum CommandActionType
    {
        Execute,
        Redo,
        Undo
    }

    public struct CommandInvokeEventStruct
    {
        public CommandActionType Action;
        public ICommand Command;
    }
    
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO commandInvokeEvent; //구독하는 쪽
        private Stack<ICommand> _undoStack = new();
        private Stack<ICommand> _redoStack = new();

        private void OnEnable()
        {
            commandInvokeEvent.AddListener<CommandInvokeEvent>(HandleCommandEvent);
        }
        
        private void HandleCommandEvent(CommandInvokeEvent obj)
        {
            CommandActionType action = obj.Value.Action;

            switch (action)
            {
                case CommandActionType.Undo:
                    Undo();
                    break;
                
                case CommandActionType.Redo:
                    Redo();
                    break;
                
                case CommandActionType.Execute:
                    ExecuteCommand(obj.Value.Command);
                    break;
            }
        }

        private void ExecuteCommand(ICommand command)
        {
            if (command == null) return;
            
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();
        }
        
        private void Undo()
        {
            if (_undoStack.Count <= 0) return;
            
            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
        
        private void Redo()
        {
            if (_redoStack.Count <= 0) return;
            
            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
        
        private void OnDisable()
        {
            commandInvokeEvent.RemoveListener<CommandInvokeEvent>(HandleCommandEvent);
        }
    }
}
