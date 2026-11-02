using System;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Body Part Type", fileName = "New Body Part Type")]
public class BodyPartType : RGScriptableObject
{
	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private string _name = string.Empty;

	[SerializeField]
	private bool _isMandatory;

	[SerializeField]
	private bool _multipleActiveAllowed;

	public string Id => _id;

	public string Name => _name;

	public bool IsMandatory => _isMandatory;

	public bool MultipleActiveAllowed => _multipleActiveAllowed;

	public bool Is(string id)
	{
		return Id.Equals(id);
	}

	public bool Equals(BodyPartType other)
	{
		return Id.Equals(other._id);
	}
}
