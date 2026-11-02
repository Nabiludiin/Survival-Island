using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_Throw : AI_State
{
	[SerializeField]
	private AI_StateMachine_HyenaBoss bossSM;

	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	private StoneDrop stonePrefab;

	[SerializeField]
	private Transform stoneSpawnParent;

	[SerializeField]
	private ParticleSystem digParticles;

	[SerializeField]
	private int numberOfThrows = 1;

	[SerializeField]
	private float rotationSpeed = 1f;

	[SerializeField]
	private float predictDistance = 1f;

	[SerializeField]
	private float throwHeight = 1f;

	[SerializeField]
	private float stoneTravelTime = 1f;

	[SerializeField]
	private float pullStoneIncrement = 0.2f;

	[SerializeField]
	private float throwAngleOffset = 90f;

	[SerializeField]
	private float acidInterval = 90f;

	[SerializeField]
	private string animationDigStartParameter = string.Empty;

	[SerializeField]
	private string animationDigEndParameter = string.Empty;

	[SerializeField]
	private string animationThrowParameter = string.Empty;

	public EventReference er_pullStone;

	private Coroutine throwRoutine;

	private StoneDrop stone;

	private bool waitingForDig;

	private bool waitingForThrow;

	private Raft_Network network;

	private AI_NetworkBehaviour networkBehaviour;

	protected override void Start()
	{
		base.Start();
		if (this.animatorController != null)
		{
			AnimatorController animatorController = this.animatorController;
			animatorController.OnAnimationEvent = (Action<string>)Delegate.Combine(animatorController.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
	}

	public override void EnterState()
	{
		base.EnterState();
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (networkBehaviour == null)
		{
			networkBehaviour = stateMachine.GetComponent<AI_NetworkBehaviour>();
		}
		stateMachine.movement.SetMovementType(MovementType.None);
		stateMachine.movement.SetMovementSpeed(0f);
		stateMachine.movement.SetRotationSpeed(rotationSpeed);
		waitingForDig = false;
		waitingForThrow = false;
		if (Raft_Network.IsHost)
		{
			throwRoutine = CoroutineManager.Singleton.AddCoroutine(ThrowMultiple());
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		if (Raft_Network.IsHost && throwRoutine != null)
		{
			CoroutineManager.Singleton.StopCoroutine(throwRoutine);
			throwRoutine = null;
		}
		animatorController.animator.ResetTrigger("PullStone");
		if (stone != null)
		{
			UnityEngine.Object.Destroy(stone.gameObject);
		}
	}

	private void OnAnimationEvent(string animationEventName)
	{
		if (!active)
		{
			return;
		}
		if (animationEventName == animationDigStartParameter)
		{
			if (digParticles != null)
			{
				digParticles.Play();
			}
			RuntimeManager.PlayOneShotAttached(er_pullStone, stateMachine.gameObject);
		}
		else if (animationEventName == animationDigEndParameter)
		{
			if (digParticles != null)
			{
				digParticles.Stop();
			}
			waitingForDig = false;
		}
		else if (animationEventName == animationThrowParameter)
		{
			waitingForThrow = false;
		}
	}

	public void SpawnStone()
	{
		stone = UnityEngine.Object.Instantiate(stonePrefab, stoneSpawnParent.position, Quaternion.identity, stoneSpawnParent);
		stone.transform.localEulerAngles = Vector3.zero;
		if (Raft_Network.IsHost)
		{
			network.RPC(new Message_NetworkBehaviour_ID(Messages.HyenaBoss_SpawnStone, network.NetworkIDManager, networkBehaviour.ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private IEnumerator ThrowMultiple()
	{
		if (Raft_Network.IsHost)
		{
			for (int i = 0; i < numberOfThrows; i++)
			{
				float num = 1f + (float)i * pullStoneIncrement;
				SetDigStoneMultiplier(num);
				network.RPC(new Message_NetworkBehaviour_ID_Float(Messages.HyenaBoss_DigStoneMultiplier, network.NetworkIDManager, networkBehaviour.ObjectIndex, num), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				yield return Throw();
			}
			stateMachine.ChangeState(nextState);
		}
	}

	private IEnumerator Throw()
	{
		Network_Player player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
		if (!bossSM.IsValidTarget(player))
		{
			yield break;
		}
		waitingForDig = true;
		animatorController.SetTrigger("Dig");
		while (waitingForDig)
		{
			yield return null;
		}
		SpawnStone();
		waitingForThrow = true;
		while (waitingForThrow)
		{
			if (!bossSM.IsValidTarget(player))
			{
				stateMachine.ChangeState(nextState);
				yield break;
			}
			Quaternion b = Quaternion.LookRotation((player.FeetPosition - stateMachine.transform.position).XZOnly(), Vector3.up);
			b *= Quaternion.Euler(0f, throwAngleOffset, 0f);
			stateMachine.movement.model.rotation = Quaternion.Slerp(stateMachine.movement.model.rotation, b, Time.deltaTime * stateMachine.movement.RotationSpeed);
			yield return null;
		}
		if (!bossSM.IsValidTarget(player))
		{
			stateMachine.ChangeState(nextState);
			yield break;
		}
		Vector3 approximateTrackedTransformPositionWithDeltaMovement = player.MovementDeltaTracker.GetApproximateTrackedTransformPositionWithDeltaMovement(predictDistance, normalizeDirection: false);
		approximateTrackedTransformPositionWithDeltaMovement.y = stateMachine.transform.position.y;
		network.RPC(new Message_NetworkBehaviour_ID_Vector3(Messages.HyenaBoss_ThrowStone, network.NetworkIDManager, networkBehaviour.ObjectIndex, approximateTrackedTransformPositionWithDeltaMovement), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		yield return ThrowStoneToPosition(approximateTrackedTransformPositionWithDeltaMovement);
	}

	private IEnumerator ThrowStoneToPosition(Vector3 targetPosition)
	{
		if (stone == null)
		{
			yield break;
		}
		stone.transform.SetParentSafe(null);
		Vector3 startPos = stone.transform.position;
		float timer = 0f;
		float acidTimer = 0f;
		while (timer < stoneTravelTime)
		{
			timer += Time.deltaTime;
			if (stone == null)
			{
				yield break;
			}
			stone.transform.position = Helper.GetBezierCurveValue(startPos, targetPosition, throwHeight, timer / stoneTravelTime);
			acidTimer += Time.deltaTime;
			if (acidTimer >= acidInterval)
			{
				acidTimer = 0f;
				bossSM.CreateAcidPool(stone.transform.position, 1.5f);
			}
			yield return null;
		}
		if (stone != null)
		{
			stone.OnStoneImpact(hitPlayersHead: false);
		}
	}

	private void SetDigStoneMultiplier(float multiplier)
	{
		animatorController.animator.SetFloat("DigStoneMultiplier", multiplier);
	}

	public bool FakeDeserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.HyenaBoss_SpawnStone:
			SpawnStone();
			return false;
		case Messages.HyenaBoss_ThrowStone:
		{
			Message_NetworkBehaviour_ID_Vector3 message_NetworkBehaviour_ID_Vector = msg as Message_NetworkBehaviour_ID_Vector3;
			CoroutineManager.Singleton.AddCoroutine(ThrowStoneToPosition(message_NetworkBehaviour_ID_Vector.Vector));
			return false;
		}
		case Messages.HyenaBoss_DigStoneMultiplier:
		{
			Message_NetworkBehaviour_ID_Float message_NetworkBehaviour_ID_Float = msg as Message_NetworkBehaviour_ID_Float;
			SetDigStoneMultiplier(message_NetworkBehaviour_ID_Float.floatValue);
			return false;
		}
		default:
			return false;
		}
	}
}
