using System;
using MapSystem;

namespace Agents.Players
{
    public interface IPartDetector
    {
        event Action<IMapPart> OnPartChangeEvent;
        IMapPart CurrentPart { get; }
    }
}