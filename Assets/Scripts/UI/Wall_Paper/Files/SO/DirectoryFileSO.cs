using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Wall_Paper.Files.SO
{
    [CreateAssetMenu(fileName = "Directory File", menuName = "UI/Wall Paper/Directory", order = 0)]
    public class DirectoryFileSO : AbstractFileSO
    {
        [SerializeField] private EventChannelSO directoryFileExecuteEvent;
        [SerializeField] private AbstractFileSO[] files;
        public AbstractFileSO[] Files => files;

        protected override void FileLoaded()
        {
            directoryFileExecuteEvent.RaiseEvent(CreateEvents.DirectoryFileDataEvent.InitData(files));
        }
    }
}