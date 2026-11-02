using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainMenuRaft
{
	public string creatorName;

	public GameObject raftParent;

	public Transform chairTransform;

	public List<Block> walkableBlocks = new List<Block>();

	public void FindBlocks()
	{
		walkableBlocks.Clear();
		Block[] componentsInChildren = raftParent.GetComponentsInChildren<Block>(includeInactive: true);
		foreach (Block block in componentsInChildren)
		{
			if (block != null && block.IsWalkable())
			{
				walkableBlocks.Add(block);
			}
		}
	}

	public bool IsValid()
	{
		if (raftParent != null)
		{
			return chairTransform != null;
		}
		return false;
	}
}
