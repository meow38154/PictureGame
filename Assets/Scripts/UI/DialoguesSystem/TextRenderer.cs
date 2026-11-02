using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.DialoguesSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextRenderer : MonoBehaviour
    {
        [SerializeField] private DialoguesTextBundleSO textBundleSO;
        [SerializeField] private float freeTime = 1.25f;
        public DialoguesTextBundleSO TextBundleSO => textBundleSO;

        private TextMeshProUGUI _textMeshProUGUI;
        private int _textCount;

        private Coroutine _textCoroutine;
        private Coroutine _eventCoroutine;

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _textMeshProUGUI.text = "";
        }

        public void AutoNextTextRender(DialoguesTextBundleSO textBundle)
        {
            StartCoroutine(AutoNextTextRenderCoroutine(textBundle));
        }

        private IEnumerator AutoNextTextRenderCoroutine(DialoguesTextBundleSO textBundle)
        {
            foreach (var data in textBundle.DialoguesDataBundle)
            {
                NextTextRender();
                yield return new WaitForSecondsRealtime(data.textDuration + freeTime);
            }
        }

    [ContextMenu("Next text")]
        public void NextTextRender()
        {
            if (_textCount >= textBundleSO.DialoguesDataBundle.Length) return;

            if (_textCoroutine != null) StopCoroutine(_textCoroutine);

            if (_eventCoroutine != null) StopCoroutine(_eventCoroutine);

            var data = textBundleSO.DialoguesDataBundle[_textCount];

            _textMeshProUGUI.text = data.text;
            _textMeshProUGUI.maxVisibleCharacters = 0;
            _textMeshProUGUI.ForceMeshUpdate();

            _textCoroutine = StartCoroutine(RenderText(data));
            _eventCoroutine = StartCoroutine(WaitEventDuringTextRendering(data));

            _textCount++;
        }

        private IEnumerator RenderText(DialoguesDataStruct data)
        {
            int characterCount = _textMeshProUGUI.textInfo.characterCount;
            float interval = data.textDuration / Mathf.Max(1, characterCount);

            for (int i = 1; i <= characterCount; i++)
            {
                yield return new WaitForSecondsRealtime(interval);
                _textMeshProUGUI.maxVisibleCharacters = i;
            }

            _textCoroutine = null;
        }

        private IEnumerator WaitEventDuringTextRendering(DialoguesDataStruct data)
        {
            yield return new WaitForSeconds(data.actionInvokeTime);
            data.EventDuringTextRendering?.Invoke();
            _eventCoroutine = null;
        }
        
        //비활성화 시 코루틴 정지
        private void OnDisable()
        {
            if (_textCoroutine != null) StopCoroutine(_textCoroutine);

            if (_eventCoroutine != null) StopCoroutine(_eventCoroutine);

            _textCoroutine = null;
            _eventCoroutine = null;
        }
    }
}