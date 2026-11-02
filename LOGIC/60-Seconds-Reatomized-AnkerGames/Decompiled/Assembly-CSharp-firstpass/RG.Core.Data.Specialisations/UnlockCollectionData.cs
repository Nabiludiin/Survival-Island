using System;
using System.Collections.Generic;
using RG.Core.Data.Abstractions;
using UnityEngine;

namespace RG.Core.Data.Specialisations;

[Serializable]
[CreateAssetMenu(fileName = "UnlockCollectionData", menuName = "RG/Data/Core/Profile/UnlockCollectionData", order = 1)]
public class UnlockCollectionData : DataObject
{
	[SerializeField]
	private List<UnlockData> _unlocks;

	public override void ResetData()
	{
		_unlocks.Clear();
	}
}
