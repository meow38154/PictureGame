namespace Agents.Players
{
    public interface IControlMovement
    {
        bool CanControl { get; }
        void SetMovementDirectionX(float movementXInput);
        void UpdateFacingDirection(float movementXKey);
    }
}