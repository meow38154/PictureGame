using System;
using UnityEngine;

namespace CoreSystem
{
    public class WorldTrmLock : MonoBehaviour
    {
        [SerializeField] private bool lockPosition;
        [SerializeField] private Vector3 position;

        [SerializeField] private bool lockRotation;
        [SerializeField] private Quaternion rotation;
        
        [SerializeField] private bool lockScale;
        [SerializeField] private Vector3 scale;

        private void LateUpdate()
        {
            if (lockPosition) transform.position = position;
            if (lockRotation) transform.rotation = rotation;
            if (lockScale) transform.localScale = scale;
        }
    }
}