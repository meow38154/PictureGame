namespace Agents
{
    public interface IAgentFreezer
    {
        bool IsFreeze { get; }
        void Freeze();
        void UnFreeze();
    }
}