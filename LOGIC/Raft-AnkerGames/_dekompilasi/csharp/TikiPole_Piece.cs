using System;
using System.Collections.Generic;
using UnityEngine;

public class TikiPole_Piece : MonoBehaviour
{
	public enum TIKIPIECEINDEX
	{
		FEET,
		MID1,
		MID2,
		HEAD
	}

	public Block block;

	[SerializeField]
	private TIKIPIECEINDEX tikiPoleIndex;

	protected bool DoesOrderMatch(TikiPole_Piece[] tikiPoles, TIKIPIECEINDEX[] targetTikiPoleStructure)
	{
		if (tikiPoles.Length != targetTikiPoleStructure.Length)
		{
			return false;
		}
		for (int i = 0; i < tikiPoles.Length; i++)
		{
			TikiPole_Piece tikiPole_Piece = tikiPoles[i];
			if (tikiPole_Piece == null)
			{
				return false;
			}
			if (tikiPole_Piece.tikiPoleIndex != targetTikiPoleStructure[i])
			{
				return false;
			}
		}
		return true;
	}

	protected TikiPole_Piece GetTikiPieceByTikiIndex(TikiPole_Piece[] tikis, TIKIPIECEINDEX index)
	{
		foreach (TikiPole_Piece tikiPole_Piece in tikis)
		{
			if (tikiPole_Piece.tikiPoleIndex == index)
			{
				return tikiPole_Piece;
			}
		}
		return null;
	}

	protected TikiPole_Piece[] FindTikisUnderneath(float range, bool includeSelf = true)
	{
		RaycastHit[] array = Physics.RaycastAll(transform.position, Vector3.down, range, LayerMasks.MASK_Block, QueryTriggerInteraction.Ignore);
		Array.Sort(array, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance));
		List<TikiPole_Piece> list = new List<TikiPole_Piece>();
		RaycastHit[] array2 = array;
		for (int num = 0; num < array2.Length; num++)
		{
			RaycastHit raycastHit = array2[num];
			if (raycastHit.collider != null)
			{
				TikiPole_Piece component = raycastHit.collider.transform.parent.GetComponent<TikiPole_Piece>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		if (includeSelf)
		{
			list.Insert(0, this);
		}
		return list.ToArray();
	}
}
