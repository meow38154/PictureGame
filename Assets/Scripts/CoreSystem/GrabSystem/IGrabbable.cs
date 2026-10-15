using UnityEngine;

namespace CoreSystem.GrabSystem
{
    public interface IGrabbable
    {
        Transform Transform { get; }
        public void OnGrabbed();
        public void OnReleased();
    }
}