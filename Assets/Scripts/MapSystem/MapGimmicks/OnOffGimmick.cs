using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class OnOffGimmick : MonoBehaviour, IToggleable
    {
        [SerializeField] private List<IMapPiece> items = new();
        public bool Current { get; private set; } = false;
        
        private TextMeshPro _onOffText;
        
        public void Toggle()
        {
            Current = !Current;
            _onOffText.text = Current ? "On" : "Off";
            foreach (IMapPiece item in items)
            {
                item.SetPieceActive(!item.IsActive);
            }
        }
    }
}