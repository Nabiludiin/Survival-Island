using System;
using FMODUnity;
using UnityEngine;

public class AI_StateMachine_StoneBird : AI_StateMachine_Animal
{
	public AI_State_StoneBird_DropStone dropStoneState;

	public ParticleSystem featherParticles;

	public Network_Player target;

	public EventReference er_land;

	public EventReference er_wingFlap;

	public EventReference er_attack;

	private float changeAnimFlySpeedDelay;

	private float changeAnimFlySpeedTimer;

	private float targetAnimflySpeed;

	private float flySpeed;

	public override void Start()
	{
		base.Start();
		AI_State_AirLand_AwayFromPlayer componentInChildren = GetComponentInChildren<AI_State_AirLand_AwayFromPlayer>();
		componentInChildren.OnFinishedLanding = (Action)Delegate.Combine(componentInChildren.OnFinishedLanding, new Action(OnFinishedLanding));
		if (Raft_Network.IsHost)
		{
			AI_State_AirLand_AwayFromPlayer componentInChildren2 = GetComponentInChildren<AI_State_AirLand_AwayFromPlayer>();
			componentInChildren2.OnStartLanding = (Action)Delegate.Combine(componentInChildren2.OnStartLanding, new Action(OnStartLanding));
			changeAnimFlySpeedDelay = 15f;
		}
		else
		{
			GetComponent<AI_Movement_Lean>().enabled = false;
		}
	}

	protected override void UpdateStateMachine()
	{
		base.UpdateStateMachine();
		SetAnimFlySpeed();
	}

	private void OnStartLanding()
	{
		animController.SetTrigger("Break");
		animController.SetFloat("FlySpeed", 1f);
		changeAnimFlySpeedTimer = 0f;
		changeAnimFlySpeedDelay = 15f;
	}

	private void OnFinishedLanding()
	{
		animController.SetBool("Flying", value: false);
		RuntimeManager.PlayOneShot(er_land, transform.position);
		ParticleManager.PlaySystem("Vulture_Land", transform.position, Quaternion.Euler(Vector3.up));
	}

	protected override void OnDamageTaken(float damage, Vector3 hitPoint, Vector3 hitNormal, EntityType damageType, bool isLocal)
	{
		SpawnFeathers();
		if (currentState != deadState && currentState != decayState)
		{
			base.OnDamageTaken(damage, hitPoint, hitNormal, damageType, isLocal);
		}
	}

	public void SpawnFeathers()
	{
		if (featherParticles != null)
		{
			featherParticles.Play();
		}
	}

	public void Animation_WingFlap()
	{
		RuntimeManager.PlayOneShot(er_wingFlap, transform.position);
	}

	private void SetAnimFlySpeed()
	{
		changeAnimFlySpeedTimer += Time.deltaTime;
		if (changeAnimFlySpeedTimer >= changeAnimFlySpeedDelay)
		{
			changeAnimFlySpeedDelay = UnityEngine.Random.Range(7.5f, 15f);
			changeAnimFlySpeedTimer = 0f;
			targetAnimflySpeed = UnityEngine.Random.value;
		}
		if (flySpeed != targetAnimflySpeed)
		{
			flySpeed = Mathf.MoveTowards(flySpeed, targetAnimflySpeed, Time.deltaTime);
			animController.SetFloat("FlySpeed", flySpeed);
		}
	}
}
