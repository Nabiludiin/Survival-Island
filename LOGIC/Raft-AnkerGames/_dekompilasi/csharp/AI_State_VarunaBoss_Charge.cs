using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_VarunaBoss_Charge : AI_State_Charge
{
	public AnimatorController animCont;

	public EventReference er_hitWall;

	private AI_StateMachine_Boss_Varuna bossStatemachine;

	protected override void Start()
	{
		base.Start();
		bossStatemachine = stateMachine as AI_StateMachine_Boss_Varuna;
	}

	public override void EnterState()
	{
		base.EnterState();
		animCont.SetBool("Charging", value: true);
	}

	protected override bool ChargeCompleted(RaycastHit hitTowards)
	{
		base.ChargeCompleted(hitTowards);
		if (hitTowards.collider != null)
		{
			if (Raft_Network.IsHost)
			{
				bossStatemachine.HitSolidWithChargeFeedback();
				Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.Charge_Hit, bossStatemachine.networkBehaviour.Network.NetworkIDManager, bossStatemachine.networkBehaviour.ObjectIndex);
				bossStatemachine.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			if (hitTowards.transform.tag == "Varuna_Boss_Pillar")
			{
				QuestInteractable_VarunaPillar component = hitTowards.collider.GetComponent<QuestInteractable_VarunaPillar>();
				if (component.CurrentObjectStateIndex != component.placeBarrelIndex)
				{
					component.SendOnInteractNetwork(null, forceSuccess: true);
					if (component.killsBoss && component.IsAtMaxStateIndex)
					{
						bossStatemachine.networkBehaviour.networkEntity.IsInvurnerable = false;
						ComponentManager<Network_Host>.Value.DamageEntity(bossStatemachine.networkBehaviour.networkEntity, bossStatemachine.networkBehaviour.transform, 999999f, Vector3.zero, Vector3.up, EntityType.Player);
						return false;
					}
					bossStatemachine.seePlayer_component.SetCooldownTimeStamp(bossStatemachine.maxChargeCooldown - Vector3.Distance(stateMachine.transform.position, bossStatemachine.enemyTarget.FeetPosition) / 6f);
					bossStatemachine.waypointCirculation.RandomNewOuterCirculationCount();
				}
				else
				{
					bossStatemachine.seePlayer_component.SetCooldownTimeStamp(bossStatemachine.maxChargeCooldown - Vector3.Distance(stateMachine.transform.position, bossStatemachine.enemyTarget.FeetPosition) / 3f);
				}
			}
			else
			{
				bossStatemachine.seePlayer_component.SetCooldownTimeStamp(bossStatemachine.maxChargeCooldown - Vector3.Distance(stateMachine.transform.position, bossStatemachine.enemyTarget.FeetPosition) / 3f);
				RuntimeManager.PlayOneShot(er_hitWall, hitTowards.point);
			}
		}
		else
		{
			bossStatemachine.seePlayer_component.SetCooldownTimeStamp(Random.Range(1f, 2f));
		}
		return true;
	}

	protected override void HitPlayerLocally(Network_Player player)
	{
		base.HitPlayerLocally(player);
		bossStatemachine.waypointCirculation.RandomNewOuterCirculationCount();
	}

	protected override Vector3 GetPlayerTargettingVector()
	{
		MovementDeltaTracker enemyTargetDeltaTracker = bossStatemachine.GetEnemyTargetDeltaTracker();
		if (enemyTargetDeltaTracker != null)
		{
			return enemyTargetDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(Vector3.Distance(stateMachine.transform.position, bossStatemachine.enemyTarget.transform.position) / 3.8f, normalizeDirection: false);
		}
		return stateMachine.transform.position;
	}
}
