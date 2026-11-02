using System.Collections;
using FMODUnity;
using UnityEngine;

public class AI_State_StoneBird_GrabStone : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private float grabStoneTime = 1f;

	[SerializeField]
	private float liftOffDelay = 1f;

	public float grabStoneTimer;

	public EventReference er_pullStoneFinish;

	public EventReference er_pullStone;

	[SerializeField]
	private float pullStoneSoundInterval = 1f;

	private float pullStoneSoundTimer;

	private AI_StateMachine_StoneBird stateMachineStoneBird;

	private Coroutine lifeStoneRutine;

	protected override void Awake()
	{
		base.Awake();
		stateMachineStoneBird = stateMachine as AI_StateMachine_StoneBird;
	}

	public override void UpdateState()
	{
		base.UpdateState();
		grabStoneTimer += Time.deltaTime;
		if (grabStoneTimer >= grabStoneTime)
		{
			grabStoneTimer = 0f;
			lifeStoneRutine = StartCoroutine(GrabStone(liftOffDelay));
		}
		pullStoneSoundTimer += Time.deltaTime;
		if (pullStoneSoundTimer >= pullStoneSoundInterval)
		{
			pullStoneSoundTimer = 0f;
			RuntimeManager.PlayOneShot(er_pullStone, stateMachine.transform.position);
			ParticleManager.PlaySystem("Vulture_PullStone", transform.position);
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		pullStoneSoundTimer = 0f;
		if (Raft_Network.IsHost)
		{
			stateMachineStoneBird.animController.SetBool("Flying", value: false);
			stateMachineStoneBird.animController.SetBool("GrabbingStone", value: true);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (Raft_Network.IsHost)
		{
			stateMachineStoneBird.animController.SetBool("Flying", value: true);
			stateMachineStoneBird.animController.SetBool("GrabbingStone", value: false);
			if (lifeStoneRutine != null)
			{
				StopCoroutine(lifeStoneRutine);
			}
		}
	}

	private IEnumerator GrabStone(float timeDelay)
	{
		stateMachineStoneBird.animController.SetBool("HasStone", value: true);
		RuntimeManager.PlayOneShot(er_pullStoneFinish, stateMachine.transform.position);
		ParticleManager.PlaySystem("Vulture_PullStone", transform.position);
		yield return new WaitForSeconds(timeDelay);
		if (Raft_Network.IsHost)
		{
			stateMachineStoneBird.dropStoneState.SetStoneModelState(state: true);
			stateMachine.ChangeState(nextState);
		}
	}

	public override RGD_AI_State SaveState()
	{
		return new RGD_Stonebird_GrabStone(this);
	}
}
