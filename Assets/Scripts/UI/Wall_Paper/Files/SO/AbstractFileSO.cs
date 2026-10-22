using UnityEngine;

namespace UI.Wall_Paper.Files.SO
{
    public abstract class AbstractFileSO : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] private string fileName;
        public string FileName => fileName;

        public abstract void FileLoaded();
    }
}