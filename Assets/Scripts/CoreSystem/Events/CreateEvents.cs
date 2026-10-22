using Agents.Players;
using CoreSystem.GrabSystem;
using DevLib.EventChannelSystem;
using MapSystem;
using UI.Wall_Paper.Files;
using UI.Wall_Paper.Files.SO;

namespace CoreSystem.Events
{
	public static class CreateEvents
	{
		public static readonly BoolEvent BoolEvent = new();
		public static readonly ControlMovementEvent ControlMovementEvent = new();
		public static readonly DirectoryFileDataEvent DirectoryFileDataEvent = new();
		public static readonly GrabbableObjectEvent GrabbableObjectEvent = new();
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

	// public class MapGridEvent : GameEvent
	// {
	// 	public IMapGrid Value { get; private set; }
	//
	// 	public MapGridEvent InitData(IMapGrid value)
	// 	{
	// 		
	// 	}
	// }
	
	public class DirectoryFileDataEvent : GameEvent
	{
		public AbstractFileSO[] Value { get; private set; }

		public DirectoryFileDataEvent InitData(AbstractFileSO[] value)
		{
			Value = value;
			return this;
		}
	}
	
	public class GrabbableObjectEvent : GameEvent
	{
		public IGrabbable Value { get; private set; }

		public GrabbableObjectEvent InitData(IGrabbable value)
		{
			Value = value;
			return this;
		}
	}
}