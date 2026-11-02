using System;
using System.Collections.Generic;
using UnityEngine;

public class NoPlayerSpawnRegion : MonoBehaviour
{
	[SerializeField]
	private Transform respawnPoint;

	[SerializeField]
	private AreaZone_PlayerCounter regionArea;

	[SerializeField]
	private GizmoBox gizmoBox;

	private void Awake()
	{
		Raft_Network.OnPlayerInitialized += OnPlayerInitialized;
		if (Raft_Network.IsHost)
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Combine(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		}
	}

	private void OnDestroy()
	{
		Raft_Network.OnPlayerInitialized -= OnPlayerInitialized;
		if (Raft_Network.IsHost)
		{
			Raft_Network.OnWorldReceivedLate = (Action)Delegate.Remove(Raft_Network.OnWorldReceivedLate, new Action(OnWorldRecievedLate));
		}
	}

	private void OnPlayerInitialized(Network_Player player)
	{
		if (player.IsLocalPlayer)
		{
			Invoke("TryToResetPlayetToRespawnPoint", 3f);
		}
	}

	private void OnWorldRecievedLate()
	{
		Invoke("TryToResetPlayetToRespawnPoint", 3f);
	}

	private void TryToResetPlayetToRespawnPoint()
	{
		List<Network_Player> playersInArea = GetPlayersInArea();
		if (playersInArea == null || !playersInArea.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < playersInArea.Count; i++)
		{
			Network_Player network_Player = playersInArea[i];
			if (!(network_Player == null) && network_Player.IsLocalPlayer)
			{
				network_Player.transform.position = respawnPoint.position;
				Debug.Log(network_Player.transform.name + " was inside of a NoPlayerSpawnRegion(" + gameObject.name + ") when loading game, moving player to respawn point");
				break;
			}
		}
	}

	private List<Network_Player> GetPlayersInArea()
	{
		if (regionArea != null && regionArea.gameObject.activeInHierarchy)
		{
			regionArea.CheckForPlayers();
			return regionArea.playersInArea;
		}
		if (gizmoBox != null && gizmoBox.gameObject.activeInHierarchy)
		{
			return gizmoBox.GetOverlaps<Network_Player>();
		}
		return null;
	}
}
