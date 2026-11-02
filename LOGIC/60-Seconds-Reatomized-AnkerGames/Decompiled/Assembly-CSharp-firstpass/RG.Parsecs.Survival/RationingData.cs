using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/New Rationing Data")]
public class RationingData : ScriptableObject
{
	public List<Ration> Rations;

	public void Reset()
	{
		Rations.Clear();
	}

	public bool IsItemRationedForCharacter(int characterIndex, string itemId)
	{
		for (int i = 0; i < Rations.Count; i++)
		{
			if (Rations[i].CharacterIndex == characterIndex && Rations[i].RationedItem.BaseStaticData.ItemId == itemId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsItemRationedForEveryone(string itemId)
	{
		for (int i = 0; i < Rations.Count; i++)
		{
			if (Rations[i].RationedItem.BaseStaticData.ItemId == itemId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsItemRationedForAnyone(string itemId)
	{
		for (int i = 0; i < Rations.Count; i++)
		{
			if (Rations[i].RationedItem.BaseStaticData.ItemId == itemId)
			{
				return true;
			}
		}
		return false;
	}

	public void RationItemForCharacter(int characterIndex, IItem item)
	{
		if (!IsItemRationedForCharacter(characterIndex, item.BaseStaticData.ItemId))
		{
			Ration ration = new Ration();
			ration.CharacterIndex = characterIndex;
			if (!(item is ConsumableRemedium) && !(item is Remedium))
			{
				throw new ArgumentException("Cannot ration an IItem that isn't a Remedium nor ConsumableRemedium");
			}
			ration.RationedItem = item;
			Rations.Add(ration);
		}
	}

	public void UnrationItemFromCharacter(int characterIndex, string itemId)
	{
		int num = -1;
		for (int i = 0; i < Rations.Count; i++)
		{
			if (Rations[i].CharacterIndex == characterIndex && itemId == Rations[i].RationedItem.BaseStaticData.ItemId)
			{
				num = i;
			}
		}
		if (num != -1)
		{
			Rations.RemoveAt(num);
		}
	}

	public float GetPlannedConsumptionForRationingOnly(string _consumableId)
	{
		float num = 0f;
		for (int i = 0; i < Rations.Count; i++)
		{
			if (Rations[i].RationedItem.BaseStaticData.ItemId == _consumableId)
			{
				num += ((ConsumableRemedium)Rations[i].RationedItem).StaticData.ConsumeRatio;
			}
		}
		return num;
	}

	public void ResetRationingData()
	{
		Rations.Clear();
	}

	private void OnValidate()
	{
		if (!Application.isPlaying)
		{
			Reset();
		}
	}
}
