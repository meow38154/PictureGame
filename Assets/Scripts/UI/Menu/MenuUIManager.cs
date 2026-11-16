using System;
using Common.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class MenuUIManager : MonoBehaviour
    {
        [SerializeField] private RectTransform selectObject;
        [ReadOnly, SerializeField] private UIButton currentSelectButton;

        private UIButton[] _buttonListAll;
        
        //[SerializeField] private Vector2 selectObjectCorrection;
        public UIButton CurrentSelectButton
        {
            get => currentSelectButton;
            set
            {
                if (currentSelectButton == value) return;
                
                currentSelectButton = value;
                selectObject.position = currentSelectButton.GetComponent<RectTransform>().position;
            }
        }
        private void Start()
        {
            _buttonListAll = GetComponentsInChildren<UIButton>(true);
            //_buttonListActive = _buttonListAll.Where(obj => obj != null && obj.gameObject.activeSelf).ToArray();

            if (_buttonListAll.Length <= 0) return;
            
            currentSelectButton = _buttonListAll[0];
        }

        private void Update()
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                HandleTextUISelectionChanged(false);
            }
            
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                HandleTextUISelectionChanged(true);
            }

            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                currentSelectButton.Interact();
            }
        }

        private void HandleTextUISelectionChanged(bool isDown)
        {
            int currentIndex = Array.IndexOf(_buttonListAll, currentSelectButton);
            int count = _buttonListAll.Length;
            
            int nextIndex = (currentIndex + (isDown ? 1 : -1) + count) % count;

            CurrentSelectButton = _buttonListAll[nextIndex];
        }
    }
}