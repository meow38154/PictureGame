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

        [Header("Animation")]
        [SerializeField] private float captureDelay = 0.2f;
        [SerializeField] private float animationDelay = 0.4f;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private Vector3 targetScale = new Vector3(2, 2.245833f, 1);
        [SerializeField] private float targetPosY = -43.75f;

        private RectTransform _imageRect;
        private Sequence _sequence;
        private bool _isTransitioning;
        private bool _isCaptureCompleted;

        private void Awake()
        {
            _imageRect = image.rectTransform;
        }

        public override AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            if (_isTransitioning) return null;

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
            if (this == null)
            {
                Destroy(screenshot);
                return;
            }

            image.gameObject.SetActive(true);
            image.sprite = Sprite.Create(screenshot, new Rect(0, 0, screenshot.width, screenshot.height), Vector2.zero);

            _isCaptureCompleted = true;
        }

        private IEnumerator OpenStageCoroutine(string sceneName)
        {
            _isTransitioning = true;
            _isCaptureCompleted = false;

            yield return new WaitForSeconds(captureDelay);

            SceneChangeAnimation();

            yield return new WaitUntil(() => _isCaptureCompleted);

            AsyncOperation operation = SceneChangeManager.Instance.ChangeScene(sceneName);
            operation.allowSceneActivation = false;

            yield return new WaitForSeconds(animationDelay);

            _sequence?.Kill();

            _sequence = DOTween.Sequence().Append(_imageRect.DOScale(targetScale, animationDuration))
                .Join(_imageRect.DOAnchorPosY(targetPosY, animationDuration))
                .SetEase(Ease.OutQuad);

            yield return _sequence.WaitForCompletion();

            yield return new WaitUntil(() => operation.progress >= 0.9f);

            operation.allowSceneActivation = true;
        }

        private void OnDestroy()
        {
            _sequence?.Kill();

            if (image == null || image.sprite == null) return;

            Sprite sprite = image.sprite;
            Texture2D texture = sprite.texture;

            image.sprite = null;

            Destroy(sprite);
            Destroy(texture);
        }
    }
}