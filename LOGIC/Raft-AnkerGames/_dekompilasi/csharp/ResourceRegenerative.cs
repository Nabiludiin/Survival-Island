using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ResourceRegenerative : MonoBehaviour
{
	public SO_ResourceRegenerative resource;

	public Action OnIsReady;

	public Action OnIsNotReady;

	public bool IsReadyAtAwake;

	private bool _isReady;

	private float regenCount;

	private Network_Player localPlayer;

	public bool IsHoldingRequiredCollectionItem => localPlayer.PlayerItemManager.useItemController.GetCurrentItemInHand() == resource.collectionItem;

	public bool IsReady
	{
		get
		{
			if (HasResource)
			{
				return _isReady;
			}
			return false;
		}
		set
		{
			if (HasResource)
			{
				_isReady = value;
				return;
			}
			_isReady = false;
			regenCount = 0f;
		}
	}

	public float NormalizedRegenTime => Mathf.Clamp01(regenCount / resource.originRegenTime);

	public bool HasResource => resource != null;

	private void RegenResource()
	{
		RegenResource(resource.originRegenTime);
	}

	private void AddRegen()
	{
		RegenResource(50f);
	}

	private void Start()
	{
		localPlayer = ComponentManager<Network_Player>.Value;
	}

	private void Awake()
	{
		if (IsReadyAtAwake)
		{
			RegenResource();
		}
	}

	public void SendIsReady()
	{
		if (OnIsReady != null)
		{
			OnIsReady();
		}
	}

	public void SendIsNotReady()
	{
		if (OnIsNotReady != null)
		{
			OnIsNotReady();
		}
	}

	public void RegenResource(float amount)
	{
		if (!IsReady)
		{
			regenCount += amount;
			if (regenCount >= resource.originRegenTime)
			{
				IsReady = true;
				SendIsReady();
			}
		}
	}

	public void DeRegenResource(float amount)
	{
		if (IsReady)
		{
			regenCount -= amount;
			if (regenCount <= resource.originRegenTime)
			{
				IsReady = false;
				SendIsNotReady();
			}
		}
	}

	public float GetRegenCount()
	{
		return regenCount;
	}

	public void SetRegenCount(float value, bool sendReadyEvents = true)
	{
		regenCount = value;
		if (regenCount >= resource.originRegenTime)
		{
			IsReady = true;
			if (sendReadyEvents)
			{
				SendIsReady();
			}
		}
		else
		{
			IsReady = false;
			if (sendReadyEvents)
			{
				SendIsNotReady();
			}
		}
	}

	public bool CanHarvestResource()
	{
		return IsReady;
	}

	public Item_Base HarvestResource()
	{
		DeplenishResource();
		SendIsNotReady();
		return resource.resource;
	}

	public void DeplenishResource()
	{
		IsReady = false;
		regenCount = 0f;
	}

	public void DisableResourcePermanently()
	{
		resource = null;
	}
}
