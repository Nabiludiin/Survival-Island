using System;
using RG.Core.Data.DataTypes;
using UnityEngine;

namespace RG.Core.Data.Abstractions;

[Serializable]
public abstract class SessionData : DataObject
{
	[SerializeField]
	private DualDictionary _values = new DualDictionary();

	public DualDictionary Values => _values;

	public override void ResetData()
	{
		_values.Reset();
	}
}
