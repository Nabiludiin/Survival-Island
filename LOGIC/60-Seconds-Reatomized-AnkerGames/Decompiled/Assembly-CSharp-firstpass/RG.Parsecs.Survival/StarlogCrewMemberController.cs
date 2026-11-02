using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class StarlogCrewMemberController : MonoBehaviour
{
	[Header("Data")]
	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private int _characterIndex;

	[SerializeField]
	private int _topFieldLineSignsAmount;

	[SerializeField]
	private int _topFieldLinesAmount;

	[SerializeField]
	private LocalizedString _personalGoalTerm;

	[SerializeField]
	private LocalizedString _updateTerm;

	[Header("References")]
	[SerializeField]
	private Image _portraitImage;

	[SerializeField]
	private TextMeshProUGUI _nameField;

	[SerializeField]
	private TextMeshProUGUI[] _attributesField;

	[SerializeField]
	private TextMeshProUGUI _topDescriptionField;

	[SerializeField]
	private TextMeshProUGUI _bottomDescriptionField;

	[SerializeField]
	private TextMeshProUGUI _personalGoalField;

	[SerializeField]
	private TextMeshProUGUI _updateField;

	[SerializeField]
	private GameObject _deceasedObject;

	private Character _character;

	private Character Character
	{
		get
		{
			if (_character == null)
			{
				_character = _characterList.CharactersInGame[_characterIndex];
			}
			return _character;
		}
	}

	public int CharacterIndex => _characterIndex;

	public void OnShow()
	{
		if (_characterList.CharactersInGame.Count <= _characterIndex)
		{
			return;
		}
		Character character = _characterList.CharactersInGame[_characterIndex];
		_portraitImage.sprite = character.StaticData.Icons.PortraitIcon;
		_nameField.text = _characterList.CharactersInGame[_characterIndex].StaticData.FullName;
		for (int i = 0; i < _attributesField.Length; i++)
		{
			CharacterAttributeValue characterAttributeValue = character.RuntimeData.CurrentAttributes[i];
			_attributesField[i].text = $"{characterAttributeValue.Attribute.Name}: {characterAttributeValue.Attribute.GetAttributeLevel(characterAttributeValue.Value).Name}";
		}
		_topDescriptionField.text = character.StaticData.Description;
		if (character.RuntimeData.IsCaptain)
		{
			_personalGoalField.gameObject.SetActive(value: true);
			if (character.StaticData.PersonalGoal != null)
			{
				_personalGoalField.text = string.Format("{0} {1} ({2})", _personalGoalTerm, ((string)character.StaticData.PersonalGoal.Name != null) ? character.StaticData.PersonalGoal.Name.ToString() : "NO GOAL NAME SET", ((string)character.StaticData.PersonalGoal.Description != null) ? character.StaticData.PersonalGoal.Description.ToString() : "NO GOAL DESCRIPTION SET");
			}
			else
			{
				_personalGoalField.text = string.Format("{0} {1}", _personalGoalTerm, "NO PERSONAL GOAL SET!");
			}
		}
		else
		{
			_personalGoalField.gameObject.SetActive(value: false);
		}
		if (character.RuntimeData.CharacterHistory.Count == 0)
		{
			_updateField.text = string.Empty;
		}
		else
		{
			_updateField.text = string.Empty;
			for (int j = 0; j < character.RuntimeData.CharacterHistory.Count; j++)
			{
				_updateField.text += character.RuntimeData.CharacterHistory[j];
			}
		}
		_deceasedObject.SetActive(!character.RuntimeData.IsAlive());
	}

	private string[] DivideTextToTwoStrings(string stringToDivide)
	{
		if (!string.IsNullOrEmpty(stringToDivide))
		{
			string text = string.Empty;
			int num = 0;
			int num2 = 0;
			string text2 = string.Empty;
			int num3 = 0;
			string[] array = stringToDivide.Split(' ');
			for (int i = 0; i < array.Length; i++)
			{
				string text3 = array[i];
				num2 += text3.Length + 1;
				if (num2 <= _topFieldLineSignsAmount)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						text2 += " ";
					}
					text2 += text3;
					continue;
				}
				num2 = 0;
				text = text + text2 + "\n";
				num++;
				if (num >= _topFieldLinesAmount)
				{
					num3 = i;
					break;
				}
				i--;
				text2 = string.Empty;
			}
			int num4 = stringToDivide.IndexOf(array[num3], StringComparison.Ordinal);
			string text4 = stringToDivide.Substring(num4, stringToDivide.Length - num4);
			return new string[2] { text, text4 };
		}
		return new string[2]
		{
			string.Empty,
			string.Empty
		};
	}

	public bool IsCharacterAlive()
	{
		return Character.RuntimeData.IsAlive();
	}
}
