using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snowmobile : MonoBehaviour_NetworkTick, IRaycastable
{
	public Action<Snowmobile, bool> OnSnowmobileReset;

	private float resetToShedTimer;

	private Raft_Network _network;

	[SerializeField]
	private AttachPlayer[] attachPlayerScripts;

	[SerializeField]
	private Collider[] colliders;

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private Renderer snowmobileRenderer;

	[SerializeField]
	private Light light;

	[SerializeField]
	private Transform groundCheckPoint;

	[SerializeField]
	private VelocityCalculator velocityCalc;

	[SerializeField]
	private AnimationCurve turnVsSpeedCurve;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private TriggerBoxEnterAction triggerBoxEnter;

	[SerializeField]
	private Vector3 cutomCenterOfMass = Vector3.zero;

	[SerializeField]
	private ParticleSystem particles;

	[SerializeField]
	private Gauge speedGauge;

	[SerializeField]
	private Gauge tankGauge;

	[SerializeField]
	private float lightLerpSpeed = 2f;

	[SerializeField]
	private float acceleration;

	[SerializeField]
	private float airControlSpeed;

	[SerializeField]
	private float maxSpeed;

	[SerializeField]
	private float maxReverseSpeed;

	[SerializeField]
	private float maxRotationSpeed = 25f;

	[SerializeField]
	private float groundRayLength;

	[SerializeField]
	private float simulatedInputSpeed;

	[SerializeField]
	private float animationSpeed;

	[SerializeField]
	private float resetToShedTime;

	[SerializeField]
	private float exitYDistance = 2f;

	[SerializeField]
	[Range(0f, 1f)]
	private float impactSoundVelocityThreshold = 0.6f;

	[SerializeField]
	[Range(0f, 1f)]
	private float impactSoundNotGroundedThreshold = 0.1f;

	[SerializeField]
	private float positionLerpSpeed;

	[SerializeField]
	private float rotationLerpSpeed;

	private Vector3 networkPosition;

	private Vector3 networkEuler;

	[SerializeField]
	private StudioEventEmitter emitter_engine;

	[SerializeField]
	private StudioEventEmitter emitter_impact;

	[SerializeField]
	private float impactCooldown;

	[SerializeField]
	private float impactTimer;

	[SerializeField]
	private Vector3 simulatedInput;

	[SerializeField]
	private bool grounded;

	[SerializeField]
	private bool prevGrounded;

	[SerializeField]
	private float notGroundedTimer;

	[SerializeField]
	private bool detroyedByWater;

	private Network_Player localPlayer;

	private static CanvasHelper canvas;

	private MaterialPropertyBlock matPropBlock;

	private bool canLeave;

	public bool IsGrounded => grounded;

	public Network_Player DrivingPlayer => attachPlayerScripts[0].carriedPlayer;

	public bool ShouldSendNetworkInfo
	{
		get
		{
			if (!(DrivingPlayer != null) || !DrivingPlayer.IsLocalPlayer)
			{
				if (DrivingPlayer == null)
				{
					return Raft_Network.IsHost;
				}
				return false;
			}
			return true;
		}
	}

	public float ResetToShedTimer
	{
		get
		{
			return resetToShedTimer;
		}
		set
		{
			resetToShedTimer = value;
			canvas.SetLoadCircle(resetToShedTimer / resetToShedTime);
		}
	}

	private Raft_Network Network
	{
		get
		{
			if (_network == null)
			{
				_network = ComponentManager<Raft_Network>.Value;
			}
			return _network;
		}
	}

	private void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		TriggerBoxEnterAction triggerBoxEnterAction = triggerBoxEnter;
		triggerBoxEnterAction.OnTriggerAction = (Action<Collider>)Delegate.Combine(triggerBoxEnterAction.OnTriggerAction, new Action<Collider>(OnTriggerBoxEnter));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetworkIDManager.RemoveNetworkID(this, typeof(Snowmobile));
		NetworkIDManager.RemoveNetworkTickID(this);
	}

	private void Update()
	{
		SetCustomPivot();
		if (canLeave)
		{
			for (int i = 0; i < attachPlayerScripts.Length; i++)
			{
				AttachPlayer attachPlayer = attachPlayerScripts[i];
				if (!(attachPlayer != null) || !attachPlayer.HasPlayerAttached || !attachPlayer.carriedPlayer.IsLocalPlayer)
				{
					continue;
				}
				InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
				{
					break;
				}
				Message_NetworkBehaviour_ID_SteamID message = new Message_NetworkBehaviour_ID_SteamID(Messages.Snowmobile_LeaveSeat, Network.NetworkIDManager, ObjectIndex, attachPlayer.carriedPlayer.steamID);
				if (Raft_Network.IsHost)
				{
					if (LeaveSeat(attachPlayer.carriedPlayer))
					{
						Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
				else
				{
					Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				break;
			}
		}
		if (ShouldSendNetworkInfo)
		{
			UpdateNetworkTick();
		}
		Network_Player drivingPlayer = DrivingPlayer;
		if (Raft_Network.IsHost)
		{
			body.isKinematic = drivingPlayer != null && !drivingPlayer.IsLocalPlayer;
		}
		else
		{
			body.isKinematic = drivingPlayer == null || !drivingPlayer.IsLocalPlayer;
		}
		if (body.isKinematic)
		{
			body.transform.position = Vector3.Lerp(body.transform.position, networkPosition, Time.deltaTime * positionLerpSpeed);
			body.transform.rotation = Quaternion.Lerp(body.transform.rotation, Quaternion.Euler(networkEuler), Time.deltaTime * rotationLerpSpeed);
		}
		if (tankGauge != null)
		{
			tankGauge.MapToGaugeTargetAngle((drivingPlayer != null) ? 1 : 0);
		}
		SetEmissionAndLights(drivingPlayer != null);
		if (drivingPlayer != null)
		{
			prevGrounded = grounded;
			grounded = false;
			if (Physics.Raycast(groundCheckPoint.position, -groundCheckPoint.up, out var _, groundRayLength, LayerMasks.MASK_Obstruction))
			{
				grounded = true;
			}
			if (!grounded)
			{
				notGroundedTimer += Time.deltaTime;
			}
			else
			{
				if (notGroundedTimer > impactSoundNotGroundedThreshold)
				{
					PlayImpactSound(velocityCalc.NormalizedVelocity, networked: false);
				}
				notGroundedTimer = 0f;
			}
			if (grounded)
			{
				particles.PlaySafe();
			}
			else
			{
				particles.StopSafe();
			}
			if (drivingPlayer.IsLocalPlayer)
			{
				HandleDrivingUpdate();
			}
			else
			{
				HandleAnimation(drivingPlayer.Animator.anim.GetFloat("VelocityX"));
			}
		}
		if (emitter_engine != null)
		{
			float normalizedVelocity = velocityCalc.NormalizedVelocity;
			emitter_engine.SetParameter("onground", grounded ? 1f : 0f);
			emitter_engine.SetParameter("velocity", 10f * normalizedVelocity);
			if (DrivingPlayer != null && DrivingPlayer.IsLocalPlayer)
			{
				emitter_engine.SetParameter("RPM_snowmobile", simulatedInput.z * normalizedVelocity);
				SimpleMonoBehaviourSingleton<HapticManager>.Instance?.SetVolume(HapticEvent.Snowmobile, normalizedVelocity);
			}
			else
			{
				emitter_engine.SetParameter("RPM_snowmobile", normalizedVelocity);
			}
			if (DrivingPlayer != null && DrivingPlayer.PlayerScript.IsDead)
			{
				if (DrivingPlayer.IsLocalPlayer)
				{
					SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Stop(HapticEvent.Snowmobile);
				}
				emitter_engine.SetParameter("velocity", 0f);
				emitter_engine.SetParameter("RPM_snowmobile", 0f);
				if (emitter_engine != null)
				{
					emitter_engine.StopSafe();
				}
			}
		}
		if (impactTimer < impactCooldown)
		{
			impactTimer += Time.deltaTime;
		}
		if (Raft_Network.IsHost && !detroyedByWater && transform.position.y < -1.5f)
		{
			detroyedByWater = true;
			OnSnowmobileReset?.Invoke(this, arg2: true);
		}
		canLeave = true;
	}

	private void OnDrawGizmosSelected()
	{
		if (groundCheckPoint != null)
		{
			Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position - groundCheckPoint.up * groundRayLength);
		}
		if (body != null)
		{
			SetCustomPivot();
			Gizmos.color = Color.magenta;
			Gizmos.DrawSphere(body.worldCenterOfMass, 0.1f);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (emitter_impact != null && impactTimer >= impactCooldown)
		{
			float normalizedVelocity = velocityCalc.NormalizedVelocity;
			if (!(collision.collider is TerrainCollider) && normalizedVelocity > 0.6f)
			{
				impactTimer = 0f;
				PlayImpactSound(normalizedVelocity, networked: true);
			}
		}
	}

	private void OnTriggerBoxEnter(Collider other)
	{
		if (other == null)
		{
			return;
		}
		Network_Player component = other.GetComponent<Network_Player>();
		if (!(component != null) || component.IsLocalPlayer)
		{
			return;
		}
		Collider[] array = colliders;
		foreach (Collider collider in array)
		{
			if (!(collider == null))
			{
				Physics.IgnoreCollision(collider, other);
				BoxCollider[] entityColliders = component.currentModel.entityColliders;
				foreach (Collider collider2 in entityColliders)
				{
					Physics.IgnoreCollision(collider, collider2, ignore: true);
				}
			}
		}
	}

	void IRaycastable.OnIsRayed()
	{
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer attachPlayer in array)
		{
			if (attachPlayer.HasPlayerAttached)
			{
				if (attachPlayer.carriedPlayer.IsLocalPlayer)
				{
					flag3 = true;
				}
				flag = false;
			}
			else
			{
				flag2 = false;
			}
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Remove"];
		if (!PlayerItemManager.IsBusy && CanvasHelper.ActiveMenu == MenuType.None && !localPlayer.PlayerScript.IsDead)
		{
			if (!flag3 && !flag2)
			{
				InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Use"), action2.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "Interact"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					Message_Snowmobile_TakeSeat message_Snowmobile_TakeSeat = new Message_Snowmobile_TakeSeat(Messages.Snowmobile_TakeSeat, Network.NetworkIDManager, ObjectIndex, localPlayer.steamID);
					if (Raft_Network.IsHost)
					{
						if (TryTakingSeat(localPlayer, ref message_Snowmobile_TakeSeat.seatIndex))
						{
							Network.RPC(message_Snowmobile_TakeSeat, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
						}
					}
					else
					{
						Network.SendP2P(Network.HostID, message_Snowmobile_TakeSeat, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					}
				}
			}
			if (flag)
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Remove"), action.GetBindingDisplayString(), MyInput.Keybinds["Remove"].MainKey, 1, 0, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "Remove"))
				{
					ResetToShedTimer += Time.deltaTime;
					if (ResetToShedTimer >= resetToShedTime)
					{
						OnSnowmobileReset?.Invoke(this, arg2: false);
						ResetToShedTimer = 0f;
					}
				}
			}
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Remove"))
		{
			ResetToShedTimer = 0f;
		}
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		canvas.displayTextManager.HideDisplayTexts();
		canvas.SetLoadCircle(state: false);
		ResetToShedTimer = 0f;
	}

	public void MakeAllPlayersLeave()
	{
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer attachPlayer in array)
		{
			if (attachPlayer.HasPlayerAttached)
			{
				LeaveSeat(attachPlayer);
			}
		}
	}

	public void ResetToDefaults()
	{
		simulatedInput = Vector3.zero;
		body.velocity = Vector3.zero;
		notGroundedTimer = 0f;
		detroyedByWater = false;
		if (emitter_engine != null)
		{
			emitter_engine.Stop();
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Stop(HapticEvent.Snowmobile);
		}
		ResetNetworkPropertiesToSnowmobile();
	}

	public void PlayImpactSound(float normalizedVelocity, bool networked)
	{
		float value = normalizedVelocity * 10f;
		if (networked)
		{
			Message_NetworkBehaviour_ID_Float message = new Message_NetworkBehaviour_ID_Float(Messages.Snowmobile_Impact, Network.NetworkIDManager, ObjectIndex, normalizedVelocity);
			if (Raft_Network.IsHost)
			{
				Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				emitter_impact.Play();
				emitter_impact.SetParameter("velocity", value);
			}
			else
			{
				Network.SendP2P(Network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			emitter_impact.Play();
			emitter_impact.SetParameter("velocity", value);
		}
	}

	private void SetCustomPivot()
	{
		if (body != null)
		{
			body.centerOfMass = cutomCenterOfMass;
		}
	}

	private void SetEmissionAndLights(bool lightsOn)
	{
		if (!(light != null))
		{
			return;
		}
		float b = (lightsOn ? 2f : 0f);
		light.intensity = Mathf.Lerp(light.intensity, b, Time.deltaTime * lightLerpSpeed);
		light.enabled = light.intensity > 0.1f;
		if (snowmobileRenderer != null)
		{
			if (matPropBlock == null)
			{
				matPropBlock = new MaterialPropertyBlock();
			}
			snowmobileRenderer.GetPropertyBlock(matPropBlock, 0);
			matPropBlock.SetFloat("_EmissionSwitch", lightsOn ? 1f : 0f);
			snowmobileRenderer.SetPropertyBlock(matPropBlock, 0);
		}
	}

	private void HandleDrivingUpdate()
	{
		float num = 0.15f;
		Vector3 b;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = PlayerInput.GetPlayerByIndex(0).actions["Move"].ReadValue<Vector2>();
			b = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			b = new Vector3(MyInput.GetAxis("Strafe"), 0f, MyInput.GetAxis("Walk"));
		}
		b.Normalize();
		simulatedInput = Vector3.Lerp(simulatedInput, b, Time.deltaTime * simulatedInputSpeed);
		if (prevGrounded && !grounded && notGroundedTimer >= num)
		{
			simulatedInput = Vector3.zero;
		}
		HandleAnimation(b.x);
		if (speedGauge != null)
		{
			speedGauge.MapToGaugeTargetAngle(b.z);
		}
		bool flag = body.transform.InverseTransformDirection(body.velocity).z < -0.1f;
		if (grounded)
		{
			Vector3 forward = transform.forward;
			Vector3 vector2 = simulatedInput.z * forward * acceleration * Time.deltaTime;
			Vector3 vector3 = body.velocity + vector2;
			float maxLength = (flag ? maxReverseSpeed : maxSpeed);
			Vector3 vector4 = Vector3.ClampMagnitude(vector3.XZOnly(), maxLength);
			body.velocity = new Vector3(vector4.x, vector3.y, vector4.z);
		}
		if (grounded)
		{
			float num2 = maxRotationSpeed * turnVsSpeedCurve.Evaluate(velocityCalc.velocityMagnitude);
			if (flag)
			{
				num2 *= -1f;
			}
			Vector3 vector5 = Vector3.up * b.x * num2 * Time.deltaTime;
			if (vector5 != Vector3.zero)
			{
				body.MoveRotation(Quaternion.Euler(body.rotation.eulerAngles + vector5));
			}
		}
		else if (notGroundedTimer > num)
		{
			Vector3 vector6 = Vector3.right * simulatedInput.z * (airControlSpeed * 0.75f) + Vector3.forward * (0f - simulatedInput.x) * airControlSpeed * Time.deltaTime;
			if (vector6 != Vector3.zero)
			{
				body.MoveRotation(Quaternion.Euler(body.rotation.eulerAngles + vector6));
			}
		}
	}

	private void HandleAnimation(float xInput)
	{
		Network_Player drivingPlayer = DrivingPlayer;
		if (animator != null && drivingPlayer != null)
		{
			if (drivingPlayer.IsLocalPlayer)
			{
				float a = animator.GetFloat("Steer");
				a = Mathf.Lerp(a, xInput, Time.deltaTime * animationSpeed);
				animator.SetFloat("Steer", a);
				DrivingPlayer.Animator.anim.SetFloat("VelocityX", a);
			}
			else
			{
				animator.SetFloat("Steer", xInput);
			}
		}
	}

	private bool TryTakingSeat(Network_Player player, ref int seatIndex)
	{
		AttachPlayer attachPlayer = null;
		if (Raft_Network.IsHost && seatIndex < 0)
		{
			for (int i = 0; i < attachPlayerScripts.Length; i++)
			{
				if (!attachPlayerScripts[i].HasPlayerAttached)
				{
					seatIndex = i;
					break;
				}
			}
		}
		if (seatIndex >= 0 && seatIndex < attachPlayerScripts.Length)
		{
			AttachPlayer attachPlayer2 = attachPlayerScripts[seatIndex];
			if (!Raft_Network.IsHost || !attachPlayer2.HasPlayerAttached)
			{
				attachPlayer = attachPlayer2;
			}
		}
		if (attachPlayer == null)
		{
			return false;
		}
		if (seatIndex == 0)
		{
			if (emitter_engine != null)
			{
				emitter_engine.Stop();
				emitter_engine.Play();
			}
			ResetNetworkPropertiesToSnowmobile();
		}
		attachPlayer.StartCarryingPlayer(player);
		player.PersonController.externalVelocity = Vector3.zero;
		player.PersonController.ResetPlayerPivotToOriginal();
		if (player.IsLocalPlayer)
		{
			player.PlayerScript.mouseLookXScript.isChild = true;
			player.PlayerScript.mouseLookXScript.ResetRotations();
			player.Animator.SetAnimation(PlayerAnimation.Index_0_Idle);
			canLeave = false;
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Start(HapticEvent.Snowmobile);
		}
		return true;
	}

	private bool LeaveSeat(Network_Player player)
	{
		if (player == null)
		{
			return false;
		}
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer attachPlayer in array)
		{
			if (attachPlayer != null && attachPlayer.carriedPlayer == player)
			{
				return LeaveSeat(attachPlayer);
			}
		}
		return false;
	}

	private bool LeaveSeat(AttachPlayer attachScript)
	{
		if (attachScript == null || attachScript.carriedPlayer == null)
		{
			return false;
		}
		Network_Player carriedPlayer = attachScript.carriedPlayer;
		attachScript.StopCarryingPlayer(manipulatePosition: true, manipulateEuler: false);
		if (carriedPlayer.IsLocalPlayer)
		{
			carriedPlayer.PlayerScript.mouseLookXScript.isChild = false;
			carriedPlayer.PlayerScript.mouseLookXScript.SetTargetRotXToCurrentRotation();
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Stop(HapticEvent.Snowmobile);
		}
		if (GetPlayerCountInSeats() == 0 && emitter_engine != null)
		{
			emitter_engine.EventInstance.keyOff();
		}
		return true;
	}

	private void ResetNetworkPropertiesToSnowmobile()
	{
		networkPosition = transform.position;
		networkEuler = transform.eulerAngles;
	}

	private int GetPlayerCountInSeats()
	{
		if (attachPlayerScripts == null)
		{
			return 0;
		}
		int num = 0;
		AttachPlayer[] array = attachPlayerScripts;
		foreach (AttachPlayer attachPlayer in array)
		{
			if (attachPlayer != null && attachPlayer.HasPlayerAttached)
			{
				num++;
			}
		}
		return num;
	}

	public RGD_Snowmobile GetRGD()
	{
		RGD_Snowmobile rGD_Snowmobile = new RGD_Snowmobile();
		rGD_Snowmobile.Position = transform.position;
		rGD_Snowmobile.Rotation = transform.eulerAngles;
		rGD_Snowmobile.mobileObjectIndex = (int)ObjectIndex;
		rGD_Snowmobile.mobileBehaviourIndex = (int)BehaviourIndex;
		List<RGD_SnowmobileSeat> list = new List<RGD_SnowmobileSeat>();
		for (int i = 0; i < attachPlayerScripts.Length; i++)
		{
			AttachPlayer attachPlayer = attachPlayerScripts[i];
			if (attachPlayer != null && attachPlayer.HasPlayerAttached)
			{
				list.Add(new RGD_SnowmobileSeat(attachPlayer.carriedPlayer, i));
			}
		}
		if (list != null && list.Count > 0)
		{
			rGD_Snowmobile.rgdSeats = list.ToArray();
		}
		return rGD_Snowmobile;
	}

	public void RestoreSeats(RGD_Snowmobile rgdSnowmobile)
	{
		if (rgdSnowmobile.rgdSeats == null || !Network)
		{
			return;
		}
		RGD_SnowmobileSeat[] rgdSeats = rgdSnowmobile.rgdSeats;
		foreach (RGD_SnowmobileSeat rGD_SnowmobileSeat in rgdSeats)
		{
			if (!(rGD_SnowmobileSeat == null))
			{
				Network_Player playerFromID = Network.GetPlayerFromID(new Network_UserId(rGD_SnowmobileSeat.steamID));
				if (playerFromID != null)
				{
					int seatIndex = rGD_SnowmobileSeat.seatIndex;
					TryTakingSeat(playerFromID, ref seatIndex);
				}
			}
		}
	}

	public override void Serialize_Update()
	{
		base.Serialize_Update();
		AddMessage(new Message_Snowmobile_Update(Messages.Snowmobile_Update, Network.NetworkIDManager, ObjectIndex, body.transform.position, body.transform.eulerAngles));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Snowmobile_Update:
		{
			Message_Snowmobile_Update message_Snowmobile_Update = msg as Message_Snowmobile_Update;
			networkPosition = message_Snowmobile_Update.Position;
			networkEuler = message_Snowmobile_Update.Euler;
			return true;
		}
		case Messages.Snowmobile_TakeSeat:
		{
			Message_Snowmobile_TakeSeat message_Snowmobile_TakeSeat = msg as Message_Snowmobile_TakeSeat;
			Network_Player playerFromID = Network.GetPlayerFromID(message_Snowmobile_TakeSeat.SteamID);
			if (playerFromID == null)
			{
				return false;
			}
			return TryTakingSeat(playerFromID, ref message_Snowmobile_TakeSeat.seatIndex);
		}
		case Messages.Snowmobile_LeaveSeat:
		{
			Message_NetworkBehaviour_ID_SteamID message_NetworkBehaviour_ID_SteamID = msg as Message_NetworkBehaviour_ID_SteamID;
			LeaveSeat(Network.GetPlayerFromID(message_NetworkBehaviour_ID_SteamID.SteamID));
			return true;
		}
		case Messages.Snowmobile_Impact:
		{
			Message_NetworkBehaviour_ID_Float message_NetworkBehaviour_ID_Float = msg as Message_NetworkBehaviour_ID_Float;
			PlayImpactSound(message_NetworkBehaviour_ID_Float.floatValue, networked: false);
			return true;
		}
		default:
			return base.Deserialize(msg, remoteID);
		}
	}
}
