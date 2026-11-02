using System;
using RG.Core.Base;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(fileName = "New Character Set", menuName = "60 Parsecs!/Characters/New Character Set")]
public class CharacterSet : RGScriptableObject
{
	[SerializeField]
	private Character[] _characters;

	public Character[] AllCharacters => _characters;

	public int Count
	{
		get
		{
			if (_characters != null)
			{
				return _characters.Length;
			}
			return 0;
		}
	}

	public Character GetCharacter(int index)
	{
		if (_characters != null && index >= 0 && index < _characters.Length)
		{
			return _characters[index];
		}
		return null;
	}

	public Character GetCharacter(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			for (int i = 0; i < _characters.Length; i++)
			{
				if (_characters[i].Equals(id))
				{
					return _characters[i];
				}
			}
		}
		return null;
	}

	public int GetCharacterId(Character character)
	{
		for (int i = 0; i < _characters.Length; i++)
		{
			if (_characters[i] == character)
			{
				return i;
			}
		}
		return -1;
	}

	public Character GetCaptain()
	{
		Character[] characters = _characters;
		foreach (Character character in characters)
		{
			if (character.RuntimeData.IsCaptain)
			{
				return character;
			}
		}
		Debug.LogWarning("Character list is empty!");
		return null;
	}
}
