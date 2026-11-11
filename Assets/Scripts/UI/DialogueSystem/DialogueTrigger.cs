using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueDataSo dialogueData;

        private void Update()
        {
            if(Keyboard.current.lKey.wasPressedThisFrame)
                TriggerDialogue();
        }

        public void TriggerDialogue()
        {
            if (dialogueData == null) 
                return;
            
            DialogueManager.Instance.Dialogue(dialogueData);
        }
    }
}