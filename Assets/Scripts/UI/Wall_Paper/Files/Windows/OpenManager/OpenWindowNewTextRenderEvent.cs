using UI.DialoguesSystem;
using UI.Wall_Paper.Files.Windows.Manager;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows.OpenManager
{
    public class OpenWindowNewTextRenderEvent : AbstractWindowOpenEvent
    {
        [SerializeField] private TextRenderer textRenderer;
        [SerializeField] private DialoguesTextBundleSO textBundleSO;
        
        public override void OpenEvent(AbstractWindowManager abstractWindowManager)
        {
            if (textRenderer == null) return;
            
            base.OpenEvent(abstractWindowManager);
            textRenderer.AutoNextTextRender(textBundleSO);
        }
    }
}