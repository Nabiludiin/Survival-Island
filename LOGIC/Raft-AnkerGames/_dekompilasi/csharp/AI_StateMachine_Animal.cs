using System;
using FMODUnity;
using UnityEngine;

public class AI_StateMachine_Animal : AI_StateMachine
{
	public AI_NetworkBehaviour_Animal networkBehaviour;

	public AnimatorController animController;

	public AI_State deadState;

	public AI_State decayState;

	public AI_State damageTakenState;

	public PickupItem pickupItem;

	[SerializeField]
	private Transform takeDamageParticleParent;

	[SerializeField]
	private string takeDamageParticleName = "Blood_Spear";

	[SerializeField]
	private string deflectDamageParticleName = "Particle_DamageDeflect";

	[SerializeField]
	public StudioEventEmitter eventRef_footStep;

	public EventReference er_idle;

	public EventReference er_die;

	public EventReference er_takeDamage;

	public EventReference er_deflectDamage;

	[Tooltip("Will animation be played from animation event or just immedietly when taking damage")]
	[SerializeField]
	protected bool playDamageFromAnimation;

	private AnimatorStorage animatorStorage;

	public override void Start()
	{
		base.Start();
		if (pickupItem != null && pickupItem.networkID != null)
		{
			pickupItem.networkID.ObjectIndex = networkBehaviour.ObjectIndex;
			NetworkIDManager.AddNetworkID(pickupItem.networkID, typeof(PickupItem_Networked));
			if (Raft_Network.IsHost)
			{
				PickupItem_Networked networkID = pickupItem.networkID;
				networkID.OnPickedUp = (Action)Delegate.Combine(networkID.OnPickedUp, new Action(OnPickedUp));
			}
		}
		networkBehaviour.networkEntity.OnDamageTakenEvent += OnDamageTaken;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (pickupItem != null && pickupItem.networkID != null)
		{
			NetworkIDManager.RemoveNetworkID(pickupItem.networkID);
			if (Raft_Network.IsHost)
			{
				PickupItem_Networked networkID = pickupItem.networkID;
				networkID.OnPickedUp = (Action)Delegate.Remove(networkID.OnPickedUp, new Action(OnPickedUp));
			}
		}
		networkBehaviour.networkEntity.OnDamageTakenEvent -= OnDamageTaken;
	}

	private void OnEnable()
	{
		if (animController != null && animController.animator != null)
		{
			animController.animator.LoadAnimatorParameters(animatorStorage);
		}
	}

	private void OnDisable()
	{
		if (animController != null && animController.animator != null)
		{
			animController.animator.SaveAnimatorParameters(ref animatorStorage);
		}
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		if (eventRef_footStep != null)
		{
			float num = animController.GetFloat("Speed");
			eventRef_footStep.SetParameter("speed", num / 2f);
		}
	}

	protected virtual void OnPickedUp()
	{
		if (Raft_Network.IsHost)
		{
			NetworkIDManager.SendIDBehaviourDead(networkBehaviour.ObjectIndex, typeof(AI_NetworkBehaviour));
		}
	}

	protected virtual void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (damageType != EntityType.None)
		{
			if (damage > 0f && !string.IsNullOrEmpty(takeDamageParticleName))
			{
				GameObject gameObject = ParticleManager.PlaySystem(takeDamageParticleName, hitPoint, Quaternion.LookRotation(hitNormal));
				if (takeDamageParticleParent != null)
				{
					gameObject.transform.SetParentSafe(takeDamageParticleParent);
				}
			}
			else if (networkBehaviour.networkEntity != null && networkBehaviour.networkEntity.IsInvurnerable && !string.IsNullOrEmpty(deflectDamageParticleName))
			{
				ParticleManager.PlaySystem(deflectDamageParticleName, hitPoint, Quaternion.LookRotation(hitNormal));
			}
		}
		if (!(currentState != deadState))
		{
			return;
		}
		if (Raft_Network.IsHost && damageType != EntityType.None && damage > 0f && animController != null)
		{
			animController.SetTrigger("TakeDamage");
		}
		if (!playDamageFromAnimation)
		{
			Animation_Damage(damage);
		}
		if (networkBehaviour.networkEntity.IsDead)
		{
			OnDeath(damageType);
			EnablePickup();
			if (Raft_Network.IsHost)
			{
				ChangeState(deadState);
			}
		}
		else if (Raft_Network.IsHost && currentState != null && !currentState.IsFearlessInState)
		{
			OnSwitchDamageTakenState();
		}
	}

	protected virtual bool ShouldAddAchievementProgressOnDeath(EntityType dmgDealerEntityType)
	{
		if (dmgDealerEntityType == EntityType.Player)
		{
			return true;
		}
		return false;
	}

	protected virtual void EnablePickup()
	{
		if (networkBehaviour.pickupItemChanneling != null)
		{
			networkBehaviour.pickupItemChanneling.SetPickupColliderState(state: true);
		}
	}

	protected virtual void Animation_Footstep()
	{
		eventRef_footStep.Play();
		eventRef_footStep.SetParameter("material", movement.GroundMaterial);
	}

	protected virtual void Animation_Damage(float damage)
	{
		if (damage > 0f)
		{
			RuntimeManager.PlayOneShotAttached(er_takeDamage, gameObject);
		}
		else if (networkBehaviour.networkEntity != null && networkBehaviour.networkEntity.IsInvurnerable)
		{
			RuntimeManager.PlayOneShotAttached(er_deflectDamage, gameObject);
		}
	}

	protected virtual void Animation_IdleSound(float chance)
	{
		if (UnityEngine.Random.Range(0f, 1f) < chance)
		{
			RuntimeManager.PlayOneShotAttached(er_idle, gameObject);
		}
	}

	protected virtual void OnDeath(EntityType damageType)
	{
		if (ShouldAddAchievementProgressOnDeath(damageType))
		{
			AchievementHandler.AddEntityKillByType(networkBehaviour.behaviourType);
		}
		RuntimeManager.PlayOneShot(er_die, transform.position);
	}

	protected virtual void OnSwitchDamageTakenState()
	{
		if (damageTakenState != null && currentState != damageTakenState && currentState != deadState)
		{
			ChangeState(damageTakenState);
		}
	}

	protected override void OnWorldShift(Vector3 shift)
	{
		base.OnWorldShift(shift);
		if (networkBehaviour != null)
		{
			networkBehaviour.networkPosition = networkBehaviour.transform.position;
		}
	}
}
