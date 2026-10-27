using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;

namespace UI.Wall_Paper.Files.SO
{
    public abstract class AbstractFileSO : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] private string fileName;
        public string FileName => fileName;
        [SerializeField] private EventChannelSO windowOpenEvent;

        public virtual void FileLoaded()
        {
            if (windowOpenEvent != null) 
                windowOpenEvent.RaiseEvent(CreateEvents.AbstractFileSOEvent.InitData(this));
        }
    }
}