using System.Collections.Generic;
using System.Text;
using RG.Core.Base;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Character Status list", fileName = "New Character Status list")]
public class CharacterStatusList : RGScriptableObject
{
	[SerializeField]
	private List<CharacterStatus> _statuses = new List<CharacterStatus>();

	public CharacterStatus GetAvailableCharacterStatus(string id)
	{
		for (int i = 0; i < _statuses.Count; i++)
		{
			if (_statuses[i].Is(id))
			{
				return _statuses[i];
			}
		}
		return null;
	}

	public new string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < _statuses.Count; i++)
		{
			stringBuilder.AppendFormat("{0} ", _statuses[i].Id);
		}
		return stringBuilder.ToString();
	}
}
