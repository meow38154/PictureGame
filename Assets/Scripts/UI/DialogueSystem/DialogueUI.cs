using System;
using System.Collections.Generic;
using Common.Attributes;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.DialogueSystem
{
    public class DialogueUI : MonoBehaviour
    {
        //등록
        [SerializeField] private EventChannelSO dialogueLineDataEvent;
        
        [SerializeField] private GameObject dialogueBackground;
        [SerializeField] private Image dialogueImage;
        [ReadOnly, SerializeField] private List<DialogueLineData> currentDialogueData;
        
        private bool _isShown = false;
        private IDialogueTextWriter writer;
        private int currentLine = 0;

        private void Awake()
        {
            writer = GetComponentInChildren<IDialogueTextWriter>();
            dialogueBackground.SetActive(false);

            dialogueLineDataEvent.AddListener<DialogueLineDataEvent>(HandleDialogueInvoker);
        }

        private void HandleDialogueInvoker(DialogueLineDataEvent evt)
        {
            ShowDialogue(evt.Value);
        }

        private void Update()
        {
            if (!_isShown || writer == null) return;

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                if(!writer.IsWriting)
                    NextLine();
                else
                    writer.SkipLine();
            }
        }

        private void OnDestroy()
        {
            dialogueLineDataEvent.RemoveListener<DialogueLineDataEvent>(HandleDialogueInvoker);
        }

        private void ShowDialogue(List<DialogueLineData> dialogueData)
        {
            if (dialogueBackground == null || _isShown)
                return;
            
            _isShown = true;
            dialogueBackground.SetActive(true);
            currentDialogueData = dialogueData;
            
            writer.WriteLine(dialogueData[currentLine]);
        }
        
        private void NextLine()
        {
            if (currentLine == currentDialogueData.Count+1)
                return;
            writer.WriteLine(currentDialogueData[currentLine]);
            currentLine++;
        }
    }
}