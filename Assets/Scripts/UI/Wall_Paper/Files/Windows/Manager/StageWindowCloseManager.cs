using DG.Tweening;
using UnityEngine;
using System.Collections;
using UI.Wall_Paper.Files.SO;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Windows.Manager
{
    public class StageWindowCloseManager : AbstractWindowManager
    {
        [SerializeField] private Vector3 targetScale = new(2, 2.245833f, 1);
        [SerializeField] private RectTransform root;
        [SerializeField] private GameObject canvas;
        private Sequence _sequence;
        [SerializeField] private float animationDuration = 0.2f;
        
        protected override void Awake()
        {
            base.Awake();
            root.sizeDelta = targetScale;
        }

        public override AbstractWindows OpenWindow(AbstractFileSO fileSO)
        {
            var window = base.OpenWindow(fileSO);

            if (window is not StageCloseWindows dWindow) return null;

            dWindow.ScreenImageChanged();
            
            StartCoroutine(AnimPlay());
            return dWindow;
        }

        private IEnumerator AnimPlay()
        {
            yield return new WaitForSeconds(1f);
            CloseWindows();
            yield return new WaitForSeconds(0.5f);
        }
        
        public void CloseWindows()
        {
            _sequence?.Kill();

            canvas.gameObject.SetActive(true);
            root.DOScale(targetScale, 0);
            root.DOAnchorPosY(-43.5f, 0);
            _sequence = DOTween.Sequence().Append(root.DOScale(Vector3.one, animationDuration)).Join(root.DOAnchorPos(Vector3.zero, animationDuration)).SetEase(Ease.OutQuad);
        }
    }
}