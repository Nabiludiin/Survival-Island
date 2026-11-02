using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

[RequireComponent(typeof(PickupItem))]
[RequireComponent(typeof(PickupItem_Networked))]
public class Plant : MonoBehaviour
{
	public delegate void OnPlantRemoved(int slotIndex);

	protected OnPlantRemoved onRemovedCallstack;

	[Tooltip("The name of the plant item")]
	public Item_Base item;

	public float growTime = 1f;

	public Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);

	public Vector3 maxScale = Vector3.one;

	private float growTimeSec;

	public bool harvestable = true;

	public bool playerCanHarvest = true;

	[HideInInspector]
	public int plantationSlotIndex;

	[HideInInspector]
	public Cropplot cropplot;

	private float growTimer;

	private bool fullyGrown;

	[HideInInspector]
	public PickupItem pickupComponent;

	private void SmallPlant()
	{
		transform.localEulerAngles = Vector3.zero;
		transform.localPosition = Vector3.zero;
		transform.localScale = minScale;
	}

	private void BigPlant()
	{
		transform.localEulerAngles = Vector3.zero;
		transform.localPosition = Vector3.zero;
		transform.localScale = maxScale;
	}

	public virtual void Awake()
	{
		growTimeSec = growTime * 60f;
		pickupComponent = GetComponent<PickupItem>();
		pickupComponent.canBePickedUp = false;
		pickupComponent.networkID = GetComponent<PickupItem_Networked>();
		pickupComponent.networkID.spawnType = ObjectSpawnType.Prefab;
		pickupComponent.networkID.stopTrackingOnPickup = true;
		pickupComponent.networkID.stopTrackUseRPC = false;
		ResetStats();
	}

	public virtual void Start()
	{
		transform.localEulerAngles = Vector3.zero;
	}

	public void Grow(Raft_Network network, Network_Player localPlayer)
	{
		if (fullyGrown)
		{
			return;
		}
		float num = Mathf.Clamp(growTimer / growTimeSec, 0f, 1f);
		Vector3 target = minScale + (maxScale - minScale) * num;
		transform.localScale = Vector3.MoveTowards(transform.localScale, target, Time.deltaTime * 20f);
		growTimer += Time.deltaTime;
		if (growTimer >= growTimeSec)
		{
			if (Raft_Network.IsHost)
			{
				Message_Plant_Complete message = new Message_Plant_Complete(Messages.PlantManager_PlantComplete, localPlayer.PlantManager, cropplot, this);
				network.RPC(message, Target.Other, EP2PSend.k_EP2PSendReliable, NetworkChannel.Channel_Game);
				Complete();
			}
			else
			{
				fullyGrown = true;
			}
		}
	}

	public void SubscribeToOnPlantRemove(OnPlantRemoved method)
	{
		onRemovedCallstack = (OnPlantRemoved)Delegate.Combine(onRemovedCallstack, method);
	}

	public bool FullyGrown()
	{
		return fullyGrown;
	}

	public void PullRoots()
	{
		if (onRemovedCallstack != null)
		{
			onRemovedCallstack(plantationSlotIndex);
		}
		plantationSlotIndex = -1;
	}

	public void SetYield(List<Cost> newYields)
	{
		if (pickupComponent.yieldHandler != null)
		{
			pickupComponent.yieldHandler.Yield.Clear();
			pickupComponent.yieldHandler.Yield.AddRange(newYields);
		}
	}

	public void IncrementGrowTimer(float amount)
	{
		SetGrowTimer(growTimer + amount);
	}

	public bool ShouldBeFullyGrown()
	{
		if (growTimer >= growTimeSec)
		{
			return true;
		}
		return false;
	}

	public void SetGrowTimer(float value)
	{
		growTimer = value;
		if (growTimer < growTimeSec)
		{
			fullyGrown = false;
		}
	}

	public float GetGrowTimer()
	{
		return growTimer;
	}

	private void ResetStats()
	{
		growTimer = 0f;
		fullyGrown = false;
		harvestable = false;
		transform.localScale = minScale;
	}

	public virtual void Complete()
	{
		fullyGrown = true;
		harvestable = true;
		growTimer = growTimeSec;
		transform.localScale = maxScale;
	}
}
