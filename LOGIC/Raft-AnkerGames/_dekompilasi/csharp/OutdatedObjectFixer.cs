using UnityEngine;

public static class OutdatedObjectFixer
{
	public static int GetFixedObjectUniqueIndex(int p_index, ref bool p_corrected)
	{
		p_corrected = true;
		switch (p_index)
		{
		case 128:
			return 195;
		case 96:
			return -1;
		default:
			p_corrected = false;
			return p_index;
		}
	}

	public static void PerformObjectFixForNonCreatedBlock(int p_oldItemIndex, RGD_Block RGD_block)
	{
		if (p_oldItemIndex == 96)
		{
			FixFoundationArmorForNonCreatedBlock(RGD_block);
		}
	}

	public static void PerformObjectFixForCreatedBlock(int p_oldItemIndex, Block p_block)
	{
		if (p_oldItemIndex == 128)
		{
			FixSharkTrophyToPlank(p_block);
		}
	}

	public static void FixFoundationArmorForNonCreatedBlock(RGD_Block rgdBlock)
	{
		if (rgdBlock == null)
		{
			return;
		}
		Vector3 position = rgdBlock.LocalBlockPosition;
		BlockCreator.SnapBuildPosition(ref position);
		Collider[] array = Physics.OverlapBox(SingletonGeneric<GameManager>.Singleton.lockedPivot.TransformPoint(position), new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, LayerMasks.MASK_Block);
		if (array == null || array.Length == 0)
		{
			Debug.LogWarning("No foundations were found when placing armor");
			return;
		}
		bool flag = false;
		Collider[] array2 = array;
		foreach (Collider collider in array2)
		{
			if (collider.CompareTag("Foundation"))
			{
				Block componentInParent = collider.transform.GetComponentInParent<Block>();
				if (!(componentInParent == null))
				{
					flag = true;
					componentInParent.Reinforced = true;
				}
				return;
			}
		}
		if (!flag)
		{
			Debug.LogWarning("Could not find block component when placing armor");
		}
	}

	public static void FixSharkTrophyToPlank(Block p_block)
	{
		Item_Base itemByIndex = ItemManager.GetItemByIndex(127);
		TrophyHolder componentInChildren = p_block.GetComponentInChildren<TrophyHolder>();
		if (componentInChildren != null)
		{
			componentInChildren.PlaceTrophy(null, itemByIndex);
		}
	}

	public static void ConvertLandmarkPoolIndexNumber(ref int p_number, string p_assetBundleKey)
	{
		if (p_assetBundleKey == string.Empty)
		{
			return;
		}
		switch (p_assetBundleKey)
		{
		case "Landmark_Fjutt":
			switch (p_number)
			{
			case 0:
				p_number = 20;
				break;
			case 1:
				p_number = 22;
				break;
			case 2:
				p_number = 23;
				break;
			case 3:
				p_number = 24;
				break;
			}
			break;
		case "Landmark":
			switch (p_number)
			{
			case 0:
				p_number = 1;
				break;
			case 1:
				p_number = 2;
				break;
			case 2:
				p_number = 3;
				break;
			case 3:
				p_number = 4;
				break;
			case 4:
				p_number = 5;
				break;
			case 5:
				p_number = 10;
				break;
			case 6:
				p_number = 11;
				break;
			case 7:
				p_number = 13;
				break;
			case 8:
				p_number = 14;
				break;
			}
			break;
		case "Landmark_Raft":
			switch (p_number)
			{
			case 0:
				p_number = 15;
				break;
			case 1:
				p_number = 16;
				break;
			case 2:
				p_number = 17;
				break;
			case 3:
				p_number = 18;
				break;
			case 4:
				p_number = 21;
				break;
			}
			break;
		case "Landmark_Radar":
			if (p_number == 0)
			{
				p_number = 19;
			}
			break;
		case "Landmark_RadarSmall":
			switch (p_number)
			{
			case 0:
				p_number = 25;
				break;
			case 1:
				p_number = 26;
				break;
			case 2:
				p_number = 27;
				break;
			}
			break;
		case "Landmark_Pilot":
			if (p_number == 0)
			{
				p_number = 31;
			}
			break;
		case "Landmark_Big":
			switch (p_number)
			{
			case 0:
				p_number = 28;
				break;
			case 1:
				p_number = 29;
				break;
			case 2:
				p_number = 30;
				break;
			}
			break;
		}
	}

	public static bool ConvertSpawnerAndSpawnableIndex(ref int uniqueSpawnerIndex, ref uint uniqueSpawnableIndex)
	{
		switch (uniqueSpawnerIndex)
		{
		case 0:
		case 1:
		case 5:
			return false;
		case 2:
			return true;
		case 3:
			if (uniqueSpawnableIndex == 1)
			{
				uniqueSpawnerIndex = 7;
				uniqueSpawnableIndex = 0u;
			}
			return true;
		case 4:
			if (uniqueSpawnableIndex == 2)
			{
				uniqueSpawnerIndex = 6;
				uniqueSpawnableIndex = 0u;
			}
			return true;
		default:
			return false;
		}
	}
}
