using System;
using FMODUnity;
using UnityEngine;

public class ResourceCollector_Shear : ResourceCollector
{
	[SerializeField]
	private Animator shearAnimator;

	public EventReference er_shearUse;

	protected override void Start()
	{
		base.Start();
		if (!playerNetwork.IsLocalPlayer)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.Action_OnDeSerializeNetworkProperties = (Action<Message_Player_Update>)Delegate.Combine(animator.Action_OnDeSerializeNetworkProperties, new Action<Message_Player_Update>(OnNetworkProperties));
		}
	}

	protected override bool Update()
	{
		base.Update();
		if (playerNetwork.IsLocalPlayer)
		{
			shearAnimator.SetBool("ItemHit", playerNetwork.Animator.anim.GetBool("ItemHit"));
		}
		return true;
	}

	private void OnNetworkProperties(Message_Player_Update msg)
	{
		if (gameObject.activeInHierarchy && !playerNetwork.IsLocalPlayer)
		{
			shearAnimator.SetBool("ItemHit", msg.Anim_ItemHit);
		}
	}

	public void Animation_Shear()
	{
		if (!er_shearUse.IsNull)
		{
			RuntimeManager.PlayOneShot(er_shearUse, transform.position);
		}
	}

	protected override void OnToolDeSelect()
	{
		base.OnToolDeSelect();
		shearAnimator.SetBool("ItemHit", value: false);
	}
}
