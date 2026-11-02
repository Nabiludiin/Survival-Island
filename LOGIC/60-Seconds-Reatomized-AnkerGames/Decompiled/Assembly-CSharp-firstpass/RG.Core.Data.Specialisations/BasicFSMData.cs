using System;
using RG.Core.Data.Abstractions;
using RG.Core.Flow.Abstractions;
using RG.Core.Flow.Specialisations;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "BasicFSMData", menuName = "RG/Data/State/BasicFSMData", order = 1)]
public class BasicFSMData : DataObject
{
	[SerializeField]
	private BasicState[] _states;

	[SerializeField]
	private VariableDictionary _values;

	public IState[] States => _states;

	public VariableDictionary Values => _values;

	public override void ResetData()
	{
		_values.Clear();
	}
}
