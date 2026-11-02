using System;
using System.Collections.Generic;
using UnityEngine;

public class YieldHandler : MonoBehaviour
{
	public SO_ItemYield yieldAsset;

	private List<Cost> yield = new List<Cost>();

	public Action OnYieldRemoved;

	public List<Cost> Yield => yield;

	private void Awake()
	{
		ResetYield();
	}

	public void ResetYield()
	{
		if (yieldAsset != null)
		{
			yield = new List<Cost>(yieldAsset.yieldAssets);
		}
	}

	public bool CollectYield(Network_Player yieldingPlayer)
	{
		if (yieldingPlayer == null || yield == null || yield.Count <= 0)
		{
			return false;
		}
		Item_Base item = yield[0].item;
		if (item == null)
		{
			return false;
		}
		if (yieldingPlayer.IsLocalPlayer)
		{
			yieldingPlayer.Inventory.AddItem(item.UniqueName, yield[0].amount);
		}
		RemoveYield();
		return true;
	}

	public void RemoveYield()
	{
		if (yield.Count > 0)
		{
			yield.RemoveAt(0);
		}
		SendOnYieldRemovedEvent();
	}

	public void RestoreYield(int yieldCount)
	{
		if (yieldCount != -1)
		{
			while (yield.Count > yieldCount)
			{
				RemoveYield();
			}
		}
	}

	public bool IsYieldEmpty()
	{
		if (yield.Count <= 0)
		{
			return true;
		}
		return false;
	}

	public void SendOnYieldRemovedEvent()
	{
		if (OnYieldRemoved != null)
		{
			OnYieldRemoved();
		}
	}
}
