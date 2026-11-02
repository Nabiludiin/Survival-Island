using Steamworks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Firework_Hand : MonoBehaviour
{
	public Firework fireworkPrefab;

	public Item_Base itemToRemove;

	[SerializeField]
	private GameObject fireworkHandModel;

	private Network_Player player;

	private void Update()
	{
		if (CanvasHelper.ActiveMenu != MenuType.None || PlayerItemManager.IsBusy)
		{
			return;
		}
		if (player == null)
		{
			player = GetComponentInParent<Network_Player>();
		}
		if (!player.IsLocalPlayer)
		{
			return;
		}
		if (!(player.BlockCreator.selectedBlock != null) || !player.BlockCreator.selectedBlock.gameObject.activeInHierarchy)
		{
			fireworkHandModel.SetActiveSafe(value: true);
			player.Animator.SetAnimation(PlayerAnimation.Index_33_Firework);
			InputAction action = PlayerInput.GetPlayerByIndex(0).actions["Fire"];
			if (SimpleMonoBehaviourSingleton<CustomInputConfig>.Instance.WasPressedThisFrame(action, "LMB"))
			{
				LaunchFireworkNetworked(transform.position, player.Camera.transform.forward, removeItemFromInventory: true, -1);
			}
		}
		else
		{
			player.Animator.SetAnimation(PlayerAnimation.Index_1_Point);
			fireworkHandModel.SetActiveSafe(value: false);
		}
	}

	public void LaunchFireworkNetworked(Vector3 startPosition, Vector3 startDirection, bool removeItemFromInventory, int colorValueIndex)
	{
		if (player == null)
		{
			player = GetComponentInParent<Network_Player>();
		}
		float lifeTime = fireworkPrefab.lifeTimeInterval.GetRandomValue();
		if (Cheat.FireworkLifeTime > 0f)
		{
			lifeTime = Random.Range(Cheat.FireworkLifeTime - 0.25f, Cheat.FireworkLifeTime + 0.25f);
		}
		Message_Firework_Launch message_Firework_Launch = new Message_Firework_Launch(Messages.FireworkHand_Launch, player, startPosition, startDirection, lifeTime, removeItemFromInventory, colorValueIndex, Raft_Network.IsHost ? SaveAndLoad.GetUniqueObjectIndex() : 0u);
		if (Raft_Network.IsHost)
		{
			LaunchFirework(message_Firework_Launch.StartPosition, message_Firework_Launch.StartDirection, message_Firework_Launch.LifeTime, message_Firework_Launch.RemoveItemFromInventory, colorValueIndex, message_Firework_Launch.FireworkObjectIndex);
			player.Network.RPC(message_Firework_Launch, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			player.SendP2P(message_Firework_Launch, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void LaunchFirework(Message_Firework_Launch msg)
	{
		LaunchFirework(msg.StartPosition, msg.StartDirection, msg.LifeTime, msg.RemoveItemFromInventory, msg.ColorValueIndex, msg.FireworkObjectIndex);
	}

	public void LaunchFirework(Vector3 startPosition, Vector3 startDirection, float lifeTime, bool removeItemFromInventory, int colorValueIndex, uint objectIndex)
	{
		Firework firework = Object.Instantiate(fireworkPrefab, startPosition, Quaternion.LookRotation(startDirection), null);
		firework.ObjectIndex = objectIndex;
		firework.SetColor(colorValueIndex);
		firework.lifeTime = lifeTime;
		NetworkIDManager.AddNetworkID(firework, typeof(Firework));
		if (player == null)
		{
			player = GetComponentInParent<Network_Player>();
		}
		if (removeItemFromInventory && player != null && player.IsLocalPlayer && player.Inventory != null)
		{
			player.Inventory.RemoveItem(itemToRemove.UniqueName, 1);
		}
	}
}
