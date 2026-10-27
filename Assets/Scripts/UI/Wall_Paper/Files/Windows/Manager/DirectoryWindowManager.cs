using UI.Wall_Paper.Files.SO;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public class DirectoryWindowManager : AbstractWindowManager
    {
        public override AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            var window = base.OpenWindow(fileSO);
            
            if (window is not DirectoryWindow dWindow || fileSO is not DirectoryFileSO dFileSo) return null;
            
            dWindow.SpawnIcons(dFileSo.Files);
            
            return dWindow;
        }
    }
}