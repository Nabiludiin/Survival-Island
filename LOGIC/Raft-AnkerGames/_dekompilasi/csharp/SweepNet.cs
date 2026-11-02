using System;
using FMODUnity;
using UnityEngine;

public class SweepNet : UsableTool
{
	public float useRange;

	public LayerMask captureLayer;

	public string captureTag;

	public EventReference er_swing;

	public EventReference er_capture;

	private PickupItem currentPickupTarget;

	protected override void Start()
	{
		base.Start();
		if (playerNetwork != null)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.OnAnimationEvent = (Action<string>)Delegate.Combine(animator.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (playerNetwork != null)
		{
			PlayerAnimator animator = playerNetwork.Animator;
			animator.OnAnimationEvent = (Action<string>)Delegate.Remove(animator.OnAnimationEvent, new Action<string>(OnAnimationEvent));
		}
	}

	private void OnAnimationEvent(string parameter)
	{
		if (!parameter.Equals("AnimationSweep"))
		{
			return;
		}
		RuntimeManager.PlayOneShotAttached(er_swing, gameObject);
		if (playerNetwork != null && playerNetwork.IsLocalPlayer && Helper.HitAtCursor(out var hit, useRange, captureLayer) && hit.collider != null && hit.collider.CompareTag(captureTag))
		{
			PickupItem item = (currentPickupTarget = hit.transform.GetComponent<PickupItem>());
			if (AttemptCaptureWithNet(hit.collider))
			{
				playerNetwork.PickupScript.AddItemToInventory(item);
				PlaySuccessfullCaptureSound();
				playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
			}
		}
	}

	private bool AttemptCaptureWithNet(Collider hit)
	{
		if (hit == null)
		{
			return false;
		}
		AI_StateMachine_Animal componentInParent = hit.GetComponentInParent<AI_StateMachine_Animal>();
		if (componentInParent != null)
		{
			AI_NetworkBehaviour networkBehaviour = componentInParent.networkBehaviour;
			if (!(networkBehaviour != null))
			{
				Debug.Log("networkig behaviour is null on", componentInParent.gameObject);
				return false;
			}
			if (!networkBehaviour.IsKilled)
			{
				playerNetwork.Network.hostNetwork.DamageEntity(networkBehaviour.networkEntity, networkBehaviour.transform, 99999f, Vector3.zero, Vector3.zero, EntityType.Player);
				return true;
			}
		}
		return false;
	}

	private void PlaySuccessfullCaptureSound()
	{
		RuntimeManager.PlayOneShotAttached(er_capture, gameObject);
	}
}
