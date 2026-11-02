using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_PufferFish_Explode : AI_State
{
	[SerializeField]
	private GameObject particlePrefab;

	[SerializeField]
	private float delayBeforeExplode = 1f;

	[SerializeField]
	private float explosionRadius = 3f;

	[SerializeField]
	private float explosionDamage = 3f;

	public EventReference er_expand;

	public EventReference er_explode;

	private AI_StateMachine_PufferFish stateMachinePufferFish;

	protected override void Awake()
	{
		base.Awake();
		stateMachinePufferFish = stateMachine as AI_StateMachine_PufferFish;
	}

	private void OnDrawGizmosSelected()
	{
		if (active)
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
			Gizmos.DrawSphere(transform.position, explosionRadius);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			Explode(stateMachine.transform.position);
		}
	}

	public void Explode(Vector3 position)
	{
		if (Raft_Network.IsHost)
		{
			Message_NetworkBehaviour_ID_Vector3 message = new Message_NetworkBehaviour_ID_Vector3(Messages.PufferFish_Explode, stateMachinePufferFish.networkBehaviour.Network.NetworkIDManager, stateMachinePufferFish.networkBehaviour.ObjectIndex, stateMachine.transform.position);
			stateMachinePufferFish.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		StartCoroutine(ExplodeCoroutine(position));
	}

	private IEnumerator ExplodeCoroutine(Vector3 position)
	{
		RuntimeManager.PlayOneShot(er_expand, position);
		stateMachinePufferFish.animController.SetBool("Explode", value: true);
		yield return new WaitForSeconds(delayBeforeExplode);
		RuntimeManager.PlayOneShot(er_explode, position);
		ParticleManager.PlaySystem("PufferFish_Explode", stateMachine.transform.position, Quaternion.LookRotation(Vector3.up));
		if (particlePrefab != null)
		{
			Object.Instantiate(particlePrefab, stateMachine.transform.position, Quaternion.identity);
		}
		if (Raft_Network.IsHost)
		{
			DamageNearbyPlayers();
		}
		Object.Destroy(stateMachine.gameObject);
	}

	private void DamageNearbyPlayers()
	{
		Network_Host value = ComponentManager<Network_Host>.Value;
		if (!(value != null))
		{
			return;
		}
		List<Network_Player> list = Helper.PlayersWithinRange(stateMachine.transform.position, explosionRadius, includeDeadPlayers: false);
		if (list == null)
		{
			return;
		}
		foreach (Network_Player item in list)
		{
			if (!(item == null))
			{
				Vector3 vector = item.transform.position - stateMachine.transform.position;
				if (!Physics.Raycast(stateMachine.transform.position, vector, explosionRadius, LayerMasks.MASK_Obstruction))
				{
					value.DamageEntity(item.Stats, null, explosionDamage, item.transform.position, -vector, EntityType.Enemy);
				}
			}
		}
	}
}
