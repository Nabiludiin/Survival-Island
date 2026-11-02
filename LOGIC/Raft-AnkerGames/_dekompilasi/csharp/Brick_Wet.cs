using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RaycastInteractable))]
public class Brick_Wet : Block, IRaycastable
{
	public float dryTimeSeconds = 20f;

	[SerializeField]
	private MeshFilter filter;

	[SerializeField]
	private Mesh dryMesh;

	public Cost pickup;

	private float timer;

	private PlayerNetworkManager playerNetworkManager;

	private Raft_Network network;

	private CanvasHelper canvas;

	public bool Complete => timer >= dryTimeSeconds;

	public float Timer
	{
		get
		{
			return timer;
		}
		set
		{
			timer = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		enabled = false;
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		enabled = true;
		timer = 0f;
		playerNetworkManager = ComponentManager<Network_Player>.Value.PlayerNetworkManager;
		network = ComponentManager<Raft_Network>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		PlayerNetworkManager.allBricks.Add(this);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (PlayerNetworkManager.allBricks.Contains(this))
		{
			PlayerNetworkManager.allBricks.Remove(this);
		}
	}

	private void Update()
	{
		if (Raft_Network.IsHost)
		{
			if (!Complete)
			{
				timer += Time.deltaTime;
				if (timer >= dryTimeSeconds)
				{
					OnFinishedDrying();
				}
			}
		}
		else
		{
			enabled = false;
		}
	}

	public void OnFinishedDrying()
	{
		timer = dryTimeSeconds;
		enabled = false;
		filter.sharedMesh = dryMesh;
		if (network == null)
		{
			network = ComponentManager<Raft_Network>.Value;
		}
		if (playerNetworkManager == null)
		{
			playerNetworkManager = ComponentManager<Network_Player>.Value.PlayerNetworkManager;
		}
		if (Raft_Network.IsHost)
		{
			network.RPC(new Message_Brick(Messages.Brick_Complete, playerNetworkManager, ObjectIndex), Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public override RGD Serialize_Save()
	{
		return new RGD_Brick(RGDType.Block_Brick, this);
	}

	public void OnIsRayed()
	{
		if (Complete && Helper.LocalPlayerIsWithinDistance(transform.position, Player.UseDistance))
		{
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
			LocalizationParameters.itemX = pickup.item.settings_Inventory.DisplayName;
			canvas.displayTextManager.ShowText(Helper.GetTerm("Game/PickUpX", applyParameters: true), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey);
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
			{
				canvas.displayTextManager.HideDisplayTexts();
				Message_Brick message = new Message_Brick(Messages.Brick_Pickup, playerNetworkManager, ObjectIndex);
				if (Raft_Network.IsHost)
				{
					network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
					playerNetworkManager.PickupWetBrick(this);
				}
				else
				{
					network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				}
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
		if (canvas != null && canvas.displayTextManager != null)
		{
			canvas.displayTextManager.HideDisplayTexts();
		}
	}
}
