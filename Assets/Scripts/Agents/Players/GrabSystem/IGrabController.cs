using UnityEngine;

namespace Agents.Players.GrabSystem
{
    public interface IGrabController
    {
        GameObject CurrentGrabbingObject { get; set; }
    }
}