using UnityEngine;

namespace UI.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueDataSo dialogueData;

        public void TriggerDialogue()
        {
            if (dialogueData != null) 
                return;
            
            DialogueManager.Instance.Dialogue(dialogueData);
        }
    }
}