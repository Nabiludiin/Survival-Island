using System;
using System.Collections.Generic;
using UnityEngine;

public class RaftCollisionManager : MonoBehaviour
{
	public bool forceSetY;

	[SerializeField]
	private Rigidbody body;

	[SerializeField]
	private Buoyancy buoyancy;

	[SerializeField]
	private RaftBounds raftBounds;

	[SerializeField]
	private Transform blockParent;

	[SerializeField]
	private string colliderPoolName = string.Empty;

	[SerializeField]
	private float foundationSizeY = 0.5f;

	[SerializeField]
	private float floorSizeY = 0.1f;

	private PoolManager poolManager;

	private Raft_Network network;

	private List<Vector3> blocks = new List<Vector3>();

	private List<BoxCollider> currentColldiers = new List<BoxCollider>();

	private void Awake()
	{
		ComponentManager<RaftCollisionManager>.Value = this;
	}

	private void Start()
	{
		network = ComponentManager<Raft_Network>.Value;
		poolManager = ComponentManager<PoolManager>.Value;
		if (network != null && Raft_Network.IsHost && this.raftBounds != null)
		{
			RaftBounds raftBounds = this.raftBounds;
			raftBounds.OnAddWalkableBlock = (Action<Block, bool>)Delegate.Combine(raftBounds.OnAddWalkableBlock, new Action<Block, bool>(OnAddWalkableBlock));
			RaftBounds raftBounds2 = this.raftBounds;
			raftBounds2.OnRemoveWalkableBlocks = (Action<List<Block>, bool>)Delegate.Combine(raftBounds2.OnRemoveWalkableBlocks, new Action<List<Block>, bool>(OnRemoveWalkableBlock));
		}
		body.freezeRotation = true;
		body.constraints = (RigidbodyConstraints)80;
	}

	private void Update()
	{
		if (Raft_Network.InMenuScene)
		{
			SetYPosition();
		}
		else if (((network != null && Raft_Network.IsHost) || forceSetY) && buoyancy.active)
		{
			SetYPosition();
		}
	}

	private void FixedUpdate()
	{
		FixBodyRotation();
	}

	public void FixBodyRotation()
	{
		if (Raft_Network.IsHost)
		{
			Vector3 eulerAngles = body.rotation.eulerAngles;
			if (Mathf.Abs(eulerAngles.x) > 0.2f || Mathf.Abs(eulerAngles.z) > 0.2f)
			{
				body.rotation = Quaternion.Euler(0f, eulerAngles.y, 0f);
			}
		}
	}

	private void OnDestroy()
	{
		if (network != null && Raft_Network.IsHost && this.raftBounds != null)
		{
			RaftBounds raftBounds = this.raftBounds;
			raftBounds.OnAddWalkableBlock = (Action<Block, bool>)Delegate.Remove(raftBounds.OnAddWalkableBlock, new Action<Block, bool>(OnAddWalkableBlock));
			RaftBounds raftBounds2 = this.raftBounds;
			raftBounds2.OnRemoveWalkableBlocks = (Action<List<Block>, bool>)Delegate.Remove(raftBounds2.OnRemoveWalkableBlocks, new Action<List<Block>, bool>(OnRemoveWalkableBlock));
		}
	}

	private void OnLoadComplete()
	{
		UpdateGrid();
	}

	public void Initialize()
	{
		if (Raft_Network.IsHost)
		{
			UpdateGrid();
		}
	}

