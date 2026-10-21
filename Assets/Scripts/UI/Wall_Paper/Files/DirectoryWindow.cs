using DevLib.ObjectPool.Runtime;
using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper.Files
{
    public class DirectoryWindow : MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform iconGroupTrm;
        [SerializeField] private WallPaperFile filePrefab;
        
        public PoolItemSO PoolItem { get; set; }
        public GameObject GameObject => gameObject;
        public void ResetItem()
        {
            
        }

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