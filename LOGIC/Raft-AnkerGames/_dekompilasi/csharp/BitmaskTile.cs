using System;
using System.Collections.Generic;
using UnityEngine;

public class BitmaskTile : MonoBehaviour
{
	public Action OnMeshChange;

	public Action OnCompleteVisual;

	[SerializeField]
	private TileBitmaskType bitmaskType;

	public Axis axisConnection = Axis.All;

	[SerializeField]
	public Transform rotationTransform;

	public MeshFilter[] meshFilters;

	[SerializeField]
	private bool useRaycastNeighbourCheck;

	[SerializeField]
	private string acceptableCollisionTag = "Untagged";

	public int currentBitmaskValue;

	public bool semiCompleteVisual;

	public TileBitmaskType BitmaskType => bitmaskType;

	public BitmaskTile[] GetNeighbours()
	{
		BitmaskTile[] array = new BitmaskTile[6];
		Vector3 vector = BlockCreator.WorldToLocalLockedPivot(transform.position);
		BitmaskTile bitmaskTile = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector + Vector3.forward * 1.5f);
		BitmaskTile bitmaskTile2 = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector - Vector3.right * 1.5f);
		BitmaskTile bitmaskTile3 = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector + Vector3.right * 1.5f);
		BitmaskTile bitmaskTile4 = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector - Vector3.forward * 1.5f);
		BitmaskTile bitmaskTile5 = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector + Vector3.up * 1.21f);
		BitmaskTile bitmaskTile6 = TileBitmaskManager.TileExistAtPosition(BitmaskType, vector - Vector3.up * 1.21f);
		if (!CanConnectToBitmaskTile(bitmaskTile))
		{
			bitmaskTile = null;
		}
		if (!CanConnectToBitmaskTile(bitmaskTile2))
		{
			bitmaskTile2 = null;
		}
		if (!CanConnectToBitmaskTile(bitmaskTile3))
		{
			bitmaskTile3 = null;
		}
		if (!CanConnectToBitmaskTile(bitmaskTile4))
		{
			bitmaskTile4 = null;
		}
		if (!CanConnectToBitmaskTile(bitmaskTile5))
		{
			bitmaskTile5 = null;
		}
		if (!CanConnectToBitmaskTile(bitmaskTile6))
		{
			bitmaskTile6 = null;
		}
		if (bitmaskTile != null && bitmaskTile.axisConnection != Axis.All && !bitmaskTile.CanConnectToBitmaskTile(this))
		{
			bitmaskTile = null;
		}
		if (bitmaskTile4 != null && bitmaskTile4.axisConnection != Axis.All && !bitmaskTile4.CanConnectToBitmaskTile(this))
		{
			bitmaskTile4 = null;
		}
		if (bitmaskTile2 != null && bitmaskTile2.axisConnection != Axis.All && !bitmaskTile2.CanConnectToBitmaskTile(this))
		{
			bitmaskTile2 = null;
		}
		if (bitmaskTile3 != null && bitmaskTile3.axisConnection != Axis.All && !bitmaskTile3.CanConnectToBitmaskTile(this))
		{
			bitmaskTile3 = null;
		}
		if (bitmaskTile != null && bitmaskTile.axisConnection != Axis.All && !bitmaskTile.CanConnectToBitmaskTile(this))
		{
			bitmaskTile = null;
		}
		if (bitmaskTile4 != null && bitmaskTile4.axisConnection != Axis.All && !bitmaskTile4.CanConnectToBitmaskTile(this))
		{
			bitmaskTile4 = null;
		}
		array[0] = bitmaskTile;
		array[1] = bitmaskTile2;
		array[2] = bitmaskTile3;
		array[3] = bitmaskTile4;
		array[4] = bitmaskTile5;
		array[5] = bitmaskTile6;
		return array;
	}

	public bool ExcludeOutOfSightNeighbours(ref BitmaskTile[] neighbours)
	{
		bool result = false;
		if (useRaycastNeighbourCheck)
		{
			if (neighbours[0] != null && !HasVisionToBitmaskTile(neighbours[0]))
			{
				neighbours[0] = null;
				result = true;
			}
			if (neighbours[1] != null && !HasVisionToBitmaskTile(neighbours[1]))
			{
				neighbours[1] = null;
				result = true;
			}
			if (neighbours[2] != null && !HasVisionToBitmaskTile(neighbours[2]))
			{
				neighbours[2] = null;
				result = true;
			}
			if (neighbours[3] != null && !HasVisionToBitmaskTile(neighbours[3]))
			{
				neighbours[3] = null;
				result = true;
			}
			if (neighbours[4] != null && !HasVisionToBitmaskTile(neighbours[4]))
			{
				neighbours[4] = null;
				result = true;
			}
			if (neighbours[5] != null && !HasVisionToBitmaskTile(neighbours[5]))
			{
				neighbours[5] = null;
				result = true;
			}
		}
		return result;
	}

	public void FloodFillOutdatedVisuals(List<BitmaskTile> touchedBitmaskTiles)
	{
		if (touchedBitmaskTiles.Contains(this))
		{
			return;
		}
		touchedBitmaskTiles.Add(this);
		Refresh(refreshNeighbour: false);
		BitmaskTile[] neighbours = GetNeighbours();
		foreach (BitmaskTile bitmaskTile in neighbours)
		{
			if (bitmaskTile != null && bitmaskTile.semiCompleteVisual)
			{
				bitmaskTile.FloodFillOutdatedVisuals(touchedBitmaskTiles);
			}
		}
	}

	public void Refresh(bool refreshNeighbour)
	{
		BitmaskTile[] neighbours = GetNeighbours();
		bool flag = ExcludeOutOfSightNeighbours(ref neighbours);
		if (!flag && semiCompleteVisual && OnCompleteVisual != null)
		{
			OnCompleteVisual();
		}
		semiCompleteVisual = flag;
		currentBitmaskValue = GetBitmaskValueFromNeighbours(neighbours);
		if (refreshNeighbour)
		{
			RefreshNeighbours();
		}
		TileBitmaskConnection bitmaskConnectionFromBitmaskValue = TileBitmaskManager.GetBitmaskConnectionFromBitmaskValue(BitmaskType, currentBitmaskValue);
		AssignMeshesFromConnection(bitmaskConnectionFromBitmaskValue);
	}

	public void RefreshNeighbours()
	{
		BitmaskTile[] neighbours = GetNeighbours();
		RefreshNeighbours(neighbours);
	}

	private void RefreshNeighbours(BitmaskTile[] neighbours)
	{
		if (neighbours == null)
		{
			return;
		}
		List<BitmaskTile> list = null;
		foreach (BitmaskTile bitmaskTile in neighbours)
		{
			if (!(bitmaskTile != null))
			{
				continue;
			}
			if (bitmaskTile.semiCompleteVisual)
			{
				if (list == null)
				{
					list = new List<BitmaskTile> { this };
				}
				bitmaskTile.FloodFillOutdatedVisuals(list);
			}
			else
			{
				bitmaskTile.Refresh(refreshNeighbour: false);
			}
		}
	}

	private bool CanConnectToBitmaskTile(BitmaskTile otherBitmaskTile)
	{
		if (otherBitmaskTile == null)
		{
			return false;
		}
		if (axisConnection == Axis.All)
		{
			return true;
		}
		if (axisConnection == Axis.None)
		{
			return false;
		}
		Vector3 position = BlockCreator.WorldToLocalLockedPivot(otherBitmaskTile.transform.position);
		Vector3 position2 = BlockCreator.WorldToLocalLockedPivot(transform.position);
		BlockCreator.SnapBuildPosition(ref position);
		BlockCreator.SnapBuildPosition(ref position2);
		Vector3 vector = position - position2;
		Transform lockedPivot = SingletonGeneric<GameManager>.Singleton.lockedPivot;
		Vector3 vector2 = Vector3.zero;
		switch (axisConnection)
		{
		case Axis.X:
			vector2 = lockedPivot.InverseTransformDirection(transform.right).RoundToDividable(0.5f);
			break;
		case Axis.NX:
			vector2 = lockedPivot.InverseTransformDirection(-transform.right).RoundToDividable(0.5f);
			break;
		case Axis.Y:
			vector2 = lockedPivot.InverseTransformDirection(transform.up).RoundToDividable(0.5f);
			break;
		case Axis.NY:
			vector2 = lockedPivot.InverseTransformDirection(-transform.up).RoundToDividable(0.5f);
			break;
		case Axis.Z:
			vector2 = lockedPivot.InverseTransformDirection(transform.forward).RoundToDividable(0.5f);
			break;
		case Axis.NZ:
			vector2 = lockedPivot.InverseTransformDirection(-transform.forward).RoundToDividable(0.5f);
			break;
		}
		return vector2.normalized == vector.normalized;
	}

	private int GetBitmaskValueFromNeighbours(BitmaskTile[] neighbours)
	{
		BitmaskTile obj = neighbours[0];
		BitmaskTile bitmaskTile = neighbours[1];
		BitmaskTile bitmaskTile2 = neighbours[2];
		BitmaskTile bitmaskTile3 = neighbours[3];
		BitmaskTile bitmaskTile4 = neighbours[4];
		BitmaskTile bitmaskTile5 = neighbours[5];
		int num = 0;
		if (obj != null)
		{
			num++;
		}
		if (bitmaskTile != null)
		{
			num += 2;
		}
		if (bitmaskTile2 != null)
		{
			num += 4;
		}
		if (bitmaskTile3 != null)
		{
			num += 8;
		}
		if (bitmaskTile4 != null)
		{
			num += 16;
		}
		if (bitmaskTile5 != null)
		{
			num += 32;
		}
		return num;
	}

	private void AssignMeshesFromConnection(TileBitmaskConnection bitmaskConnection)
	{
		if (bitmaskConnection == null || !meshFilters.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < bitmaskConnection.meshes.Length; i++)
		{
			if (i >= 0 && i < meshFilters.Length && bitmaskConnection.meshes[i] != null && meshFilters[i] != null)
			{
				meshFilters[i].sharedMesh = bitmaskConnection.meshes[i];
			}
		}
		if (rotationTransform != null)
		{
			rotationTransform.localEulerAngles = bitmaskConnection.eulerRotation;
		}
		if (OnMeshChange != null)
		{
			OnMeshChange();
		}
	}

	private bool HasVisionToBitmaskTile(BitmaskTile otherTile)
	{
		if (!otherTile.useRaycastNeighbourCheck)
		{
			return true;
		}
		if (otherTile != null)
		{
			Vector3 direction = otherTile.transform.position - transform.position;
			float maxDistance = direction.magnitude * 0.95f;
			if (Physics.Raycast(transform.position, direction, out var hitInfo, maxDistance, LayerMasks.MASK_Block, QueryTriggerInteraction.Ignore) && !hitInfo.transform.tag.Equals(acceptableCollisionTag))
			{
				return false;
			}
			return true;
		}
		return false;
	}
}
