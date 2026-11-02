using System;
using EZCameraShake;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
	public Action<bool> OnThirdpersonModelChange;

	public Action<bool> OnThirdpersonStateChange;

	public float lerpCameraMovementSpeed = 1f;

	public float lerpCameraMovementBackSpeed = 1f;

	public float lerpCameraRotationSpeed = 1f;

	public float pivotYOffset = 0.5f;

	public float crouchHeightOffset = 0.5f;

	public float zoomSpeed = 1f;

	public float zoomLerpSpeed = 3f;

	public Interval_Float rotationInterval;

	public Interval_Float zoomInterval;

	public Vector3 localCameraOffset;

	public float fallbackSensitivity = 1f;

	private AnimatorStorage storage;

	private Network_Player playerNetwork;

	private Settings settings;

	private CharacterModelModifications currentModel;

	private CanvasHelper canvas;

	private Transform cameraTransform;

	private Transform cameraRotateTransform;

	private Vector3 localCameraRotation;

	private bool thirdPersonState;

	private bool thirdPersonModel;

	private float zoom;

	private float targetZoom;

	private InputAction actionMove;

	private InputAction actionRotate;

	private InputAction actionLook;

	public bool ThirdPersonState
	{
		get
		{
			return thirdPersonState;
		}
		set
		{
			SetThirdPersonState(value, force: false);
		}
	}

	public bool ThirdPersonModel
	{
		get
		{
			return thirdPersonModel;
		}
		set
		{
			SetThirdPersonModel(value, force: false);
		}
	}

	public float Sensitivity
	{
		get
		{
			if (settings != null)
			{
				if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					return settings.controls.MouseSensitivity;
				}
				return settings.controls.LookSensitivity;
			}
			return fallbackSensitivity;
		}
	}

	public bool InvertMouseY
	{
		get
		{
			if (settings != null)
			{
				if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
				{
					return settings.controls.InvertMouseY;
				}
				return settings.controls.InvertLookY;
			}
			return false;
		}
	}

	private void Start()
	{
		playerNetwork = GetComponentInParent<Network_Player>();
		settings = ComponentManager<Settings>.Value;
		if (playerNetwork.IsLocalPlayer)
		{
			cameraTransform = playerNetwork.Camera.transform;
			currentModel = playerNetwork.currentModel;
			canvas = ComponentManager<CanvasHelper>.Value;
			cameraRotateTransform = new GameObject("ThirdPersonCameraRotator").transform;
			cameraRotateTransform.SetParentSafe(transform);
		}
		actionMove = PlayerInput.GetPlayerByIndex(0).actions["Move"];
		actionRotate = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
		actionLook = PlayerInput.GetPlayerByIndex(0).actions["Look"];
	}

	private void OnDestroy()
	{
		playerNetwork = null;
	}

	private void Update()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			HandleThirdPerson();
		}
	}

	public void ForceThirdPersonState(bool thirdPersonState)
	{
		SetThirdPersonState(thirdPersonState, force: true);
	}

	private bool CanGoIntoThirdperson()
	{
		if (playerNetwork == null || !playerNetwork.IsLocalPlayer)
		{
			return false;
		}
		if (CanvasHelper.ActiveMenu != MenuType.None)
		{
			return false;
		}
		if (playerNetwork.PlayerScript.IsDead)
		{
			return false;
		}
		if (playerNetwork.RessurectComponent.IsCarrying || playerNetwork.RessurectComponent.BeingCarried)
		{
			return false;
		}
		if (playerNetwork.NoteBookUI != null && playerNetwork.NoteBookUI.isDisplayed)
		{
			return false;
		}
		return true;
	}

	private void SetThirdPersonModel(bool thirdPersonModel, bool force)
	{
		if ((force || CanGoIntoThirdperson()) && this.thirdPersonModel != thirdPersonModel)
		{
			this.thirdPersonModel = thirdPersonModel;
			playerNetwork.Animator.anim.SaveAnimatorParameters(ref storage);
			if (thirdPersonModel)
			{
				playerNetwork.Animator.anim.runtimeAnimatorController = currentModel.thirdPersonController;
			}
			else
			{
				playerNetwork.Animator.anim.runtimeAnimatorController = currentModel.firstPersonController;
			}
			currentModel.SetFullBodyMeshState(thirdPersonModel);
			currentModel.SetArmMeshState(!thirdPersonModel);
			playerNetwork.Animator.anim.Rebind();
			playerNetwork.cameraBobAnimator.enabled = !thirdPersonModel;
			cameraTransform.GetComponent<CameraShaker>().enabled = !thirdPersonModel;
			playerNetwork.playerPivot.localEulerAngles = Vector3.zero;
			playerNetwork.Animator.anim.LoadAnimatorParameters(storage);
			if (OnThirdpersonModelChange != null)
			{
				OnThirdpersonModelChange(thirdPersonModel);
			}
		}
	}

	private void SetThirdPersonState(bool thirdPersonState, bool force)
	{
		if (force || CanGoIntoThirdperson())
		{
			this.thirdPersonState = thirdPersonState;
			SetThirdPersonModel(thirdPersonState, force);
			if (thirdPersonState)
			{
				cameraTransform.SetParent(playerNetwork.transform);
				cameraRotateTransform.SetParent(null);
				targetZoom = (zoom = 1f);
			}
			else
			{
				cameraTransform.SetParent(currentModel.cameraHolder);
				Transform transform = cameraTransform;
				Vector3 localPosition = (playerNetwork.Camera.transform.localEulerAngles = Vector3.zero);
				transform.localPosition = localPosition;
				localCameraRotation = Vector3.zero;
				cameraRotateTransform.SetParent(currentModel.cameraHolder);
			}
			if (!playerNetwork.BedComponent.Sleeping)
			{
				playerNetwork.PlayerScript.SetMouseLookScripts(!thirdPersonState);
			}
			Player.UseDistance = (thirdPersonState ? Player.UseDistanceThirdperson : Player.UseDistanceDefault);
			OnThirdpersonStateChange?.Invoke(thirdPersonState);
		}
	}

	private void HandleThirdPerson()
	{
		if (!ThirdPersonState)
		{
			return;
		}
		Vector3 vector2;
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
		{
			Vector2 vector = actionMove.ReadValue<Vector2>();
			vector2 = new Vector3(vector.x, 0f, vector.y);
		}
		else
		{
			vector2 = new Vector3(MyInput.GetAxis("Strafe"), 0f, MyInput.GetAxis("Walk"));
		}
		bool flag = vector2 != Vector3.zero;
		if (CanvasHelper.ActiveMenu == MenuType.None && !SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(actionRotate, "Rotate"))
		{
			Vector2 vector4;
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				Vector2 vector3 = actionLook.ReadValue<Vector2>();
				vector4 = new Vector2(vector3.x, 0f - vector3.y);
			}
			else
			{
				vector4 = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			}
			float x = (InvertMouseY ? vector4.y : (0f - vector4.y));
			localCameraRotation += new Vector3(x, vector4.x, 0f) * Sensitivity;
			localCameraRotation.x = Mathf.Clamp(localCameraRotation.x, rotationInterval.minValue, rotationInterval.maxValue);
		}
		if (flag && !playerNetwork.BedComponent.Sleeping && !playerNetwork.PlayerNetworkManager.IsAttached)
		{
			playerNetwork.playerPivot.localEulerAngles = new Vector3(0f, localCameraRotation.y, 0f);
		}
		if (Input.GetKey(KeyCode.LeftAlt))
		{
			targetZoom = Mathf.Clamp(targetZoom + Input.mouseScrollDelta.y * -1f * zoomSpeed, zoomInterval.minValue, zoomInterval.maxValue);
		}
		zoom = Mathf.Lerp(zoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
		Vector3 vector5 = playerNetwork.transform.position + playerNetwork.transform.up * pivotYOffset;
		if (playerNetwork.PersonController.crouching)
		{
			vector5 -= playerNetwork.transform.up * crouchHeightOffset;
		}
		Vector3 position = vector5 + playerNetwork.transform.right * localCameraOffset.x + playerNetwork.transform.up * localCameraOffset.y + playerNetwork.transform.forward * (localCameraOffset.z * zoom);
		cameraRotateTransform.position = position;
		cameraRotateTransform.RotateAround(vector5, Vector3.up, localCameraRotation.y);
		cameraRotateTransform.RotateAround(vector5, cameraTransform.right, localCameraRotation.x);
		cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraRotateTransform.position, Time.deltaTime * lerpCameraMovementSpeed);
		if (flag)
		{
			cameraTransform.LookAt(vector5);
		}
		else
		{
			Quaternion b = Quaternion.LookRotation(vector5 - cameraTransform.position);
			cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, b, Time.deltaTime * lerpCameraRotationSpeed);
		}
		playerNetwork.PlayerScript.SetMouseLookScripts(canLook: false);
	}

	private void SetSpecificAnimationParameters()
	{
		if (playerNetwork.PlayerNetworkManager.IsAttached)
		{
			playerNetwork.Animator.SetAnimation(ThirdPersonModel ? PlayerFullBodyAnimation.Sitting_1 : PlayerFullBodyAnimation.None_0);
		}
	}
}
