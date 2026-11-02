using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class SteeringWheel : MonoBehaviour_ID_Network, IRaycastable
{
	public SteeringWheelRudderAttachment rudderAttachment;

	public BoxCollider rudderPipeCollider;

	public Transform smallerWheelCog;

	[HideInInspector]
	public SteeringWheelRudderRotator rudderRotator;

	public float targetLocalEulerZ;

	[SerializeField]
	private Transform rotationTransform;

	[SerializeField]
	private float deadZoneAngle = 10f;

	[SerializeField]
	private float maxRotationAngle = 80f;

	[SerializeField]
	private float steeringWheelLerpSpeed = 2f;

	private Raft_Network network;

	private DisplayTextManager displayText;

	private Network_Player localPlayer;

	private bool hasBeenPlaced;

	private bool isDisplayingText;

	public float SteeringRotation => Helper.MapValue(0f - maxRotationAngle, maxRotationAngle, targetLocalEulerZ, -1f, 1f);

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (localPlayer != null && localPlayer.PlayerScript != null)
		{
			localPlayer.PlayerScript.SetLockMouseLook(isLocked: false);
		}
		NetworkIDManager.RemoveNetworkID(this);
		RaftVelocityManager.RemoveSteeringWheel(this);
	}

	public void OnValidate()
	{
		if (rudderAttachment == null)
		{
			rudderAttachment = GetComponent<SteeringWheelRudderAttachment>();
		}
	}

	private void Update()
	{
		Vector3 localEulerAngles = rotationTransform.localEulerAngles;
		float num = ((localEulerAngles.z > 180f) ? (0f - (360f - localEulerAngles.z)) : localEulerAngles.z);
		float num2 = Mathf.Abs(targetLocalEulerZ - num);
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "Rotate") && num2 < 0.1f && num > 0f - deadZoneAngle && num < deadZoneAngle)
		{
			targetLocalEulerZ = 0f;
		}
		localEulerAngles.z = Mathf.LerpAngle(localEulerAngles.z, targetLocalEulerZ, Time.deltaTime * steeringWheelLerpSpeed);
		rotationTransform.localEulerAngles = localEulerAngles;
		smallerWheelCog.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, 0f - localEulerAngles.z);
		if (rudderRotator != null)
		{
			rudderRotator.RotateRudder(localEulerAngles);
		}
	}

	public void OnIsRayed()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
		isDisplayingText = true;
		displayText.ShowText(Helper.GetTerm("Game/RotateSmooth2"), action.GetBindingDisplayString(), "RSH", MyInput.Keybinds["Rotate"].MainKey);
		bool flag = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action, "Rotate");
		bool flag2 = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Rotate");
		bool flag3 = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action, "Rotate");
		if (flag2)
		{
			localPlayer.PlayerScript.SetLockMouseLook(isLocked: true);
		}
		if (flag3)
		{
			localPlayer.PlayerScript.mouseLookXScript.SetTargetRotXToCurrentRotation();
			localPlayer.PlayerScript.SetLockMouseLook(isLocked: false);
		}
		if (flag)
		{
			InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Look"];
			float xAxis = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetXAxis(action2, "Mouse X");
			Message_SteeringWheel_Rotate message = new Message_SteeringWheel_Rotate(Messages.SteeringWheelRotate, network.NetworkIDManager, ObjectIndex, xAxis);
			if (Raft_Network.IsHost)
			{
				Rotate(xAxis);
			}
			else
			{
				network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (isDisplayingText)
		{
			displayText.HideDisplayTexts();
			isDisplayingText = false;
		}
		localPlayer.PlayerScript.SetLockMouseLook(isLocked: false);
	}

	private void OnBlockPlaced()
	{
		hasBeenPlaced = true;
		NetworkIDManager.AddNetworkID(this);
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
		displayText = ComponentManager<DisplayTextManager>.Value;
		RaftVelocityManager.AddSteeringWheel(this);
		rudderAttachment.BuildRudderPipesAttachment(this);
	}

	private void Rotate(float degree)
	{
		targetLocalEulerZ = Mathf.Clamp(targetLocalEulerZ + degree, 0f - maxRotationAngle, maxRotationAngle);
		if (Raft_Network.IsHost)
		{
			Message_SteeringWheel_Rotate message = new Message_SteeringWheel_Rotate(Messages.SteeringWheelRotate, network.NetworkIDManager, ObjectIndex, targetLocalEulerZ);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_SteeringWheel(RGDType.Block_SteeringWheel, this);
	}

	public void RestoreWheel(RGD_SteeringWheel rgdWheel)
	{
		targetLocalEulerZ = Mathf.Clamp(rgdWheel.currentRotation, 0f - maxRotationAngle, maxRotationAngle);
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (msg.Type == Messages.SteeringWheelRotate)
		{
			Message_SteeringWheel_Rotate message_SteeringWheel_Rotate = msg as Message_SteeringWheel_Rotate;
			if (Raft_Network.IsHost)
			{
				Rotate(message_SteeringWheel_Rotate.rotationDegree);
			}
			else
			{
				targetLocalEulerZ = message_SteeringWheel_Rotate.rotationDegree;
			}
			return false;
		}
		return base.Deserialize(msg, remoteID);
	}
}
