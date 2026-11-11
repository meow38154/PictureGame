using System;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.DialogueSystem
{
    public class DialogueTrigger : MonoBehaviour
    {
        //보내는 거
        [SerializeField] private EventChannelSO dialogueLineDataEvent;
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
            
            dialogueLineDataEvent.RaiseEvent(CreateEvents.DialogueLineDataEvent.InitData(dialogueData.LineDatas));
        }
    }
}