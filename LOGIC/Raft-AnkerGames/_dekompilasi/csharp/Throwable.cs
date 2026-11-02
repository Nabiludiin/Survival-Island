using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ChargeMeter))]
public class Throwable : MonoBehaviour_NetworkTick
{
	public Action Action_OnWaterHit;

	public Action Action_OnThrow;

	private bool inHand = true;

	private bool inWater;

	private bool canThrow = true;

	private bool isLocked;

	public Rigidbody throwableObjectBody;

	public Vector3 throwForceMultiplier;

	public bool thrownByAnimationEvent;

	public BoxCollider throwablePhysicsCollider;

	public WaterFloatSemih2 waterFloat;

	public bool setThrowableToSurface = true;

	[HideInInspector]
	public Transform throwableObject;

	public EventReference er_Throw;

	public EventReference er_WaterHit;

	public EventReference er_Impact;

	private Network_Player playerNetwork;

	private CanvasHelper canvas;

	private ChargeMeter chargeMeter;

	private Transform raftTransform;

	private Transform buoyancyTransform;

	private Transform throwableObjectParent;

	private Vector3 lastThrowForce;

	private Vector3 throwableStartPosition;

	private Vector3 throwableStartRotation;

	private Vector3 throwableStartLocalScale;

	public float networkLerpSpeed = 6f;

	[HideInInspector]
	public bool networkPulling;

	[HideInInspector]
	private Vector3 networkPosition;

	private bool initialized;

	private bool needToReleaseBeforeThrow;

	private Settings settings;

	public bool InHand
	{
		get
		{
			return inHand;
		}
		set
		{
			inHand = value;
		}
	}

	public bool InWater
	{
		get
		{
			return inWater;
		}
		set
		{
			inWater = value;
		}
	}

	public bool CanThrow
	{
		get
		{
			return canThrow;
		}
		set
		{
			canThrow = value;
		}
	}

	public bool IsLocked
	{
		get
		{
			return isLocked;
		}
		set
		{
			isLocked = value;
		}
	}

	public bool UsingPhysicsCollider => throwablePhysicsCollider != null;

	public InputAction ThrowableAction => PlayerInput.GetPlayerByIndex(0).actions["Fire"];

