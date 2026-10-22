using DevLib.ObjectPool.Runtime;
using UI.Wall_Paper.Files.SO;
using UI.Wall_Paper.Files.Wall_Paper_File;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DirectoryWindow : AbstractWindows
    {
        [SerializeField] private Transform iconGroupTrm;
        [SerializeField] private WallPaperFile filePrefab;

        public void SpawnIcons(AbstractFileSO[] files)
        {
            foreach (var file in files)
            {
                WallPaperFile wallPaperFile = Instantiate(filePrefab, iconGroupTrm);
                wallPaperFile.FileSo = file;
            }
        }
    }
}