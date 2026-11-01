using System;
using System.Collections.Generic;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper.Taskbar
{
    public class TaskbarIconManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO registerTaskbarIconEvent;
        [SerializeField] private EventChannelSO unRegisterTaskbarIconEvent;

        private HashSet<AbstractFileSO> barIconList;

        private void Awake()
        {
            registerTaskbarIconEvent.AddListener<AbstractFileSOEvent>(HandleRegisterTaskbarIcon);
            registerTaskbarIconEvent.AddListener<AbstractFileSOEvent>(HandleUnRegisterTaskbarIcon);
        }

        private void OnDestroy()
        {
            registerTaskbarIconEvent.RemoveListener<AbstractFileSOEvent>(HandleRegisterTaskbarIcon);
            registerTaskbarIconEvent.RemoveListener<AbstractFileSOEvent>(HandleUnRegisterTaskbarIcon);
        }

        private void HandleRegisterTaskbarIcon(AbstractFileSOEvent obj)
        {
            barIconList.Add(obj.Value);
        }        
        
        private void HandleUnRegisterTaskbarIcon(AbstractFileSOEvent obj)
        {
            barIconList.Remove(obj.Value);
        }

        private void TaskbarIconUpdate()
        {
            foreach (var i in barIconList)
            {
                
            }
        }

    }
}