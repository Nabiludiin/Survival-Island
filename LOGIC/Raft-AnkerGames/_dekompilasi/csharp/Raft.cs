using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Raft : MonoBehaviour
{
	private int _anchorCount;

	public static Vector3 direction = new Vector3(0f, 0f, 1f);

	public float velocityLimit = 1.5f;

	[Space(10f)]
	[Tooltip("Max speed for the rigidbodys velocity")]
	public float maxVelocity = 5f;

	[Tooltip("Max speed for waterdrift, sail, motorwheels etc")]
	public float maxSpeed = 1.5f;

	[SerializeField]
	private float currentMovementSpeed = 1f;

	public float waterDriftSpeed = 1.5f;

	public float accelerationSpeed = 2f;

	public float deAccelerationSpeed = 1f;

	public float maxSteeringTorque = 100f;

	[SerializeField]
	private float networkLerpSpeed = 5f;

	[SerializeField]
	private float maxDistanceFromAnchorPoint = 5f;

	public List<Anchor_Stationary> remoteAnchors = new List<Anchor_Stationary>();

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private Buoyancy buoyancy;

	public EventReference er_anchored;

	[SerializeField]
	private StudioEventEmitter eventEmitter_idle;

	[SerializeField]
	public BlockCollisionConsolidator blockCollisionConsolidator;

	private float speed;

	private Raft_Network network;

	private Vector3 networkPosition;

	private float networkYRot;

	private float networkVelocity;

	private Network_Player localPlayer;

	private Vector3 previousPosition;

	private Vector3 moveDirection;

	private Vector3 anchorPosition;

	private Vector3 creativeTargetPos;

	public bool IsAnchored => AnchorCount > 0;

	public int AnchorCount
	{
		get
		{
			return _anchorCount;
		}
		private set
		{
			_anchorCount = Mathf.Clamp(value, 0, int.MaxValue);
		}
	}

	public Vector3 Velocity => body.velocity;

	public Vector3 VelocityDirection => body.transform.InverseTransformDirection(Velocity);

	public bool Moving
	{
		get
		{
			if (network == null || Raft_Network.IsHost)
			{
				return Velocity.sqrMagnitude > velocityLimit;
			}
			return networkVelocity > velocityLimit;
		}
	}

	private void Awake()
	{
		ComponentManager<Raft>.Value = this;
	}

	private void Start()
	{
		SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(OnLoadComplete));
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = network.GetLocalPlayer();
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(BlockCreator_PlaceBlockCallStack));
		eventEmitter_idle.Play();
		Invoke("LateStart", 3f);
	}

	private void LateStart()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.isRaftAlwaysAnchored)
		{
			creativeTargetPos = new Vector3(transform.position.x, 0.4f, transform.position.z);
			transform.localPosition = creativeTargetPos;
		}
	}

	private void OnDestroy()
	{
		network = null;
		localPlayer = null;
		eventEmitter_idle.Stop();
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		SaveAndLoad.LoadComplete = (Action)Delegate.Remove(SaveAndLoad.LoadComplete, new Action(OnLoadComplete));
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Remove(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(BlockCreator_PlaceBlockCallStack));
	}

	private void Update()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.isRaftAlwaysAnchored)
		{
			if (Vector3.Distance(transform.position, creativeTargetPos) >= 0.2f)
			{
				transform.localPosition = creativeTargetPos;
			}
		}
		else
		{
			if (!Raft_Network.WorldHasBeenRecieved && !GameManager.IsInNewGame)
			{
				return;
			}
			direction = (body.transform.position - previousPosition).normalized;
			direction.y = 0f;
			if (!Raft_Network.IsHost)
			{
				Vector3 position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * networkLerpSpeed);
				position.y = buoyancy.CenterPointY;
				Vector3 euler = new Vector3(0f, Mathf.LerpAngle(transform.eulerAngles.y, networkYRot, Time.deltaTime * networkLerpSpeed), 0f);
				transform.SetPositionAndRotation(position, Quaternion.Euler(euler));
				body.transform.SetPositionAndRotation(transform.position, transform.rotation);
			}
			else
			{
				ControlSpeed();
				Vector3 position2 = Vector3.Lerp(transform.position, body.transform.position, Time.deltaTime * 5f);
				transform.SetPositionAndRotation(position2, Quaternion.Euler(0f, body.transform.eulerAngles.y, 0f));
			}
			eventEmitter_idle.SetParameter("roughness", WaterRoughnessManager.Roughness);
			if (localPlayer == null)
			{
				localPlayer = network.GetLocalPlayer();
				return;
			}
			if (localPlayer.PersonController.HasRaftAsParent)
			{
				if (eventEmitter_idle.transform.parent != localPlayer.transform)
				{
					eventEmitter_idle.transform.SetParent(localPlayer.transform);
				}
				Vector3 position3 = localPlayer.transform.position;
				position3.y = 0f;
				eventEmitter_idle.transform.position = position3;
				return;
			}
			if (eventEmitter_idle.transform.parent != transform)
			{
				eventEmitter_idle.transform.SetParent(transform);
			}
			Block block = BlockCreator.ClosestFoundationToPosition(localPlayer.transform.position);
			if (block != null)
			{
				eventEmitter_idle.transform.position = block.transform.position;
			}
			else if (eventEmitter_idle.transform.localPosition != Vector3.zero)
			{
				eventEmitter_idle.transform.localPosition = Vector3.zero;
			}
		}
	}

	private void FixedUpdate()
	{
		if (!Raft_Network.IsHost || GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.isRaftAlwaysAnchored || (!Raft_Network.WorldHasBeenRecieved && !GameManager.IsInNewGame))
		{
			return;
		}
		if (!IsAnchored)
		{
			if (speed != 0f)
			{
				moveDirection = Vector3.forward;
				bool flag = RaftVelocityManager.MotorDirection != Vector3.zero;
				if (!flag)
				{
					List<Sail> allSails = Sail.AllSails;
					Vector3 zero = Vector3.zero;
					for (int i = 0; i < allSails.Count; i++)
					{
						Sail sail = allSails[i];
						if (sail.open)
						{
							zero += sail.GetNormalizedDirection();
						}
					}
					if (zero.z < 0f)
					{
						if ((double)Mathf.Abs(zero.x) > 0.7)
						{
							zero.z = (moveDirection.z = 0f);
						}
						else
						{
							zero.z = -0.8f;
						}
					}
					moveDirection += zero;
				}
				else
				{
					moveDirection = RaftVelocityManager.MotorDirection;
				}
				currentMovementSpeed = speed;
				if (flag)
				{
					currentMovementSpeed = RaftVelocityManager.motorSpeed;
					if (RaftVelocityManager.MotorWheelWeightStrength == MotorWheel.WeightStrength.Weak)
					{
						currentMovementSpeed *= 0.5f;
					}
					if (currentMovementSpeed < speed)
					{
						currentMovementSpeed = speed;
					}
				}
				if (speed != 0f)
				{
					if (currentMovementSpeed > maxSpeed)
					{
						currentMovementSpeed = maxSpeed;
					}
					moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
					body.AddForce(moveDirection * currentMovementSpeed);
				}
			}
			List<SteeringWheel> steeringWheels = RaftVelocityManager.steeringWheels;
			float num = 0f;
			foreach (SteeringWheel item in steeringWheels)
			{
				num += item.SteeringRotation;
			}
			num = Mathf.Clamp(num, -1f, 1f);
			if (num != 0f)
			{
				Vector3 torque = new Vector3(0f, Mathf.Tan(MathF.PI / 180f * num), 0f) * maxSteeringTorque;
				body.AddTorque(torque, ForceMode.Acceleration);
			}
		}
		else
		{
			float num2 = transform.position.DistanceXZ(anchorPosition);
			if (num2 >= maxDistanceFromAnchorPoint * 3f)
			{
				anchorPosition = transform.position;
			}
			if (num2 > maxDistanceFromAnchorPoint)
			{
				Vector3 vector = anchorPosition - transform.position;
				vector.y = 0f;
				body.AddForce(vector.normalized * 2f);
			}
		}
		if (body.velocity.sqrMagnitude > maxVelocity)
		{
			body.velocity = Vector3.ClampMagnitude(body.velocity, maxVelocity);
		}
		eventEmitter_idle.SetParameter("velocity", body.velocity.sqrMagnitude / maxVelocity);
		previousPosition = body.transform.position;
	}

	private void OnDrawGizmos()
	{
	}

	private void OnWorldShift(Vector3 shift)
	{
		body.transform.position -= shift;
		transform.position -= shift;
		anchorPosition -= shift;
		SingletonGeneric<GameManager>.Singleton.buoyancy.transform.position -= shift;
		SingletonGeneric<GameManager>.Singleton.raftFollowParent.position -= shift;
		SingletonGeneric<GameManager>.Singleton.water.transform.position -= shift;
		if (!Raft_Network.IsHost)
		{
			networkPosition = transform.position;
		}
	}

	private void OnLoadComplete()
	{
		anchorPosition = transform.position;
	}

	private void BlockCreator_PlaceBlockCallStack(Block block, Network_Player playerNetwork, bool replicating, int hotSlotIndex)
	{
		eventEmitter_idle.SetParameter("size", (float)BlockCreator.FoundationCount / 250f);
	}

	public void AddAnchor(bool playSound, GameObject soundParent = null)
	{
		AnchorCount++;
		if (AnchorCount == 1)
		{
			anchorPosition = transform.position;
		}
		if (playSound && IsAnchored)
		{
			RuntimeManager.PlayOneShotAttached(er_anchored, (soundParent == null) ? gameObject : soundParent);
		}
	}

	public void RemoveAnchor(int removeAnchorCount = 1)
	{
		AnchorCount -= removeAnchorCount;
	}

	public void AddForce(Vector3 direction, float force, ForceMode forceMode, bool forceThroughAnchor = false)
	{
		if ((forceThroughAnchor || !IsAnchored) && Raft_Network.IsHost)
		{
			direction.y = 0f;
			body.AddForce(direction * force, forceMode);
		}
	}

	private void ControlSpeed()
	{
		if (IsAnchored)
		{
			speed = Mathf.MoveTowards(speed, 0f, Time.deltaTime * deAccelerationSpeed);
		}
		else
		{
			speed = Mathf.MoveTowards(speed, waterDriftSpeed, Time.deltaTime * accelerationSpeed);
		}
	}

	public void SetNetworkProperties(Message_Raft msg)
	{
		if (transform != null)
		{
			networkPosition = new Vector3(msg.PositionX, 0f, msg.PositionZ);
			networkYRot = msg.RotationY;
			networkVelocity = msg.Velocity;
			eventEmitter_idle.SetParameter("velocity", networkVelocity / maxVelocity);
		}
	}
}
