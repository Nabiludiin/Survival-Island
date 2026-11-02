using System;
using FMODUnity;
using UnityEngine;

public class Shovel : UseableItem
{
	[SerializeField]
	private float useRange = 4f;

	public EventReference er_DigDown;

	public EventReference er_DigThrow;

	[SerializeField]
	private ParticleSystem dirtParticles;

	public GameObject shovelHead;

	public LayerMask diggableLayer;

	public string diggableTag;

	private PickupItem currentTarget;

	private PoolManager poolManager;

	private TreasurePointManager treasurePointManager;

	private TreasurePoint closestTreasurePoint;

	private SoundManager soundManager;

	protected override void Start()
	{
		base.Start();
		soundManager = ComponentManager<SoundManager>.Value;
		canvas = ComponentManager<CanvasHelper>.Value;
		poolManager = ComponentManager<PoolManager>.Value;
		treasurePointManager = ComponentManager<TreasurePointManager>.Value;
		OnChannelFinished = (Action)Delegate.Combine(OnChannelFinished, new Action(OnShovelDone));
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnChannelFinished = (Action)Delegate.Remove(OnChannelFinished, new Action(OnShovelDone));
	}

	public void DigDown()
	{
		if (!playerNetwork.IsLocalPlayer)
		{
			return;
		}
		closestTreasurePoint = treasurePointManager.GetClosestTreasurePointTo(playerNetwork.FeetPosition, 5f, includeUnBuried: false, includeUnrevealed: false, out var distanceToPoint);
		if (closestTreasurePoint != null && distanceToPoint < 3f && Helper.HitAtCursor(out var hit, useRange, LayerMasks.MASK_Obstruction))
		{
			soundManager.PlayOneShotNetworked(er_DigDown, shovelHead.transform.position);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.Hit);
			if (closestTreasurePoint.currentExcevationProgress == 0)
			{
				closestTreasurePoint.SendNetworkMoveChest(hit.point, hit.normal);
			}
			if (closestTreasurePoint.currentExcevationProgress + 1 >= closestTreasurePoint.excevationCount)
			{
				playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
			}
			if (closestTreasurePoint.IsBuried)
			{
				closestTreasurePoint.SendNetworkProgressExcevation(1);
			}
		}
		if (!(currentTarget == null) && isChanellingItem)
		{
			PlayDirtParticles(currentTarget.transform.position, Quaternion.LookRotation(currentTarget.transform.up));
			soundManager.PlayOneShotNetworked(er_DigDown, shovelHead.transform.position);
			SimpleMonoBehaviourSingleton<HapticManager>.Instance.PlayOneShot(HapticEvent.Hit);
		}
	}

	public void DigThrow()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			if (closestTreasurePoint != null)
			{
				closestTreasurePoint = null;
				soundManager.PlayOneShotNetworked(er_DigThrow, shovelHead.transform.position);
			}
			if (!(currentTarget == null) && isChanellingItem)
			{
				PlayDirtParticles(currentTarget.transform.position, Quaternion.LookRotation(currentTarget.transform.up));
				soundManager.PlayOneShotNetworked(er_DigThrow, shovelHead.transform.position);
			}
		}
	}

	public override bool ChannelItem()
	{
		if (Helper.HitAtCursor(out var hit, useRange, diggableLayer))
		{
			if (hit.collider != null && hit.collider.CompareTag(diggableTag))
			{
				canvas.SetAimSprite(AimSprite.Default);
				playerNetwork.Animator.anim.SetBool("ItemHit", value: true);
				PickupItem component = hit.transform.GetComponent<PickupItem>();
				currentTarget = component;
				base.ChannelItem();
				return true;
			}
		}
		else
		{
			base.ResetItemChannel();
			currentTarget = null;
		}
		return false;
	}

	public void ResetShovel()
	{
		if (playerNetwork != null)
		{
			playerNetwork.Animator.anim.SetBool("ItemHit", value: false);
			if (canvas != null)
			{
				canvas.SetAimSprite(AimSprite.Default);
			}
		}
	}

	public void OnShovelDone()
	{
		if (playerNetwork.IsLocalPlayer)
		{
			PlayDirtParticles(currentTarget.transform.position, Quaternion.LookRotation(currentTarget.transform.up));
			playerNetwork.PickupScript.PickupItem(currentTarget, forcePickup: true, triggerHandAnimation: false);
			playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		}
	}

	protected override void OnToolDeSelect()
	{
		base.OnToolDeSelect();
		ResetShovel();
	}

	protected override void ResetItemChannel()
	{
		base.ResetItemChannel();
		ResetShovel();
	}

	private void PlayDirtParticles(Vector3 position, Quaternion rotation)
	{
		if (!(dirtParticles == null))
		{
			dirtParticles.transform.position = position;
			dirtParticles.transform.rotation = rotation;
			dirtParticles.Play(withChildren: true);
		}
	}
}
