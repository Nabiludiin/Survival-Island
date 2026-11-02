using System;
using FMOD.Studio;
using FMODUnity;
using UltimateWater;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PersonController : MonoBehaviour
{
	public static Action<SubmersionState> CameraSubmersionAction;

	private SubmersionState submersionState;

	public float normalSpeed = 3f;

	public float sprintSpeed = 6f;

	public float swimSpeed = 2f;

	public float gravity = 20f;

	public float stopGravitySpeed = 5f;

	[SerializeField]
	private float minValidGroundAngle;

	[SerializeField]
	private float groundVelocityModifier = 1f;

	[SerializeField]
	private float waterVelocityModifier = 1f;

	[SerializeField]
	private float currentGroundAngle;

	[SerializeField]
	private Vector3 groundAngle;

	[SerializeField]
	private Vector3 groundPoint;

	public float jumpSpeed = 8f;

	[SerializeField]
	private float jumpCooldown = 0.3f;

	[SerializeField]
	private float coyoteJumpDuration = 0.4f;

	[SerializeField]
	private float crouchHeightCamera = 0.5f;

	[SerializeField]
	private float crouchSpeed = 1f;

	[Tooltip("Collider height is divided by this number")]
	[SerializeField]
	private float crouchHeight = 2f;

	[Space(10f)]
	public ControllerType controllerType;

	public bool moving;

	public bool sprinting;

	private bool cheatSprinting;

	private bool isMovementFree = true;

	public float groundRayLength;

	public float downwardSlopeRayLength;

	public float slopeMinDistance = 0.05f;

	public float submersionCameraOffset;

	public bool networkGrounded;

	public float network_movementLerpSpeed = 8f;

	public float network_rotationLerpSpeed = 10f;

	public float moveMagLimit = 0.1f;

	[HideInInspector]
	public bool justLanded;

	public EventReference er_jump;

	public EventReference er_landGround;

	public EventReference er_landWater;

	[SerializeField]
	private StudioEventEmitter eventEmitter_footstep;

	[SerializeField]
	private StudioEventEmitter eventEmitter_swim;

	[SerializeField]
	private StudioEventEmitter eventEmitter_fall3D;

	[SerializeField]
	private StudioEventEmitter eventEmitter_fall2D;

	private EventInstance eventInstance_jump;

	private EventInstance eventInstance_land;

	private string defaultWalkSound;

	[SerializeField]
	private float stepDistance = 0.25f;

	[SerializeField]
	private float swimStepDistance = 0.25f;

	[SerializeField]
	private float fallSoundMaxValue = 20f;

	private Vector3 lastPlayedFootstepPos;

	private float _currentFallDuration;

	[SerializeField]
	private float fallDamageMultiplier = 20f;

	[SerializeField]
	private float minimumFallDuration = 1.4f;

	[SerializeField]
	private float baseFallDamage = 15f;

	[SerializeField]
	private float maxFallDamage = 50f;

	private WaterCamera waterCamera;

	[HideInInspector]
	public CharacterController controller;

	public Vector3 externalVelocity;

	public float maxExternalWaterVelocity;

	public float maxExternalGroundedVelocity;

	public float externalVelocityGroundedDamping = 1f;

	public float externalVelocityWaterDamping = 2f;

	public float externalVelocityGroundControll = 0.1f;

	public float externalVelocityWaterControll = 0.2f;

	[SerializeField]
	private bool recentlyJumped;

	private float resetVelocityOnGroundedTimer;

	private Transform camTransform;

	private Transform lockedPivot;

	[HideInInspector]
	public WaterFloatOxygenBounds waterFloat;

	private Vector3 moveDirection = Vector3.zero;

	private Vector3 originalPlayerPivotPosition;

	private bool completelyStarted;

	private bool networkPositionIsLocalLockedPivot;

	private Vector3 networkPosition;

	private float networkRotationY;

	private float networkRotationX;

	private Network_Player playerNetwork;

	private RaycastHit groundRaycastHit;

	private bool autoRun;

	private Settings settings;

	private bool runToggled;

	private bool usedCoyoteJump;

	[HideInInspector]
	public bool rotateWithBuoyancy;

	public bool climbing;

	public bool airPillar;

	public bool crouching;

	public Transform ladder;

	public PlayerStats playerStats;

	public Terrain_GroundRaycaster terrainRaycaster;

	private float originalControllerColliderHeight;

	private float defaultCharacterGroundedFallSpeed = -0.1f;

	private InputAction actionMove;

	[SerializeField]
	private InputActionReference inputJump;

	[SerializeField]
	private InputActionReference inputSprint;

	[SerializeField]
	private InputActionReference inputCrouch;

	public bool HasRaftAsParent
	{
		get
		{
			if (transform == null || transform.parent == null)
			{
				return false;
			}
			return transform.parent == lockedPivot;
		}
	}

	public SubmersionState SubmersionState => submersionState;

	public float GroundVelocityModifier
	{
		get
		{
			return groundVelocityModifier;
		}
		set
		{
			groundVelocityModifier = value;
		}
	}

	public float WaterVelocityModifier
	{
		get
		{
			return waterVelocityModifier;
		}
		set
		{
			waterVelocityModifier = value;
		}
	}

	public bool CanCoyoteJump => CurrentFallDuration < coyoteJumpDuration;

	public bool IsConsideredGrounded
	{
		get
		{
			if (IsGrounded || CanCoyoteJump)
			{
				return !recentlyJumped;
			}
			return false;
		}
	}

	public bool IsGrounded => controller.isGrounded;

	public bool IsMovementFree
	{
		get
		{
			return isMovementFree;
		}
		set
		{
			isMovementFree = value;
		}
	}

	public bool IsNotFreeFalling
	{
		get
		{
			if (controllerType != ControllerType.Water && !climbing)
			{
				return !IsMovementFree;
			}
			return true;
		}
	}

	public float CurrentFallDuration => _currentFallDuration;

	public float DistanceToObstructionUnderneath
	{
		get
		{
			if (HasObstructionUnderneath)
			{
				return Vector3.Distance(transform.position, groundRaycastHit.point);
			}
			return float.MaxValue;
		}
	}

	public bool HasObstructionUnderneath => groundRaycastHit.collider != null;

	public bool IsWithinOxygenRegion
	{
		get
		{
			if (SubmersionState != SubmersionState.None)
			{
				return waterFloat.HasOxygenRegion;
			}
			return true;
		}
	}

	private void Awake()
	{
		playerNetwork = GetComponent<Network_Player>();
		waterFloat = GetComponent<WaterFloatOxygenBounds>();
		controller = GetComponent<CharacterController>();
		this.playerStats = GetComponent<PlayerStats>();
		PlayerStats playerStats = this.playerStats;
		playerStats.OnDeath = (Action)Delegate.Combine(playerStats.OnDeath, new Action(OnPlayerDeath));
		originalControllerColliderHeight = controller.height;
		originalPlayerPivotPosition = playerNetwork.playerPivot.localPosition;
		defaultWalkSound = eventEmitter_footstep.Event;
	}

	private void Start()
	{
		camTransform = playerNetwork.CameraTransform;
		if (SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		waterCamera = playerNetwork.CameraTransform.GetComponent<WaterCamera>();
		if (playerNetwork.IsLocalPlayer)
		{
			Helper.SetCursorVisibleAndLockState(state: false, CursorLockMode.Locked);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			eventEmitter_fall2D.Play();
		}
		else
		{
			eventEmitter_fall3D.Play();
		}
		eventInstance_jump = RuntimeManager.CreateInstance(er_jump);
		eventInstance_land = RuntimeManager.CreateInstance(er_landGround);
		RuntimeManager.AttachInstanceToGameObject(eventInstance_land, transform);
		RuntimeManager.AttachInstanceToGameObject(eventInstance_jump, transform);
		lastPlayedFootstepPos = transform.position;
		settings = ComponentManager<Settings>.Value;
		actionMove = PlayerInput.GetPlayerByIndex(0).actions["Move"];
		completelyStarted = true;
	}

	private void Update()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			Physics.SphereCast(transform.position, 0.5f, Vector3.down, out groundRaycastHit, 100f + BlockCreator.MaxHeight, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore);
			float num = 0f;
			num = ((!waterFloat.HasOxygenRegion) ? waterFloat.PrevYPos : waterFloat.oxygenRegionComponent.SurfacePointY);
			float y = playerNetwork.Camera.transform.position.y;
			if (y > num)
			{
				CameraSubmersionChanged(SubmersionState.None, !Cheat.UseCheats || !playerNetwork.flightCamera.enabled);
			}
			if (IsWithinOxygenRegion)
			{
				CameraSubmersionChanged(SubmersionState.None, setControllerType: false);
			}
			if (y <= num && !waterFloat.HasOxygenRegion)
			{
				CameraSubmersionChanged(SubmersionState.Full, !Cheat.UseCheats || !playerNetwork.flightCamera.enabled);
			}
			if (!playerNetwork.PlayerScript.IsDead)
			{
				if (resetVelocityOnGroundedTimer > 0f)
				{
					resetVelocityOnGroundedTimer = Mathf.Clamp(resetVelocityOnGroundedTimer - Time.deltaTime, 0f, float.MaxValue);
				}
				if (controllerType == ControllerType.Ground && groundRaycastHit.collider != null)
				{
					terrainRaycaster.DoRay();
				}
				else
				{
					terrainRaycaster.Reset();
				}
				if (controllerType == ControllerType.Ground)
				{
					GroundControll(groundRaycastHit);
					if (terrainRaycaster.IsCurrentTerrainIDUsingCustomFriction())
					{
						SO_TerrainType currentTerrainType = terrainRaycaster.CurrentTerrainType;
						Vector3 normalized = moveDirection.XZOnly().normalized;
						Vector3 vector = moveDirection.XZOnly() * (1f - currentTerrainType.normalMovementControll);
						moveDirection -= vector;
						AddExternalVelocity(normalized * currentTerrainType.externalVelocityAcceleration * Time.deltaTime);
						ClampExternalVelocity(currentTerrainType.maxExternalVelocity);
						externalVelocity.x = Mathf.MoveTowards(externalVelocity.x, 0f, currentTerrainType.externalVelocityGroundedDamping * Time.deltaTime);
						externalVelocity.z = Mathf.MoveTowards(externalVelocity.z, 0f, currentTerrainType.externalVelocityGroundedDamping * Time.deltaTime);
					}
					else
					{
						ClampExternalVelocity(maxExternalGroundedVelocity);
						externalVelocity.x = Mathf.MoveTowards(externalVelocity.x, 0f, externalVelocityGroundedDamping * Time.deltaTime);
						externalVelocity.z = Mathf.MoveTowards(externalVelocity.z, 0f, externalVelocityGroundedDamping * Time.deltaTime);
					}
				}
				else if (controllerType == ControllerType.Water)
				{
					WaterControll();
					ClampExternalVelocity(maxExternalWaterVelocity);
					externalVelocity.x = Mathf.MoveTowards(externalVelocity.x, 0f, externalVelocityWaterDamping * Time.deltaTime);
					externalVelocity.z = Mathf.MoveTowards(externalVelocity.z, 0f, externalVelocityWaterDamping * Time.deltaTime);
				}
				if (externalVelocity.XZOnly() != Vector3.zero)
				{
					Vector3 vector2 = moveDirection.XZOnly().normalized * (externalVelocity.magnitude * externalVelocityGroundControll);
					if (controllerType == ControllerType.Ground)
					{
						_ = resetVelocityOnGroundedTimer;
						_ = 0f;
					}
					moveDirection += externalVelocity.XZOnly() + vector2;
				}
				controller.Move(moveDirection * Time.deltaTime);
				HandleSlopes();
			}
			if (GameModeValueManager.GetCurrentGameModeValue().playerSpecificVariables.recieveFallDamage)
			{
				HandleFalling();
			}
		}
		else
		{
			Vector3 position = transform.position;
			if (playerNetwork.BedComponent != null && !playerNetwork.BedComponent.Sleeping && playerNetwork.RessurectComponent != null && !playerNetwork.RessurectComponent.BeingCarried && (bool)playerNetwork.PlayerNetworkManager && !playerNetwork.PlayerNetworkManager.IsAttached && isMovementFree)
			{
				if (!playerNetwork.PlayerScript.IsDead)
				{
					float y2 = Mathf.LerpAngle(transform.eulerAngles.y, networkRotationY, Time.deltaTime * network_rotationLerpSpeed);
					transform.eulerAngles = new Vector3(transform.eulerAngles.x, y2, transform.eulerAngles.z);
					if (controllerType == ControllerType.Water)
					{
						Vector3 eulerAngles = playerNetwork.playerPivot.eulerAngles;
						eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, networkRotationX, Time.deltaTime * network_rotationLerpSpeed);
						playerNetwork.playerPivot.eulerAngles = eulerAngles;
					}
					else
					{
						ResetPlayerPivotToOriginal();
					}
				}
				if (networkPositionIsLocalLockedPivot)
				{
					transform.SetParentSafe(lockedPivot);
				}
				else if (!networkPositionIsLocalLockedPivot)
				{
					transform.SetParentSafe(null);
				}
				if (networkPositionIsLocalLockedPivot)
				{
					if (Vector3.Distance(transform.localPosition, networkPosition) > 10f)
					{
						transform.localPosition = networkPosition;
					}
					transform.localPosition = Vector3.Lerp(transform.localPosition, networkPosition, Time.deltaTime * network_movementLerpSpeed);
				}
				else
				{
					if (Vector3.Distance(transform.position, networkPosition) > 10f)
					{
						transform.position = networkPosition;
					}
					transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * network_movementLerpSpeed);
				}
			}
			if (controller.isGrounded)
			{
				playerNetwork.Animator.animTargetVelocity.y = 0f;
				playerNetwork.Animator.anim.SetFloat("VelocityY", 0f);
			}
			float magnitude = (transform.position - position).magnitude;
			moving = magnitude > moveMagLimit;
			SetGroundMaterial();
			HandleFootstepSound();
			HandleFallSound((transform.position.y - position.y) * 10f);
			HandleSwimSound();
		}
		controller.height = (crouching ? (originalControllerColliderHeight / crouchHeight) : originalControllerColliderHeight);
		controller.center = (crouching ? new Vector3(0f, (0f - originalControllerColliderHeight) / (crouchHeight * 2f), 0f) : Vector3.zero);
		if (playerNetwork.IsLocalPlayer)
		{
			Vector3 vector3 = originalPlayerPivotPosition;
			if (playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
			{
				vector3.y -= 0.05f;
			}
			else
			{
				vector3.y -= crouchHeightCamera;
			}
			Vector3 target = (crouching ? vector3 : originalPlayerPivotPosition);
			playerNetwork.playerPivot.localPosition = Vector3.MoveTowards(playerNetwork.playerPivot.localPosition, target, Time.deltaTime * crouchSpeed);
		}
		if (playerNetwork.PlayerScript.IsDead)
		{
			if (playerNetwork.IsLocalPlayer)
			{
				bool flag = Physics.Raycast(transform.position, Vector3.down, out var hitInfo, 1f, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore);
				if (flag)
				{
					if (Helper.ObjectIsOnLayer(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_Raft))
					{
						if (!HasRaftAsParent)
						{
							transform.SetParent(lockedPivot);
						}
					}
					else if (transform.parent != null)
					{
						transform.SetParent(null);
					}
					waterFloat.setToSurface = false;
					IsMovementFree = false;
					controller.enabled = false;
					externalVelocity = Vector3.zero;
				}
				else
				{
					if (transform.parent != null)
					{
						transform.SetParent(null);
					}
					waterFloat.setToSurface = controllerType == ControllerType.Water && !playerNetwork.PlayerScript.diedUnderWater;
					if (!waterFloat.setToSurface && !playerNetwork.PlayerScript.diedUnderWater)
					{
						IsMovementFree = true;
						controller.enabled = true;
						externalVelocity.y -= gravity * Time.deltaTime;
						controller.Move(Vector3.up * externalVelocity.y * Time.deltaTime);
					}
					else
					{
						IsMovementFree = false;
						controller.enabled = false;
						externalVelocity = Vector3.zero;
					}
				}
				playerNetwork.Animator.anim.SetBool("InAir", !flag);
			}
			playerNetwork.Animator.anim.SetBool("InWater", controllerType == ControllerType.Water);
		}
		HandleRotationOfPlayer();
		if (Cheat.UseCheats && Input.GetMouseButtonDown(4))
		{
			cheatSprinting = !cheatSprinting;
		}
	}

	private void OnDestroy()
	{
		playerNetwork = null;
		waterFloat = null;
		controller = null;
		playerStats = null;
		eventInstance_jump.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eventInstance_jump.release();
		eventInstance_jump.clearHandle();
		eventInstance_land.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eventInstance_land.release();
		eventInstance_land.clearHandle();
	}

	private void OnDisable()
	{
		autoRun = false;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.tag == "Ladder" && playerNetwork.CarryingComponent.CanJumpWithCarriedItem)
		{
			climbing = true;
			ladder = other.transform;
		}
		else if (other.tag == "AirPillar")
		{
			airPillar = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ladder")
		{
			DetachFromLadders();
		}
		else if (other.tag == "AirPillar")
		{
			airPillar = false;
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		currentGroundAngle = hit.normal.y;
		groundAngle = hit.normal.normalized;
		groundPoint = hit.point;
	}

	private void OnPlayerDeath()
	{
		DetachFromLadders();
		airPillar = false;
	}

	public void CameraSubmersionChanged(SubmersionState state, bool setControllerType)
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		if (submersionState != state)
		{
			if (state == SubmersionState.Partial)
			{
				state = SubmersionState.Full;
			}
			submersionState = state;
			if (CameraSubmersionAction != null)
			{
				CameraSubmersionAction(submersionState);
			}
		}
		if (!setControllerType)
		{
			return;
		}
		switch (submersionState)
		{
		case SubmersionState.Full:
			if (!climbing)
			{
				SwitchControllerType(ControllerType.Water);
			}
			break;
		case SubmersionState.None:
		case SubmersionState.Partial:
			break;
		}
	}

	public void SetNetworkProperties(Message_Player_Update msg)
	{
		Vector4 animationVelocity = msg.AnimationVelocity;
		if (playerNetwork.Animator != null)
		{
			if (animationVelocity.x != 0f || animationVelocity.z != 0f)
			{
				playerNetwork.Animator.anim.SetFloat("MoveSpeed", 1f);
			}
			else
			{
				playerNetwork.Animator.anim.SetFloat("MoveSpeed", 0f);
			}
		}
		networkPosition = msg.Position;
		networkRotationX = msg.RotationX;
		networkRotationY = msg.RotationY;
		networkGrounded = msg.ControllerIsGrounded;
		if (controllerType != msg.ControllerType)
		{
			SwitchControllerType(msg.ControllerType);
		}
		networkPositionIsLocalLockedPivot = msg.RaftAsParent;
		if (controllerType == ControllerType.Ground)
		{
			for (int i = 0; i < msg.anim_triggers.Length; i++)
			{
				if (msg.anim_triggers[i] == "Jump")
				{
					RuntimeManager.PlayOneShot(er_jump, transform.position);
				}
			}
		}
		if (msg.Anim_JustLanded)
		{
			eventInstance_land.start();
		}
		crouching = msg.Anim_Crouching;
	}

	public void SetNetworkPosition(Vector3 position)
	{
		networkPosition = position;
	}

	public void ResetPlayerPivotToOriginal()
	{
		if (playerNetwork != null && playerNetwork.playerPivot != null)
		{
			playerNetwork.playerPivot.localPosition = originalPlayerPivotPosition;
			playerNetwork.playerPivot.localEulerAngles = Vector3.zero;
		}
	}

	public void ParentPlayerToLockedPivot()
	{
		if (transform != null && lockedPivot != null)
		{
			transform.SetParentSafe(lockedPivot);
		}
	}

	public void SetExternalVelocity(Vector3 velocity, float resetVelocityOnGroundedTimer = 0f)
	{
		externalVelocity = velocity;
		if (resetVelocityOnGroundedTimer > 0f)
		{
			this.resetVelocityOnGroundedTimer = resetVelocityOnGroundedTimer;
		}
	}

	public void AddExternalVelocity(Vector3 velocity, float resetVelocityOnGroundedTimer = 0f)
	{
		externalVelocity += velocity;
		if (resetVelocityOnGroundedTimer > 0f)
		{
			this.resetVelocityOnGroundedTimer = resetVelocityOnGroundedTimer;
		}
	}

	public void ResetExternalVelocity()
	{
		externalVelocity = Vector3.zero;
		resetVelocityOnGroundedTimer = 0f;
	}

	public void SwitchControllerType(ControllerType newType)
	{
		if (newType == controllerType || playerNetwork.RessurectComponent.BeingCarried || !completelyStarted)
		{
			return;
		}
		switch (newType)
		{
		case ControllerType.Ground:
			airPillar = false;
			waterFloat.setToSurface = false;
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.cameraBobAnimator.SetBool("InWater", value: false);
			}
			playerNetwork.Animator.anim.SetBool("InWater", value: false);
			if (!playerNetwork.IsLocalPlayer)
			{
				Vector3 eulerAngles = playerNetwork.playerPivot.eulerAngles;
				eulerAngles.x = 0f;
				playerNetwork.playerPivot.eulerAngles = eulerAngles;
			}
			RuntimeManager.PlayOneShot(er_landWater, transform.position);
			if (!playerNetwork.IsLocalPlayer)
			{
				ParticleManager.PlaySystem("WaterSplash_Player", transform.position);
			}
			break;
		case ControllerType.Water:
			if (HasRaftAsParent)
			{
				return;
			}
			crouching = false;
			externalVelocity.x = (externalVelocity.z = 0f);
			if (playerNetwork.IsLocalPlayer)
			{
				playerNetwork.cameraBobAnimator.SetBool("InWater", value: true);
			}
			playerNetwork.Animator.anim.SetBool("InWater", value: true);
			playerNetwork.Animator.anim.SetBool("InAir", value: false);
			playerNetwork.Animator.anim.SetBool("Crouching", value: false);
			RuntimeManager.PlayOneShot(er_landWater, transform.position);
			eventEmitter_fall2D.SetParameter("velocity", 0f);
			eventEmitter_fall3D.SetParameter("velocity", 0f);
			if (!playerNetwork.IsLocalPlayer)
			{
				ParticleManager.PlaySystem("WaterSplash_Player", transform.position);
			}
			break;
		}
		controllerType = newType;
	}

	public void HandleRotationOfPlayer()
	{
		if (!playerNetwork.IsLocalPlayer || playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel)
		{
			transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
		}
		else if (controllerType == ControllerType.Ground)
		{
			if (rotateWithBuoyancy || playerNetwork.BlockCreator.aimingWithPlaceable || playerNetwork.Animator.IsInFullBodyAnimation(PlayerFullBodyAnimation.Ziplining_4))
			{
				transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
			}
			else if (!playerNetwork.RessurectComponent.BeingCarried && !Player.LocalPlayerIsDead)
			{
				transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
			}
		}
	}

	public void ResetFallDuration()
	{
		_currentFallDuration = 0f;
	}

	public void SetCorrectGroundParent(RaycastHit groundHit)
	{
		if (lockedPivot == null && SingletonGeneric<GameManager>.Singleton != null)
		{
			lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		}
		bool flag = false;
		bool flag2 = groundHit.collider != null;
		if (flag2)
		{
			flag = Helper.ObjectIsOnLayer(groundHit.transform.gameObject, LayerMasks.MASK_GroundMask_NonRaft);
			if (groundHit.distance < 0.4f && resetVelocityOnGroundedTimer <= 0f && !terrainRaycaster.IsCurrentTerrainIDUsingCustomFriction())
			{
				externalVelocity.x = (externalVelocity.z = 0f);
			}
			if (!flag && (transform.parent == null || !HasRaftAsParent))
			{
				transform.SetParent(lockedPivot);
				return;
			}
		}
		if ((!flag2 && !controller.isGrounded && transform.parent != null) || (flag2 & flag))
		{
			transform.SetParent(null);
		}
	}

	public void DetachFromLadders()
	{
		climbing = false;
		ladder = null;
	}

	private void HandleSlopes()
	{
		if (controllerType != ControllerType.Ground)
		{
			return;
		}
		Debug.DrawRay(groundPoint, groundAngle, Color.magenta);
		if (terrainRaycaster.IsCurrentTerrainIDUsingCustomFriction())
		{
			AddExternalVelocity((groundAngle + Vector3.down) * 1f);
		}
		if (!climbing && !controller.isGrounded && !recentlyJumped)
		{
			if (externalVelocity.y <= 0f)
			{
				Vector3 vector = transform.position - Vector3.up * (controller.height / 2f);
				bool flag = Physics.Raycast(vector, Vector3.down, out var hitInfo, downwardSlopeRayLength, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore);
				Debug.DrawRay(vector, Vector3.down * downwardSlopeRayLength, (!flag) ? Color.yellow : Color.red);
				if (flag && hitInfo.distance >= slopeMinDistance)
				{
					controller.Move(Vector3.down * hitInfo.distance);
				}
			}
		}
		else if ((terrainRaycaster.IsCurrentTerrainIDUsingCustomFriction() || transform.position.y > 2f) && currentGroundAngle <= minValidGroundAngle && !climbing && !recentlyJumped && groundAngle != Vector3.zero)
		{
			controller.Move(Vector3.down + groundAngle * Time.deltaTime);
			groundAngle = Vector3.zero;
		}
	}

	private void HandleFalling()
	{
		_currentFallDuration += Time.deltaTime;
		if (IsNotFreeFalling)
		{
			ResetFallDuration();
		}
		if (controller.isGrounded)
		{
			OnHitGround();
		}
	}

	private void OnHitGround()
	{
		if (_currentFallDuration >= minimumFallDuration)
		{
			CalculateFallDamage(_currentFallDuration);
		}
		ResetFallDuration();
	}

	private void CalculateFallDamage(float p_fallDuration)
	{
		float num = baseFallDamage;
		num += p_fallDuration * p_fallDuration * fallDamageMultiplier;
		if (num > maxFallDamage)
		{
			num = maxFallDamage;
		}
		playerNetwork.Stats.Damage(num, transform.position, Vector3.up, EntityType.FallDamage, isLocal: false);
	}

	private void ClampExternalVelocity(float value)
	{
		externalVelocity.x = Mathf.Clamp(externalVelocity.x, 0f - value, value);
		externalVelocity.z = Mathf.Clamp(externalVelocity.z, 0f - value, value);
	}

	private void SetGroundMaterial()
	{
		if (Physics.Raycast(transform.position, Vector3.down, out var hitInfo, groundRayLength, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore))
		{
			float value = ((!Helper.ObjectIsOnLayer(hitInfo.transform.gameObject, LayerMasks.MASK_GroundMask_NonRaft)) ? 1 : 2);
			eventEmitter_footstep.SetParameter("material", value);
			eventInstance_jump.setParameterByName("material", value);
			eventInstance_land.setParameterByName("material", value);
		}
	}

	private void ResetJump()
	{
		recentlyJumped = false;
	}

	private void HandleFallSound(float velocity)
	{
		float value = Mathf.Abs(Mathf.Clamp(velocity, 0f - fallSoundMaxValue, 0f));
		if (playerNetwork.IsLocalPlayer)
		{
			eventEmitter_fall2D.SetParameter("velocity", value);
		}
		else
		{
			eventEmitter_fall3D.SetParameter("velocity", value);
		}
	}

	private void HandleFootstepSound()
	{
		if (controllerType != ControllerType.Ground)
		{
			return;
		}
		if (playerNetwork.IsLocalPlayer)
		{
			if (!controller.isGrounded)
			{
				return;
			}
		}
		else if (!controller.isGrounded && !networkGrounded)
		{
			return;
		}
		bool hasRaftAsParent = HasRaftAsParent;
		if (!moving)
		{
			lastPlayedFootstepPos = (hasRaftAsParent ? transform.localPosition : transform.position);
			return;
		}
		Vector3 b = (hasRaftAsParent ? transform.localPosition : transform.position);
		b.y = lastPlayedFootstepPos.y;
		float num = Vector3.Distance(lastPlayedFootstepPos, b);
		float num2 = (sprinting ? (stepDistance * 1.5f) : stepDistance);
		if (num >= num2)
		{
			lastPlayedFootstepPos = (hasRaftAsParent ? transform.localPosition : transform.position);
			eventEmitter_footstep.Play();
			if (terrainRaycaster.CurrentTerrainType != null)
			{
				eventEmitter_footstep.SetParameter("Terrain", terrainRaycaster.CurrentTerrainType.terrainWalkSoundID);
			}
			else
			{
				eventEmitter_footstep.SetParameter("Terrain", 0f);
			}
		}
	}

	private void HandleSwimSound()
	{
		if (controllerType != ControllerType.Water)
		{
			return;
		}
		if (!moving)
		{
			lastPlayedFootstepPos = transform.position;
			return;
		}
		Vector3 position = transform.position;
		if (Vector3.Distance(lastPlayedFootstepPos, position) >= swimStepDistance)
		{
			lastPlayedFootstepPos = transform.position;
			eventEmitter_swim.Play();
		}
	}

	private void HandleAutoRun(ref Vector3 input)
	{
		if (MyInput.GetButtonDown("AutoRun"))
		{
			autoRun = !autoRun;
		}
		if (input.x != 0f || input.z != 0f)
		{
			autoRun = false;
		}
		if (autoRun)
		{
			input.x = (input.y = 0f);
			input.z = 1f;
			sprinting = true;
		}
	}

	private bool AllowJump()
	{
		if (!playerNetwork.CarryingComponent.CanJumpWithCarriedItem)
		{
			return false;
		}
		if (IsMovementFree && (currentGroundAngle >= minValidGroundAngle || transform.position.y < 2f))
		{
			return true;
		}
		return false;
	}

	private void GroundControll(RaycastHit groundHit)
	{
		SetCorrectGroundParent(groundHit);
		SetGroundMaterial();
		if (climbing && (ladder == null || !playerNetwork.CarryingComponent.CanJumpWithCarriedItem))
		{
			DetachFromLadders();
		}
		if (settings.controls.RunToggle)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputSprint, "Sprint") && CanvasHelper.ActiveMenu == MenuType.None)
			{
				runToggled = !runToggled;
			}
			sprinting = runToggled;
		}
		else
		{
			sprinting = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputSprint, "Sprint") && CanvasHelper.ActiveMenu == MenuType.None;
		}
		Vector3 input;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = actionMove.ReadValue<Vector2>();
			input = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			input = new Vector3(MyInput.GetAxis("Strafe"), 0f, MyInput.GetAxis("Walk"));
		}
		HandleAutoRun(ref input);
		moving = input != Vector3.zero;
		if (!IsMovementFree)
		{
			sprinting = false;
			input = Vector3.zero;
			moving = false;
			moveDirection = Vector3.zero;
		}
		bool flag;
		bool flag2;
		if (settings.controls.CrouchToggle)
		{
			flag = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCrouch, "Crouch") && !crouching;
			flag2 = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCrouch, "Crouch") && crouching;
		}
		else
		{
			flag = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputCrouch, "Crouch");
			flag2 = !flag;
		}
		if (flag)
		{
			crouching = true;
		}
		bool flag3 = false;
		if (crouching)
		{
			flag3 = Physics.BoxCast(transform.position - Vector3.up * 0.25f, Vector3.one / 8f, Vector3.up, out var _, Quaternion.identity, originalControllerColliderHeight / 2f, LayerMasks.MASK_GroundMask, QueryTriggerInteraction.Ignore);
			Debug.DrawLine(transform.position, transform.position + Vector3.up * (originalControllerColliderHeight / 2f), flag3 ? Color.red : Color.green, 1f);
			if (!flag3 & flag2)
			{
				crouching = false;
			}
		}
		if (playerNetwork.currentModel.thirdPersonSettings.ThirdPersonState)
		{
			moveDirection = camTransform.forward * input.z + camTransform.right * input.x;
		}
		else if (!climbing)
		{
			moveDirection = transform.right * input.x + transform.forward * input.z;
		}
		else
		{
			moveDirection = camTransform.forward.normalized * input.z + camTransform.right * input.x;
		}
		if (controller.isGrounded && !climbing)
		{
			usedCoyoteJump = false;
			if (externalVelocity.y < -4f)
			{
				eventInstance_land.start();
				justLanded = true;
			}
			if (resetVelocityOnGroundedTimer == 0f)
			{
				externalVelocity.y = defaultCharacterGroundedFallSpeed;
			}
		}
		else
		{
			externalVelocity.y -= gravity * Time.deltaTime;
		}
		if (!flag3 && !climbing && AllowJump() && SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputJump.action, "Jump"))
		{
			bool flag4 = false;
			if (CanCoyoteJump && !usedCoyoteJump && !controller.isGrounded && !recentlyJumped)
			{
				usedCoyoteJump = true;
				flag4 = true;
			}
			else if (controller.isGrounded)
			{
				flag4 = true;
			}
			if (flag4)
			{
				recentlyJumped = true;
				playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_Jump);
				CancelInvoke("ResetJump");
				Invoke("ResetJump", jumpCooldown);
				externalVelocity.y = jumpSpeed;
				eventInstance_jump.start();
			}
		}
		if (climbing || playerNetwork.UsingNoclip)
		{
			externalVelocity = Vector3.zero;
		}
		float value = ((camTransform.eulerAngles.x > 180f) ? (0f - (360f - camTransform.eulerAngles.x)) : camTransform.eulerAngles.x);
		Vector4 zero = Vector4.zero;
		zero.x = ((input.z < 0f) ? (0f - input.x) : input.x);
		zero.y = externalVelocity.y;
		zero.z = input.z;
		zero.w = (playerNetwork.currentModel.thirdPersonSettings.ThirdPersonModel ? 0.2f : Helper.MapValue(playerNetwork.PlayerScript.mouseLookYScript.minimumY, playerNetwork.PlayerScript.mouseLookYScript.maximumY, value, -1f, 1f));
		if (sprinting)
		{
			if (input.x != 0f)
			{
				zero.x += ((input.x < 0f) ? (-0.5f) : 0.5f);
			}
			if (input.z != 0f)
			{
				zero.z += ((input.z < 0f) ? (-0.5f) : 0.5f);
			}
		}
		if (GroundVelocityModifier != 0f)
		{
			zero *= GroundVelocityModifier;
		}
		if (Stat_WellBeing.Factor == WellBeing.Bad)
		{
			zero.x = Mathf.Clamp(zero.x, -0.5f, 0.5f);
			zero.z = Mathf.Clamp(zero.z, -0.5f, 0.5f);
		}
		playerNetwork.Animator.SetAnimVelocity(zero);
		playerNetwork.Animator.anim.SetBool("InAir", !controller.isGrounded);
		playerNetwork.Animator.anim.SetFloat("MoveSpeed", (input.x != 0f || input.z != 0f) ? 1f : 0f);
		playerNetwork.Animator.anim.SetBool("Crouching", crouching);
		float num = (sprinting ? sprintSpeed : normalSpeed);
		if (Stat_WellBeing.Factor == WellBeing.Bad || crouching)
		{
			num = normalSpeed * Stat_WellBeing.groundSpeedMultiplier;
		}
		num *= GroundVelocityModifier;
		if (sprinting && cheatSprinting)
		{
			num *= 4f;
		}
		if (climbing)
		{
			if (Mathf.Abs(ladder.InverseTransformDirection(moveDirection).normalized.z) > 0.75f && moving && moveDirection.y >= -0.5f && input.z != 0f)
			{
				moveDirection.y = 1f;
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputJump.action, "Jump"))
			{
				moveDirection.y = 1f;
			}
		}
		moveDirection.Normalize();
		moveDirection *= num;
		float num2 = 0f;
		if (input == Vector3.zero)
		{
			num2 = 0.15f;
		}
		else
		{
			num2 = (sprinting ? (sprintSpeed / normalSpeed) : 1f);
			if (Stat_WellBeing.Factor == WellBeing.Bad)
			{
				num2 *= Stat_WellBeing.groundSpeedMultiplier;
			}
			if (GroundVelocityModifier != 0f)
			{
				num2 *= GroundVelocityModifier;
			}
		}
		playerNetwork.cameraBobAnimator.SetFloat("Speed", num2);
		if (!climbing && !playerNetwork.UsingNoclip)
		{
			moveDirection.y = externalVelocity.y;
		}
		HandleFootstepSound();
		HandleFallSound(externalVelocity.y + 5f);
	}

	private void WaterControll()
	{
		if (climbing && ladder != null)
		{
			SwitchControllerType(ControllerType.Ground);
			return;
		}
		Vector3 input;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = actionMove.ReadValue<Vector2>();
			input = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			input = new Vector3(MyInput.GetAxis("Strafe"), 0f, MyInput.GetAxis("Walk"));
		}
		HandleAutoRun(ref input);
		moving = input != Vector3.zero;
		moveDirection = camTransform.forward.normalized * input.z + camTransform.right * input.x;
		if (!IsMovementFree)
		{
			sprinting = false;
			input = Vector3.zero;
			moving = false;
			moveDirection = Vector3.zero;
		}
		if (externalVelocity.y != 0f)
		{
			externalVelocity.y = Mathf.Lerp(externalVelocity.y, 0f, Time.deltaTime * stopGravitySpeed);
			if (Mathf.Abs(externalVelocity.y) <= 0.1f)
			{
				externalVelocity.y = 0f;
			}
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputJump.action, "Jump") && SubmersionState != SubmersionState.None && IsMovementFree)
		{
			moveDirection.y = 1f;
			moving = true;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputCrouch.action, "LeftControl") && IsMovementFree)
		{
			moveDirection.y = -1f;
			moving = true;
		}
		if (SubmersionState == SubmersionState.None)
		{
			if (controller.isGrounded)
			{
				SwitchControllerType(ControllerType.Ground);
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputJump.action, "Jump") && IsMovementFree)
			{
				SwitchControllerType(ControllerType.Ground);
				externalVelocity.y = jumpSpeed * 1.25f;
			}
			if (moveDirection.y > -0.5f)
			{
				moveDirection.y = 0f;
			}
		}
		waterFloat.setToSurface = controllerType == ControllerType.Water && SubmersionState == SubmersionState.None && moveDirection.y >= 0f && externalVelocity.y == 0f;
		playerNetwork.Animator.SetAnimVelocity(new Vector3(input.x, 0f, input.z));
		float num = swimSpeed;
		if (Stat_WellBeing.Factor == WellBeing.Bad)
		{
			num = swimSpeed * Stat_WellBeing.swimSpeedMultiplier;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(inputSprint.action, "Sprint") && cheatSprinting)
		{
			num = swimSpeed * 5f;
		}
		num *= WaterVelocityModifier;
		moveDirection.Normalize();
		moveDirection *= num;
		float num2 = 0f;
		if (moving)
		{
			num2 = 0.5f;
		}
		else
		{
			num2 = 1f;
			if (Stat_WellBeing.Factor == WellBeing.Bad)
			{
				num2 *= Stat_WellBeing.swimSpeedMultiplier;
			}
		}
		playerNetwork.cameraBobAnimator.SetFloat("Speed", num2);
		if (externalVelocity.y != 0f)
		{
			moveDirection.y = externalVelocity.y;
		}
		moveDirection += externalVelocity;
		HandleSwimSound();
	}
}
