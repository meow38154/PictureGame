using UnityEngine;

namespace UI.Wall_Paper.Files.SO
{
    [CreateAssetMenu(fileName = "Wall Paper File Data", menuName = "UI/Wall Paper/File Data", order = 0)]
    public class WallPaperFileDataSO : ScriptableObject
    {
        [SerializeField] private AbstractFileSO[] files;
        public AbstractFileSO[] Files => files;
    }
}