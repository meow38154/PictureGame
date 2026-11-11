using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.DialoguesSystem
{
    public class DialogueManualTurn : MonoBehaviour
    {
        private TextRenderer[] textRenderers;

        private void Awake()
        {
            textRenderers = GetComponentsInChildren<TextRenderer>();
        }

        private void Update()
        {
            if (!Keyboard.current.enterKey.wasPressedThisFrame) return;
            
            TextsRender();
        }

        public void TextsRender()
        {
            foreach (var textRenderer in textRenderers)
            {
                textRenderer.NextTextRender();
            }
        }
    }
}