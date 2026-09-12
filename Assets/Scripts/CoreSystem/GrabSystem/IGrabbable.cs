using UnityEngine;

namespace CoreSystem.GrabSystem
{
    public interface IGrabbable
    {
        Transform Transform { get; }
        Vector2 Pivot { get; }
        Rigidbody2D Rigidbody { get; }
        
        void OnGrabbed();
        void OnReleased();
    }
}