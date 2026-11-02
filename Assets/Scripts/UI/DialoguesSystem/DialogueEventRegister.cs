using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.DialoguesSystem
{
    [RequireComponent(typeof(TextRenderer))]
    public class DialogueEventRegister : MonoBehaviour
    {
        [SerializeField] private int number;
        [SerializeField] private UnityEvent onDialogueEvent;

        private TextRenderer textRenderer;
        
        private void Awake()
        {
            textRenderer = GetComponent<TextRenderer>();
            textRenderer.TextBundleSO.DialoguesDataBundle[number].EventDuringTextRendering += HandleEventInvoke;
        }

        private void OnDestroy()
        {
            textRenderer.TextBundleSO.DialoguesDataBundle[number].EventDuringTextRendering -= HandleEventInvoke;
        }

        private void HandleEventInvoke()
        {
            onDialogueEvent?.Invoke();
        }
    }
}