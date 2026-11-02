using System;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
public class BasicStateData : DataObject
{
	[SerializeField]
	private VariableDictionary _values;

	public VariableDictionary Values => _values;

	public override void ResetData()
	{
		_values.Clear();
	}
}
