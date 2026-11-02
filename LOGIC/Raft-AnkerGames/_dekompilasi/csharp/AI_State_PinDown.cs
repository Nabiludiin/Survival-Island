using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_PinDown : AI_State
{
	[SerializeField]
	private AI_State nextState;

	[SerializeField]
	private AttachPlayer attachPlayerScript;

	[SerializeField]
	private AnimatorController animatorController;

	[SerializeField]
	private ParticleSystem droolParticles;

	[SerializeField]
	private float movementSpeed = 1f;

	[SerializeField]
	private float accelerationSpeed = 5f;

	[SerializeField]
	private float chargeUpTime = 1f;

	[SerializeField]
	private float rotateSpeed = 1f;

	[SerializeField]
	private float reachPlayerDistance = 1f;

	[SerializeField]
	private float totalDamage = 1f;

	[SerializeField]
	private float totalPinTime = 1f;

	[SerializeField]
	private float maxChargeTime = 1f;

	[SerializeField]
	private string attackRoarAnimationParameter = string.Empty;

	[SerializeField]
	private string attackLoadUpAnimationParameter = string.Empty;

	[SerializeField]
	private StudioEventEmitter emitter_charge;

	[SerializeField]
	private StudioEventEmitter emitter_pindDown;

	public EventReference er_roar;

	public EventReference er_loadUp;

	private AI_StateMachine_HyenaBoss bossSM;

	private Raft_Network network;

	private Network_Host networkHost;

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
		bossSM = stateMachine as AI_StateMachine_HyenaBoss;
		if (networkBehaviour == null)
		{
			networkBehaviour = stateMachine.GetComponent<AI_NetworkBehaviour>();
		}
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (networkHost == null)
		{
			networkHost = ComponentManager<Network_Host>.Value;
		}
		stateMachine.movement.SetMovementType(MovementType.None);
		stateMachine.movement.SetMovementSpeed(0f);
		stateMachine.movement.SetRotationSpeed(rotateSpeed);
		if (Raft_Network.IsHost)
		{
			StartCoroutine(PinDown());
		}
		if (droolParticles != null)
		{
			droolParticles.Play();
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		stateMachine.movement.SetMovementSpeed(stateMachine.movement.defaultMovementSpeed);
		if (droolParticles != null)
		{
			droolParticles.Stop();
		}
		if (emitter_charge != null)
		{
			emitter_charge.Stop();
		}
	}

	private void OnAnimationEvent(string animationEventName)
	{
		if (active)
		{
			if (animationEventName == attackRoarAnimationParameter)
			{
				RuntimeManager.PlayOneShot(er_roar, stateMachine.transform.position);
			}
			else if (animationEventName == attackLoadUpAnimationParameter)
			{
				RuntimeManager.PlayOneShot(er_loadUp, stateMachine.transform.position);
			}
		}
	}

	private IEnumerator PinDown()
	{
		Network_Player player = Helper.ClosestPlayerToPoint(stateMachine.transform.position, includeDeadPlayers: false);
		if (!bossSM.IsValidTarget(player))
		{
			stateMachine.ChangeState(nextState);
			yield break;
		}
		stateMachine.movement.SetMovementSpeed(0f);
		float angle = float.MaxValue;
		while (angle > 30f)
		{
			if (!bossSM.IsValidTarget(player))
			{
				stateMachine.ChangeState(nextState);
				yield break;
			}
			stateMachine.movement.Look(stateMachine.movement.model, player.FeetPosition, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			Vector3 vector = player.FeetPosition - stateMachine.transform.position;
			angle = Mathf.Abs(Vector3.Angle(vector.XZOnly(), stateMachine.movement.model.forward.XZOnly()));
			yield return null;
		}
		animatorController.SetTrigger("PinLoad");
		float timer = 0f;
		while (timer < chargeUpTime)
		{
			if (!bossSM.IsValidTarget(player))
			{
				stateMachine.ChangeState(nextState);
				yield break;
			}
			timer += Time.deltaTime;
			stateMachine.movement.Look(stateMachine.movement.model, player.FeetPosition, stateMachine.movement.RotationSpeed, ignoreZRotation: true);
			yield return null;
		}
		timer = 0f;
		stateMachine.movement.SetMovementType(MovementType.NavmeshAgent);
		if (emitter_charge != null)
		{
			emitter_charge.PlaySafe();
			if (Raft_Network.IsHost)
			{
				network.RPC(new Message_NetworkBehaviour_ID(Messages.HyenaBoss_StartCharge, network.NetworkIDManager, networkBehaviour.ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		float distance;
		do
		{
			timer += Time.deltaTime;
			if (timer >= maxChargeTime || !bossSM.IsValidTarget(player))
			{
				stateMachine.ChangeState(nextState);
				yield break;
			}
			stateMachine.movement.ChangeMovementSpeedTowards(movementSpeed, accelerationSpeed);
			stateMachine.movement.MoveLook_Navmesh(player.FeetPosition, stateMachine.movement.MovementSpeed);
			distance = stateMachine.transform.position.DistanceXZ(player.FeetPosition);
			yield return null;
		}
		while (distance > reachPlayerDistance);
		stateMachine.movement.SetMovementType(MovementType.None);
		if (!bossSM.IsValidTarget(player))
		{
			stateMachine.ChangeState(nextState);
			yield break;
		}
		StartPin(player);
		timer = 0f;
		float tickRate = 1f;
		int num = Mathf.Clamp(Mathf.FloorToInt(totalPinTime / tickRate), 1, int.MaxValue);
		float num2 = totalDamage * GameModeValueManager.GetCurrentGameModeValue().utopiaBossVariables.damageMultiplier;
		float damagePerTick = num2 / (float)num;
		float tickTimer = 0f;
		while (timer < totalPinTime)
		{
			timer += Time.deltaTime;
			tickTimer += Time.deltaTime;
			if (player == null)
			{
				StopPin();
				stateMachine.ChangeState(nextState);
				yield break;
			}
			if (tickTimer >= tickRate)
			{
				tickTimer -= tickRate;
				networkHost.DamageEntity(player.Stats, null, damagePerTick, player.transform.position, Vector3.up, EntityType.Enemy);
			}
			yield return null;
		}
		StopPin();
		yield return new WaitForSeconds(1f);
		stateMachine.ChangeState(nextState);
	}

	private void StartPin(Network_Player player)
	{
		if (!(player == null))
		{
			attachPlayerScript.StartCarryingPlayer(player);
			animatorController.SetBool("Pin", value: true);
			if (Raft_Network.IsHost)
			{
				network.RPC(new Message_NetworkBehaviour_ID_SteamID(Messages.HyenaBoss_StartPin, network.NetworkIDManager, networkBehaviour.ObjectIndex, player.steamID), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			if (emitter_pindDown != null)
			{
				emitter_pindDown.Stop();
				emitter_pindDown.Play();
			}
			if (emitter_charge != null)
			{
				emitter_charge.Stop();
			}
		}
	}

	private void StopPin()
	{
		attachPlayerScript.StopCarryingPlayer(manipulatePosition: true);
		animatorController.SetBool("Pin", value: false);
		if (Raft_Network.IsHost)
		{
			network.RPC(new Message_NetworkBehaviour_ID(Messages.HyenaBoss_StopPin, network.NetworkIDManager, networkBehaviour.ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		if (emitter_pindDown != null)
		{
			emitter_pindDown.Stop();
		}
	}

	public bool FakeDeserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.HyenaBoss_StartPin:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			StartPin(network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return false;
		}
		case Messages.HyenaBoss_StopPin:
			StopPin();
			return false;
		case Messages.HyenaBoss_StartCharge:
			if (emitter_charge != null)
			{
				emitter_charge.PlaySafe();
			}
			return false;
		default:
			return false;
		}
	}
}
