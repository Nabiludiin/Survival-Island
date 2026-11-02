using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PickupItem))]
public class HarvestableTree : MonoBehaviour
{
	public delegate void OnTreeHarvest();

	[SerializeField]
	private bool hideOnDepleted;

	[SerializeField]
	private string particleSystemName = string.Empty;

	[SerializeField]
	private Transform[] particleSpawnPositions;

	private PickupItem pickupItem;

	public bool Depleted => pickupItem.yieldHandler.Yield.Count <= 0;

	public PickupItem_Networked PickupNetwork => pickupItem.networkID;

	public PickupItem PickupItem => pickupItem;

	public event OnTreeHarvest OnTreeHarvestEvent;

	private void CopyYieldFromPickup()
	{
		if (!(GetComponent<PickupItem>() != null))
		{
			Debug.Log("Can't find PickupItem script");
		}
	}

	private void Awake()
	{
		pickupItem = GetComponent<PickupItem>();
		Landmark componentInParent = GetComponentInParent<Landmark>();
		if (componentInParent != null)
		{
			componentInParent.OnLandmarkReset = (Action)Delegate.Combine(componentInParent.OnLandmarkReset, new Action(OnLandmarkReset));
		}
	}

	private void OnLandmarkReset()
	{
		if (pickupItem == null || pickupItem.yieldHandler == null)
		{
			Debug.Log("Yield handler is null on ", gameObject);
		}
		pickupItem.yieldHandler.ResetYield();
	}

	public bool Harvest(PlayerInventory playerInventory)
	{
		if (Depleted)
		{
			return false;
		}
		if (!pickupItem.canBePickedUp)
		{
			return false;
		}
		Cost cost = pickupItem.yieldHandler.Yield.Last();
		if (cost == null || cost.item == null)
		{
			return false;
		}
		if (playerInventory != null)
		{
			playerInventory.AddItem(cost.item.UniqueName, cost.amount);
			if (pickupItem.dropper != null)
			{
				Item_Base[] randomItems = pickupItem.dropper.GetRandomItems();
				for (int i = 0; i < randomItems.Length; i++)
				{
					if (randomItems[i] != null)
					{
						playerInventory.AddItem(randomItems[i].UniqueName, 1);
					}
				}
			}
		}
		pickupItem.yieldHandler.Yield.Remove(cost);
		if (Depleted)
		{
			if (!string.IsNullOrEmpty(particleSystemName))
			{
				for (int j = 0; j < particleSpawnPositions.Length; j++)
				{
					Transform transform = particleSpawnPositions[j];
					if (!(transform == null))
					{
						ParticleManager.PlaySystem(particleSystemName, transform.position, transform.rotation);
					}
				}
			}
			ComponentManager<SoundManager>.Value.PlayBlockBreak(base.transform.position, withWaterSound: false);
			if (hideOnDepleted)
			{
				gameObject.SetActive(value: false);
			}
		}
		if (OnTreeHarvestEvent != null)
		{
			OnTreeHarvestEvent();
		}
		return true;
	}
}
