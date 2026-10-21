using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper.Files
{
    public class DirectoryWindowManager : MonoBehaviour
    {
        [SerializeField] private Transform windowsTrm;
        [SerializeField] private DirectoryWindow directoryWindowPrefab;
        [SerializeField] private EventChannelSO directoryFileExecuteEvent;

        private void OnEnable()
        {
            directoryFileExecuteEvent.AddListener<DirectoryFileDataEvent>(HandleDirectoryFileExecuteEvent);
        }

        private void HandleDirectoryFileExecuteEvent(DirectoryFileDataEvent obj)
        {
            OpenDirectoryWindow(obj.Value);
        }
        
        public void OpenDirectoryWindow(AbstractFileSO[] files)
        {
            DirectoryWindow window = Instantiate(directoryWindowPrefab, windowsTrm);
            window.SpawnIcons(files);
        }

        private void OnDisable()
        {
            directoryFileExecuteEvent.RemoveListener<DirectoryFileDataEvent>(HandleDirectoryFileExecuteEvent);
        }
    }
}