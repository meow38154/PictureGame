using Agents.Players;
using DevLib.EventChannelSystem;

namespace CoreSystem.Events
{
	public static class CreateEvents
	{
		public static readonly BoolEvent BoolEvent = new();
	}

	public class BoolEvent : GameEvent
	{
		public bool Value { get; private set; }

		public BoolEvent InitData(bool value)
		{
			Value = value;
			return this;
		}
	}	
	
	public class ControlMovementEvent : GameEvent
	{
		public IControlMovement Value { get; private set; }

		public ControlMovementEvent InitData(IControlMovement value)
		{
			Value = value;
			return this;
		}
	}
}