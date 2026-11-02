using System;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Visuals/Visual Id", fileName = "New Visual Id")]
public class VisualId : RGScriptableObject
{
	[HideInInspector]
	[SerializeField]
	private string _id;

	[HideInInspector]
	[SerializeField]
	private string _prefix;

	public string Id => Guid;

	public override string ToString()
	{
		return Id;
	}

	public override bool Equals(object other)
	{
		VisualId visualId = other as VisualId;
		if (visualId != null)
		{
			return Id.Equals(visualId.Id);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}
