using System;
using System.Collections.Generic;
using UnityEngine;

public class AreaZone_PlayerCounter : AreaZone
{
	public enum PlayerCheckPosition
	{
		CENTER,
		FEET
	}

	public Action OnPlayersInAreaChanged;

	[SerializeField]
	private GameObject localPlayerObject;

	[SerializeField]
	private bool useInterval;

	[SerializeField]
	private float checkForPlayerInterval;

	[SerializeField]
	private PlayerCheckPosition playerCheckPosition;

	[SerializeField]
	private float checkForPlayerTimer;

	public List<Network_Player> playersInArea = new List<Network_Player>();

	[HideInInspector]
	public bool isConsideredDestroyed;

	protected Network_Player localPlayer;

	private static Raft_Network network;

	private bool isAreaActive = true;

	public bool IsConsideredActive
	{
		get
		{
			if (isAreaActive)
			{
				return !isConsideredDestroyed;
			}
			return false;
		}
	}

	public bool PlayersIsInArea
	{
		get
		{
			if (playersInArea != null)
			{
				return playersInArea.ContainsItems();
			}
			return false;
		}
	}

	protected virtual void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	protected virtual void Update()
	{
		if (localPlayer != null)
		{
			bool flag = false;
			flag = GetNormalizedDistanceToPoint(localPlayer.transform.position) > 0f;
			if (localPlayerObject != null)
			{
				localPlayerObject.SetActiveSafe(flag);
			}
		}
		if (!IsConsideredActive)
		{
			return;
		}
		if (!useInterval)
		{
			CheckForPlayers();
			return;
		}
		checkForPlayerTimer += Time.deltaTime;
		if (checkForPlayerTimer >= checkForPlayerInterval)
		{
			OnCheckForPlayersInterval();
		}
	}

	public void CheckForPlayers()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (!(network != null))
		{
			return;
		}
		bool flag = false;
		List<Network_Player> list = new List<Network_Player>();
		foreach (Network_Player value in network.remoteUsers.Values)
		{
			if (!(value != null))
			{
				continue;
			}
			Vector3 point = Vector3.zero;
			switch (playerCheckPosition)
			{
			case PlayerCheckPosition.CENTER:
				point = value.transform.position;
				break;
			case PlayerCheckPosition.FEET:
				point = value.FeetPosition;
				break;
			}
			if (GetNormalizedDistanceToPoint(point) > 0f)
			{
				list.Add(value);
				if (!playersInArea.Contains(value))
				{
					playersInArea.Add(value);
					OnAddNewPlayer(value);
					flag = true;
				}
			}
		}
		for (int i = 0; i < playersInArea.Count; i++)
		{
			Network_Player network_Player = playersInArea[i];
			if (!(network_Player == null))
			{
				if (!list.Contains(network_Player))
				{
					playersInArea.RemoveAt(i);
					OnRemovePlayer(network_Player);
					flag = true;
					i--;
				}
				else
				{
					_ = network_Player.IsLocalPlayer;
				}
			}
		}
		if (flag)
		{
			OnPlayersInAreaChanged?.Invoke();
		}
	}

	public void SetAreaActive(bool value)
	{
		isAreaActive = value;
	}

	public Network_Player GetLocalPlayerInArea()
	{
		if (!PlayersIsInArea)
		{
			return null;
		}
		foreach (Network_Player item in playersInArea)
		{
			if (item.IsLocalPlayer)
			{
				return item;
			}
		}
		return null;
	}

	protected virtual void OnAddNewPlayer(Network_Player player)
	{
	}

	protected virtual void OnRemovePlayer(Network_Player player)
	{
	}

	protected virtual void OnCheckForPlayersInterval()
	{
		checkForPlayerTimer = 0f;
		CheckForPlayers();
	}
}
