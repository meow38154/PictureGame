using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.DialogueSystem
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogueTextWriter : MonoBehaviour, IDialogueTextWriter
    {
        private TextMeshProUGUI _dialogueText;
        
        public bool IsWriting {get; private set;}
        bool _isSkipped = false;

        private void Awake()
        {
            _dialogueText = GetComponent<TextMeshProUGUI>();
            _dialogueText.richText = true;
        }

        private IEnumerator WriteText(DialogueLineData lineData)
        {

            foreach (DialogueTextData textData in lineData.DialogueTextData)
            {
                _dialogueText.text += $"<size={textData.TextSize * 100}%>";
                foreach (char c in textData.Text)
                {
                    _dialogueText.text += $"{c}";
                    if(!_isSkipped)
                        yield return new WaitForSeconds(0.1f*textData.TextSpeed);
                }
                _dialogueText.text += $"</size>";
            }
            
        }
        
        public void WriteLine(DialogueLineData lineData)
        {
            if (_dialogueText == null)
                return;
            IsWriting = true;
            _dialogueText.text = "";
            
            StartCoroutine(WriteText(lineData));
            
            IsWriting = false;
            _isSkipped = false;
        }

        public void SkipLine()
        {
            if (!IsWriting) return;
            
            _isSkipped = true;
            
        }
    }
}