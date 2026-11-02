using System.Collections.Generic;
using UnityEngine;

public class RaftColliderPannkaka : MonoBehaviour
{
	[SerializeField]
	private Transform blockParent;

	[SerializeField]
	private Transform pannkaksParent;

	private RaftCollisionManager collManager;

	[SerializeField]
	private RaftBounds raftBounds;

	[SerializeField]
	private List<BlockData> allBlocks = new List<BlockData>();

	[SerializeField]
	private List<LineData> allLines = new List<LineData>();

	[SerializeField]
	private List<BlockData> mergedBlocks = new List<BlockData>();

	[SerializeField]
	private List<BlockData> unmergedBlocks = new List<BlockData>();

	[SerializeField]
	private List<MergeData> mergeDatas = new List<MergeData>();

	private int widthCount;

	private int lengthCount;

	private int heightCount;

	private void Start()
	{
		collManager = ComponentManager<RaftCollisionManager>.Value;
	}

	private void Update()
	{
	}

	public void GenerateCollision()
	{
		PreWarmAllBlocks();
		StartMergeCalculation();
	}

	private void PreWarmAllBlocks()
	{
		allBlocks.Clear();
		allLines.Clear();
		mergedBlocks.Clear();
		unmergedBlocks.Clear();
		mergeDatas.Clear();
		List<Vector3> blockPositions = collManager.GetBlockPositions();
		for (int i = 0; i < blockPositions.Count; i++)
		{
			BlockData blockData = new BlockData();
			blockData.pos = blockPositions[i];
			allBlocks.Add(blockData);
		}
		widthCount = (int)((float)Mathf.CeilToInt(raftBounds.Width) / 1.5f);
		lengthCount = (int)((float)Mathf.CeilToInt(raftBounds.Length) / 1.5f);
		heightCount = BlockCreator.FloorCount + 1;
	}

	public void StartMergeCalculation()
	{
		Vector3 bottomLeft = raftBounds.BottomLeft;
		bottomLeft = blockParent.InverseTransformPoint(bottomLeft);
		bottomLeft.y = 0f;
		bottomLeft += Vector3.forward * 0.75f + Vector3.right * 0.75f;
		CreateLines(bottomLeft);
		MergeLines(allLines);
	}

	private void CreateLines(Vector3 p_startPos)
	{
		for (int i = 0; i < lengthCount; i++)
		{
			LineData lineData = new LineData(new Vector3(1f, 0f));
			BlockData blockDataFromGridPos = GetBlockDataFromGridPos(p_startPos + Vector3.forward * 1.5f * i);
			lineData.startBlock = blockDataFromGridPos;
			if (lineData.startBlock != null)
			{
				CheckLine(lineData);
				allLines.Add(lineData);
			}
			else
			{
				lineData = null;
			}
		}
	}

	private void CheckLine(LineData p_Line)
	{
		for (int i = 0; i < widthCount; i++)
		{
			Vector3 pos = p_Line.startBlock.pos + p_Line.GetDirection() * 1.5f * i;
			BlockData blockDataFromGridPos = GetBlockDataFromGridPos(pos);
			if (blockDataFromGridPos != null)
			{
				blockDataFromGridPos.isInLine = true;
				p_Line.blocksInLine.Add(blockDataFromGridPos);
				continue;
			}
			p_Line.lastBlock = p_Line.blocksInLine[p_Line.blocksInLine.Count - 1];
			break;
		}
	}

	private void CreateLineCollider(LineData p_line)
	{
		BoxCollider boxCollider = pannkaksParent.gameObject.AddComponent<BoxCollider>();
		Vector3 zero = Vector3.zero;
		zero.y = 0.75f;
		zero.z = 1.5f;
		zero += p_line.GetDirection() * p_line.blocksInLine.Count * 1.5f;
		Vector3 position = (p_line.startBlock.pos + p_line.blocksInLine[p_line.blocksInLine.Count - 1].pos) / 2f;
		Vector3 position2 = blockParent.TransformPoint(position);
		boxCollider.size = zero;
		boxCollider.center = pannkaksParent.InverseTransformPoint(position2);
		p_line.lineCollider = boxCollider;
	}

	private void CreateMergeDataCollider(MergeData p_mergeData)
	{
		BoxCollider boxCollider = pannkaksParent.gameObject.AddComponent<BoxCollider>();
		Vector3 zero = Vector3.zero;
		zero.y = 0.75f;
		zero.x = (float)p_mergeData.averageLenght * 1.5f;
		zero.z = (float)p_mergeData.mergedLines.Count * 1.5f;
		boxCollider.size = zero;
		p_mergeData.mergeCollider = boxCollider;
	}

	private void MergeLines(List<LineData> p_lines)
	{
		int averageLineLenght = GetAverageLineLenght(p_lines);
		int p_longestLineLength = 0;
		int p_shortestLineLength = 9999;
		CheckLongestAndShortestLine(p_lines, ref p_longestLineLength, ref p_shortestLineLength);
		Debug.Log("averageLine" + averageLineLenght);
		MergeData mergeData = new MergeData(averageLineLenght);
		mergeDatas.Add(mergeData);
		for (int i = 0; i < p_lines.Count && p_lines[i].blocksInLine.Count >= averageLineLenght; i++)
		{
			mergeData.mergedLines.Add(p_lines[i]);
		}
		CreateMergeDataCollider(mergeData);
	}

	private int GetAverageLineLenght(List<LineData> p_lines)
	{
		int num = 0;
		int count = p_lines.Count;
		for (int i = 0; i < p_lines.Count; i++)
		{
			num += p_lines[i].blocksInLine.Count;
		}
		Debug.Log("Amount of blocks" + num + "calculated avergeLine" + num / count);
		return num / count;
	}

	private void CheckLongestAndShortestLine(List<LineData> p_lines, ref int p_longestLineLength, ref int p_shortestLineLength)
	{
		for (int i = 0; i < p_lines.Count; i++)
		{
			int count = p_lines[i].blocksInLine.Count;
			if (count > p_longestLineLength)
			{
				p_longestLineLength = count;
			}
			else if (count < p_shortestLineLength)
			{
				p_shortestLineLength = count;
			}
		}
	}

	private bool ContainsGridPos(Vector3 pos)
	{
		for (int i = 0; i < allBlocks.Count; i++)
		{
			if (Vector3.Distance(allBlocks[i].pos, pos) <= 0.05f)
			{
				return true;
			}
		}
		return false;
	}

	private BlockData GetBlockDataFromGridPos(Vector3 pos)
	{
		for (int i = 0; i < allBlocks.Count; i++)
		{
			if (Vector3.Distance(allBlocks[i].pos, pos) <= 0.05f)
			{
				return allBlocks[i];
			}
		}
		return null;
	}
}
