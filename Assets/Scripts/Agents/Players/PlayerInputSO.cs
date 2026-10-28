using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Agents.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/Player input", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        [SerializeField] private float doubleClickInterval = 0.5f;
        [SerializeField] private float doubleClickMaxDistance = 2f;
        public event Action OnMouseLeftButtonPressed;
        public event Action OnMouseLeftDoubleButtonPressed;
        public event Action OnMouseLeftButtonReleased;
        public event Action OnJumpKeyPressed;
        public event Action OnJumpKeyReleased;        
        public event Action OnLCtrlKeyPressed;
        public event Action OnLCtrlKeyReleased;        
        public event Action OnZKeyPressed;
        public event Action OnZKeyReleased;       
        public event Action OnYKeyPressed;
        public event Action OnYKeyReleased;

        private Controls _controls;
        
        private bool _canDoubleClick;
        private float _lastClickTime;
        private Vector2 _prevMouseScreenPos;
        
        public Vector2 MovementKey { get; private set; }

        public bool CanMovement { get; set; } = true;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            if(_controls != null)
                _controls.Player.Disable();
        }
        
        
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!CanMovement)
            {
                MovementKey = Vector2.zero;
                return;
            }
            MovementKey = context.ReadValue<Vector2>(); 
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnJumpKeyPressed?.Invoke();            
            if (context.canceled)
                OnJumpKeyReleased?.Invoke();
        }

        public void OnCapture(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnMouseLeftButtonPressed?.Invoke();

                Vector2 currentMousePos = Utility.GetMouseScreenPosition();
                float currentTime = Time.unscaledTime;

                if (_canDoubleClick && currentTime - _lastClickTime < doubleClickInterval && 
                    Vector2.Distance(_prevMouseScreenPos, currentMousePos) < doubleClickMaxDistance)
                {
                    OnMouseLeftDoubleButtonPressed?.Invoke();
                    _canDoubleClick = false;
                }
                else
                {
                    _canDoubleClick = true;
                    _lastClickTime = currentTime;
                    _prevMouseScreenPos = currentMousePos;
                }
            }

            if (context.canceled)
                OnMouseLeftButtonReleased?.Invoke();
        }

        public void OnLeftCtrl(InputAction.CallbackContext context)
        {
            if (context.performed) OnLCtrlKeyPressed?.Invoke();
            if (context.canceled) OnLCtrlKeyReleased?.Invoke();
        }

        public void OnZ(InputAction.CallbackContext context)
        {
            if (context.performed) OnZKeyPressed?.Invoke();
            if (context.canceled) OnZKeyReleased?.Invoke();
        }

        public void OnY(InputAction.CallbackContext context)
        {
            if (context.performed) OnYKeyPressed?.Invoke();
            if (context.canceled) OnYKeyReleased?.Invoke();
        }
    }
}