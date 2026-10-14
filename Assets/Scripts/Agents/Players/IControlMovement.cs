namespace Agents.Players
{
    public interface IControlMovement
    {
        bool Freeze { get; }
        void SetMovementDirectionX(float movementXInput);
        void UpdateFacingDirection(float movementXKey);
         bool PlayerIsStop();
    }
}