using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class OnOffGimmick : MonoBehaviour, IToggleable
    {
        [SerializeField] private List<GameObject> items;
        
        private const string ON_TEXT = "ON";
        private const string OFF_TEXT = "OFF";

        public bool IsOn { get; private set; } = false;
        
        private TextMeshPro _onOffText;

        private void Awake()
        {
            _onOffText = gameObject.GetComponentInChildren<TextMeshPro>();
        }

        public void OnOffToggle()
        {
            IsOn = !IsOn;
            _onOffText.text = IsOn ? ON_TEXT : OFF_TEXT;
            foreach (GameObject item in items)
            {
                if (item.TryGetComponent(out IMapPiece mapPiece))
                {
                    mapPiece.SetPieceActive(!mapPiece.IsActive);
                    Debug.Log(mapPiece.IsActive);
                }
            }
        }
    }
}