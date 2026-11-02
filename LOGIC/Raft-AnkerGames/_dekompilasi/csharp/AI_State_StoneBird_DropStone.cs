using FMODUnity;
using Steamworks;
using UnityEngine;

public class AI_State_StoneBird_DropStone : AI_State
{
	[SerializeField]
	private AI_State_CirculateSpawn_Air_StoneBird circulateState;

	[SerializeField]
	private AI_Movement movement;

	[SerializeField]
	private Transform stoneModel;

	[SerializeField]
	private StoneDrop stoneDropPrefab;

	[SerializeField]
	public float altitudeAbovePlayer = 15f;

	[SerializeField]
	private float dropDistanceFromPlayer = 0.5f;

	[SerializeField]
	private float dropForce = 100f;

	[SerializeField]
	private float dropStoneCooldown = 10f;

	[SerializeField]
	private float dropStoneTimer;

	private AI_StateMachine_StoneBird stateMachineStoneBird;

	private Vector3 targetPoint;

	public bool HasStone => stoneModel.gameObject.activeInHierarchy;

	public bool CanDropStone => dropStoneTimer <= 0f;

	public float AltitudeAbovePlayer => altitudeAbovePlayer;

	public float DropStoneTimer
	{
		get
		{
			return dropStoneTimer;
		}
		set
		{
			dropStoneTimer = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		stateMachineStoneBird = stateMachine as AI_StateMachine_StoneBird;
	}

	private void OnDrawGizmos()
	{
		if (active)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(targetPoint, 0.5f);
			Gizmos.DrawLine(stateMachine.transform.position, targetPoint);
		}
	}

	private void Update()
	{
		if (Raft_Network.IsHost && dropStoneTimer > 0f)
		{
			dropStoneTimer -= Time.deltaTime;
			if (dropStoneTimer < 0f)
			{
				dropStoneTimer = 0f;
			}
		}
	}

	public override void UpdateState()
	{
		base.UpdateState();
		if (!Raft_Network.IsHost)
		{
			return;
		}
		if (!HasValidTarget() || !HasStone)
		{
			stateMachine.ChangeState(circulateState);
			return;
		}
		Network_Player network_Player = Helper.ClosestPlayerToPoint(stateMachineStoneBird.transform.position, includeDeadPlayers: false);
		if (network_Player != null && network_Player != stateMachineStoneBird.target)
		{
			stateMachineStoneBird.target = network_Player;
		}
		targetPoint = stateMachineStoneBird.target.transform.position;
		targetPoint.y += altitudeAbovePlayer;
		movement.ChangeMovementSpeedTowards(movement.defaultMovementSpeed, 5f);
		movement.MoveLookObstruction_Translate(stateMachine.transform, targetPoint, movement.MovementSpeed, movement.RotationSpeed);
		float num = Vector3.Distance(stateMachine.transform.position, targetPoint);
		float num2 = Vector3.Distance(stateMachine.transform.position, new Vector3(targetPoint.x, stateMachine.transform.position.y, targetPoint.z));
		if (num <= dropDistanceFromPlayer || num2 <= dropDistanceFromPlayer)
		{
			DropStone(new Vector3(targetPoint.x, stoneModel.position.y, targetPoint.z));
			stateMachine.ChangeState(circulateState);
		}
	}

	public void DropStone(Vector3 dropPosition)
	{
		RuntimeManager.PlayOneShot(stateMachineStoneBird.er_attack, stateMachine.transform.position);
		Object.Instantiate(stoneDropPrefab, dropPosition, Quaternion.identity).rigidBody.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
		SetStoneModelState(state: false);
		stateMachineStoneBird.animController.SetBool("HasStone", value: false);
		if (Raft_Network.IsHost)
		{
			dropStoneTimer = dropStoneCooldown;
			Message_NetworkBehaviour_ID_Vector3 message = new Message_NetworkBehaviour_ID_Vector3(Messages.StoneBird_DropStone, stateMachineStoneBird.networkBehaviour.Network.NetworkIDManager, stateMachineStoneBird.networkBehaviour.ObjectIndex, dropPosition);
			stateMachineStoneBird.networkBehaviour.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void SetStoneModelState(bool state)
	{
		stoneModel.gameObject.SetActiveSafe(state);
	}

	private bool HasValidTarget()
	{
		if (stateMachineStoneBird.target == null)
		{
			return false;
		}
		if (stateMachineStoneBird.target.PersonController.controllerType == ControllerType.Water && stateMachineStoneBird.target.transform.position.y <= -2f)
		{
			return false;
		}
		return true;
	}

	public override RGD_AI_State SaveState()
	{
		return new RGD_Stonebird_DropStone(this);
	}
}
