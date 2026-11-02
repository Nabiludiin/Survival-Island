using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviour_ID : MonoBehaviour
{
	private uint objectIndex;

	public uint ObjectIndex
	{
		get
		{
			return objectIndex;
		}
		set
		{
			objectIndex = value;
		}
	}

	public virtual RGD Serialize_Save()
	{
		return null;
	}

	public virtual List<RGD> Serialize_SaveMultiple()
	{
		return null;
	}

	protected virtual void OnDestroy()
	{
	}
}
