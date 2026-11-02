using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineData
{
	public BlockData startBlock;

	public BlockData lastBlock;

	private Vector3 dir;

	public BoxCollider lineCollider;

	public List<BlockData> blocksInLine;

	public LineData(Vector2 p_dir)
	{
		blocksInLine = new List<BlockData>();
		dir = p_dir;
	}

	public Vector3 GetDirection()
	{
		return dir;
	}
}
