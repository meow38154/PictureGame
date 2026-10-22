using Agents.Players;
using Common.Attributes;
using DevLib.ModuleSystem;
using TMPro;
using UI.Wall_Paper.Files.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Wall_Paper.Files.Wall_Paper_File
{
    [DefaultExecutionOrder(-100)]
    public class WallPaperFile : ModuleOwner, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private AbstractFileSO fileSo;
        [SerializeField] private PlayerInputSO playerInputSO;
        [ReadOnly, SerializeField] private TextMeshProUGUI fileNameText;
        [ReadOnly, SerializeField] private Image image;
        
        public bool OnMouseIcon { get; private set; }

        private bool _designated;
        private IHoverHighlightActive hoverHighlight;
        
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

        
        protected override void Awake()
        {
            base.Awake();
            hoverHighlight = GetModule<IHoverHighlightActive>();
            Debug.Assert(hoverHighlight != null,  nameof(hoverHighlight) + " != null");
            playerInputSO.OnMouseLeftDoubleButtonPressed += HandleOnFileLoaded;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_designated) ChangeHoverHighlight(0.375f);
            
            OnMouseIcon = true;
        }

        private void Update()
        {
            if (!Mouse.current.leftButton.wasReleasedThisFrame) return;
            
            if (!OnMouseIcon)
            {
                _designated = false;
                ChangeHoverHighlight(0f);
                return;
            }

            _designated = true;

            ChangeHoverHighlight(0.75f);
        }

        private void HandleOnFileLoaded()
        {
            if (!OnMouseIcon) return;
            fileSo.FileLoaded();
            if (!_designated) ChangeHoverHighlight(0);
        }

        private void OnDestroy()
        {
            playerInputSO.OnMouseLeftDoubleButtonPressed -= HandleOnFileLoaded;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_designated) ChangeHoverHighlight(0);
            
            OnMouseIcon = false;
        }

        private void ChangeHoverHighlight(float a)
        {
            Image hoverImage = hoverHighlight.HoverHighlightImage;
            
            Color color = hoverImage.color;
            hoverImage.color = new Color(color.r, color.g, color.b, a);
        }
    }
}