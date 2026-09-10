using CoreSystem.GrabSystem;

namespace Agents.Players.GrabSystem
{
    public interface IGrabFinder
    {
        bool TryFindObject(out IGrabbable grabbable);
    }
}