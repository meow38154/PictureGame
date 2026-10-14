namespace MapSystem.MapGimmicks
{
    public interface IToggleable
    {
        bool Current { get; }
        
        void Toggle();
    }
}