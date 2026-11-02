using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockQuadType", menuName = "Raft/Block/BlockQuadType", order = 1)]
public class SO_BlockQuadType : ScriptableObject
{
	[SerializeField]
	private List<Item_Base> acceptableBlockTypes = new List<Item_Base>();

	public bool AcceptsBlock(Item_Base block)
	{
		if (block == null)
		{
			return false;
		}
		foreach (Item_Base acceptableBlockType in acceptableBlockTypes)
		{
			if (!(acceptableBlockType == null) && acceptableBlockType.UniqueIndex == block.UniqueIndex)
			{
				return true;
			}
		}
		return false;
	}
}
