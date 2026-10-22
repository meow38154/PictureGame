namespace MapSystem.MapGimmicks
{
    public interface IToggleable
    {
        bool IsOn { get; }
        
        void OnOffToggle();
    }
}