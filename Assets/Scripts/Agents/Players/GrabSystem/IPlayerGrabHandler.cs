using CoreSystem.GrabSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public interface IPlayerGrabHandler
    {
        bool IsGrabbing { get; }
        void Grab(IGrabbable target, Transform handTrm);
        void Release();
    }
}