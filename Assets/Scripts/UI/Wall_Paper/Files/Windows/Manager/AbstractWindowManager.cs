using System;
using UI.Wall_Paper.Files.SO;
using UI.Wall_Paper.Files.Windows.OpenManager;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public abstract class AbstractWindowManager : MonoBehaviour
    {
        [field: SerializeField] public AbstractFileSO FileSo { get; private set; }

        [SerializeField] private Transform windowsTrm;
        [SerializeField] private AbstractWindows windowPrefab;
        
        public event Action<AbstractWindowManager> OnOpenWindow;

        private void Awake()
        {
            var events = GetComponentsInChildren<AbstractWindowOpenEvent>();

            foreach (var e in events)
            {
                OnOpenWindow += e.OpenEvent;
            }
        }

        public virtual AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            AbstractWindows window = Instantiate(windowPrefab, windowsTrm);
            window.WindowOpen();
            OnOpenWindow?.Invoke(this);
            window.ReflectWindowData(fileSO.FileName, fileSO.Icon);
            return window;
        }

        public virtual void CloseWindow(AbstractWindows abstractWindow)
        {
            abstractWindow.WindowClose();
        }
    }
}