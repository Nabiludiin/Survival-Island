using System;
using RG.Core.Data.Abstractions;

namespace RG.Core.Data.Specialisations;

[Serializable]
public abstract class PlayerStatsData : DataObject, ISerializable
{
	public override void ResetData()
	{
	}

	public void Load()
	{
	}

	public void Save()
	{
	}
}
