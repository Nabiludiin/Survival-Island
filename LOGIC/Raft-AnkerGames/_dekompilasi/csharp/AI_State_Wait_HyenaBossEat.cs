using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class AI_State_Wait_HyenaBossEat : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	public Transform meatTargetPoint;

	[SerializeField]
	private float eatDuration;

	[SerializeField]
	private float rotationSpeed;

	public EventReference er_eat;

	private AI_StateMachine_HyenaBoss bossSM;

	private Boss_OlofPart2 olof;

	private Coroutine eatRoutine;

	protected override void Awake()
	{
		base.Awake();
		bossSM = stateMachine as AI_StateMachine_HyenaBoss;
	}

	private void OnDestroy()
	{
		if (eatRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(eatRoutine);
			eatRoutine = null;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (!(olof == null))
		{
			return;
		}
		Transform objectOfInterest = ObjectManager.GetObjectOfInterest(ObjectOfInterestType.Utopia_BossOlofPart2);
		if (objectOfInterest != null)
		{
			olof = objectOfInterest.GetComponent<Boss_OlofPart2>();
			if (olof != null)
			{
				olof.hyenaState = this;
				Boss_OlofPart2 boss_OlofPart = olof;
				boss_OlofPart.OnThrowMeat = (Action)Delegate.Combine(boss_OlofPart.OnThrowMeat, new Action(OnOlofThrowMeat));
			}
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (Raft_Network.IsHost)
		{
			Vector3 position = stateMachine.transform.position;
			AreaZone bossZone = bossSM.GetBossZone();
			if (bossZone != null)
			{
				position.y = bossZone.transform.position.y;
			}
			stateMachine.transform.position = position;
		}
		stateMachine.movement.SetMovementType(MovementType.None);
		stateMachine.movement.SetMovementSpeed(0f);
		stateMachine.movement.SetRotationSpeed(rotationSpeed);
		meatTargetPoint.localScale = Vector3.one;
		if (olof != null)
		{
			olof.ResetThrow();
		}
		if (eatRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(eatRoutine);
			eatRoutine = null;
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (eatRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(eatRoutine);
			eatRoutine = null;
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (Raft_Network.IsHost && olof != null && eatRoutine == null)
		{
			stateMachine.movement.Look(stateMachine.movement.model, olof.transform.position, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			if (Mathf.Abs(Vector3.Angle((olof.transform.position - stateMachine.transform.position).XZOnly(), stateMachine.movement.model.forward.XZOnly())) < 10f)
			{
				olof.StartThrowingMeat();
			}
		}
	}

	private void OnOlofThrowMeat()
	{
		if (active)
		{
			if (eatRoutine != null)
			{
				CoroutineManager.Singleton.StopCoroutine(eatRoutine);
			}
			eatRoutine = CoroutineManager.Singleton.AddCoroutine(Eat());
		}
	}

	private IEnumerator Eat()
	{
		if (Raft_Network.IsHost)
		{
			animatorController.SetTrigger("Eat");
		}
		RuntimeManager.PlayOneShot(er_eat, stateMachine.transform.position);
		yield return new WaitForSeconds(eatDuration);
		olof.meat.gameObject.SetActiveSafe(value: false);
		if (Raft_Network.IsHost)
		{
			stateMachine.ChangeState(nextState);
		}
		eatRoutine = null;
	}
}
