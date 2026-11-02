using System;
using UnityEngine;

public class AI_NetworkBehaviour_BugSwarm : AI_NetworkBehaviour_Animal
{
	private TimerEventer damageTimer;

	public float damageRadius = 5f;

	public float damageFrequency = 10f;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (damageTimer != null)
		{
			damageTimer.OnTimerReached = null;
			damageTimer = null;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (Raft_Network.IsHost)
		{
			damageTimer = new TimerEventer(damageFrequency + UnityEngine.Random.Range(-1f, 1f));
			TimerEventer timerEventer = damageTimer;
			timerEventer.OnTimerReached = (Action)Delegate.Combine(timerEventer.OnTimerReached, new Action(DamageClosestPlayer));
		}
	}

	protected override void UpdateHost()
	{
		base.UpdateHost();
		if (damageTimer != null)
		{
			damageTimer.DoUpdate(Time.deltaTime, autoResetTimer: true);
		}
	}

	private void DamageClosestPlayer()
	{
		Network_Player network_Player = Helper.ClosestPlayerToPoint(transform.position, 5f, includeDeadPlayers: false);
		if (network_Player != null)
		{
			network.hostNetwork.DamageEntity(network_Player.Stats, transform, 5f, network_Player.transform.position, Vector3.up, EntityType.Enemy);
		}
	}
}
