using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueBackground;
        [SerializeField] private Image dialogueImage;
        
        private bool _isShown = false;
        private IDialogueTextWriter writer;
        private DialogueDataSo currentDialogueData;
        private int currentLine = 0;

        private void Awake()
        {
            writer = GetComponentInChildren<IDialogueTextWriter>();
            dialogueBackground.SetActive(false);
        }

        private void Update()
        {
            if (!_isShown || writer == null) return;

            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                if(!writer.IsWriting)
                    NextLine();
                else
                    writer.SkipLine();
            }
        }

        public void ShowDialogue(DialogueDataSo dialogueData)
        {
            if (dialogueBackground == null || _isShown)
                return;
            
            _isShown = true;
            dialogueBackground.SetActive(true);
            currentDialogueData = dialogueData;
            
            writer.WriteLine(dialogueData.LineDatas[currentLine]);
        }
        
        private void NextLine()
        {
            currentLine++;
            writer.WriteLine(currentDialogueData.LineDatas[currentLine]);
        }
    }
}