using System;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/End Game Editor/End Game Text Id", fileName = "New Text Id")]
public class EndGameTextId : ScriptableObject
{
	[SerializeField]
	[Tooltip("Unique Id of this scriptable object")]
	private string _id;

	public string Id => _id;

	public override bool Equals(object other)
	{
		EndGameTextId endGameTextId = other as EndGameTextId;
		if (endGameTextId != null)
		{
			return _id.Equals(endGameTextId.Id);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _id.GetHashCode();
	}
}
