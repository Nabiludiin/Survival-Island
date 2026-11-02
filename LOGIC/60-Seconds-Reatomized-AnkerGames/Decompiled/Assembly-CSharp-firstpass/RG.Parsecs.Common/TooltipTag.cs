using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(fileName = "New Tooltip Tag", menuName = "60 Parsecs!/Tooltips/New Tooltip Tag")]
public class TooltipTag : RGScriptableObject
{
	[HideInInspector]
	[SerializeField]
	private string _id;

	[SerializeField]
	private string _name;

	public string Id => Guid;

	public string Name => _name;

	public override string ToString()
	{
		return Guid;
	}

	public override bool Equals(object other)
	{
		if (other is TooltipTag)
		{
			return Guid.Equals(((TooltipTag)other).Guid);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Guid.GetHashCode();
	}
}
