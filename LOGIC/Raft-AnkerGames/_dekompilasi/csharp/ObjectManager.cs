using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
	public Object_Chicken Object_Chicken;

	private static Dictionary<ObjectOfInterestType, Transform> ObjectsOfInterest = new Dictionary<ObjectOfInterestType, Transform>();

	private void Awake()
	{
		ComponentManager<ObjectManager>.Value = this;
	}

	private void Start()
	{
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnPlaceBlock));
	}

	private void OnDestroy()
	{
		ObjectsOfInterest.Clear();
	}

	public void OnPlaceBlock(Block block, Network_Player player, bool replicating, int hotslotIndex)
	{
		if (replicating || !(player != null) || !player.IsLocalPlayer || hotslotIndex < 0)
		{
			return;
		}
		Slot slot = player.Inventory.GetSlot(hotslotIndex);
		if (slot != null && !slot.IsEmpty && slot.itemInstance.UniqueIndex == 257 && !slot.itemInstance.exclusiveString.IsNullOrEmpty())
		{
			TextWriterObject componentInChildren = block.GetComponentInChildren<TextWriterObject>();
			if (componentInChildren != null)
			{
				componentInChildren.SetTextNetworked(slot.itemInstance.exclusiveString);
			}
		}
	}

	public static void AddObjectOfInterest(ObjectOfInterestType type, Transform transformOfInterest)
	{
		if (type != ObjectOfInterestType.None && !ObjectsOfInterest.ContainsKey(type))
		{
			ObjectsOfInterest.Add(type, transformOfInterest);
		}
	}

	public static void RemoveObjectOfInterest(ObjectOfInterestType type)
	{
		if (type != ObjectOfInterestType.None && ObjectsOfInterest.ContainsKey(type))
		{
			ObjectsOfInterest.Remove(type);
		}
	}

	public static Transform GetObjectOfInterest(ObjectOfInterestType type)
	{
		if (ObjectsOfInterest.ContainsKey(type))
		{
			return ObjectsOfInterest[type];
		}
		return null;
	}
}
