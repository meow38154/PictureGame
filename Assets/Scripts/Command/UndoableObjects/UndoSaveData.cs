using UnityEngine;

namespace Command.UndoableObjects
{
    //object로 좀 더 괜찮게 만들 수 있겠다만 일단 구현이 더 중요하니 보류
    
    public struct UndoSaveData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Transform Parent;
        
        public object CustomData;
    }
}