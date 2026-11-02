using System;
using FMODUnity;
using UnityEngine;

public class Item_ApplyBuff : UseableItem
{
	public SO_Buff buffEffect;

	[SerializeField]
	private Item_Base itemToConsume;

	[SerializeField]
	[EventRef]
	private string eventRef_applyBuff = string.Empty;

	protected override void Start()
	{
		base.Start();
		OnChannelFinished = (Action)Delegate.Combine(OnChannelFinished, new Action(TriggerEffect));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnChannelFinished = (Action)Delegate.Remove(OnChannelFinished, new Action(TriggerEffect));
	}

	protected override void HandleChannelDelay()
	{
		base.HandleChannelDelay();
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveItem(itemToConsume.UniqueName, 1);
		}
	}

	private void TriggerEffect()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger, triggering: true);
			playerNetwork.Stats.buffManager.AddBuff(buffEffect);
			playerNetwork.Stats.Consume(itemToConsume);
		}
		if (eventRef_applyBuff.IsValid())
		{
			RuntimeManager.PlayOneShot(eventRef_applyBuff, transform.position);
		}
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveItem(itemToConsume.UniqueName, 1);
		}
	}
}
