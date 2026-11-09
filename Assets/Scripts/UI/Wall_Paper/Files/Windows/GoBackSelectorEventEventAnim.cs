using System.Collections;
using CoreSystem.Events;
using DevLib.EventChannelSystem;
using DG.Tweening;
using UnityEngine;

namespace UI.Wall_Paper.Files.Windows
{
    public class GoBackSelectorEventEventAnim : MonoBehaviour
    {
        [SerializeField] private EventChannelSO goBackSelectorEvent;
        [SerializeField] private float animationDuration; 
        [SerializeField] private RectTransform root;

        private void Awake()
        {
            goBackSelectorEvent.AddListener<BoolEvent>(HandleImageChangeEvent);
        }

        private void HandleImageChangeEvent(BoolEvent obj)
        {
            if (!obj.Value) return;
            
            root.DOScale(Vector3.one, animationDuration);
            root.DOAnchorPos(Vector3.zero, animationDuration);
        }
        

        private void OnDestroy()
        {
            goBackSelectorEvent.RemoveListener<BoolEvent>(HandleImageChangeEvent);
        }
    }
}