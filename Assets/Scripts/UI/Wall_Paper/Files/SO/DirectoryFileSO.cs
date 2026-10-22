using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Wall_Paper.Files.SO
{
    [CreateAssetMenu(fileName = "Directory File", menuName = "UI/Wall Paper/Directory", order = 0)]
    public class DirectoryFileSO : AbstractFileSO
    {
        //DirectoryFileDataEvent에 파일 리스트를 보낸다
        [SerializeField] private EventChannelSO directoryFileExecuteEvent;
        [SerializeField] private AbstractFileSO[] files;
        public AbstractFileSO[] Files => files;

        public override void FileLoaded()
        {
            if (directoryFileExecuteEvent != null) directoryFileExecuteEvent.RaiseEvent(CreateEvents.DirectoryFileDataEvent.InitData(files));
            else Debug.LogWarning("No directory file loaded");
        }
    }
}