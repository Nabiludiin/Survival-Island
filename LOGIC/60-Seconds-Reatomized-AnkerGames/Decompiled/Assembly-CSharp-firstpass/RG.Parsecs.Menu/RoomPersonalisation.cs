using System.Collections.Generic;
using RG.Parsecs.EventEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Menu;

public class RoomPersonalisation : MonoBehaviour
{
	public CharacterSet characterSet;

	public GlobalIntVariable previousCaptain;

	[SerializeField]
	private List<CharacterActionsMenu> actions = new List<CharacterActionsMenu>();

	public void PersonalizeStation()
	{
		if (previousCaptain.Value == -1)
		{
			return;
		}
		Character character = characterSet.GetCharacter(previousCaptain.Value);
		foreach (CharacterActionsMenu action in actions)
		{
			if (action.character.Equals(character))
			{
				action.action.Invoke();
				break;
			}
		}
	}
}
