using System;
using UnityEngine;

namespace CoreSystem
{
    public class MouseDragMover : MonoBehaviour
    {
        private bool _isClick;

        private void OnMouseDown()
        {
            _isClick = true;
        }

        private void Update()
        {
            if (!_isClick) return;

            transform.position = Utility.GetMouseWorldPosition();
        }

        private void OnMouseUp()
        {
            _isClick = false;
        }
    }
}