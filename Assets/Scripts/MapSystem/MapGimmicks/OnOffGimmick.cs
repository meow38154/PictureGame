using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MapSystem.MapGimmicks
{
    public class OnOffGimmick : MonoBehaviour, IToggleable
    {
        [SerializeField] private List<GameObject> items;

        public bool Current { get; private set; } = false;
        
        private TextMeshPro _onOffText;

        private void Awake()
        {
            _onOffText = gameObject.GetComponentInChildren<TextMeshPro>();
        }

        public void Toggle()
        {
            Current = !Current;
            _onOffText.text = Current ? "On" : "Off";
            foreach (GameObject item in items)
            {
                if (item.TryGetComponent(out IMapPiece mapPiece))
                {
                    mapPiece.SetPieceActive(!mapPiece.IsActive);
                    Debug.Log(mapPiece.IsActive);
                }
            }
            Debug.Log("dd");
        }
    }
}