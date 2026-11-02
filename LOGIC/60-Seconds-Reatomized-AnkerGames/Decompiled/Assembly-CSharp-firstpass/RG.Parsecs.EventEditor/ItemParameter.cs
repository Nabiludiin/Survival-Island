using System;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Serializable]
public class ItemParameter
{
	[Tooltip("Name of the parameter")]
	public string Name;

	[Tooltip("Item-type parameter value")]
	public IItem Item;
}
