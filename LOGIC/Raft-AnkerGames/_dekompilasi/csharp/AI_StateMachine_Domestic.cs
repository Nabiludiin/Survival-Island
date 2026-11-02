using FMODUnity;
using UnityEngine;

public class AI_StateMachine_Domestic : AI_StateMachine_Animal
{
	public EventReference er_distressed;

	public EventReference er_eatGrass;

	[HideInInspector]
	public AI_NetworkBehaviour_Domestic networkBehaviourDomestic;

	public override void Awake()
	{
		base.Awake();
		networkBehaviourDomestic = networkBehaviour as AI_NetworkBehaviour_Domestic;
	}

	public virtual void Animation_Eat()
	{
		RuntimeManager.PlayOneShot(er_eatGrass, transform.position);
	}

	protected override void OnDeath(EntityType damageType)
	{
		base.OnDeath(damageType);
		if (Raft_Network.IsHost && networkBehaviourDomestic != null && networkBehaviourDomestic.carryScript.IsBeingCarried)
		{
			networkBehaviourDomestic.carryScript.OnStopCarry(networkBehaviourDomestic.carryScript.carryingPlayer, arg2: false);
		}
	}

	protected override void OnSwitchDamageTakenState()
	{
		if (networkBehaviourDomestic != null && networkBehaviourDomestic.DomesticState == DomesticStateType.Landmark && !networkBehaviourDomestic.carryScript.IsBeingCarried && !networkBehaviourDomestic.captureScript.IsCaptured)
		{
			base.OnSwitchDamageTakenState();
		}
	}

	protected override void EnablePickup()
	{
		if (!networkBehaviourDomestic.IsTamed || string.IsNullOrEmpty(networkBehaviourDomestic.nameTag.GetText()))
		{
			networkBehaviour.pickupItemChanneling.SetPickupColliderState(state: false);
		}
		else
		{
			base.EnablePickup();
		}
	}

	protected override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		if (Raft_Network.IsHost && transform.parent == null && networkBehaviourDomestic != null && networkBehaviourDomestic.IsTamed && networkBehaviourDomestic.lockedPivot != null && networkBehaviourDomestic.lockedPivot.position.DistanceXZ(transform.position) >= (float)ChunkManager.ChunkSize)
		{
			NetworkIDManager.SendIDBehaviourDead(networkBehaviourDomestic.ObjectIndex, typeof(AI_NetworkBehaviour), removeInstantly: true);
		}
	}
}
