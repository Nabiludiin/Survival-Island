using EZCameraShake;
using UltimateWater;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlightCamera : MonoBehaviour
{
	[Tooltip("How fast does velocity go towards max velocity")]
	public float accelerationSpeed;

	[Tooltip("How fast does velocity go towards zero?")]
	public float deAccelerationSpeed;

	[Tooltip("Max speed when flying")]
	public float maxVelocity = 10f;

	[Tooltip("Makes camera focus on a certain transform")]
	public Transform cameraLockTarget;

	public float cameraOffset = 0.7f;

	public Vector3 recordPosition = Vector3.zero;

	public Vector3 recordEuler = Vector3.zero;

	public MouseLook mouseLookScript;

	private Network_Player player;

	private Vector3 velocity;

	private bool record;

	private void Awake()
	{
		player = GetComponentInParent<Network_Player>();
	}

	private void OnDestroy()
	{
		player = null;
	}

	private void Update()
	{
		if (Cheat.AllowCheatsForLocalPlayer() && !ChatTextFieldController.IsChatWindowSelected)
		{
			if (Input.GetKeyDown(KeyCode.PageDown))
			{
				SaveRecordPos();
			}
			if (Input.GetKeyDown(KeyCode.PageUp))
			{
				ToggleRecordState();
			}
		}
		mouseLookScript.enabled = cameraLockTarget == null && !record;
		if (record)
		{
			PlaceCameraAtRecordSpot();
		}
		else
		{
			bool flag = PlayerInput.GetPlayerByIndex(0).actions["Crouch"].ReadValue<float>() == 1f;
			Vector3 vector2;
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.Gamepad)
			{
				Vector2 vector = PlayerInput.GetPlayerByIndex(0).actions["Move"].ReadValue<Vector2>();
				vector2 = new Vector3(vector.x, 0f, vector.y);
			}
			else
			{
				vector2 = new Vector3(MyInput.GetAxis("Strafe"), 0f, MyInput.GetAxis("Walk"));
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(PlayerInput.GetPlayerByIndex(0).actions["Jump"], KeyCode.Space))
			{
				vector2.y = 1f;
			}
			else if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(PlayerInput.GetPlayerByIndex(0).actions["Sprint"], KeyCode.LeftControl))
			{
				vector2.y = -1f;
			}
			if (!ChatTextFieldController.IsChatWindowSelected)
			{
				velocity += vector2 * (accelerationSpeed * (flag ? 5f : 1f)) * Time.deltaTime;
				float num = (flag ? (maxVelocity * 5f) : maxVelocity);
				velocity.x = Mathf.Clamp(velocity.x, 0f - num, num);
				velocity.y = Mathf.Clamp(velocity.y, 0f - num, num);
				velocity.z = Mathf.Clamp(velocity.z, 0f - num, num);
				transform.position += transform.forward * velocity.z * Time.deltaTime;
				transform.position += transform.right * velocity.x * Time.deltaTime;
				transform.position += Vector3.up * velocity.y * Time.deltaTime;
				if (vector2.x == 0f)
				{
					velocity.x = Helper.Damp(velocity.x, 0f, 0.1f, Time.deltaTime * deAccelerationSpeed);
				}
				if (vector2.y == 0f)
				{
					velocity.y = Helper.Damp(velocity.y, 0f, 0.1f, Time.deltaTime * deAccelerationSpeed);
				}
				if (vector2.z == 0f)
				{
					velocity.z = Helper.Damp(velocity.z, 0f, 0.1f, Time.deltaTime * deAccelerationSpeed);
				}
			}
			RaycastHit hit;
			if (cameraLockTarget != null)
			{
				transform.LookAt(cameraLockTarget.position + Vector3.up * cameraOffset);
			}
			else if ((Cheat.AllowCheatsForLocalPlayer() & flag) && MyInput.GetButtonDown("RMB") && Helper.HitAtCursor(out hit, 1000f))
			{
				transform.position += (hit.point - transform.position).normalized * (hit.distance - 2f);
				velocity = Vector3.zero;
			}
		}
		player.PlayerScript.SetMouseLookScripts(canLook: false);
		player.playerPivot.localEulerAngles = Vector3.zero;
	}

	public void Enable(bool disablePlayerMovement)
	{
		bool flag = (mouseLookScript.enabled = true);
		enabled = flag;
		player.currentModel.thirdPersonSettings.ThirdPersonModel = true;
		player.cameraBobAnimator.enabled = false;
		transform.SetParent(null);
		if (disablePlayerMovement)
		{
			player.PersonController.IsMovementFree = false;
			player.PlayerScript.SetMouseLookScripts(canLook: false);
		}
		transform.GetComponent<CameraShaker>().enabled = false;
		transform.rotation = Quaternion.identity;
		player.PlayerScript.mouseLookXScript.CopyXRotationToOther(mouseLookScript);
		player.PlayerScript.mouseLookYScript.CopyYRotationToOther(mouseLookScript);
		transform.position += Vector3.up;
	}

	public void Disable(bool teleportPlayerToCamera)
	{
		bool flag = (mouseLookScript.enabled = false);
		enabled = flag;
		player.flightCamera.cameraLockTarget = null;
		player.currentModel.thirdPersonSettings.ThirdPersonModel = false;
		player.PersonController.IsMovementFree = true;
		player.PersonController.externalVelocity = Vector3.zero;
		player.PersonController.CameraSubmersionChanged(SubmersionState.None, setControllerType: true);
		player.PersonController.SwitchControllerType(ControllerType.Ground);
		player.PlayerScript.SetMouseLookScripts(canLook: true);
		if (teleportPlayerToCamera)
		{
			mouseLookScript.CopyXRotationToOther(player.PlayerScript.mouseLookXScript);
			mouseLookScript.CopyYRotationToOther(player.PlayerScript.mouseLookYScript);
			player.PersonController.transform.position = base.transform.position;
			Physics.SyncTransforms();
		}
		base.transform.SetParent(player.currentModel.cameraHolder);
		Transform transform = base.transform;
		Vector3 localPosition = (base.transform.localEulerAngles = Vector3.zero);
		transform.localPosition = localPosition;
		base.transform.GetComponent<CameraShaker>().enabled = true;
		player.cameraBobAnimator.enabled = true;
	}

	public void Toggle(bool disablePlayerMovement)
	{
		if (enabled)
		{
			Disable(Input.GetKey(KeyCode.LeftShift));
		}
		else
		{
			Enable(disablePlayerMovement);
		}
	}

	public void ToggleRecordState()
	{
		record = !record;
		if (record)
		{
			LoadRecordPos();
			ChatManager.SendDebugChatMessage("Camera recording enabled");
		}
		else
		{
			ChatManager.SendDebugChatMessage("Camera recording disabled");
		}
	}

	private void SaveRecordPos()
	{
		PlayerPrefs.SetFloat("CameraRecordPosX", transform.position.x);
		PlayerPrefs.SetFloat("CameraRecordPosY", transform.position.y);
		PlayerPrefs.SetFloat("CameraRecordPosZ", transform.position.z);
		PlayerPrefs.SetFloat("CameraRecordRotX", transform.eulerAngles.x);
		PlayerPrefs.SetFloat("CameraRecordRotY", transform.eulerAngles.y);
		PlayerPrefs.SetFloat("CameraRecordRotZ", transform.eulerAngles.z);
		ChatManager.SendDebugChatMessage("Camera record pos/rot saved");
	}

	private void LoadRecordPos()
	{
		transform.position = (recordPosition = new Vector3(PlayerPrefs.GetFloat("CameraRecordPosX"), PlayerPrefs.GetFloat("CameraRecordPosY"), PlayerPrefs.GetFloat("CameraRecordPosZ")));
		transform.eulerAngles = (recordEuler = new Vector3(PlayerPrefs.GetFloat("CameraRecordRotX"), PlayerPrefs.GetFloat("CameraRecordRotY"), PlayerPrefs.GetFloat("CameraRecordRotZ")));
	}

	private void PlaceCameraAtRecordSpot()
	{
		transform.position = recordPosition;
		transform.eulerAngles = recordEuler;
	}
}
