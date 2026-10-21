using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.WallPaperIcon
{
    public class WallPaperIconHoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject hoverHighlight;

        private void Awake()
        {
            if (hoverHighlight == null) hoverHighlight = gameObject;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverHighlight.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hoverHighlight.SetActive(false);
        }
    }
}