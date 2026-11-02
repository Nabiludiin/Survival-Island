using System;
using UnityEngine;

public class ResourceCollector : UseableItem
{
	public LayerMask toolInteractableLayer;

	public string toolInteractTag;

	[SerializeField]
	private float toolUseRange = 3f;

	private ResourceRegenerative targetResource;

	protected override void Start()
	{
		base.Start();
		canvas = ComponentManager<CanvasHelper>.Value;
		if (playerNetwork.IsLocalPlayer)
		{
			OnChannelFinished = (Action)Delegate.Combine(OnChannelFinished, new Action(OnHarvest));
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		OnChannelFinished = (Action)Delegate.Remove(OnChannelFinished, new Action(OnHarvest));
	}

	protected override bool Update()
	{
		base.Update();
		if (!playerNetwork.IsLocalPlayer)
		{
			return false;
		}
		bool flag = false;
		if (isChanellingItem)
		{
			flag = ChannelItem();
		}
		if (!flag && currentItemChannelTime > 0f)
		{
			ResetItemChannel();
		}
		return true;
	}

	public virtual void OnHarvest()
	{
		if (playerNetwork.IsLocalPlayer && targetResource.IsReady)
		{
			Item_Base item_Base = targetResource.HarvestResource();
			playerNetwork.Inventory.AddItem(item_Base.UniqueName, 1);
			playerNetwork.Inventory.RemoveDurabillityFromHotSlot();
		}
	}

	public override bool ChannelItem()
	{
		if (Helper.HitAtCursor(out var hit, toolUseRange, toolInteractableLayer))
		{
			if (hit.collider != null && hit.collider.CompareTag(toolInteractTag))
			{
				ResourceRegenerative componentInChildren = hit.transform.GetComponentInChildren<ResourceRegenerative>();
				if (componentInChildren != null && componentInChildren.IsReady && componentInChildren.IsHoldingRequiredCollectionItem)
				{
					base.ChannelItem();
					targetResource = componentInChildren;
					canvas.SetAimSprite(AimSprite.Default);
					return true;
				}
			}
		}
		else
		{
			base.ResetItemChannel();
		}
		return false;
	}
}
