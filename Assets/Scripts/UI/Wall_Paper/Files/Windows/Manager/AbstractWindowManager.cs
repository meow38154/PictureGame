using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public abstract class AbstractWindowManager : MonoBehaviour
    {
        [field: SerializeField] public AbstractFileSO FileSo { get; private set; }

        [SerializeField] private Transform windowsTrm;
        [SerializeField] private AbstractWindows windowPrefab;
        
        public virtual AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            AbstractWindows window = Instantiate(windowPrefab, windowsTrm);
            window.WindowOpen();
            return window;
        }

        public virtual void CloseWindow(AbstractWindows abstractWindow)
        {
            abstractWindow.WindowClose();
        }
    }
}