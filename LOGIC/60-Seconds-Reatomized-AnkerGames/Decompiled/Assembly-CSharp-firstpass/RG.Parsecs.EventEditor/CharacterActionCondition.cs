using System.Collections.Generic;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[CreateAssetMenu(fileName = "New Character Action Condition", menuName = "60 Parsecs!/Event Editor/Action Conditions/New Character Condition")]
public class CharacterActionCondition : BaseActionCondition
{
	[SerializeField]
	private List<Character> _characters;

	public override bool CanSelect()
	{
		bool flag = true;
		for (int i = 0; i < _characters.Count; i++)
		{
			flag &= _characters[i].CanLock() && !_characters[i].RuntimeData.HasStatusPreventingSelectionInEventChoices();
		}
		return flag;
	}

	public override void OnSelect()
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			_characters[i].Lock();
		}
	}

	public override void OnDeselect()
	{
		for (int i = 0; i < _characters.Count; i++)
		{
			_characters[i].Unlock();
		}
	}
}
