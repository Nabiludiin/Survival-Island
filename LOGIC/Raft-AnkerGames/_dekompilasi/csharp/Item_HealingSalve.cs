using System;
using FMODUnity;
using UnityEngine;

public class Item_HealingSalve : UseableItem
{
	public SO_Buff_DOT healEffect;

	[SerializeField]
	private Item_Base healingItemToConsume;

	public EventReference er_applySalve;

	protected override void Start()
	{
		base.Start();
		OnChannelFinished = (Action)Delegate.Combine(OnChannelFinished, new Action(TriggerHeal));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnChannelFinished = (Action)Delegate.Remove(OnChannelFinished, new Action(TriggerHeal));
	}

	protected override void HandleChannelDelay()
	{
		base.HandleChannelDelay();
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveItem(healingItemToConsume.UniqueName, 1);
		}
	}

	private void TriggerHeal()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Stats.buffManager.AddBuff(healEffect);
		}
		if (!er_applySalve.IsNull)
		{
			RuntimeManager.PlayOneShot(er_applySalve, transform.position);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveItem(healingItemToConsume.UniqueName, 1);
		}
	}
}
