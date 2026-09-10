using CoreSystem.GrabSystem;
using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public interface IGrabMover
    {
        void MoveGrabbable(IGrabbable grabbable, Vector2 position);
    }
}