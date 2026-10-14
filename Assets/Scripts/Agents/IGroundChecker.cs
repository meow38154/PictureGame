using System;
using UnityEngine;

namespace Agents
{
    public interface IGroundChecker
    {
        Collider2D CurrentGround { get; }

        event Action<Collider2D> OnGroundEnter;
        event Action<Collider2D> OnGroundExit;
        
        bool IsGroundChecking();
    }
}