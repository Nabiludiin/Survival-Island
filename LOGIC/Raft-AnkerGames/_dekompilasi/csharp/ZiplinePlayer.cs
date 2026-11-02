using System;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZiplinePlayer : MonoBehaviour
{
	public Action OnZiplineAttach;

	public Action OnZiplineDeattach;

	public bool drawGizmos;

	public LineRenderer buildZiplineRenderer;

	[SerializeField]
	private AttachPlayer attachPlayerScript;

	[SerializeField]
	private GameObject ziplineTool;

	[SerializeField]
	private GameObject ziplineToolEletric;

	[SerializeField]
	private Transform ziplineToolPivot;

	[SerializeField]
	private Transform ziplineElectricToolPivot;

	[SerializeField]
	private float zipplineYOffset;

	[SerializeField]
	private float maxZiplineSpeed = 10f;

	[SerializeField]
	private float maxElectricZiplineSpeed = 10f;

	[SerializeField]
	private float electricZiplineSpeed = 5f;

	[SerializeField]
	private float ziplineSpeedDamping = 1f;

	[SerializeField]
	private Interval_Float ziplineSpeedAcceleration;

	[SerializeField]
	private float ziplineSlackScrollSpeed = 10f;

	[SerializeField]
	private float ziplineSlackKeyboardSpeed = 10f;

	[SerializeField]
	private float minDistanceFromGround = 0.1f;

	[SerializeField]
	private Interval_Float ziplineSlackInterval;

	[SerializeField]
	private LayerMask AbortCollision;

	[SerializeField]
	private float AbortGroundOffset;

	private float speedDirectionModifier = 1f;

	private int currentZiplineModelIndex = 1;

	private bool usingMotor;

	private float movementInput;

	[SerializeField]
	private ParticleSystem particles_FP;

	[SerializeField]
	private ParticleSystem particles_TP;

	[SerializeField]
	private AnimationCurve particleEmissionCurve;

	[SerializeField]
	private StudioEventEmitterSustain eventEmitter_Zipline;

	public EventReference er_ZiplineImpact;

	[SerializeField]
	private StudioEventEmitter eventEmitter_ZiplineMotor;

	private SoundManager soundManager;

	private CanvasHelper canvas;

	private MeshPath_Zipline ziplinePath;

	private ZiplineMovementInfo moveInfo;

	private Network_Player player;

	private Vector3 localPlayerZiplinePosition;

	private Transform lockedPivot;

	[SerializeField]
	private float ziplineSpeed = 3f;

	private bool canBounce;

	private MaterialPropertyBlock mpbLinerenderer;

	[SerializeField]
	private float motorSoundRampUpMultipier = 3f;

	private float motorActiveDuration;

	[SerializeField]
	private InputActionReference inputMove;

	[SerializeField]
	private InputActionReference inputCancel;

	[SerializeField]
	private InputActionReference inputJump;

	[SerializeField]
	private InputActionReference inputContext;

	[SerializeField]
	private InputActionReference inputSlackUp;

	[SerializeField]
	private InputActionReference inputSlackDown;

	public bool IsAttachedToZipline => ziplinePath != null;

	public bool ViableForMotorUsage => currentZiplineModelIndex == 1;

	private void Awake()
	{
		MeshPathBase.OnFinishedCreatingMeshPath = (Action)Delegate.Combine(MeshPathBase.OnFinishedCreatingMeshPath, new Action(OnFinishedCreatingMeshPath));
	}

	private void Start()
	{
		lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		player = GetComponentInParent<Network_Player>();
		canvas = ComponentManager<CanvasHelper>.Value;
		soundManager = ComponentManager<SoundManager>.Value;
		DisableModels();
		if (player.IsLocalPlayer)
		{
			ThirdPerson thirdPersonSettings = player.currentModel.thirdPersonSettings;
			thirdPersonSettings.OnThirdpersonStateChange = (Action<bool>)Delegate.Combine(thirdPersonSettings.OnThirdpersonStateChange, new Action<bool>(ThirdPersonStateChange));
		}
	}

	private void OnDestroy()
	{
		player = null;
	}

	private void OnDrawGizmos()
	{
		if (drawGizmos && IsAttachedToZipline)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(ziplinePath.transform.TransformPoint(moveInfo.forwardPosition), 0.1f);
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(ziplinePath.transform.TransformPoint(moveInfo.backwardPosition), 0.1f);
		}
	}

	private void Update()
	{
		if (!player.IsLocalPlayer)
		{
			UpdateZiplineRemote();
			if (buildZiplineRenderer.enabled)
			{
				buildZiplineRenderer.enabled = false;
			}
		}
		else
		{
			if (IsAttachedToZipline)
			{
				if (ViableForMotorUsage)
				{
					movementInput = (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad ? inputMove.action.ReadValue<Vector2>().y : MyInput.GetAxis("Walk"));
					if (movementInput != 0f)
					{
						if (!usingMotor)
						{
							eventEmitter_ZiplineMotor.Stop();
							eventEmitter_ZiplineMotor.PlaySafe();
						}
						motorActiveDuration += Time.deltaTime * motorSoundRampUpMultipier;
						if (motorActiveDuration > 10f)
						{
							motorActiveDuration = 10f;
						}
						eventEmitter_ZiplineMotor.SetParameter("Velocity", motorActiveDuration);
						usingMotor = true;
					}
					else
					{
						motorActiveDuration = 0f;
						eventEmitter_ZiplineMotor.SetParameter("Velocity", motorActiveDuration);
						usingMotor = false;
						eventEmitter_ZiplineMotor.EventInstance.keyOff();
					}
				}
				UpdateZiplineMovement();
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Zipline/Detach"), inputJump.action.GetBindingDisplayString(), MyInput.Keybinds["Jump"].MainKey, 1, 1, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "Cancel") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputJump, "Jump"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					DetachFromCurrentZipline();
				}
				if (IsAttachedToZipline)
				{
					Collider[] array = Physics.OverlapCapsule(player.transform.TransformPoint(new Vector3(0f, 0f - AbortGroundOffset, 0f)), player.transform.position, 0.1f, AbortCollision.value);
					if (array.Length != 0)
					{
						bool flag = false;
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i].transform.GetComponentInParent<Block_Foundation>() != null)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							DetachFromCurrentZipline();
						}
					}
				}
			}
			else if (attachPlayerScript.carriedPlayer != null)
			{
				DetachFromCurrentZipline();
			}
			if (buildZiplineRenderer.enabled != MeshPathBase.IsCreatingMeshPath)
			{
				buildZiplineRenderer.enabled = MeshPathBase.IsCreatingMeshPath;
			}
			if (buildZiplineRenderer.enabled)
			{
				MeshPathBase startBase = MeshPathBase.StartBase;
				MeshPathBase currentlyAimedAtBase = MeshPathBase.CurrentlyAimedAtBase;
				Vector3 position = startBase.connectPoint.position;
				Vector3 vector = ((currentlyAimedAtBase == null) ? (player.transform.position + player.playerPivot.forward.XZOnly() * 1.5f) : currentlyAimedAtBase.connectPoint.position);
				float num = Vector3.Distance(position, vector);
				float maxLength = startBase.pathPrefab.maxLength;
				bool flag2 = num <= maxLength;
				if (!flag2)
				{
					vector = position + (vector - position).normalized * maxLength;
				}
				List<Vector3> list = MeshPath.CreatePointsForPath(position, vector, MeshPathBase.CurrentSlackSetting);
				buildZiplineRenderer.positionCount = list.Count;
				buildZiplineRenderer.SetPositions(list.ToArray());
				if (flag2)
				{
					MeshPathBase.MeshPathCreationIsObstructed = MeshPath.DoesConnectionExist(startBase, currentlyAimedAtBase);
					if (!MeshPathBase.MeshPathCreationIsObstructed)
					{
						for (int j = 0; j < list.Count - 2; j++)
						{
							Vector3 vector2 = list[j];
							Vector3 vector3 = list[j + 1];
							_ = vector3 - vector2;
							if (Physics.Linecast(vector2, vector3, out var hitInfo, LayerMasks.MASK_Block, QueryTriggerInteraction.Ignore))
							{
								MeshPathBase.MeshPathCreationIsObstructed = true;
								break;
							}
							if (j > 0 && Physics.Raycast(vector2, Vector3.down, out hitInfo, minDistanceFromGround, LayerMasks.MASK_Block, QueryTriggerInteraction.Ignore))
							{
								MeshPathBase.MeshPathCreationIsObstructed = true;
								break;
							}
						}
					}
				}
				else
				{
					MeshPathBase.MeshPathCreationIsObstructed = true;
				}
				if (mpbLinerenderer == null)
				{
					mpbLinerenderer = new MaterialPropertyBlock();
				}
				buildZiplineRenderer.GetPropertyBlock(mpbLinerenderer);
				mpbLinerenderer.SetColor("_Color1", MeshPathBase.MeshPathCreationIsObstructed ? Color.red : Color.green);
				mpbLinerenderer.SetColor("_Color2", MeshPathBase.MeshPathCreationIsObstructed ? Color.red : Color.green);
				buildZiplineRenderer.SetPropertyBlock(mpbLinerenderer);
			}
			if (MeshPathBase.IsCreatingMeshPath)
			{
				player.Animator.SetAnimation(PlayerAnimation.Index_1_Point);
				player.PlayerItemManager.HideItemInHand();
			}
			if (MeshPathBase.IsCreatingMeshPath)
			{
				if (MeshPathBase.StartBase.isPathSlackModifyable)
				{
					float num2;
					if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
					{
						num2 = inputSlackUp.action.ReadValue<float>() * ziplineSlackScrollSpeed * Time.deltaTime;
						num2 -= inputSlackDown.action.ReadValue<float>() * ziplineSlackScrollSpeed * Time.deltaTime;
					}
					else
					{
						num2 = Input.GetAxis("Mouse ScrollWheel") * ziplineSlackScrollSpeed * Time.deltaTime;
					}
					if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus))
					{
						num2 -= ziplineSlackKeyboardSpeed * Time.deltaTime;
					}
					else if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
					{
						num2 += ziplineSlackKeyboardSpeed * Time.deltaTime;
					}
					MeshPathBase.TargetSlackSetting = Mathf.Clamp(MeshPathBase.TargetSlackSetting + num2, ziplineSlackInterval.minValue, ziplineSlackInterval.maxValue);
					MeshPathBase.CurrentSlackSetting = Mathf.Lerp(MeshPathBase.CurrentSlackSetting, MeshPathBase.TargetSlackSetting, Time.deltaTime * 8f);
				}
				canvas.displayTextManager.ShowText(Helper.GetTerm(MeshPathBase.StartBase.termEndDraggingRope), inputContext.action.GetBindingDisplayString(), MyInput.Keybinds["RMB"].MainKey, 1, 0, clearAllTexts: false);
				if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputCancel, "Cancel") || SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputContext, "RMB"))
				{
					canvas.displayTextManager.HideDisplayTexts();
					MeshPathBase.AbortCreatingMeshPath();
					player.Inventory.hotbar.ReselectCurrentSlot();
					return;
				}
			}
		}
		HandleParticleEmission();
	}

	private void UpdateZiplineMovement()
	{
		Vector3 vector = ziplinePath.transform.InverseTransformDirection(ziplinePath.transform.up);
		Vector3 vector2 = moveInfo.GeLocalTargetPositionFromSpeed(ziplineSpeed);
		Vector3 vector3 = vector2 - localPlayerZiplinePosition;
		Quaternion rotation = Quaternion.LookRotation(moveInfo.lastPointPosition - moveInfo.firstPointPosition, ziplinePath.transform.up);
		ziplineToolPivot.rotation = rotation;
		ziplineElectricToolPivot.rotation = rotation;
		bool flag = Physics.Raycast(ziplinePath.transform.TransformPoint(localPlayerZiplinePosition + vector.normalized * zipplineYOffset), vector3, out var hitInfo, 0.4f, LayerMasks.MASK_ZiplineCollision, QueryTriggerInteraction.Ignore);
		if (!flag)
		{
			speedDirectionModifier = 1f;
			if (vector3.y < 0f)
			{
				if (moveInfo.backwardPosition == vector2)
				{
					speedDirectionModifier = -1f;
				}
			}
			else if (moveInfo.forwardPosition == vector2)
			{
				speedDirectionModifier = -1f;
			}
			if ((speedDirectionModifier > 0f && ziplineSpeed < 0f) || (speedDirectionModifier < 0f && ziplineSpeed > 0f))
			{
				speedDirectionModifier *= ziplineSpeedDamping;
			}
			float num = ziplineSpeedAcceleration.minValue;
			if (vector3.y < 0f)
			{
				num = ziplineSpeedAcceleration.GetValueFromNormalized(Mathf.Clamp01(vector3.y / -2f));
			}
			if (!usingMotor)
			{
				ziplineSpeed += num * speedDirectionModifier * Time.deltaTime;
				ziplineSpeed = Mathf.Clamp(ziplineSpeed, 0f - maxZiplineSpeed, maxZiplineSpeed);
			}
			else
			{
				int num2 = ((CalculateInitialSpeedBetweenPathAndPlayer(ziplinePath, player) > 0f) ? 1 : (-1));
				num2 *= (int)movementInput;
				ziplineSpeed += num * (float)num2 * electricZiplineSpeed * Time.deltaTime;
				ziplineSpeed = Mathf.Clamp(ziplineSpeed, 0f - maxElectricZiplineSpeed, maxElectricZiplineSpeed);
			}
			localPlayerZiplinePosition = Vector3.MoveTowards(localPlayerZiplinePosition, vector2, Mathf.Abs(ziplineSpeed) * Time.deltaTime);
			player.transform.position = ziplinePath.transform.TransformPoint(localPlayerZiplinePosition + vector.normalized * zipplineYOffset);
			moveInfo.lastMovedDirection = vector3;
		}
		float num3 = Helper.MapValue(0f, maxZiplineSpeed, Mathf.Abs(ziplineSpeed), 0f, 10f);
		eventEmitter_Zipline.eventEmitter.SetParameter("velocity", num3);
		SimpleMonoBehaviourSingleton<HapticManager>.Instance?.SetVolume(HapticEvent.Zipline, num3 / 10f);
		if (localPlayerZiplinePosition == vector2)
		{
			bool forward = moveInfo.forwardPosition == vector2;
			moveInfo.IncrementIndex(ziplinePath, forward);
		}
		if (flag)
		{
			if (hitInfo.normal.y > 0.9f)
			{
				if (soundManager != null)
				{
					soundManager.PlayOneShotNetworked(er_ZiplineImpact, transform.position);
				}
				DetachFromCurrentZipline();
				return;
			}
			BounceZiplineSpeed();
		}
		player.PersonController.HandleRotationOfPlayer();
	}

	private void UpdateZiplineRemote()
	{
		if (!(player != null))
		{
			return;
		}
		bool flag = player.Animator.IsInFullBodyAnimation(PlayerFullBodyAnimation.Ziplining_4);
		ziplineTool.SetActiveSafe(flag);
		ziplineToolEletric.SetActiveSafe(flag);
		if (flag)
		{
			Vector3 averageLocalDeltaVector = player.MovementDeltaTracker.GetAverageLocalDeltaVector();
			eventEmitter_Zipline.eventEmitter.SetParameter("velocity", Helper.MapValue(0f, 1f, Mathf.Abs(averageLocalDeltaVector.magnitude), 0f, 10f));
			if (ViableForMotorUsage)
			{
				eventEmitter_ZiplineMotor.PlaySafe();
				eventEmitter_ZiplineMotor.SetParameter("velocity", 5f);
			}
			if (ziplinePath != null)
			{
				Quaternion rotation = Quaternion.LookRotation(ziplinePath.GetDirectionFromStartPointToEndPoint(), ziplinePath.transform.up);
				ziplineToolPivot.rotation = rotation;
				ziplineElectricToolPivot.rotation = rotation;
			}
		}
	}

	private void HandleParticleEmission()
	{
		if (!(particles_FP == null) && !(particles_TP == null))
		{
			if (player != null && (ziplineTool.activeInHierarchy || ziplineToolEletric.activeInHierarchy))
			{
				float num = 0f;
				num = ((!player.IsLocalPlayer) ? Mathf.Clamp01(Mathf.Abs(player.MovementDeltaTracker.GetAverageLocalDeltaVector().magnitude)) : Mathf.Clamp01(Mathf.Abs(ziplineSpeed) / maxZiplineSpeed));
				float num2 = particleEmissionCurve.Evaluate(num);
				ParticleSystem.EmissionModule emission = particles_FP.emission;
				emission.rateOverTime = num2;
				ParticleSystem.EmissionModule emission2 = particles_TP.emission;
				emission2.rateOverTime = num2;
			}
			else if (player != null)
			{
				float num3 = 0f;
				ParticleSystem.EmissionModule emission3 = particles_FP.emission;
				emission3.rateOverTime = num3;
				ParticleSystem.EmissionModule emission4 = particles_TP.emission;
				emission4.rateOverTime = num3;
			}
		}
	}

	private void ThirdPersonStateChange(bool newThirdPersonState)
	{
		if (IsAttachedToZipline)
		{
			if (newThirdPersonState)
			{
				player.PlayerScript.SetMouseLookScripts(canLook: false);
				player.PlayerScript.mouseLookYCameraScript.ResetRotations();
				player.SetHatParentToCamera(value: false);
				particles_FP?.StopSafe();
				particles_TP?.PlaySafe();
			}
			else
			{
				player.PlayerScript.SetMouseLookScripts(canLook: true);
				player.PlayerScript.mouseLookYCameraScript.enabled = false;
				player.SetHatParentToCamera(value: true);
				particles_TP?.StopSafe();
				particles_FP?.PlaySafe();
			}
		}
	}

	private void OnFinishedCreatingMeshPath()
	{
		if (player != null && player.IsLocalPlayer)
		{
			player.Inventory.hotbar.ReselectCurrentSlot();
		}
	}

	public bool AttachToZipline(MeshPath_Zipline ziplinePath, int modelIndex = 1)
	{
		if (player.IsLocalPlayer)
		{
			if (ziplinePath == null)
			{
				return false;
			}
			if (IsAttachedToZipline)
			{
				return false;
			}
			OnZiplineAttach?.Invoke();
			int closestPointIndexFromWorldPosition = ziplinePath.GetClosestPointIndexFromWorldPosition(player.transform.position);
			canBounce = true;
			this.ziplinePath = ziplinePath;
			ziplineSpeed = CalculateInitialSpeedBetweenPathAndPlayer(ziplinePath, player);
			moveInfo.SetInfoFromPointIndex(ziplinePath, closestPointIndexFromWorldPosition, ziplineSpeed);
			moveInfo.lastMovedDirection = Vector3.zero;
			moveInfo.firstPointPosition = ziplinePath.GetLocalPointPositionFromIndex(0);
			moveInfo.lastPointPosition = ziplinePath.GetLocalPointPositionFromIndex(ziplinePath.points.Count - 1);
			attachPlayerScript.firstPersonParent = ((ziplinePath.transform.parent == lockedPivot) ? lockedPivot : null);
			attachPlayerScript.StartCarryingPlayer(player, manipulatePosition: false);
			localPlayerZiplinePosition = ziplinePath.GetLocalPointPositionFromIndex(closestPointIndexFromWorldPosition);
			player.transform.position = ziplinePath.transform.TransformPoint(localPlayerZiplinePosition + Vector3.up * zipplineYOffset);
			player.PersonController.externalVelocity = Vector3.zero;
			player.PersonController.ResetPlayerPivotToOriginal();
			if (!player.currentModel.thirdPersonSettings.ThirdPersonModel)
			{
				player.PlayerScript.mouseLookYScript.enabled = false;
				player.PlayerScript.mouseLookYCameraScript.enabled = true;
				player.PlayerScript.mouseLookYScript.CopyRotationToOther(player.PlayerScript.mouseLookYCameraScript);
				player.PlayerScript.mouseLookYScript.ResetRotations();
				player.SetHatParentToCamera(value: true);
			}
			player.PlayerItemManager.HideItemInHand();
			player.Animator.SetAnimation(PlayerFullBodyAnimation.Ziplining_4);
			if (eventEmitter_Zipline != null)
			{
				eventEmitter_Zipline.Play();
				SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Start(HapticEvent.Zipline);
			}
			(player.currentModel.thirdPersonSettings.ThirdPersonModel ? particles_TP : particles_FP)?.PlaySafe();
			usingMotor = false;
			Slot_Equip equipmentSlotFromEquipmentType = player.Inventory.GetEquipmentSlotFromEquipmentType(EquipSlotType.ZiplineTool);
			if (equipmentSlotFromEquipmentType != null)
			{
				if (equipmentSlotFromEquipmentType.GetItemBase().UniqueIndex == 549)
				{
					currentZiplineModelIndex = 1;
				}
				else
				{
					currentZiplineModelIndex = 0;
				}
			}
			ToggleModel(ViableForMotorUsage);
			return true;
		}
		currentZiplineModelIndex = modelIndex;
		this.ziplinePath = ziplinePath;
		if (IsAttachedToZipline)
		{
			player.transform.SetParentSafe((ziplinePath.transform.parent == lockedPivot) ? lockedPivot : null);
		}
		player.PlayerItemManager.HideItemInHand();
		player.Animator.SetAnimation(PlayerFullBodyAnimation.Ziplining_4);
		if (modelIndex == 1)
		{
			ToggleModel(viableMotor: true);
		}
		else
		{
			ToggleModel(viableMotor: false);
		}
		if (eventEmitter_Zipline != null)
		{
			eventEmitter_Zipline.Play();
		}
		particles_TP?.PlaySafe();
		return true;
	}

	public void DetachFromCurrentZiplineClient()
	{
		if (player != null && !player.IsLocalPlayer)
		{
			player.Animator.SetAnimation(PlayerFullBodyAnimation.None_0);
			DisableModels();
			ziplinePath = null;
			if (eventEmitter_Zipline != null)
			{
				eventEmitter_Zipline.Stop();
			}
			particles_TP?.Play();
		}
	}

	public void DetachFromCurrentZipline(bool keepMomentum = true)
	{
		if (!player.IsLocalPlayer)
		{
			return;
		}
		OnZiplineDeattach?.Invoke();
		if (ziplinePath != null)
		{
			if (keepMomentum)
			{
				Vector3 direction = moveInfo.lastMovedDirection.normalized * Mathf.Abs(ziplineSpeed);
				direction = ziplinePath.transform.TransformDirection(direction);
				direction.y = player.PersonController.jumpSpeed;
				player.PersonController.externalVelocity = direction;
			}
			player.Inventory.RemoveDurabillityFromEquipment(EquipSlotType.ZiplineTool);
		}
		CancelInvoke("ResetCanBounce");
		ziplinePath = null;
		attachPlayerScript.StopCarryingPlayer(manipulatePosition: true);
		attachPlayerScript.firstPersonParent = null;
		DisableModels();
		player.Inventory.hotbar.ReselectCurrentSlot();
		player.Animator.SetAnimation(PlayerFullBodyAnimation.None_0);
		player.PlayerScript.mouseLookYScript.enabled = true;
		player.PlayerScript.mouseLookYCameraScript.enabled = false;
		player.PlayerScript.mouseLookYCameraScript.CopyRotationToOther(player.PlayerScript.mouseLookYScript);
		player.PlayerScript.mouseLookYCameraScript.ResetRotations();
		player.PersonController.ResetFallDuration();
		player.SetHatParentToCamera(value: false);
		player.SendP2P(new Message_NetworkBehaviour(Messages.ZiplineDetach, player), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		if (eventEmitter_Zipline != null)
		{
			eventEmitter_Zipline.Stop();
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.Stop(HapticEvent.Zipline);
		}
		particles_FP?.StopSafe();
		particles_TP?.StopSafe();
	}

	public void ToggleModel(bool viableMotor)
	{
		if (viableMotor)
		{
			ziplineTool.SetActiveSafe(value: false);
			ziplineToolEletric.SetActiveSafe(value: true);
		}
		else
		{
			ziplineTool.SetActiveSafe(value: true);
			ziplineToolEletric.SetActiveSafe(value: false);
		}
	}

	private void BounceZiplineSpeed()
	{
		if (canBounce && Mathf.Abs(ziplineSpeed) > 0.5f)
		{
			canBounce = false;
			CancelInvoke("ResetCanBounce");
			Invoke("ResetCanBounce", 0.25f);
			ziplineSpeed *= -0.5f;
			ziplineSpeed = Mathf.Clamp(ziplineSpeed, -8f, 8f);
			if (soundManager != null)
			{
				soundManager.PlayOneShotNetworked(er_ZiplineImpact, transform.position);
			}
		}
	}

	private void ResetCanBounce()
	{
		canBounce = true;
	}

	private float CalculateInitialSpeedBetweenPathAndPlayer(MeshPath_Zipline path, Network_Player player)
	{
		Vector3 worldPointPositionFromIndex = path.GetWorldPointPositionFromIndex(0);
		Vector3 normalized = (path.GetWorldPointPositionFromIndex(path.points.Count - 1) - worldPointPositionFromIndex).normalized;
		Vector3 normalized2 = player.playerPivot.forward.normalized;
		normalized.y = (normalized2.y = 0f);
		float num = Vector3.Angle(normalized2, normalized);
		int num2 = 1;
		if (num > 90f)
		{
			num2 = -1;
		}
		float num3 = 0.1f;
		if (player.PersonController.moving)
		{
			num3 = ((!player.PersonController.sprinting) ? (player.PersonController.normalSpeed * 1.25f) : (player.PersonController.sprintSpeed * 1.25f));
		}
		return num3 * (float)num2;
	}

	private void DisableModels()
	{
		ziplineTool.SetActiveSafe(value: false);
		ziplineToolEletric.SetActiveSafe(value: false);
	}
}
