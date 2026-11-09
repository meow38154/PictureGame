using System;
using CoreSystem.Manager;
using UI.Wall_Paper.Files.SO;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public class StageWindowManager : AbstractWindowManager
    {
        /*[SerializeField] private Image image;

        [Header("Animation")]
        [SerializeField] private float captureDelay = 0.2f;
        [SerializeField] private float animationDelay = 0.4f;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float sceneChangeDelay = 2f;
        [SerializeField] private Vector3 targetScale = new(2, 2.245833f, 1);
        [SerializeField] private float targetPosY = -43.75f;
        [SerializeField] private GameObject canvas;

        private RectTransform _imageRect;
        private Sequence _sequence;


        protected override void Awake()
        {
            base.Awake();

            _imageRect = image.rectTransform;
        }

        public override AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            var window = base.OpenWindow(fileSO);

            if (window == null) Debug.Log("d");

            if (window is not StageWindows dWindow || fileSO is not WindowFileSO dWindowSo)
            {
                Debug.Log("s");
                return null;
            }

            dWindow.ScreenImageChanged(dWindowSo.SceneName);

            StartCoroutine(OpenStageCoroutine(dWindowSo.SceneName));

            return dWindow;
        }

        private IEnumerator OpenStageCoroutine(string sceneName)
        {
            yield return new WaitForSeconds(captureDelay);

            yield return new WaitForSeconds(animationDelay);

            _sequence?.Kill();

            _sequence = DOTween.Sequence()
                .Append(_imageRect.DOScale(targetScale, animationDuration))
                .Join(_imageRect.DOAnchorPosY(targetPosY, animationDuration))
                .SetEase(Ease.OutQuad);

            yield return _sequence.WaitForCompletion();

            yield return new WaitForSeconds(sceneChangeDelay);

            AsyncOperation operation = SceneChangeManager.Instance.ChangeScene(sceneName);
            operation.allowSceneActivation = false;

            yield return new WaitUntil(() => operation.progress >= 0.9f);

            operation.allowSceneActivation = true;
            canvas.gameObject.SetActive(false);
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
        }*/
    }
}