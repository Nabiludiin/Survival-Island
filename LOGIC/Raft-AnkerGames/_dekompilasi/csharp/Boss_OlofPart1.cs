using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss_OlofPart1 : MonoBehaviour_ID_Network
{
	[SerializeField]
	private Dialogue dialogue;

	[SerializeField]
	private LandmarkEntityCollection entityCollection;

	[SerializeField]
	private QuestEventBase olofExitInteractable;

	[SerializeField]
	private TimerEventer spawnHyenaTimer;

	[SerializeField]
	private TimerEventer timerCheckForSpawnCondition;

	public int deathDifferenceRequiredToSpawn;

	private bool canSpawn;

	private List<LandmarkEntitySpawner> availableSpawners = new List<LandmarkEntitySpawner>();

	private Raft_Network network;

	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			timerCheckForSpawnCondition.Initialize();
			TimerEventer timerEventer = timerCheckForSpawnCondition;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(CheckSpawnCondition));
			spawnHyenaTimer.Initialize();
			TimerEventer timerEventer2 = spawnHyenaTimer;
			timerEventer2.OnTimerReached = (Action)Delegate.Combine(timerEventer2.OnTimerReached, new Action(ActivateRespawn));
		}
		network = ComponentManager<Raft_Network>.Value;
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			timerCheckForSpawnCondition.DoUpdate(Time.deltaTime, autoResetTimer: true);
			if (canSpawn)
			{
				spawnHyenaTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
			}
		}
	}

	private void ActivateRespawn()
	{
		ActivateSpawn(availableSpawners);
	}

	private void ActivateSpawn(List<LandmarkEntitySpawner> spawners)
	{
		foreach (LandmarkEntitySpawner spawner in spawners)
		{
			spawner.ManuallySpawn();
		}
		if (dialogue != null)
		{
			dialogue.PlayNextVoice_Networked(null);
		}
	}

	private void CheckSpawnCondition()
	{
		if (olofExitInteractable != null && olofExitInteractable.CurrentObjectStateIndex > 0)
		{
			availableSpawners.Clear();
			SetCanSpawn(value: false);
			return;
		}
		availableSpawners = entityCollection.GetSpawnersWhichHaventSpawned();
		if (Mathf.Abs(availableSpawners.Count - entityCollection.spawners.Length) <= deathDifferenceRequiredToSpawn)
		{
			SetCanSpawn(value: true);
		}
		else
		{
			SetCanSpawn(value: false);
		}
	}

	private void SetCanSpawn(bool value)
	{
		if (canSpawn != value)
		{
			canSpawn = value;
			spawnHyenaTimer.ResetTimer();
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.Dialogue)
		{
			Message_Dialogue message_Dialogue = msg as Message_Dialogue;
			dialogue.PlayVoice(message_Dialogue, network.GetPlayerFromID(message_Dialogue.SteamID));
			return true;
		}
		return base.Deserialize(msg, remoteID);
	}
}
