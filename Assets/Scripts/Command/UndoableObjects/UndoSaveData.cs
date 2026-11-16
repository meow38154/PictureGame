using UnityEngine;

namespace Command.UndoableObjects
{
    public struct UndoSaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Transform Parent;
        
        public object CustomData;
    }
}