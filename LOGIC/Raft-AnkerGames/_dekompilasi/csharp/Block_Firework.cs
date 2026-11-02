using FMODUnity;
using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Block_Firework : Block, IRaycastable, IBlockPlaced
{
	private static DisplayTextManager displayTextManager;

	private static Network_Player localPlayer;

	[SerializeField]
	private GameObject fireworkModel;

	[SerializeField]
	private ParticleSystem fuseParticle;

	public RotationComponent rotationComponent;

	[SerializeField]
	private float fuseTime = 2f;

	[SerializeField]
	private StudioEventEmitter emitter_fuse;

	private bool hasBeenUsed;

	private Raft_Network network;

	void IRaycastable.OnIsRayed()
	{
		if (hasBeenUsed || displayTextManager == null)
		{
			return;
		}
		InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Interact"];
		displayTextManager.ShowText(Helper.GetTerm("Game/LaunchFirework"), action.GetBindingDisplayString(), MyInput.Keybinds["Interact"].MainKey, 0, 0, clearAllTexts: false);
		if (!SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "Interact"))
		{
			return;
		}
		Message_NetworkBehaviour_ID message = new Message_NetworkBehaviour_ID(Messages.FireworkBlock_LightFuse, network.NetworkIDManager, ObjectIndex);
		if (Raft_Network.IsHost)
		{
			if (LightFuse())
			{
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
			}
		}
		else
		{
			network.SendP2P(network.HostID, message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		displayTextManager.HideDisplayTexts();
	}

	void IRaycastable.OnRayEnter()
	{
	}

	void IRaycastable.OnRayExit()
	{
		if (displayTextManager != null)
		{
			displayTextManager.HideDisplayTexts();
		}
	}

	void IBlockPlaced.OnBlockPlacedInterface()
	{
		displayTextManager = ComponentManager<CanvasHelper>.Value.displayTextManager;
		network = ComponentManager<Raft_Network>.Value;
	}

	public bool LightFuse()
	{
		if (Raft_Network.IsHost && (hasBeenUsed || IsInvoking("Launch")))
		{
			return false;
		}
		hasBeenUsed = true;
		SetOnOffColliderState(state: false);
		fuseParticle.Play();
		emitter_fuse.PlaySafe();
		Invoke("Launch", fuseTime);
		return true;
	}

	public override void SetInstanceColorAndPattern(SO_ColorValue primary, SO_ColorValue secondary, int paintSide, uint patternIndex)
	{
		base.SetInstanceColorAndPattern(primary, primary, 3, 0u);
	}

	public override bool HasColor(SO_ColorValue primary, SO_ColorValue secondary, int side, uint patternIndex)
	{
		return primary.IsSameColor(currentColorA);
	}

	private void RemoveBlock()
	{
		BlockCreator.RemoveBlockNetwork(this, null, updateRaftBounds: true);
	}

	private void Launch()
	{
		fuseParticle.Stop();
		emitter_fuse.StopSafe();
		fireworkModel.SetActiveSafe(value: false);
		if (Raft_Network.IsHost)
		{
			if (localPlayer == null)
			{
				localPlayer = ComponentManager<Network_Player>.Value;
			}
			int colorValueIndex = ((currentColorA == null) ? (-1) : ((int)currentColorA.uniqueColorIndex));
			localPlayer.FireworkHand.LaunchFireworkNetworked(fireworkModel.transform.position, fireworkModel.transform.up, removeItemFromInventory: false, colorValueIndex);
		}
		if (Raft_Network.IsHost)
		{
			Invoke("RemoveBlock", 5f);
		}
	}

	public override RGD_Block GetBlockCreationData()
	{
		return new RGD_Block_Firework(RGDType.Block_Firework, this);
	}
}
