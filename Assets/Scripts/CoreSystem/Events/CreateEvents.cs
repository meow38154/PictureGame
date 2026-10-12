using DevLib.EventChannelSystem;
using DevLib.ObjectPool.Runtime;
using MapSystem;
using UnityEngine;

namespace CoreSystem.Events
{
	public static class CreateEvents
	{
		public static readonly PlayerOnGroundCheck PlayerOnGroundCheck = new();
	}

	public class PlayerOnGroundCheck : GameEvent
	{
		public bool IsFrozen { get; private set; }

		public PlayerOnGroundCheck InitData(bool isFrozen)
		{
			IsFrozen = isFrozen;
			return this;
		}
	}
}