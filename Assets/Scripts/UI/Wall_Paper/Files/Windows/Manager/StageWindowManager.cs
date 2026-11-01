using CoreSystem.Manager;
using UI.Wall_Paper.Files.SO;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public class StageWindowManager : AbstractWindowManager
    {
        [SerializeField] private Image image;
        
        public override AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            var window = base.OpenWindow(fileSO);
            
            if (window is not StageWindows dWindow || fileSO is not WindowFileSO dWindowSo) return null;
            
            dWindow.ScreenImageChanged(dWindowSo.SceneName);
            
            StartCoroutine(OpenStageCoroutine(dWindowSo.SceneName));
            
            return dWindow;
        }
        
        private void SceneChangeAnimation()
        {
            Utility.GetCurrentSceneScreen(new Rect(0, 0, Screen.width, Screen.height), HandleApplicationCapture);
        }

        private void HandleApplicationCapture(Texture2D screenshot)
        {
            image.gameObject.SetActive(true);
            image.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), Vector2.zero);
        }
        
        //수정할꺼임!!!!

        private IEnumerator OpenStageCoroutine(string sceneName)
        {
            yield return new WaitForSeconds(0.2f);
            SceneChangeAnimation();
            yield return new WaitForSeconds(0.4f);
            
            var rect = image.GetComponent<RectTransform>();
            
            rect.DOScale(new Vector3(2, 2.245833f, 1), 0.2f);
            rect.DOAnchorPosY(-43.75f, 0.2f);
            
            yield return new WaitForSeconds(0.3f);
            
            SceneManager.LoadScene(sceneName);
        }
    }
}