using System;
using MapSystem;

namespace Agents.Players
{
    public interface IPartFollower
    {
        bool IsActive { get; set; }
        IMapPiece CurrentPart { get; }
    }
}