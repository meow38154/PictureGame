using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI
{
    public class PanelToggleActive : MonoBehaviour
    {
        [SerializeField] public GameObject toggleObject;
        [SerializeField] public UnityEvent OnEnable;
        [SerializeField] public UnityEvent OnDisable;
        
        private void Update()
        {
            if (!Keyboard.current.escapeKey.wasPressedThisFrame) return;
            
            Toggle();
        }

        public void Toggle()
        {
            bool data = !toggleObject.activeSelf;
                
            toggleObject.SetActive(data);
                
            if (data) OnEnable?.Invoke();
            else OnDisable?.Invoke();
        }
    }
}