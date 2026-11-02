using System;
using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
[CreateAssetMenu(menuName = "60 Parsecs!/Characters/New Attribute", fileName = "New Attribute")]
public class CharacterAttribute : CharacterProperty
{
	[SerializeField]
	private AttributeLevel[] _levels;

	public AttributeLevel GetAttributeLevel(float value)
	{
		for (int i = 0; i < _levels.Length; i++)
		{
			if (_levels[i].Range.Contains(value))
			{
				return _levels[i];
			}
		}
		Debug.LogError($"There is no attribute level for this value {value} in {name}");
		return default;
	}

	public static float CalculateTeamAttributeValue(string attributeId)
	{
		List<Character> charactersInGame = CharacterManager.Instance.GetCharacterList().CharactersInGame;
		float num = (float)charactersInGame.Count * 0.125f;
		float num2 = 0f;
		for (int i = 0; i < CharacterManager.Instance.GetCharacterList().CharactersInGame.Count; i++)
		{
			if (charactersInGame[i].RuntimeData.IsAlive() && charactersInGame[i].RuntimeData.IsDrawnOnShip())
			{
				num2 += charactersInGame[i].RuntimeData.GetAttributeById(attributeId).Value;
			}
		}
		return num2 / 4f * 0.5f + num;
	}
}
