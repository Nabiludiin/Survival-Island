using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Binoculars : UsableTool
{
	[SerializeField]
	private float activeFOV = 30f;

	[SerializeField]
	private float minFOV;

	[SerializeField]
	private float zoomAmount;

	[SerializeField]
	private float zoomSpeed;

	[SerializeField]
	private MeshRenderer binocularModel;

	[SerializeField]
	private float fadeSpeed = 1f;

	[SerializeField]
	private float sensitivityMultiplier = 0.5f;

	[SerializeField]
	private InputActionReference inputZoomIn;

	[SerializeField]
	private InputActionReference inputZoomOut;

	[SerializeField]
	private InputActionReference inputUse;

	private CanvasHelper canvas;

	private Settings settings;

	private float defaultFOV = 50f;

	private float targetFOV = 50f;

	private float maxFOV;

	private bool active;

	private float defaultMouseSensitivity;

	private float defaultLookSensitivity;

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
		settings = ComponentManager<Settings>.Value;
		maxFOV = Mathf.Clamp(defaultFOV - 10f, minFOV, defaultFOV);
		defaultMouseSensitivity = settings.controls.MouseSensitivity;
		defaultLookSensitivity = settings.controls.LookSensitivity;
	}

	protected override bool Update()
	{
		if (!playerNetwork.IsLocalPlayer || CanvasHelper.ActiveMenu != MenuType.None)
		{
			return false;
		}
		if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(inputUse, "LMB"))
		{
			SetState(state: true);
		}
		else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(inputUse, "LMB"))
		{
			SetState(state: false);
		}
		float value = Helper.NormalizeValue(activeFOV, minFOV, maxFOV);
		if (active)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				if (inputZoomIn.action.WasPressedThisFrame())
				{
					targetFOV -= 9f;
				}
				else if (inputZoomOut.action.WasPressedThisFrame())
				{
					targetFOV += 9f;
				}
			}
			else
			{
				float axis = Input.GetAxis("Mouse ScrollWheel");
				if (axis < 0f)
				{
					targetFOV += zoomAmount * Time.deltaTime;
				}
				else if (axis > 0f)
				{
					targetFOV -= zoomAmount * Time.deltaTime;
				}
			}
			targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
			activeFOV = Mathf.Lerp(activeFOV, targetFOV, Time.deltaTime * zoomSpeed);
			if (canvas.binocularImage.activeSelf)
			{
				settings.controls.LookSensitivity = defaultLookSensitivity * Helper.MapValue(0f, 1f, value, sensitivityMultiplier, 1f);
				settings.controls.MouseSensitivity = defaultMouseSensitivity * Helper.MapValue(0f, 1f, value, sensitivityMultiplier, 1f);
				playerNetwork.Camera.fieldOfView = activeFOV;
			}
		}
		return true;
	}

	protected override void OnToolDeSelect()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			base.OnToolDeSelect();
			StopAllCoroutines();
			ResetBinoculars();
			canvas.genericBlackFade.SetAlpha(0f);
		}
	}

	private void ResetBinoculars()
	{
		PlayerItemManager.IsBusy = false;
		if (playerAnimator != null)
		{
			playerAnimator.anim.SetBool("ItemHit", value: false);
		}
		SetRenderState(state: true);
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		if (canvas != null && canvas.binocularImage != null)
		{
			canvas.binocularImage.SetActive(value: false);
		}
		if (playerNetwork != null && playerNetwork.Camera != null)
		{
			playerNetwork.Camera.fieldOfView = defaultFOV;
		}
		if (settings != null)
		{
			settings.controls.LookSensitivity = defaultLookSensitivity;
			settings.controls.MouseSensitivity = defaultMouseSensitivity;
		}
	}

	private void SetState(bool state)
	{
		if (active != state)
		{
			active = state;
			PlayerItemManager.IsBusy = active;
			playerAnimator.anim.SetBool("ItemHit", active);
			StopAllCoroutines();
			if (active)
			{
				StartCoroutine(Activate());
			}
			else
			{
				StartCoroutine(Deactivate());
			}
		}
	}

	private void SetRenderState(bool state)
	{
		binocularModel.enabled = state;
		playerNetwork.currentModel.SetArmMeshState(state);
	}

	private IEnumerator Activate()
	{
		yield return StartCoroutine(canvas.genericBlackFade.FadeToAlpha(1f, fadeSpeed));
		SetRenderState(state: false);
		canvas.binocularImage.SetActive(value: true);
		playerNetwork.Camera.fieldOfView = (targetFOV = activeFOV);
		settings.controls.LookSensitivity = defaultLookSensitivity * sensitivityMultiplier;
		settings.controls.MouseSensitivity = defaultMouseSensitivity * sensitivityMultiplier;
		yield return StartCoroutine(canvas.genericBlackFade.FadeToAlpha(0f, fadeSpeed));
	}

	private IEnumerator Deactivate()
	{
		yield return StartCoroutine(canvas.genericBlackFade.FadeToAlpha(1f, fadeSpeed * 2f));
		ResetBinoculars();
		yield return StartCoroutine(canvas.genericBlackFade.FadeToAlpha(0f, fadeSpeed * 2f));
	}
}
