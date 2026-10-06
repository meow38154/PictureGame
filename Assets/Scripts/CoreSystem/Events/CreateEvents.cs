using DevLib.EventChannelSystem;
using DevLib.ObjectPool.Runtime;
using MapSystem;
using UnityEngine;

namespace CoreSystem.Events
{
	public static class CreateEvents
	{
		public static readonly ShowPoolingVfx ShowPoolingVfx = new ShowPoolingVfx();
		public static readonly CurrentPlayerPartChanged CurrentPlayerPartChanged = new();
		public static readonly PlayerTransformRegistered PlayerTransformRegistered = new();
	}

	public class ShowPoolingVfx : GameEvent
	{
		public PoolItemSO ItemData { get; private set; }
		public Vector3 Position { get; private set; }
		public Quaternion Rotation { get; private set; }

		public ShowPoolingVfx InitData(PoolItemSO  itemData, Vector3 position, Quaternion rotation)
		{
			ItemData = itemData;
			Position = position;
			Rotation = rotation;
			return this;
		}
	}
	
	public class CurrentPlayerPartChanged : GameEvent
	{
		public MapPart CurrentPart { get; private set; }
		public Transform PlayerTrm { get; private set; }

		public CurrentPlayerPartChanged InitData(MapPart currentPart, Transform playerTrm)
		{
			PlayerTrm = playerTrm;
			CurrentPart = currentPart;
			return this;
		}
	}

	public class PlayerTransformRegistered : GameEvent
	{
		public Transform PlayerTrm { get; private set; }

		public PlayerTransformRegistered InitData(Transform playerTrm)
		{
			PlayerTrm = playerTrm;
			return this;
		}
	}
}