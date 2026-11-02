using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class BlockCollisionConsolidator : MonoBehaviour
{
	private class MeshBaker
	{
		public int meshID;

		public bool failed;

		public void BakeCollisionMesh()
		{
			try
			{
				Physics.BakeMesh(meshID, convex: false);
			}
			catch (Exception)
			{
				failed = true;
			}
		}
	}

	private struct SubMesh
	{
		public int verticesStart;

		public int indicesStart => verticesStart / 8 * CollisionConverting.CUBE_INDECIS.Length;
	}

	private const float SizeXZ = 500f;

	private const float SizeY = 200f;

	private const float MinY = -1f;

	private const float CellSize = 8f;

	private const int XIndexOffset = 1;

	private const int YIndexOffset = 500;

	private const int ZIndexOffset = 100000;

	private static readonly Vector3 SpaceSize = new Vector3(500f, 200f, 500f);

	private static readonly Vector3 SpaceOffset = new Vector3(-250f, -1f, -250f);

	private static readonly Vector3 SpaceABS = SpaceSize * 8f;

	private static readonly Vector3 HalfCell = new Vector3(4f, 4f, 4f);

	private static int[] CubeIndices;

	private Dictionary<int, List<Block>> blockMap = new Dictionary<int, List<Block>>();

	private Dictionary<Block, BoxCollider[]> queuedBlocks = new Dictionary<Block, BoxCollider[]>();

	private Dictionary<Block, int> blockToIndex = new Dictionary<Block, int>();

	private Dictionary<int, Block> indexToBlock = new Dictionary<int, Block>();

	private List<SubMesh> blockMeshList = new List<SubMesh>();

	private List<Vector3> vertices = new List<Vector3>();

	private List<int> indices = new List<int>();

	private List<BoxCollider> collidersToDeactivate = new List<BoxCollider>();

	private List<int> lastActiveBlocks = new List<int>();

	private List<int> newActiveBlocksCache = new List<int>();

	private Dictionary<int, int> tempActivatedCells = new Dictionary<int, int>();

	private List<BoxCollider> tempActivatedBlocks = new List<BoxCollider>();

	private HashSet<int> duplicateFilterCashed = new HashSet<int>();

	private Mesh bakingCollisionMesh;

	private MeshCollider meshCollider;

	private Network_Player localPlayer;

	private int lastPlayerSpatialIndex;

	private bool drawAllCollidersState = true;

	private MeshBaker bakerObject;

	private Thread bakeThread;

	private Queue<(Thread, Mesh)> discardedBakeThreads = new Queue<(Thread, Mesh)>();

	public bool isDirty { get; set; }

	private void Awake()
	{
		CubeIndices = new int[27];
		CubeIndices[0] = 0;
		CubeIndices[1] = 1;
		CubeIndices[2] = -1;
		CubeIndices[3] = -500;
		CubeIndices[4] = 500;
		CubeIndices[5] = CubeIndices[3] - 1;
		CubeIndices[6] = CubeIndices[4] - 1;
		CubeIndices[7] = CubeIndices[3] + 1;
		CubeIndices[8] = CubeIndices[4] + 1;
		for (int i = 0; i < 9; i++)
		{
			CubeIndices[i + 9] = CubeIndices[i];
			CubeIndices[i + 18] = CubeIndices[i];
		}
		Action<int, int> action = (int startIndex, int zOffset) =>
		{
			for (int j = 0; j < 9; j++)
			{
				CubeIndices[startIndex + j] += zOffset * 100000;
			}
		};
		action(9, -1);
		action(18, 1);
	}

	private void OnEnable()
	{
		ComponentManager<BlockCollisionConsolidator>.Value = this;
		localPlayer = ComponentManager<Network_Player>.Value;
		meshCollider = gameObject.AddComponent<MeshCollider>();
		List<Block> placedBlocks = BlockCreator.GetPlacedBlocks();
		for (int i = 0; i < placedBlocks.Count; i++)
		{
			AddBlock(placedBlocks[i]);
		}
		isDirty = true;
	}

	private void OnDisable()
	{
		foreach (KeyValuePair<int, List<Block>> item in blockMap)
		{
			for (int i = 0; i < item.Value.Count; i++)
			{
				for (int j = 0; j < item.Value[i].blockColliders.Length; j++)
				{
					item.Value[i].blockColliders[j].enabled = true;
				}
			}
		}
		if (meshCollider != null)
		{
			UnityEngine.Object.Destroy(meshCollider.sharedMesh);
			meshCollider.sharedMesh = null;
		}
		blockMap.Clear();
		queuedBlocks.Clear();
		blockToIndex.Clear();
		indexToBlock.Clear();
		blockMeshList.Clear();
		vertices.Clear();
		indices.Clear();
		collidersToDeactivate.Clear();
		lastActiveBlocks.Clear();
		newActiveBlocksCache.Clear();
		tempActivatedCells.Clear();
		tempActivatedBlocks.Clear();
		ComponentManager<BlockCollisionConsolidator>.Value = null;
		localPlayer = null;
	}

	private void Update()
	{
		try
		{
			for (int i = 0; i < tempActivatedBlocks.Count; i++)
			{
				if (tempActivatedBlocks[i] != null)
				{
					tempActivatedBlocks[i].enabled = false;
				}
			}
			tempActivatedBlocks.Clear();
			UpdateActiveCollision();
			duplicateFilterCashed.Clear();
			foreach (KeyValuePair<int, int> tempActivatedCell in tempActivatedCells)
			{
				if (tempActivatedCell.Value > 0)
				{
					TempActivateCellAndNeighbours(tempActivatedCell.Key, duplicateFilterCashed);
				}
			}
			duplicateFilterCashed.Clear();
			while (discardedBakeThreads.Count > 0 && !discardedBakeThreads.Peek().Item1.IsAlive)
			{
				(Thread, Mesh) tuple = discardedBakeThreads.Dequeue();
				if (tuple.Item2 != null)
				{
					UnityEngine.Object.Destroy(tuple.Item2);
				}
			}
			if (isDirty)
			{
				foreach (KeyValuePair<Block, BoxCollider[]> queuedBlock in queuedBlocks)
				{
					int verticesStart = 0;
					for (int j = 0; j < queuedBlock.Value.Length; j++)
					{
						verticesStart = vertices.Count;
						CollisionConverting.ConstructMeshFromBox(vertices, indices, queuedBlock.Value[j], transform);
						collidersToDeactivate.Add(queuedBlock.Value[j]);
					}
					int count = blockMeshList.Count;
					blockMeshList.Add(new SubMesh
					{
						verticesStart = verticesStart
					});
					blockToIndex.Add(queuedBlock.Key, count);
					indexToBlock.Add(count, queuedBlock.Key);
				}
				queuedBlocks.Clear();
				if (bakeThread != null && bakeThread.IsAlive)
				{
					discardedBakeThreads.Enqueue((bakeThread, bakingCollisionMesh));
				}
				bakingCollisionMesh = new Mesh();
				bakingCollisionMesh.indexFormat = IndexFormat.UInt32;
				bakingCollisionMesh.SetVertices(vertices);
				bakingCollisionMesh.SetTriangles(indices, 0);
				bakingCollisionMesh.RecalculateBounds();
				bakerObject = new MeshBaker
				{
					meshID = bakingCollisionMesh.GetInstanceID()
				};
				ThreadStart start = bakerObject.BakeCollisionMesh;
				bakeThread = new Thread(start);
				bakeThread.Start();
				isDirty = false;
			}
			if (bakeThread != null && !bakeThread.IsAlive && bakingCollisionMesh != null)
			{
				if (bakerObject.failed)
				{
					Debug.LogError("BlockCollisionConsolidator thread failed, disableing until next restart");
					enabled = false;
					isDirty = false;
				}
				else
				{
					if (meshCollider.sharedMesh != null)
					{
						UnityEngine.Object.Destroy(meshCollider.sharedMesh);
					}
					meshCollider.sharedMesh = bakingCollisionMesh;
					bakingCollisionMesh = null;
					for (int k = 0; k < collidersToDeactivate.Count; k++)
					{
						if (!(collidersToDeactivate[k] != null))
						{
							continue;
						}
						Vector3 position = transform.InverseTransformPoint(collidersToDeactivate[k].transform.position);
						(bool, int) cellIndex = GetCellIndex(position);
						bool flag = false;
						for (int l = 0; l < CubeIndices.Length; l++)
						{
							if (cellIndex.Item2 == lastPlayerSpatialIndex + CubeIndices[l])
							{
								flag = true;
								break;
							}
						}
						foreach (KeyValuePair<int, int> tempActivatedCell2 in tempActivatedCells)
						{
							if (tempActivatedCell2.Value > 0)
							{
								for (int m = 0; m < CubeIndices.Length; m++)
								{
									int num = tempActivatedCell2.Key + CubeIndices[m];
									if (cellIndex.Item2 == num)
									{
										flag = true;
										break;
									}
								}
							}
							if (flag)
							{
								break;
							}
						}
						collidersToDeactivate[k].enabled = flag;
					}
					collidersToDeactivate.Clear();
				}
			}
		}
		catch (Exception arg)
		{
			Debug.LogError($"BlockCollisionConsolidator failed, disableing until next restart: {arg}");
			enabled = false;
			isDirty = false;
		}
		Physics.SyncTransforms();
	}

	public void AddBlock(Block block)
	{
		if (blockToIndex.ContainsKey(block))
		{
			Debug.LogError($"ContainsKey {block}", block);
		}
		Vector3 position = transform.InverseTransformPoint(block.transform.position);
		(bool, int) cellIndex = GetCellIndex(position);
		if (cellIndex.Item1)
		{
			queuedBlocks.Add(block, block.blockColliders);
			if (!blockMap.TryGetValue(cellIndex.Item2, out var value))
			{
				value = new List<Block>();
				blockMap.Add(cellIndex.Item2, value);
			}
			value.Add(block);
			isDirty = true;
		}
	}

	public void RemoveBlock(Block removingBlock)
	{
		if (blockToIndex.ContainsKey(removingBlock))
		{
			Vector3 position = transform.InverseTransformPoint(removingBlock.transform.position);
			(bool, int) cellIndex = GetCellIndex(position);
			if (blockMap.TryGetValue(cellIndex.Item2, out var value))
			{
				value.Remove(removingBlock);
			}
			int num = blockMeshList.Count - 1;
			SubMesh subMesh = blockMeshList[blockToIndex[removingBlock]];
			SubMesh subMesh2 = blockMeshList[num];
			for (int i = 0; i < 8; i++)
			{
				vertices[subMesh.verticesStart + i] = vertices[subMesh2.verticesStart + i];
			}
			vertices.RemoveRange(subMesh2.verticesStart, 8);
			indices.RemoveRange(subMesh2.indicesStart, CollisionConverting.CUBE_INDECIS.Length);
			Block block = indexToBlock[num];
			if (removingBlock != block)
			{
				blockToIndex[block] = blockToIndex[removingBlock];
				indexToBlock[blockToIndex[block]] = block;
			}
			blockToIndex.Remove(removingBlock);
			indexToBlock.Remove(num);
			blockMeshList.RemoveAt(num);
			isDirty = true;
		}
	}

	public int TempActivateCellAndNeighbours(Vector3 cellPosition)
	{
		Vector3 position = transform.InverseTransformPoint(cellPosition);
		(bool, int) cellIndex = GetCellIndex(position);
		if (!tempActivatedCells.ContainsKey(cellIndex.Item2))
		{
			tempActivatedCells.Add(cellIndex.Item2, 0);
		}
		tempActivatedCells[cellIndex.Item2]++;
		Update();
		return cellIndex.Item2;
	}

	public void RemoveTempActivate(int cellIndex)
	{
		tempActivatedCells[cellIndex] = Mathf.Max(tempActivatedCells[cellIndex] - 1, 0);
	}

	public static (bool, int) PositionToCellIndex(Vector3 position, Vector3 spaceSize, Vector3 absSpaceSize, float cellSize, Vector3 globalOffset)
	{
		position -= globalOffset;
		bool item = true;
		if (position.x < 0f || position.y < 0f || position.z < 0f)
		{
			item = false;
		}
		if (position.x > absSpaceSize.x || position.y > absSpaceSize.y || position.z > absSpaceSize.z)
		{
			item = false;
		}
		int num = (int)(Mathf.Floor(position.z / cellSize) * (spaceSize.x * spaceSize.y));
		int num2 = (int)(Mathf.Floor(position.y / cellSize) * spaceSize.x);
		return (item, (int)Mathf.Floor(position.x / cellSize) + num + num2);
	}

	public static Vector3 CellIndexToPosition(int index, Vector3 spaceSize, float cellSize, Vector3 globalOffset)
	{
		int num = (int)(spaceSize.x * spaceSize.y);
		float num2 = Mathf.Floor(index / num);
		float num3 = num2 * (float)num;
		float num4 = Mathf.Floor(((float)index - num3) / spaceSize.x);
		float num5 = num4 * spaceSize.x;
		return new Vector3(Mathf.Floor(((float)index - (num3 + num5)) % spaceSize.x), num4, num2) * cellSize + globalOffset;
	}

	private (bool, int) GetCellIndex(Vector3 position)
	{
		return PositionToCellIndex(position, SpaceSize, SpaceABS, 8f, SpaceOffset);
	}

	private Vector3 GetCellPosition(int index)
	{
		return CellIndexToPosition(index, SpaceSize, 8f, SpaceOffset);
	}

	private void TempActivateCellAndNeighbours(int cellIndex, HashSet<int> duplicateFilter)
	{
		for (int i = 0; i < CubeIndices.Length; i++)
		{
			int num = cellIndex + CubeIndices[i];
			if (!duplicateFilter.Add(num) || !blockMap.TryGetValue(num, out var value))
			{
				continue;
			}
			for (int num2 = value.Count - 1; num2 >= 0; num2--)
			{
				if (value[num2] == null)
				{
					value.RemoveAt(num2);
				}
				else
				{
					for (int j = 0; j < value[num2].blockColliders.Length; j++)
					{
						if (!value[num2].blockColliders[j].enabled)
						{
							tempActivatedBlocks.Add(value[num2].blockColliders[j]);
						}
						value[num2].blockColliders[j].enabled = true;
					}
				}
			}
		}
	}

	private void UpdateActiveCollision()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
			return;
		}
		Vector3 position = transform.InverseTransformPoint(localPlayer.CameraTransform.position);
		(bool, int) cellIndex = GetCellIndex(position);
		if (cellIndex.Item2 == lastPlayerSpatialIndex)
		{
			return;
		}
		for (int i = 0; i < CubeIndices.Length; i++)
		{
			lastActiveBlocks.Add(lastPlayerSpatialIndex + CubeIndices[i]);
		}
		for (int j = 0; j < CubeIndices.Length; j++)
		{
			newActiveBlocksCache.Add(cellIndex.Item2 + CubeIndices[j]);
		}
		for (int k = 0; k < lastActiveBlocks.Count; k++)
		{
			if (!newActiveBlocksCache.Contains(lastActiveBlocks[k]) && blockMap.TryGetValue(lastActiveBlocks[k], out var value))
			{
				SetBlocksActive(value, active: false);
			}
		}
		for (int l = 0; l < newActiveBlocksCache.Count; l++)
		{
			if (!lastActiveBlocks.Contains(newActiveBlocksCache[l]) && blockMap.TryGetValue(newActiveBlocksCache[l], out var value2))
			{
				SetBlocksActive(value2, active: true);
			}
		}
		lastActiveBlocks.Clear();
		newActiveBlocksCache.Clear();
		lastPlayerSpatialIndex = cellIndex.Item2;
	}

	private void SetBlocksActive(List<Block> blocks, bool active)
	{
		for (int num = blocks.Count - 1; num >= 0; num--)
		{
			if (blocks[num] == null)
			{
				blocks.RemoveAt(num);
			}
			else
			{
				SetBlockActive(blocks[num], active);
			}
		}
	}

	private void SetBlockActive(Block block, bool active)
	{
		for (int i = 0; i < block.blockColliders.Length; i++)
		{
			block.blockColliders[i].enabled = active;
		}
	}

	private void OnDrawGizmos()
	{
		if (!localPlayer)
		{
			return;
		}
		if (drawAllCollidersState)
		{
			Gizmos.color = Color.green;
			foreach (KeyValuePair<int, List<Block>> item in blockMap)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					if (item.Value[i] == null)
					{
						continue;
					}
					for (int j = 0; j < item.Value[i].blockColliders.Length; j++)
					{
						if (!(item.Value[i].blockColliders[j] == null) && item.Value[i].blockColliders[j].enabled)
						{
							Gizmos.DrawSphere(item.Value[i].blockColliders[j].transform.position, 0.1f);
						}
					}
				}
			}
			Gizmos.color = Color.red;
			{
				foreach (KeyValuePair<int, List<Block>> item2 in blockMap)
				{
					for (int k = 0; k < item2.Value.Count; k++)
					{
						if (item2.Value[k] == null)
						{
							continue;
						}
						for (int l = 0; l < item2.Value[k].blockColliders.Length; l++)
						{
							if (!(item2.Value[k].blockColliders[l] == null) && !item2.Value[k].blockColliders[l].enabled)
							{
								Gizmos.DrawSphere(item2.Value[k].blockColliders[l].transform.position, 0.1f);
							}
						}
					}
				}
				return;
			}
		}
		Vector3 position = transform.InverseTransformPoint(localPlayer.CameraTransform.position);
		(bool, int) cellIndex = GetCellIndex(position);
		if (cellIndex.Item1)
		{
			Gizmos.color = Color.magenta;
			Vector3 cellPosition = GetCellPosition(cellIndex.Item2);
			cellPosition = transform.TransformPoint(cellPosition);
			Gizmos.DrawWireCube(cellPosition + HalfCell, new Vector3(8f, 8f, 8f));
		}
	}

	[ContextMenu("Draw All Colliders State")]
	private void DrawAllCollidersState()
	{
		drawAllCollidersState = !drawAllCollidersState;
	}
}
