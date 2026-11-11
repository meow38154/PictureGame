using Common;
using UnityEngine;

namespace UI.DialogueSystem
{
    public class DialogueManager : MonoSingleton<DialogueManager>
    {

        [SerializeField] private DialogueUI dialogueUI;
        

        public void Dialogue(DialogueDataSo dialogueData)
        {
            if (dialogueData == null) 
                return;
            
            dialogueUI.ShowDialogue(dialogueData);
        }
    }
}