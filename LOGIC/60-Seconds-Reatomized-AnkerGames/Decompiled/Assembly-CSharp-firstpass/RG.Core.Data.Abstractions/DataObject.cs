using RG.Core.Base;

namespace RG.Core.Data.Abstractions;

public abstract class DataObject : RGScriptableObject
{
	public abstract void ResetData();
}
