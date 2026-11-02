using RG.Core.Base;
using UnityEngine;

public abstract class ObjectsReferenceList : RGScriptableObject
{
	public abstract void Build();

	public abstract void Destroy();

	public abstract Object GetReferenceObjectByGUID(string id);
}
