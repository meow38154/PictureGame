using UI.Wall_Paper.Files;
using UI.Wall_Paper.Files.SO;
using UnityEngine;

namespace UI.Wall_Paper
{
    public class FileDataLoader : MonoBehaviour
    {
        [SerializeField] private WallPaperFileDataSO data;
        [SerializeField] private Transform wallPaperTrm;
        [SerializeField] private WallPaperFile filePrefab;
        
        private void Awake()
        {
            if (data == null) return;
            
            foreach (var file in data.Files)
            {
                if (wallPaperTrm == null || filePrefab == null) continue;
                
                WallPaperFile wallPaperFile = Instantiate(filePrefab, wallPaperTrm);
                wallPaperFile.FileSo = file;
            }
        }
    }
}