using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Door : MonoBehaviour_Network, IRaycastable
{
	public bool open;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Collider doorCollider;

	public EventReference er_Open;

	public EventReference er_Close;

	public EventReference er_CloseSqueal;

	private Raft_Network network;

	private CanvasHelper canvas;

	private bool placed;

	private void Start()
	{
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
	}

	public void OnBlockPlaced()
	{
		placed = true;
	}

	public void Open()
	{
		if (placed)
		{
			open = true;
			if (doorCollider != null)
			{
				doorCollider.enabled = false;
			}
			animator.SetBool("Open", open);
			RuntimeManager.PlayOneShotAttached(er_Open, gameObject);
		}
	}

	private void Close()
	{
		if (placed)
		{
			open = false;
			if (doorCollider != null)
			{
				doorCollider.enabled = true;
			}
			animator.SetBool("Open", open);
			RuntimeManager.PlayOneShotAttached(er_CloseSqueal, gameObject);
		}
	}

	public void Animation_DoorClosed()
	{
		if (placed)
		{
			RuntimeManager.PlayOneShotAttached(er_Close, gameObject);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Door(RGDType.Block_Door, GetComponent<Block>(), open);
	}

	public override void Serialize_Create()
	{
		if (open)
		{
			AddMessage(new Message_NetworkBehaviour(Messages.Door_Open, this));
		}
	}

	public override bool Deserialize(Message_NetworkBehaviour msg, Network_UserId remoteID)
	{
		switch (msg.Type)
		{
		case Messages.Door_Open:
			Open();
			return true;
		case Messages.Door_Close:
			Close();
			return true;
		default:
			return false;
		}
	}

	public void OnIsRayed()
	{
		if (canvas == null)
		{
			return;
		}
		if (Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			canvas.displayTextManager.ShowText(open ? Helper.GetTerm("Game/Close") : Helper.GetTerm("Game/Open"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				return;
			}
			if (Raft_Network.IsHost)
			{
				if (open)
				{
					Close();
					network.RPC(new Message_NetworkBehaviour(Messages.Door_Close, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
				else
				{
					Open();
					network.RPC(new Message_NetworkBehaviour(Messages.Door_Open, this), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
			}
			else
			{
				network.SendP2P(network.HostID, new Message_NetworkBehaviour(open ? Messages.Door_Close : Messages.Door_Open, this), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}

	public void OnRayEnter()
	{
	}

	public void OnRayExit()
	{
		if (!(canvas == null))
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}
}
