using System;
using System.Collections;
using FMODUnity;
using Steamworks;
using UnityEngine;

public class HazmatSuit : MonoBehaviour
{
	public Action OnHazmatEquipped;

	[SerializeField]
	private Network_Player playerNetwork;

	[SerializeField]
	private float wearableTime = 5f;

	public EventReference er_equip;

	public EventReference er_unEquip;

	private static CanvasHelper canvas;

	private BuffDisplayObject displayedBuff;

	public bool Equipped => Timer > 0f;

	public float Timer { get; set; }

	private void Update()
	{
		if (playerNetwork.PlayerScript.IsDead || !(Timer > 0f))
		{
			return;
		}
		Timer -= Time.deltaTime;
		if (Timer <= 0f)
		{
			Timer = 0f;
			if (playerNetwork.IsLocalPlayer)
			{
				UnEquipNetworked();
			}
		}
	}

	public IEnumerator EquipDelayed(float delay, float duration = -1f)
	{
		yield return new WaitForSeconds(delay);
		Equip(duration);
	}

	public void Equip(float duration = -1f)
	{
		if (!Equipped)
		{
			RuntimeManager.PlayOneShot(er_equip, transform.position);
		}
		Timer = ((duration < 0f) ? wearableTime : duration);
		playerNetwork.currentModel.UpdateArmAndBodyMesh();
		playerNetwork.PlayerEquipment.SetAllEquipmentModelVisible(visible: false);
		playerNetwork.currentModel.ActivateHairStyle(HairStyle.Bald, useAsCurrentHairStyle: false);
		if (playerNetwork.IsLocalPlayer)
		{
			displayedBuff = ComponentManager<BuffDisplayer>.Value.DisplayBuff(BuffDisplayType.Hazmat_Good, wearableTime, wearableTime - Timer);
		}
		SetHazmatUIState(state: true);
		OnHazmatEquipped?.Invoke();
	}

	public void EquipNetworked(float duration = -1f)
	{
		Message_HazmatSuit message = new Message_HazmatSuit(Messages.HazmatSuit, playerNetwork, equiped: true, duration);
		if (Raft_Network.IsHost)
		{
			Equip(duration);
			playerNetwork.Network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
		else
		{
			playerNetwork.SendP2P(message, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
		}
	}

	public void UnEquip(bool onBuffExpired = false)
	{
		if (Equipped | onBuffExpired)
		{
			RuntimeManager.PlayOneShot(er_unEquip, transform.position);
		}
		Timer = 0f;
		playerNetwork.currentModel.UpdateArmAndBodyMesh();
		playerNetwork.PlayerEquipment.SetAllEquipmentModelVisible(visible: true);
		playerNetwork.currentModel.ActivateHairStyle(playerNetwork.currentModel.CurrentHairStyle, useAsCurrentHairStyle: false);
		if (playerNetwork.IsLocalPlayer && displayedBuff != null && !displayedBuff.isExpired)
		{
			displayedBuff.Expire();
		}
		SetHazmatUIState(state: false);
	}

	public void UnEquipNetworked()
	{
		playerNetwork.SendP2P(new Message_HazmatSuit(Messages.HazmatSuit, playerNetwork, equiped: false), EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
	}

	private void SetHazmatUIState(bool state)
	{
		if (playerNetwork.IsLocalPlayer)
		{
			if (canvas == null)
			{
				canvas = ComponentManager<CanvasHelper>.Value;
			}
			canvas.hazmatSuitImage.SetActiveSafe(state);
		}
	}
}