	private void UpdateGrid()
	{
		if (!Raft_Network.IsHost)
		{
			return;
		}
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			PoolManager.ReturnObject(transform.GetChild(0).gameObject);
		}
		if (!GameModeValueManager.GetCurrentGameModeValue().raftSpecificVariables.handleRaftCollision)
		{
			return;
		}
		currentColldiers.Clear();
		Vector3 bottomLeft = raftBounds.BottomLeft;
		bottomLeft = blockParent.InverseTransformPoint(bottomLeft);
		bottomLeft.y = 0f;
		bottomLeft += Vector3.forward * 0.75f + Vector3.right * 0.75f;
		int num = (int)((float)Mathf.CeilToInt(raftBounds.Width) / 1.5f);
		int lengthCount = (int)((float)Mathf.CeilToInt(raftBounds.Length) / 1.5f);
		int num2 = BlockCreator.FloorCount + 1;
		if (num2 > 10)
		{
			num2 = 10;
		}
		for (int j = 0; j < num2; j++)
		{
			Vector3 startPosition = bottomLeft;
			for (int k = 0; k < num; k++)
			{
				GenerateColliderAbove(startPosition, lengthCount, j);
				startPosition += Vector3.right * 1.5f;
			}
			bottomLeft += Vector3.up * 1.21f;
		}
	}

	private void UpdateBoxLineFromBuildPos(Vector3 localBuildPos)
	{
		List<BoxCollider> colliderFromPos = GetColliderFromPos(localBuildPos);
		if (colliderFromPos != null)
		{
			foreach (BoxCollider item in colliderFromPos)
			{
				currentColldiers.Remove(item);
				PoolManager.ReturnObject(item.gameObject);
			}
		}
		Vector3 bottomLeft = raftBounds.BottomLeft;
		bottomLeft = blockParent.InverseTransformPoint(bottomLeft);
		bottomLeft.x = localBuildPos.x;
		bottomLeft.y = 0f;
		bottomLeft.z += 0.75f;
		BlockCreator.SnapBuildPosition(ref bottomLeft);
		int lengthCount = (int)((float)Mathf.CeilToInt(raftBounds.Length) / 1.5f);
		int floor = (int)((float)Mathf.RoundToInt(localBuildPos.y / 1.21f) * 1.21f);
		GenerateColliderAbove(bottomLeft, lengthCount, floor);
	}

	private void SetYPosition()
	{
		Vector3 position = body.position;
		position.y = buoyancy.CenterPointY;
		body.MovePosition(position);
	}

	private bool ContainsGridPos(Vector3 pos)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			if (Vector3.Distance(blocks[i], pos) <= 0.05f)
			{
				return true;
			}
		}
		return false;
	}

	private bool RemoveGridPos(Vector3 pos)
	{
		bool result = false;
		for (int i = 0; i < blocks.Count; i++)
		{
			if (Vector3.Distance(blocks[i], pos) <= 0.05f)
			{
				blocks.RemoveAt(i);
				i--;
				result = true;
			}
		}
		return result;
	}

	private void GenerateColliderAbove(Vector3 startPosition, int lengthCount, int floor)
	{
		int num = 0;
		startPosition.y = (float)floor * 1.21f;
		Vector3 vector = startPosition;
		Vector3 vector2 = Vector3.positiveInfinity;
		for (int i = 0; i < lengthCount + 1; i++)
		{
			if (ContainsGridPos(vector))
			{
				num++;
				if (num == 1)
				{
					vector2 = vector;
				}
			}
			else if (num >= 1)
			{
				GameObject gameObject = poolManager.GetObject(colliderPoolName, returnObjectActive: false);
				BoxCollider component = gameObject.GetComponent<BoxCollider>();
				float num2 = ((floor == 0) ? foundationSizeY : floorSizeY);
				float num3 = (float)num * 1.5f;
				Vector3 localPosition = vector2 + Vector3.forward * (num3 / 2f - 0.75f);
				component.size = new Vector3(1.5f, num2, num3);
				component.center = new Vector3(0f, (0f - num2) / 2f, 0f);
				gameObject.transform.SetParent(transform);
				gameObject.transform.localPosition = localPosition;
				gameObject.transform.localEulerAngles = Vector3.zero;
				gameObject.SetActive(value: true);
				currentColldiers.Add(component);
				num = 0;
			}
			else
			{
				num = 0;
			}
			vector += Vector3.forward * 1.5f;
		}
	}

	private List<BoxCollider> GetColliderFromPos(Vector3 localPos)
	{
		List<BoxCollider> list = null;
		foreach (BoxCollider currentColldier in currentColldiers)
		{
			Vector3 position = currentColldier.transform.localPosition;
			BlockCreator.SnapBuildPosition(ref position);
			if (position.x == localPos.x && position.y == localPos.y)
			{
				if (list == null)
				{
					list = new List<BoxCollider>();
				}
				list.Add(currentColldier);
			}
		}
		return list;
	}

	private void OnAddWalkableBlock(Block block, bool replicating)
	{
		if (block == null)
		{
			return;
		}
		Vector3 position = block.transform.localPosition;
		BlockCreator.SnapBuildPosition(ref position);
		if (!ContainsGridPos(position))
		{
			blocks.Add(position);
			if (!replicating && !SaveAndLoad.IsGameLoading)
			{
				UpdateBoxLineFromBuildPos(position);
			}
		}
	}

	private void OnRemoveWalkableBlock(List<Block> removedWalkables, bool replicating)
	{
		if (removedWalkables == null)
		{
			return;
		}
		_ = Time.realtimeSinceStartup;
		bool flag = false;
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < removedWalkables.Count; i++)
		{
			Vector3 position = removedWalkables[i].transform.localPosition;
			BlockCreator.SnapBuildPosition(ref position);
			if (RemoveGridPos(position))
			{
				list.Add(position);
				flag = true;
			}
		}
		if (flag && !replicating)
		{
			for (int j = 0; j < list.Count; j++)
			{
				UpdateBoxLineFromBuildPos(list[j]);
			}
		}
	}

	public List<Vector3> GetBlockPositions()
	{
		return blocks;
	}
}
