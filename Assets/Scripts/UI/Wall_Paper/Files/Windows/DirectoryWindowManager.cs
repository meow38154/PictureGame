using System;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    public class DirectoryWindowManager : MonoBehaviour
    {
        [SerializeField] private Transform windowsTrm;
        [SerializeField] private DirectoryWindow directoryWindowPrefab;
        
        //값을 받습니다.
        [SerializeField] private EventChannelSO directoryFileExecuteEvent;

        private void Awake()
        {
            if (windowsTrm == null) Debug.LogWarning("WindowOpenAnimation couldn't find windowsTrm");
            if (directoryWindowPrefab == null) Debug.LogWarning("WindowOpenAnimation couldn't find directoryWindowPrefab");
            if (directoryFileExecuteEvent == null) Debug.LogWarning("WindowOpenAnimation couldn't find directoryFileExecuteEvent");
        }

        private void OnEnable()
        {
            directoryFileExecuteEvent.AddListener<DirectoryFileDataEvent>(HandleDirectoryFileExecuteEvent);
        }

        private void HandleDirectoryFileExecuteEvent(DirectoryFileDataEvent obj)
        {
            OpenDirectoryWindow(obj.Value);
        }
        
        private void OpenDirectoryWindow(AbstractFileSO[] files)
        {
            DirectoryWindow abstractWindow = Instantiate(directoryWindowPrefab, windowsTrm);
            abstractWindow.SpawnIcons(files);
        }

        private void OnDisable()
        {
            directoryFileExecuteEvent.RemoveListener<DirectoryFileDataEvent>(HandleDirectoryFileExecuteEvent);
        }
    }
}