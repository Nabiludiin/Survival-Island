using System;
using UnityEngine;

public class EarlyAccessBlueprintFixer : MonoBehaviour
{
	private void Start()
	{
		if (Raft_Network.IsHost)
		{
			SaveAndLoad.LoadComplete = (Action)Delegate.Combine(SaveAndLoad.LoadComplete, new Action(OnLoadComplete));
		}
	}

	private void OnLoadComplete()
	{
		Inventory_ResearchTable value = ComponentManager<Inventory_ResearchTable>.Value;
		if (value == null)
		{
			return;
		}
		if (NoteBook.HasUnlockedFrequencyNoteAtDestination(ChunkPointType.Landmark_RadioTower))
		{
			Item_Base itemByIndex = ItemManager.GetItemByIndex(372);
			if (value.CanResearchBlueprint(itemByIndex))
			{
				value.ResearchBlueprint(itemByIndex);
			}
		}
		if (NoteBook.HasUnlockedFrequencyNoteAtDestination(ChunkPointType.Landmark_CaravanIsland))
		{
			Item_Base itemByIndex2 = ItemManager.GetItemByIndex(373);
			if (value.CanResearchBlueprint(itemByIndex2))
			{
				value.ResearchBlueprint(itemByIndex2);
			}
		}
	}
}
