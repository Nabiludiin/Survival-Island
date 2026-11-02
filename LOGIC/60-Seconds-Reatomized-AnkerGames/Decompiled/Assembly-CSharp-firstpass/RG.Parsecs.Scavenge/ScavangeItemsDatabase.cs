using System;
using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[CreateAssetMenu(fileName = "ScavangeItemsDatabase", menuName = "Scavenge/ScavangeItemsDatabase")]
public class ScavangeItemsDatabase : ScriptableObject
{
	public ScavangeItemDataEntry[] scavangeItems;

	public ScavangeItemDataEntry[] characterList;

	public ScavangeItemData Soup;

	public ScavangeItemDataEntry[] allScavangeItems
	{
		get
		{
			List<ScavangeItemDataEntry> list = new List<ScavangeItemDataEntry>();
			list.AddRange(scavangeItems);
			ScavangeItemDataEntry[] array = characterList;
			foreach (ScavangeItemDataEntry scavangeItemDataEntry in array)
			{
				ScavengeCharacterItem scavengeCharacterItem = scavangeItemDataEntry.scavangeItems.survivalItemRef as ScavengeCharacterItem;
				if (scavengeCharacterItem != null && !scavengeCharacterItem.character.RuntimeData.IsCaptain)
				{
					list.Add(scavangeItemDataEntry);
				}
			}
			return list.ToArray();
		}
	}

	public ScavangeItemDataEntry CaptainCharacter
	{
		get
		{
			ScavangeItemDataEntry[] array = characterList;
			foreach (ScavangeItemDataEntry scavangeItemDataEntry in array)
			{
				ScavengeCharacterItem scavengeCharacterItem = scavangeItemDataEntry.scavangeItems.survivalItemRef as ScavengeCharacterItem;
				if (scavengeCharacterItem != null && scavengeCharacterItem.character.RuntimeData.IsCaptain)
				{
					return scavangeItemDataEntry;
				}
			}
			Debug.LogError("Captain not found");
			return null;
		}
	}

	public void ResetState()
	{
		ScavangeItemDataEntry[] array = allScavangeItems;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].scavangeItems.survivalItemRef.CreateNewRuntimeData();
		}
	}

	internal int TotalItemsGathered(List<string> itemIDs)
	{
		int num = 0;
		ScavangeItemDataEntry[] array = scavangeItems;
		foreach (ScavangeItemDataEntry scavangeItemDataEntry in array)
		{
			if (!(scavangeItemDataEntry.scavangeItems == Soup) && !(scavangeItemDataEntry.scavangeItems.survivalItemRef as ScavangeResource != null) && scavangeItemDataEntry.scavangeItems.survivalItemRef.BaseRuntimeData.IsAvailable)
			{
				num++;
				itemIDs.Add(scavangeItemDataEntry.scavangeItems.survivalItemRef.BaseStaticData.ItemId);
			}
		}
		return num;
	}

	internal int TotalSoupsGathered()
	{
		return (int)(Soup.survivalItemRef.BaseRuntimeData as ConsumableRuntimeData).Amount;
	}

	public bool CheckHandsFullAchievement(List<string> itemIDs)
	{
		int num = 0;
		ScavangeItemDataEntry[] array = scavangeItems;
		foreach (ScavangeItemDataEntry scavangeItemDataEntry in array)
		{
			if (!(scavangeItemDataEntry.scavangeItems == Soup) && !(scavangeItemDataEntry.scavangeItems.survivalItemRef as ScavangeResource != null))
			{
				num++;
			}
		}
		return itemIDs.Count >= num;
	}

	internal void CheckSoupOdysseyAchievement(int scavengedSoups)
	{
		throw new NotImplementedException();
	}
}
