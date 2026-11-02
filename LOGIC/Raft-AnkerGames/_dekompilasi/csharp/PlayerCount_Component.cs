using System;
using UnityEngine;

public class PlayerCount_Component : MonoBehaviour
{
	[Serializable]
	public class PlayerRange
	{
		public Interval_Int lowRange;

		public Interval_Int mediumRange;

		public Interval_Int highRange;
	}

	public PlayerRange playerRange;

	protected Interval_Int currentRange;

	private static Raft_Network network;

	private void Start()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		Raft_Network.OnDisconnectEvent += UpdateRange;
		Raft_Network.OnPlayerInitialized += UpdateRange;
		UpdateRange();
	}

	public void UpdateRange(Network_Player player)
	{
		UpdateRange();
	}

	public void UpdateRange(Network_UserId steamID, DisconnectReason disconnectReason)
	{
		UpdateRange();
	}

	protected virtual void RangeUpdated(Interval_Int newRange)
	{
		currentRange = newRange;
	}

	private void UpdateRange()
	{
		int playerCount = network.PlayerCount;
		if (playerRange.highRange.IsInsideOfIntervalInclusive(playerCount))
		{
			RangeUpdated(playerRange.highRange);
		}
		else if (playerRange.mediumRange.IsInsideOfIntervalInclusive(playerCount))
		{
			RangeUpdated(playerRange.mediumRange);
		}
		else
		{
			RangeUpdated(playerRange.lowRange);
		}
	}
}
