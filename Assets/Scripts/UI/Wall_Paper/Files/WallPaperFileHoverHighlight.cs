using DevLib.ModuleSystem;
using UnityEngine;

namespace UI.Wall_Paper.Files
{
    public class WallPaperFileHoverHighlight : Module
    {
        [SerializeField] private GameObject hoverHighlight;

        public override void Initialize(ModuleOwner owner)
        {
            base.Initialize(owner);
            
            if (hoverHighlight == null) hoverHighlight = gameObject;
        }
    }
}