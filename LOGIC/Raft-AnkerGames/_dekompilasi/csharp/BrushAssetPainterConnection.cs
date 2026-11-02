using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BrushAssetPainterConnection
{
	[SerializeField]
	private List<Transform> paintedObjects = new List<Transform>();

	public List<Transform> PaintedObjects
	{
		get
		{
			return paintedObjects;
		}
		set
		{
			paintedObjects = value;
		}
	}
}
