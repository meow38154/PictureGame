using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Wall_Paper.Files.SO
{
    [CreateAssetMenu(fileName = "Scene file", menuName = "UI/Wall Paper/Scene", order = 0)]
    public class SceneFileSO : AbstractFileSO
    {
        [SerializeField] private string sceneName;

        protected override void FileLoaded()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}