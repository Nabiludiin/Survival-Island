using System;
using FMODUnity;
using UnityEngine;

public class ThrowableComponent_Bow : ThrowableComponent
{
	[SerializeField]
	private Animator bowAnimator;

	[SerializeField]
	private StudioEventEmitter emitter_drawBow;

	[SerializeField]
	private StudioEventEmitter emitter_fireBow;

	private Settings settings;

	protected override void Start()
	{
		base.Start();
		settings = ComponentManager<Settings>.Value;
		if (!playerNetwork.IsLocalPlayer)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.Action_OnDeSerializeNetworkProperties = (Action<Message_Player_Update>)Delegate.Combine(animator.Action_OnDeSerializeNetworkProperties, new Action<Message_Player_Update>(OnNetworkProperties));
		}
	}

	private void OnNetworkProperties(Message_Player_Update msg)
	{
		if (!gameObject.activeInHierarchy || playerNetwork.IsLocalPlayer)
		{
			return;
		}
		bool flag = bowAnimator.GetBool("ItemHit");
		bowAnimator.SetBool("ItemHit", msg.Anim_ItemHit);
		playerNetwork.Animator.anim.SetBool("ItemHit", msg.Anim_ItemHit);
		if (!flag && msg.Anim_ItemHit && !emitter_drawBow.IsPlaying())
		{
			emitter_drawBow.Play();
		}
		string[] anim_triggers = msg.anim_triggers;
		for (int i = 0; i < anim_triggers.Length; i++)
		{
			if (anim_triggers[i] == "ItemHitTrigger")
			{
				bowAnimator.SetTrigger("ItemHitTrigger");
				break;
			}
		}
	}

	protected override void CallStartChargeEvent()
	{
		base.CallStartChargeEvent();
		emitter_drawBow.PlaySafe();
	}

	protected override void ReleaseButton()
	{
		base.ReleaseButton();
		bowAnimator.SetTrigger("ItemHitTrigger");
		playerNetwork.Animator.SetAnimation(PlayerAnimation.Trigger_ItemHitTrigger);
		emitter_fireBow.Play();
	}

	protected override void CallThrowEvent(Vector3 position, Vector3 force)
	{
		base.CallThrowEvent(position, force);
		if (playerNetwork.IsLocalPlayer)
		{
			playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		}
	}

	protected override void HandleLocalClient()
	{
		base.HandleLocalClient();
		bool value = chargeMeter.ChargeNormal != 0f && canThrow;
		bowAnimator.SetBool("ItemHit", value);
		playerNetwork.Animator.anim.SetBool("ItemHit", value);
	}
}
