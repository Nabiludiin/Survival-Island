using System;
using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Anchor_Stationary : MonoBehaviour_Network, IRaycastable
{
	[SerializeField]
	private Anchor anchor;

	[SerializeField]
	private Rigidbody anchorBody;

	[SerializeField]
	private SpringJoint joint;

	[SerializeField]
	private float dropSpeed = 1f;

	[SerializeField]
	private float weighSpeed = 1f;

	[SerializeField]
	private Rope rope;

	public Animator anim;

	[SerializeField]
	private Transform ropeConnectionPoint;

	[SerializeField]
	private Transform usePoint;

	public bool remoteControlled;

	[SerializeField]
	private StudioEventEmitter eventEmitter_WeighLoop;

	public EventReference er_eventEmitter_SwitchUp;

	public EventReference er_eventEmitter_SwitchDown;

	private DisplayTextManager displayTextManager;

	private Raft raft;

	private bool canUse = true;

	private bool atBottom;

	private bool droppingAnchor;

	private bool weighingAnchor;

	private Raft_Network network;

	private bool hasBeenPlaced;

	public bool AtBottom
	{
		get
		{
			return atBottom;
		}
		set
		{
			atBottom = value;
		}
	}

	public bool CanUse
	{
		get
		{
			return canUse;
		}
		set
		{
			canUse = value;
		}
	}

	public bool DroppingAnchor
	{
		get
		{
			return droppingAnchor;
		}
		set
		{
			droppingAnchor = value;
		}
	}

	public float JointDistance
	{
		get
		{
			return joint.minDistance;
		}
		set
		{
			joint.minDistance = value;
		}
	}

	public Rope Rope => rope;

	public Transform RopeConnectionPoint => ropeConnectionPoint;

	private void Start()
	{
		displayTextManager = ComponentManager<DisplayTextManager>.Value;
		network = ComponentManager<Raft_Network>.Value;
		raft = ComponentManager<Raft>.Value;
	}

	public void OnBlockPlaced()
	{
		WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Combine(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
		hasBeenPlaced = true;
		rope.autoTileMaterial = true;
		anchorBody.transform.SetParent(transform.parent);
		if (remoteControlled)
		{
			if (raft == null)
			{
				raft = ComponentManager<Raft>.Value;
			}
			raft.remoteAnchors.Add(this);
		}
		if (EngineControls.AllEngineControls.Count > 0 && EngineControls.AnchorsAreDown)
		{
			DropAnchor();
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (hasBeenPlaced)
		{
			WorldShiftManager.OnWorldShift = (Action<Vector3>)Delegate.Remove(WorldShiftManager.OnWorldShift, new Action<Vector3>(OnWorldShift));
			eventEmitter_WeighLoop.Stop();
			if (network == null)
			{
				network = ComponentManager<Raft_Network>.Value;
			}
			if (raft == null)
			{
				raft = ComponentManager<Raft>.Value;
			}
			if ((atBottom || droppingAnchor) && raft != null)
			{
				raft.RemoveAnchor();
			}
			if (remoteControlled && raft.remoteAnchors.Contains(this))
			{
				raft.remoteAnchors.Remove(this);
			}
			if (anchorBody != null)
			{
				UnityEngine.Object.Destroy(anchorBody.gameObject);
			}
		}
	}

	private void Update()
	{
		if (hasBeenPlaced)
		{
			if (droppingAnchor)
			{
				DroppingUpdate();
			}
			if (weighingAnchor)
			{
				WeighUpdate();
			}
			if (Raft_Network.IsHost && remoteControlled && CanUse && EngineControls.AllEngineControls.Count > 0 && EngineControls.AnchorsAreDown != AtBottom)
			{
				network.RPC(new Message_NetworkBehaviour(Messages.StationaryAnchorUse, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				Use();
			}
			rope.SetPosition(0, anchorBody.transform);
			rope.SetPosition(1, ropeConnectionPoint);
		}
	}

	public Transform GetAnchorTransform()
	{
		return anchorBody.transform;
	}

	public Rigidbody GetAnchorBody()
	{
		return anchorBody;
	}

	public Anchor GetAnchor()
	{
		return anchor;
	}

	private void Use()
	{
		canUse = false;
		if (atBottom)
		{
			WeighAnchor();
		}
		else
		{
			DropAnchor();
		}
	}

	private void DroppingUpdate()
	{
		float num = Vector3.Distance(ropeConnectionPoint.position, anchorBody.position);
		float num2 = 2f;
		float num3 = Mathf.Clamp(num * 0.1f, 0f, num2);
		SetAnimSpeed(num3, num3 / num2);
		joint.minDistance += dropSpeed * Time.deltaTime;
		if (anchorBody.position.y < GameManager.OceanBottom)
		{
			OnAnchorHit();
		}
	}

	private void WeighUpdate()
	{
		if (joint.minDistance > 0f)
		{
			joint.minDistance -= weighSpeed * Time.deltaTime;
			if (joint.minDistance <= 0f)
			{
				weighingAnchor = false;
				canUse = true;
				atBottom = false;
				SetAnimSpeed(0f, 0f);
			}
		}
	}

	private void DropAnchor()
	{
		if (raft == null)
		{
			raft = ComponentManager<Raft>.Value;
		}
		if (Raft_Network.IsHost)
		{
			raft.AddAnchor(playSound: true, gameObject);
		}
		weighingAnchor = false;
		droppingAnchor = true;
		anchorBody.transform.SetParent(raft.transform);
		SetAnimSpeed(0f, 0f);
		if (remoteControlled)
		{
			anim.SetBool("Up", value: false);
			if (!er_eventEmitter_SwitchDown.IsNull)
			{
				RuntimeManager.PlayOneShot(er_eventEmitter_SwitchDown, usePoint.position);
			}
		}
	}

	private void WeighAnchor()
	{
		if (raft == null)
		{
			raft = ComponentManager<Raft>.Value;
		}
		raft.RemoveAnchor();
		weighingAnchor = true;
		atBottom = false;
		droppingAnchor = false;
		anchorBody.isKinematic = false;
		anchorBody.transform.SetParent(transform.parent);
		SetAnimSpeed(-0.5f, 0.25f);
		if (remoteControlled)
		{
			anim.SetBool("Up", value: true);
			if (!er_eventEmitter_SwitchUp.IsNull)
			{
				RuntimeManager.PlayOneShot(er_eventEmitter_SwitchUp, usePoint.position);
			}
		}
	}

	private void SetAnimSpeed(float animSpeed, float soundSpeed)
	{
		anim.SetFloat("Speed", animSpeed);
		if (soundSpeed == 0f && eventEmitter_WeighLoop.IsPlaying())
		{
			eventEmitter_WeighLoop.Stop();
		}
		else if (soundSpeed > 0f && !eventEmitter_WeighLoop.IsPlaying())
		{
			eventEmitter_WeighLoop.Play();
		}
		eventEmitter_WeighLoop.SetParameter("velocity", soundSpeed);
	}

	public void OnAnchorHit()
	{
		bool flag = AtBottom;
		if (!atBottom && droppingAnchor)
		{
			SetAnimSpeed(0f, 0f);
			droppingAnchor = false;
			anchorBody.isKinematic = true;
			anchorBody.transform.SetParent(null);
			canUse = true;
			atBottom = true;
		}
		if (flag != AtBottom && network != null && Raft_Network.IsHost)
		{
			network.RPC(new Message_StationaryAnchorCreate(Messages.StationaryAnchorCreate, this, anchorBody.transform, atBottom, joint.minDistance), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	private void OnWorldShift(Vector3 shift)
	{
		if (anchorBody != null && anchorBody.transform.parent == null)
		{
			anchorBody.transform.position -= shift;
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_AnchorStationary(RGDType.Block_AnchorStationary, GetComponent<Block>(), this);
	}

	public override void Serialize_Create()
	{
		if (atBottom)
		{
			AddMessage(new Message_StationaryAnchorCreate(Messages.StationaryAnchorCreate, this, anchorBody.transform, atBottom: true, joint.minDistance));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.StationaryAnchorCreate:
		{
			Message_StationaryAnchorCreate message_StationaryAnchorCreate = msg as Message_StationaryAnchorCreate;
			if (message_StationaryAnchorCreate.atBottom)
			{
				SetAnimSpeed(0f, 0f);
				atBottom = true;
				canUse = true;
				droppingAnchor = false;
				anchorBody.isKinematic = true;
				anchorBody.transform.SetParent(null);
				anchorBody.transform.position = message_StationaryAnchorCreate.Position;
				anchorBody.transform.eulerAngles = message_StationaryAnchorCreate.Rotation;
				rope.SetPosition(0, anchorBody.transform);
				rope.SetPosition(1, ropeConnectionPoint);
				joint.minDistance = message_StationaryAnchorCreate.jointDistance;
				if (raft == null)
				{
					raft = ComponentManager<Raft>.Value;
				}
				if (raft != null)
				{
					raft.AddAnchor(playSound: true, gameObject);
				}
				else
				{
					Debug.LogError("Null, Stationary anchor deserialize 'raft' reference is null");
				}
			}
			return false;
		}
		case Messages.StationaryAnchorUse:
			if (Raft_Network.IsHost && !canUse)
			{
				return false;
			}
			Use();
			return true;
		default:
			return false;
		}
	}

	public void OnIsRayed()
	{
		if (canUse && Helper.LocalPlayerIsWithinDistance(usePoint.position, Player.UseDistance))
		{
			if (remoteControlled && EngineControls.AllEngineControls.Count > 0)
			{
				displayTextManager.ShowText(Helper.GetTerm("Game/ControlledViaEngineControls"));
				return;
			}
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			string term = (atBottom ? "Game/WeighAnchor" : "Game/DropAnchor");
			displayTextManager.ShowText(Helper.GetTerm(term, applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				if (Raft_Network.IsHost)
				{
					network.RPC(new Message_NetworkBehaviour(Messages.StationaryAnchorUse, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					Use();
				}
				else
				{
					network.SendP2P(network.HostID, new Message_NetworkBehaviour(Messages.StationaryAnchorUse, this), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
		}
		else
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	public void OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		displayTextManager.HideDisplayTexts();
	}
}