	private void Awake()
	{
		if (throwableObjectBody != null)
		{
			throwableObject = throwableObjectBody.transform;
		}
		settings = ComponentManager<Settings>.Value;
	}

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		canvas = ComponentManager<CanvasHelper>.Value;
		raftTransform = ComponentManager<Raft>.Value.transform;
		buoyancyTransform = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		chargeMeter = GetComponent<ChargeMeter>();
		if (!playerNetwork.IsLocalPlayer && throwablePhysicsCollider != null)
		{
			throwablePhysicsCollider.enabled = false;
		}
		Initialize();
	}

	private void Update()
	{
		if (UsingPhysicsCollider && !InHand && !InWater && TouchingRaft())
		{
			throwableObject.SetParent(buoyancyTransform);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			HandleLocalClient();
		}
		else
		{
			HandleRemoteClient();
		}
	}

	private void HandleRemoteClient()
	{
		if (InHand)
		{
			return;
		}
		LookForWaterHitRemoteClient();
		if (setThrowableToSurface)
		{
			if (InWater)
			{
				throwableObject.position = Vector3.Lerp(throwableObject.position, networkPosition, networkLerpSpeed * Time.deltaTime);
			}
			else
			{
				networkPosition = throwableObject.position;
			}
		}
	}

	private void HandleLocalClient()
	{
		if (IsLocked)
		{
			needToReleaseBeforeThrow = true;
			return;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			needToReleaseBeforeThrow = true;
			return;
		}
		if (InWater || !InHand)
		{
			PlayerItemManager.IsBusy = true;
		}
		InputAction inputAction = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
		if (needToReleaseBeforeThrow)
		{
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputAction, "LMB"))
			{
				needToReleaseBeforeThrow = false;
			}
			return;
		}
		if (CanThrow)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputAction, "LMB"))
			{
				PlayerItemManager.IsBusy = true;
				chargeMeter.Charge(inputAction.ReadValue<float>());
				canvas.SetLoadCircle(GetChargeNormal());
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputAction, "LMB"))
			{
				if (chargeMeter.ChargeNormal > 0f)
				{
					float num = (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad ? 1.5f : 1f);
					lastThrowForce = Camera.main.transform.forward * (chargeMeter.ChargeNormal * throwForceMultiplier.z) + Camera.main.transform.right * (chargeMeter.ChargeNormal * throwForceMultiplier.x) + Vector3.up * (chargeMeter.ChargeNormal * throwForceMultiplier.y * num);
					if (!thrownByAnimationEvent)
					{
						Animation_HookThrow();
					}
					else if (Action_OnThrow != null)
					{
						Action_OnThrow();
					}
					RuntimeManager.PlayOneShot(er_Throw, transform.position);
				}
				chargeMeter.Reset();
				canvas.SetLoadCircle(state: false);
			}
		}
		else
		{
			chargeMeter.Reset();
			canvas.SetLoadCircle(state: false);
		}
		if (!InHand && throwableObject.position.y <= waterFloat.PrevYPos && !InWater)
		{
			InWater = true;
			if (setThrowableToSurface)
			{
				waterFloat.setToSurface = true;
				throwableObjectBody.isKinematic = true;
			}
			RuntimeManager.PlayOneShot(er_WaterHit, throwableObject.position);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot(HapticEvent.Splash);
			if (throwablePhysicsCollider != null)
			{
				throwablePhysicsCollider.enabled = false;
			}
			if (Action_OnWaterHit != null)
			{
				Action_OnWaterHit();
			}
		}
		if (!InHand && InWater)
		{
			UpdateNetworkTick();
		}
	}

	public void ResetLastKnownForce()
	{
		lastThrowForce = Vector3.zero;
	}

	public void ResetCanThrow()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
		}
		CanThrow = true;
		chargeMeter.Reset();
		canvas.SetLoadCircle(state: false);
	}

	public void PickupThrowable(bool setDirty = true)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		throwableObject.parent = throwableObjectParent;
		throwableObject.localPosition = throwableStartPosition;
		throwableObject.localEulerAngles = throwableStartRotation;
		throwableObject.localScale = throwableStartLocalScale;
		waterFloat.setToSurface = false;
		throwableObjectBody.isKinematic = true;
		InHand = true;
		InWater = false;
		if (playerNetwork.IsLocalPlayer)
		{
			PlayerItemManager.IsBusy = false;
			if (setDirty)
			{
				SetDirty();
			}
		}
		ResetLastKnownForce();
	}

	public float GetChargeNormal()
	{
		return chargeMeter.ChargeNormal;
	}

	public bool TouchingRaft()
	{
		if (throwablePhysicsCollider == null)
		{
			return false;
		}
		return Helper.IsColliding(throwablePhysicsCollider, LayerMasks.MASK_GroundMask_Raft, 1.5f);
	}

	public void LookForWaterHitRemoteClient()
	{
		if (setThrowableToSurface && !throwableObjectBody.isKinematic && (InWater || throwableObject.position.y <= waterFloat.PrevYPos))
		{
			throwableObjectBody.isKinematic = true;
			RuntimeManager.PlayOneShot(er_WaterHit, throwableObject.position);
			if (Action_OnWaterHit != null)
			{
				Action_OnWaterHit();
			}
		}
	}

	public void SetPosition(Vector3 position)
	{
		throwableObject.position = position;
	}

	public Vector3 GetPositon()
	{
		return throwableObject.position;
	}

	public void OnThrowablePhysicsImpact(Collision collision)
	{
		if (!er_Impact.IsNull)
		{
			RuntimeManager.PlayOneShot(er_Impact, throwableObjectBody.position);
		}
	}

	public void Animation_HookThrow()
	{
		Message_Throw message = new Message_Throw(Messages.ThrowableThrow, this, lastThrowForce);
		if (Raft_Network.IsHost)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				Throw(lastThrowForce);
				playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void Throw(Vector3 force)
	{
		if (IsLocked || force == Vector3.zero)
		{
			return;
		}
		if (throwablePhysicsCollider != null)
		{
			throwablePhysicsCollider.enabled = true;
		}
		InHand = false;
		CanThrow = false;
		throwableObject.SetParent((playerNetwork.transform.parent == null) ? null : raftTransform);
		throwableObjectBody.isKinematic = false;
		throwableObjectBody.AddForce(force);
		throwableObjectBody.angularVelocity = new Vector3(UnityEngine.Random.Range(0f, 3f), UnityEngine.Random.Range(0f, 3f), 0f);
		if (!thrownByAnimationEvent)
		{
			if (Action_OnThrow != null)
			{
				Action_OnThrow();
			}
			chargeMeter.Reset();
		}
		ResetLastKnownForce();
	}

	private void Initialize()
	{
		if (!initialized)
		{
			initialized = true;
			if ((bool)waterFloat)
			{
				waterFloat.setToSurface = false;
			}
			if (throwablePhysicsCollider != null)
			{
				throwablePhysicsCollider.enabled = false;
			}
			throwableObjectParent = throwableObject.parent;
			throwableStartPosition = throwableObject.localPosition;
			throwableStartRotation = throwableObject.localEulerAngles;
			throwableStartLocalScale = throwableObject.localScale;
			throwableObjectBody.isKinematic = true;
		}
	}

	public override void Serialize_Update()
	{
		AddMessage(new Message_ThrowableUpdate(Messages.Update_Throwable, this, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (playerNetwork == null)
		{
			playerNetwork = GetComponentInParent<Network_Player>();
		}
		if (!initialized)
		{
			return false;
		}
		switch (msg.Type)
		{
		case Messages.Update_Throwable:
		{
			Message_ThrowableUpdate message_ThrowableUpdate = msg as Message_ThrowableUpdate;
			networkPosition = message_ThrowableUpdate.GetPosition();
			InWater = message_ThrowableUpdate.inWater;
			if (message_ThrowableUpdate.inHand && !InHand)
			{
				PickupThrowable();
			}
			else if (!message_ThrowableUpdate.inHand && InHand)
			{
				InHand = false;
				throwableObject.SetParent(raftTransform);
			}
			networkPulling = message_ThrowableUpdate.pulling;
			if (Raft_Network.IsHost)
			{
				playerNetwork.Network.RPCExclude(msg, Target.Other, remoteID, EP2PSend.k_EP2PSendUnreliable, NetworkChannel.Channel_Game);
			}
			return false;
		}
		case Messages.ThrowableThrow:
		{
			Message_Throw message_Throw = msg as Message_Throw;
			Throw(message_Throw.Force);
			return true;
		}
		default:
			return false;
		}
	}
}
