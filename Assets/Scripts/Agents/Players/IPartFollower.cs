using System;
using MapSystem;

namespace Agents.Players
{
    public interface IPartFollower
    {
        IMapPiece CurrentPart { get; }
    }
}