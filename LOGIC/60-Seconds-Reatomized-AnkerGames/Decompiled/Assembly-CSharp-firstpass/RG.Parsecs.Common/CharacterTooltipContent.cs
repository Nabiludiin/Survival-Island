using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.Common;

public class CharacterTooltipContent : TooltipContent
{
	[SerializeField]
	[Range(0f, 3f)]
	private int _characterIndex;

	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private bool _useForcedCharacter;

	[Tooltip("If Use Forced Character is true, this character will be used. In that case it can't be null.")]
	[SerializeField]
	private Character _forcedCharacter;

	public Character Character
	{
		get
		{
			if (_useForcedCharacter)
			{
				if (_forcedCharacter == null)
				{
					Debug.LogWarning("Bad setup in CharacterTooltipContent in " + gameObject.name);
					return null;
				}
				return _forcedCharacter;
			}
			if (_characterList == null || _characterList.CharactersInGame.Count <= _characterIndex)
			{
				Debug.LogWarning("Bad setup in CharacterTooltipContent in " + gameObject.name);
				return null;
			}
			return _characterList.CharactersInGame[_characterIndex];
		}
	}

	public override bool IsValid()
	{
		if (_characterList != null)
		{
			return _characterList.CharactersInGame.Count >= _characterIndex;
		}
		return false;
	}
}
