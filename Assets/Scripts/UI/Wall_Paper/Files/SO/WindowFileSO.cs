using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Wall_Paper.Files.SO
{
    [CreateAssetMenu(fileName = "Scene file", menuName = "UI/Wall Paper/Scene", order = 0)]
    public class WindowFileSO : AbstractFileSO
    {
        [SerializeField] private string sceneName;
        public string SceneName => sceneName;
    }
}