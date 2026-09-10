using UnityEngine;

namespace CoreSystem.GrabSystem
{
    public interface IGrabbable
    {
        Transform Transform { get; }
        Transform Pivot { get; }
        Rigidbody2D Rigidbody { get; }
        
        void OnGrabbed();
        void OnReleased();
    }
}