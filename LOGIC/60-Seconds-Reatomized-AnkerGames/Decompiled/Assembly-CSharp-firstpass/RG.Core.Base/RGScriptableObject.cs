using UnityEngine;

namespace RG.Core.Base;

public abstract class RGScriptableObject : ScriptableObject
{
	[SerializeField]
	[HideInInspector]
	private string _guid;

	public virtual string Guid => _guid;

	public virtual string GuidPrefix => GetType().Name;
}
