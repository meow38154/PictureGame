using Common.Attributes;
using DevLib.ModuleSystem;
using TMPro;
using UI.Wall_Paper.Files.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files
{
    [DefaultExecutionOrder(-100)]
    public class WallPaperFile : ModuleOwner, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AbstractFileSO fileSo;
        [ReadOnly, SerializeField] private TextMeshProUGUI fileNameText;
        [ReadOnly, SerializeField] private Image image;
        
        public AbstractFileSO FileSo
        {
            get => fileSo;
            set
            {
                fileSo = value;
                OnDataReflect();
            }
        }
        private void OnDataReflect()
        {
            if (image == null) image = GetComponentInChildren<Image>();
            if (fileNameText == null) fileNameText = GetComponentInChildren<TextMeshProUGUI>();
            image.sprite = fileSo.Icon;
            fileNameText.text = fileSo.FileName;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}