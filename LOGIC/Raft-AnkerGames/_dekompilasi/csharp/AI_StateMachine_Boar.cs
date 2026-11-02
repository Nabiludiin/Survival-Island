using FMODUnity;
using UnityEngine;

public class AI_StateMachine_Boar : AI_StateMachine_Animal
{
	private Vector3 _chargeDirection = Vector3.zero;

	private bool _isCurrentlyFearless;

	private int currentFearlessStacks;

	public AttachPlayer attachPlayer;

	public AI_State defaultState;

	public Network_Player enemyTarget;

	public EventReference er_ChargeLoad;

	public EventReference er_ChargeUpDone;

	public EventReference er_Charge;

	public EventReference er_ChargeHit;

	public EventReference er_HitWall;

	public EventReference er_takeDamageNoSqueal;

	public Vector3 ChargeDirection
	{
		get
		{
			return _chargeDirection;
		}
		set
		{
			_chargeDirection = value;
		}
	}

	public bool IsCurrentlyFearless
	{
		get
		{
			if (currentFearlessStacks > 0)
			{
				return true;
			}
			return _isCurrentlyFearless;
		}
	}

	public override void Start()
	{
		base.Start();
		eventRef_footStep.SetParameter("material", 2f);
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		if (currentState != deadState && currentState != decayState)
		{
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
		}
	}

	private void Animation_ChargeLoad()
	{
		RuntimeManager.PlayOneShot(er_ChargeLoad, transform.position);
	}

	protected override void Animation_Footstep()
	{
		eventRef_footStep.Play();
	}

	public bool IsStillAlive()
	{
		if (currentState != deadState)
		{
			return !networkBehaviour.networkEntity.IsDead;
		}
		return false;
	}

	protected override void Animation_Damage(float damage)
	{
		if (IsCurrentlyFearless)
		{
			RuntimeManager.PlayOneShot(er_takeDamageNoSqueal, transform.position);
		}
		else
		{
			base.Animation_Damage(damage);
		}
	}

	protected override void OnSwitchDamageTakenState()
	{
		if (!IsCurrentlyFearless)
		{
			base.OnSwitchDamageTakenState();
		}
	}

	public void SetIsCurrentlyFearless(bool value, int fearlessStacks = 0)
	{
		_isCurrentlyFearless = value;
		SetFearlessStacks(fearlessStacks);
	}

	public void SetFearlessStacks(int stacks)
	{
		currentFearlessStacks = stacks;
	}

	public void ModifyFearlessStacks(int stacks)
	{
		currentFearlessStacks += stacks;
	}
}
