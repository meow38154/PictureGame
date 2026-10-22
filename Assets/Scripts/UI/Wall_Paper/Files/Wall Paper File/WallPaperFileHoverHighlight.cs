using DevLib.ModuleSystem;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Wall_Paper_File
{
    [RequireComponent(typeof(Image))]
    
    public class WallPaperFileHoverHighlight : Module, IHoverHighlightActive
    { 
        private Image hoverHighlightImage;
        public Image HoverHighlightImage => hoverHighlightImage;
        
        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            
            hoverHighlightImage = GetComponent<Image>();
        }

        public void HoverActive(bool value)
        {
            hoverHighlightImage.gameObject.SetActive(value);
        }
    }
}