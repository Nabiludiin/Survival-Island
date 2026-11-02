using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationComponent : MonoBehaviour, IRaycastable
{
	public int displayTextIndex;

	public Transform rotationTransform;

	public bool isRotatable = true;

	public bool invertRotation;

	public bool invertRotationBasedOnCamera;

	public Axis rotationAxis;

	public MonoBehaviour_ID_Network targetTrackedNetworkID;

	public bool clampRotation;

	public float minRotation;

	public float maxRotation;

	[SerializeField]
	private bool forceSetPosition = true;

	protected Raft_Network network;

	protected Network_Player localPlayer;

	private Vector3 startPosition;

	private CanvasHelper canvas;

	[SerializeField]
	private float currentRotation;

	public float CurrentRotation => currentRotation;

	private void Awake()
	{
		currentRotation = GetRotationFromAxis();
		startPosition = rotationTransform.localPosition;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	public void OnIsRayed()
	{
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
		InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Look"];
		if (!isRotatable)
		{
			return;
		}
		if (canvas == null)
		{
			canvas = ComponentManager<CanvasHelper>.Value;
		}
		if (canvas != null)
		{
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/RotateSmooth2"), action.GetBindingDisplayString(), "RSH", MyInput.Keybinds["Rotate"].MainKey, displayTextIndex, 0, clearAllTexts: false);
		}
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer != null)
		{
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Rotate"))
			{
				localPlayer.PlayerScript.SetMouseLookScripts(canLook: false);
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Rotate"))
			{
				localPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
			}
		}
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "Rotate"))
		{
			return;
		}
		float num = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetXAxis(action2, "Mouse X");
		if (invertRotationBasedOnCamera && rotationAxis != Axis.Y && rotationAxis != Axis.NY)
		{
			Vector3 vector = rotationTransform.InverseTransformPoint(localPlayer.CameraTransform.position);
			if ((rotationAxis == Axis.X || rotationAxis == Axis.NX) && vector.x < 0f)
			{
				num *= -1f;
			}
			else if ((rotationAxis == Axis.Z || rotationAxis == Axis.NZ) && vector.z < 0f)
			{
				num *= -1f;
			}
		}
		else if (invertRotation)
		{
			num = 0f - num;
		}
		RotateNetworked(num);
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (localPlayer != null && localPlayer.PlayerScript != null)
		{
			localPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
		}
		if (canvas != null && canvas.displayTextManager != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public float GetRotationFromAxis(Axis axis)
	{
		switch (axis)
		{
		case Axis.X:
		case Axis.NX:
			return rotationTransform.localEulerAngles.x;
		case Axis.Y:
		case Axis.NY:
			return rotationTransform.localEulerAngles.y;
		case Axis.Z:
		case Axis.NZ:
			return rotationTransform.localEulerAngles.z;
		default:
			return 0f;
		}
	}

	public void SetRotation(float rotation)
	{
		Vector3 localEulerAngles = rotationTransform.localEulerAngles;
		switch (rotationAxis)
		{
		case Axis.X:
		case Axis.NX:
			localEulerAngles.x = rotation;
			break;
		case Axis.Y:
		case Axis.NY:
			localEulerAngles.y = rotation;
			break;
		case Axis.Z:
		case Axis.NZ:
			localEulerAngles.z = rotation;
			break;
		}
		rotationTransform.localEulerAngles = localEulerAngles;
		currentRotation = rotation;
		if (forceSetPosition)
		{
			ForceResetPosition();
		}
	}

	public void SetRotation(Vector3 euler)
	{
		rotationTransform.localEulerAngles = euler;
		currentRotation = GetRotationFromAxis(rotationAxis);
		if (forceSetPosition)
		{
			ForceResetPosition();
		}
	}

	private void ForceResetPosition()
	{
		rotationTransform.localPosition = startPosition;
	}

	protected void RotateNetworked(float degree)
	{
		if (degree != 0f)
		{
			if (Raft_Network.IsHost)
			{
				Rotate(degree);
				Message_RotationComponentSetRotation message = new Message_RotationComponentSetRotation(Messages.RotationComponentSetRotation, network.NetworkIDManager, targetTrackedNetworkID.ObjectIndex, rotationTransform.localEulerAngles);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
			else
			{
				Message_RotationComponentRotate message2 = new Message_RotationComponentRotate(Messages.RotationComponentRotate, network.NetworkIDManager, targetTrackedNetworkID.ObjectIndex, degree);
				network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	protected void Rotate(float degree)
	{
		_ = currentRotation;
		currentRotation += degree;
		if (clampRotation)
		{
			currentRotation = Mathf.Clamp(currentRotation, minRotation, maxRotation);
		}
		switch (rotationAxis)
		{
		case Axis.X:
			rotationTransform.localEulerAngles = new Vector3(currentRotation, 0f, 0f);
			break;
		case Axis.Y:
			rotationTransform.localEulerAngles = new Vector3(0f, currentRotation, 0f);
			break;
		case Axis.Z:
			rotationTransform.localEulerAngles = new Vector3(0f, 0f, currentRotation);
			break;
		}
		if (forceSetPosition)
		{
			ForceResetPosition();
		}
	}

	private float GetRotationFromAxis()
	{
		return rotationAxis switch
		{
			Axis.X => rotationTransform.localEulerAngles.x, 
			Axis.Y => rotationTransform.localEulerAngles.y, 
			Axis.Z => rotationTransform.localEulerAngles.z, 
			_ => 0f, 
		};
	}

	public static float Clamp0360(float eulerAngles)
	{
		float num = eulerAngles - (float)Mathf.CeilToInt(eulerAngles / 360f) * 360f;
		if (num < 0f)
		{
			num += 360f;
		}
		return num;
	}

	public virtual bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.RotationComponentRotate:
		{
			Message_RotationComponentRotate message_RotationComponentRotate = msg as Message_RotationComponentRotate;
			if (message_RotationComponentRotate != null)
			{
				if (Raft_Network.IsHost)
				{
					Rotate(message_RotationComponentRotate.Rotation);
					Message_RotationComponentSetRotation message = new Message_RotationComponentSetRotation(Messages.RotationComponentSetRotation, network.NetworkIDManager, targetTrackedNetworkID.ObjectIndex, rotationTransform.localEulerAngles);
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					return false;
				}
				Debug.Log("client should not recieve rotation message");
			}
			else
			{
				Debug.Log("Rotation message became null");
			}
			break;
		}
		case Messages.RotationComponentSetRotation:
			if (!Raft_Network.IsHost)
			{
				Message_RotationComponentSetRotation message_RotationComponentSetRotation = msg as Message_RotationComponentSetRotation;
				if (message_RotationComponentSetRotation != null)
				{
					SetRotation(message_RotationComponentSetRotation.Euler);
				}
			}
			else
			{
				Debug.Log("Host should not recieve set rotation message");
			}
			break;
		}
		return false;
	}

	public RGD_RotationComponent GetSaveData()
	{
		return new RGD_RotationComponent(this);
	}
}
