using System;
using UnityEngine;
using UnityEngine.Events;

namespace MapSystem.MapGimmicks
{
    public class PressurePlateGimmick : MonoBehaviour
    {
        [SerializeField] public UnityEvent onPress;
        [SerializeField] public UnityEvent onUnpress;

        private bool _pressed = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IMapPiece piece) && !_pressed)
            {
                _pressed = true;
                ChangeState();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IMapPiece piece) && _pressed)
            {
                _pressed = false;
                ChangeState();
            }
        }

        private void ChangeState()
        {
            Debug.Log("아 :"+_pressed);
            if(_pressed)
                onPress.Invoke();
            else
                onUnpress.Invoke();
        }
    }
}