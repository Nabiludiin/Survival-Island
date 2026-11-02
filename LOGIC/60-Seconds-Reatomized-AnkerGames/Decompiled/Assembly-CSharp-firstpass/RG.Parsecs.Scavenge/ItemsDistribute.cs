using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ItemsDistribute
{
	[SerializeField]
	private List<ItemSpawnPoint> _spawnPoints = new List<ItemSpawnPoint>();

	public ScavangeItemsDatabase itemsDatabase;

	public void Reset()
	{
		itemsDatabase.ResetState();
		itemsDatabase.CaptainCharacter.scavangeItems.survivalItemRef.Add();
	}

	public void AddSpawnPoints(List<ItemSpawnPoint> spawnPoints)
	{
		_spawnPoints = spawnPoints;
	}

	public IEnumerator SpawnItems(int seed, ScavengeTutorialDriver tutorialDriver)
	{
		yield return null;
		List<ScavangeItemData> list = new List<ScavangeItemData>();
		List<PriorityItemSpawnPoint> list2 = new List<PriorityItemSpawnPoint>();
		if (tutorialDriver != null)
		{
			list2.AddRange(tutorialDriver.SpawnPoints);
		}
		UnityEngine.Random.InitState(seed);
		ScavangeItemDataEntry[] allScavangeItems = itemsDatabase.allScavangeItems;
		foreach (ScavangeItemDataEntry scavangeItemDataEntry in allScavangeItems)
		{
			int amount = scavangeItemDataEntry.GetAmount();
			for (int j = 0; j < amount; j++)
			{
				list.Add(scavangeItemDataEntry.scavangeItems);
			}
		}
		List<ScavangeItemData> list3 = list.OrderBy((ScavangeItemData scavangeItemData) => scavangeItemData.ItemSpawnDifficulty).ToList();
		foreach (PriorityItemSpawnPoint item in list2)
		{
			item.AssignItem();
		}
		foreach (ScavangeItemData itm in list3)
		{
			if (list2.Count > 0)
			{
				PriorityItemSpawnPoint priorityItemSpawnPoint = list2.Find((PriorityItemSpawnPoint priorityItemSpawnPoint2) => priorityItemSpawnPoint2.SpawnItem == itm);
				if (priorityItemSpawnPoint != null)
				{
					list2.Remove(priorityItemSpawnPoint);
					continue;
				}
			}
			itm.GetBestSpawnPointForMe(_spawnPoints).AssignItem(itm);
		}
	}
}
