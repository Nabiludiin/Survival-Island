using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorCollection
{
	[Header("Element name")]
	public string name;

	[Header("In which order in the circle, 0 is bottom, CCW")]
	public int order;

	[Header("Colors are sorted dark to bright, i.e assign dark colors first")]
	public List<SO_ColorValue> colorValues = new List<SO_ColorValue>();
}
