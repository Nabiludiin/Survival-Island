using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sail : MonoBehaviour_Network, SceneEventInterface, IRaycastable
{
	public static List<Sail> AllSails = new List<Sail>();

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Transform rotationTransform;

	[SerializeField]
	private GameObject paintCollider;

	[SerializeField]
	private float force;

	[SerializeField]
	private float remoteClientLerpSpeed = 2f;

	[SerializeField]
	private Interval_Float backwardAngle;

	[SerializeField]
	private Interval_Float forwardAngle;

	public bool open;

	public float forceMultiplier;

	public bool lookingBackward;

	public bool lookingForward;

	public EventReference er_setSail;

	[SerializeField]
	private StudioEventEmitter eventEmitter_sailFlap;

	private Raft raft;

	private CanvasHelper canvas;

	private Raft_Network network;

	private Network_Player localPlayer;

	private IEnumerator rotationCoroutine;

	private float targetLocalEulerY;

	private bool isRotating;

	private bool blockPlaced;

	public float LocalRotation => rotationTransform.localEulerAngles.y;

	private void Start()
	{
		raft = ComponentManager<Raft>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		network = ComponentManager<Raft_Network>.Value;
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	private void Update()
	{
		if (!Raft_Network.IsHost)
		{
			Vector3 localEulerAngles = rotationTransform.localEulerAngles;
			if (Mathf.Abs(localEulerAngles.y - targetLocalEulerY) < 1f)
			{
				localEulerAngles.y = targetLocalEulerY;
			}
			else
			{
				localEulerAngles.y = Mathf.LerpAngle(localEulerAngles.y, targetLocalEulerY, Time.deltaTime * remoteClientLerpSpeed);
			}
			rotationTransform.localEulerAngles = localEulerAngles;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (AllSails != null && AllSails.Contains(this))
		{
			AllSails.Remove(this);
		}
	}

	private void OnBlockPlaced()
	{
		if (!AllSails.Contains(this))
		{
			AllSails.Add(this);
		}
		blockPlaced = true;
	}

	public void OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			AllSails.Clear();
		}
	}

	public void OnIsRayed()
	{
		if (!blockPlaced || canvas == null || canvas.displayTextManager == null || localPlayer == null || localPlayer.RessurectComponent.IsCarrying || localPlayer.RessurectComponent.BeingCarried || localPlayer.BedComponent.Sleeping)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		InputAction action2 = PlayerInput.GetPlayerByIndex(0).actions["Rotate"];
		InputAction action3 = PlayerInput.GetPlayerByIndex(0).actions["Look"];
		if (Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance) && CanvasHelper.ActiveMenu == MenuType.None)
		{
			if (!open)
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			}
			else
			{
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/Close"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
				canvas.displayTextManager.ShowText(Helper.GetTerm("Game/RotateSmooth2"), action2.GetBindingDisplayString(), "RSH", MyInput.Keybinds["Rotate"].MainKey, 1, 1, clearAllTexts: false);
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				Message_NetworkBehaviour message = new Message_NetworkBehaviour(open ? Messages.Sail_Close : Messages.Sail_Open, this);
				if (Raft_Network.IsHost)
				{
					if (open)
					{
						Close();
					}
					else
					{
						Open();
					}
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action2, "Rotate"))
			{
				localPlayer.PlayerScript.SetMouseLookScripts(canLook: false);
				isRotating = true;
			}
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasReleasedThisFrame(action2, "Rotate"))
			{
				localPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
				isRotating = false;
			}
			bool flag = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.IsPressed(action2, "Rotate");
			if (flag)
			{
				float xAxis = SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.GetXAxis(action3, "Mouse X");
				if (Raft_Network.IsHost)
				{
					Rotate(xAxis);
				}
				else
				{
					Message_Sail_Rotate message2 = new Message_Sail_Rotate(Messages.Sail_Rotate, this, xAxis);
					network.SendP2P(network.HostID, message2, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			paintCollider.SetActiveSafe(!flag);
		}
		else
		{
			if (isRotating)
			{
				isRotating = false;
				localPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
			}
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (localPlayer != null)
		{
			localPlayer.PlayerScript.SetMouseLookScripts(canLook: true);
		}
		if (canvas != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public Vector3 GetNormalizedDirection()
	{
		float f = rotationTransform.eulerAngles.y * (MathF.PI / 180f);
		return new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f)).normalized;
	}

	public void SetRotation(float localEulerY)
	{
		Vector3 localEulerAngles = rotationTransform.localEulerAngles;
		localEulerAngles.y = localEulerY;
		rotationTransform.localEulerAngles = localEulerAngles;
		if (!Raft_Network.IsHost)
		{
			targetLocalEulerY = localEulerY;
		}
	}

	public void Open()
	{
		open = true;
		animator.SetBool("SailDown", open);
		eventEmitter_sailFlap.Play();
		RuntimeManager.PlayOneShot(er_setSail, transform.position);
	}

	private void Close()
	{
		open = false;
		animator.SetBool("SailDown", open);
		eventEmitter_sailFlap.Stop();
		RuntimeManager.PlayOneShot(er_setSail, transform.position);
	}

	private void CalculateForceMultiplier()
	{
		if (rotationTransform == null)
		{
			forceMultiplier = 0f;
			return;
		}
		float y = rotationTransform.eulerAngles.y;
		lookingBackward = y < backwardAngle.maxValue && y > backwardAngle.minValue;
		lookingForward = y > forwardAngle.maxValue || y < forwardAngle.minValue;
		if (lookingForward)
		{
			forceMultiplier = 0.2f;
		}
		else if (lookingBackward)
		{
			forceMultiplier = 0.75f;
		}
		else
		{
			forceMultiplier = 1f;
		}
	}

	private void Rotate(float degree)
	{
		rotationTransform.Rotate(rotationTransform.up, degree, Space.World);
		if (Raft_Network.IsHost)
		{
			Message_Sail_Rotate message = new Message_Sail_Rotate(Messages.Sail_Rotate, this, rotationTransform.localEulerAngles.y);
			network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Sail(RGDType.Block_Sail, this);
	}

	public override void Serialize_Create()
	{
		AddMessage(new Message_Sail_Create(Messages.Sail_Create, this, this));
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		switch (msg.Type)
		{
		case Messages.Sail_Create:
			(msg as Message_Sail_Create).rgdSail.RestoreSail(this);
			return false;
		case Messages.Sail_Open:
			Open();
			return true;
		case Messages.Sail_Close:
			Close();
			return true;
		case Messages.Sail_Rotate:
		{
			Message_Sail_Rotate message_Sail_Rotate = msg as Message_Sail_Rotate;
			if (Raft_Network.IsHost)
			{
				Rotate(message_Sail_Rotate.rotationDegree);
			}
			else
			{
				targetLocalEulerY = message_Sail_Rotate.rotationDegree;
			}
			return false;
		}
		default:
			return false;
		}
	}
}
