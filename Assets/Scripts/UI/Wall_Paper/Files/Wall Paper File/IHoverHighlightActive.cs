using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Wall_Paper_File
{
    public interface IHoverHighlightActive
    {
        void HoverActive(bool value);
        
        Image HoverHighlightImage { get; }
    }
}