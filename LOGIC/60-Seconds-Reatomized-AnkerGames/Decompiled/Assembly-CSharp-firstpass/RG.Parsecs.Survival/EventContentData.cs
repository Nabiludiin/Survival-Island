using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Core.SaveSystem;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class EventContentData : ILocalizationParamsManager
{
	public enum ESpriteAlign
	{
		LEFT,
		CENTER,
		RIGHT
	}

	public enum EEventContentType
	{
		TEXT,
		TEXT_ICON,
		SPRITE,
		CHOICE,
		TOP_TEXT
	}

	public enum ETextIconContentType
	{
		NONE,
		ADDITION,
		SUBTRACTION
	}

	[Serializable]
	public struct EventContentDataWrapper
	{
		public EEventContentType ContentType;

		public List<string> ChoiceSprites;

		public string Sprite;

		public string PureText;

		public ETextIconContentType IconType;

		public float Amount;

		public string CharacterID;

		public string ExpeditionCharacterID;

		public string ItemID;

		public LocalizedString Text;

		public List<LocalizedString> Texts;

		public List<string> VariableNames;

		public List<LocalizedString> VariableValues;

		public List<int> Variables;

		public string ResourceID;

		public ESpriteAlign SpriteAlign;

		public bool Damaged;
	}

	[SerializeField]
	private EEventContentType _contentType;

	[SerializeField]
	private Sprite[] _choiceSprites;

	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private string _pureText;

	[SerializeField]
	private ETextIconContentType _textIconType;

	[SerializeField]
	private float _amount;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Character _expeditionCharacter;

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private List<LocalizedString> _texts = new List<LocalizedString>();

	[SerializeField]
	private List<string> _variableNames = new List<string>();

	[SerializeField]
	private List<LocalizedString> _variableValues = new List<LocalizedString>();

	[SerializeField]
	private List<int> _localVariables;

	[SerializeField]
	private Resource _resource;

	[SerializeField]
	private ESpriteAlign _spriteAlign;

	[SerializeField]
	private bool _damaged;

	private const string EXPEDITION_PREFIX = "EXPEDITION";

	private const string EXPEDITION_CHARACTER_NAME_I2LOC_PARAM = "EXPEDITION_CHARACTER_NAME";

	private const string EXPEDITION_CHARACTER_SURNAME_I2LOC_PARAM = "EXPEDITION_CHARACTER_SURNAME";

	private const string EXPEDITION_CHARACTER_FULL_NAME_I2LOC_PARAM = "EXPEDITION_CHARACTER_FULL_NAME";

	private const string EXPEDITION_CHARACTER_SEX = "EXPEDITION_CHARACTER_SEX";

	private LocalizedString _text
	{
		get
		{
			if (_texts != null && _texts.Count > 0)
			{
				return _texts[0];
			}
			return null;
		}
		set
		{
			if (_texts == null)
			{
				_texts = new List<LocalizedString>();
			}
			if (_texts.Count == 0)
			{
				_texts.Add(value);
			}
			else
			{
				_texts[0] = value;
			}
		}
	}

	public List<int> LocalVariablesInts
	{
		get
		{
			return _localVariables;
		}
		set
		{
			_localVariables = value;
		}
	}

	public Character Character
	{
		get
		{
			return _character;
		}
		set
		{
			_character = value;
		}
	}

	public Character ExpeditionCharacter
	{
		get
		{
			return _expeditionCharacter;
		}
		set
		{
			_expeditionCharacter = value;
		}
	}

	public IItem Item
	{
		get
		{
			return _item;
		}
		set
		{
			_item = value;
		}
	}

	public EEventContentType ContentType => _contentType;

	public LocalizedString Text => _text;

	public Sprite Sprite => _sprite;

	public Sprite[] ChoiceSprites => _choiceSprites;

	public string PureText => _pureText;

	public ETextIconContentType TextIconType => _textIconType;

	public float Amount => _amount;

	public Resource Resource => _resource;

	public ESpriteAlign SpriteAlign
	{
		get
		{
			return _spriteAlign;
		}
		set
		{
			_spriteAlign = value;
		}
	}

	public bool Damaged => _damaged;

	private void RegisterManagers()
	{
		if ((bool)_character)
		{
			_character.RegisterInI2Loc();
		}
		if ((bool)_item)
		{
			_item.RegisterToI2Loc();
		}
		if (_localVariables != null || _expeditionCharacter != null)
		{
			RegisterToI2Loc();
		}
	}

	private void UnregisterManagers()
	{
		if ((bool)_item)
		{
			_item.UnregisterFromI2Loc();
		}
		if ((bool)_character)
		{
			_character.UnregisterFromI2Loc();
		}
		if (_localVariables != null || _expeditionCharacter != null)
		{
			UnregisterFromI2Loc();
		}
	}

	public string GetTranslatedText()
	{
		RegisterManagers();
		string text = string.Empty;
		for (int i = 0; i < _texts.Count; i++)
		{
			text += _texts[i];
			if (i < _variableNames.Count)
			{
				text = text.Replace(_variableNames[i], _variableValues[i].ToString());
			}
		}
		UnregisterManagers();
		return text;
	}

	public EventContentData(EEventContentType contentType, Sprite sprite, ESpriteAlign spriteAlign)
	{
		_contentType = contentType;
		_sprite = sprite;
		_spriteAlign = spriteAlign;
	}

	public EventContentData(EEventContentType contentType, LocalizedString text)
	{
		_contentType = contentType;
		_text = text;
	}

	public EventContentData(EEventContentType contentType, LocalizedString text, string variable, LocalizedString variableValue)
	{
		_contentType = contentType;
		_text = text;
		if (_variableNames == null)
		{
			_variableNames = new List<string>();
		}
		_variableNames.Add(variable);
		if (_variableValues == null)
		{
			_variableValues = new List<LocalizedString>();
		}
		_variableValues.Add(variableValue);
	}

	public EventContentData(EEventContentType contentType, string text)
	{
		_contentType = contentType;
		_pureText = text;
	}

	public EventContentData(EEventContentType contentType, Sprite sprite)
	{
		_contentType = contentType;
		_sprite = sprite;
	}

	public EventContentData(EEventContentType contentType, LocalizedString text, Sprite sprite)
	{
		_contentType = contentType;
		_text = text;
		_sprite = sprite;
	}

	public EventContentData(EEventContentType contentType, ETextIconContentType type, float amount, IItem item)
	{
		_contentType = contentType;
		_textIconType = type;
		_amount = amount;
		_item = item;
	}

	public EventContentData(EEventContentType contentType, ETextIconContentType type, bool damaged, IItem item)
	{
		_contentType = contentType;
		_textIconType = type;
		_damaged = damaged;
		_item = item;
	}

	public EventContentData(EEventContentType contentType, ETextIconContentType type, float amount, Resource resource)
	{
		_contentType = contentType;
		_textIconType = type;
		_amount = amount;
		_resource = resource;
	}

	public EventContentData(EEventContentType contentType, List<Sprite> choiceSprites)
	{
		_contentType = contentType;
		_choiceSprites = new Sprite[choiceSprites.Count];
		int num = choiceSprites.Count - 1;
		int num2 = choiceSprites.Count - 1;
		while (num >= 0)
		{
			if (!(choiceSprites[num] == null))
			{
				_choiceSprites[num2] = choiceSprites[num];
				num2--;
			}
			num--;
		}
	}

	public void AddTextLine(LocalizedString text, string variable, LocalizedString variableValue)
	{
		_texts.Add(text);
		_variableNames.Add(variable);
		_variableValues.Add(variableValue);
	}

	public EventContentData(EventContentDataWrapper wrapper, SaveEvent saveEvent)
	{
		_contentType = wrapper.ContentType;
		if (wrapper.ChoiceSprites.Count > 0)
		{
			_choiceSprites = new Sprite[4];
			for (int i = 0; i < wrapper.ChoiceSprites.Count; i++)
			{
				if (wrapper.ChoiceSprites[i] == "null")
				{
					_choiceSprites[i] = null;
					continue;
				}
				Sprite sprite = ContentManager.GetSprite(wrapper.ChoiceSprites[i]);
				if (sprite == null)
				{
					Debug.LogError($"Cannot find sprite {wrapper.ChoiceSprites[i]} in asset bundle");
				}
				else
				{
					_choiceSprites[i] = sprite;
				}
			}
		}
		if (!string.IsNullOrEmpty(wrapper.Sprite))
		{
			Sprite sprite2 = ContentManager.GetSprite(wrapper.Sprite);
			if (sprite2 == null)
			{
				Debug.LogError($"Cannot find sprite {wrapper.Sprite} in asset bundle");
			}
			else
			{
				_sprite = (string.IsNullOrEmpty(wrapper.Sprite) ? null : sprite2);
			}
		}
		_pureText = wrapper.PureText;
		_textIconType = wrapper.IconType;
		_amount = wrapper.Amount;
		_texts = new List<LocalizedString>();
		_variableNames = new List<string>();
		_variableValues = new List<LocalizedString>();
		if (saveEvent.SaveVersion < 3)
		{
			if (!string.IsNullOrEmpty(wrapper.Text))
			{
				_text = wrapper.Text;
			}
		}
		else
		{
			for (int j = 0; j < wrapper.Texts.Count; j++)
			{
				_texts.Add(wrapper.Texts[j]);
			}
			for (int k = 0; k < wrapper.VariableNames.Count; k++)
			{
				_variableNames.Add(wrapper.VariableNames[k]);
			}
			for (int l = 0; l < wrapper.VariableValues.Count; l++)
			{
				_variableValues.Add(wrapper.VariableValues[l]);
			}
		}
		if (!string.IsNullOrEmpty(wrapper.CharacterID))
		{
			_character = (Character)saveEvent.GetReferenceObjectByID(wrapper.CharacterID);
		}
		if (!string.IsNullOrEmpty(wrapper.ExpeditionCharacterID))
		{
			_expeditionCharacter = (Character)saveEvent.GetReferenceObjectByID(wrapper.ExpeditionCharacterID);
		}
		if (!string.IsNullOrEmpty(wrapper.ItemID))
		{
			_item = (IItem)saveEvent.GetReferenceObjectByID(wrapper.ItemID);
		}
		_localVariables = wrapper.Variables;
		if (!string.IsNullOrEmpty(wrapper.ResourceID))
		{
			_resource = (Resource)saveEvent.GetReferenceObjectByID(wrapper.ResourceID);
		}
		_spriteAlign = wrapper.SpriteAlign;
		_damaged = wrapper.Damaged;
	}

	private void RegisterToI2Loc()
	{
		if (!LocalizationManager.ParamManagers.Contains(this))
		{
			LocalizationManager.ParamManagers.Add(this);
			LocalizationManager.LocalizeAll(Force: true);
		}
	}

	private void UnregisterFromI2Loc()
	{
		LocalizationManager.ParamManagers.Remove(this);
	}

	public string GetParameterValue(string param)
	{
		if (char.IsNumber(param[0]) && param.Contains("|"))
		{
			int index = Convert.ToInt32(param.Substring(0, param.IndexOf("|", StringComparison.Ordinal)));
			return param.Substring(param.IndexOf("|", StringComparison.Ordinal) + 1).Split('/')[_localVariables[index]];
		}
		if (char.IsNumber(param[0]) && !param.Contains("|"))
		{
			int index2 = Convert.ToInt32(param);
			return _localVariables[index2].ToString();
		}
		if (param.StartsWith("EXPEDITION"))
		{
			switch (param)
			{
			case "EXPEDITION_CHARACTER_NAME":
				return _expeditionCharacter.StaticData.Name;
			case "EXPEDITION_CHARACTER_SURNAME":
				return _expeditionCharacter.StaticData.Surname;
			case "EXPEDITION_CHARACTER_FULL_NAME":
				return string.Concat(_expeditionCharacter.StaticData.Name, " ", _expeditionCharacter.StaticData.Surname);
			}
			if (param.StartsWith("EXPEDITION_CHARACTER_SEX"))
			{
				return GetOptionBySex(param);
			}
		}
		return null;
	}

	private string GetOptionBySex(string text)
	{
		string[] array = text.Substring(text.IndexOf("|", StringComparison.Ordinal) + 1).Split('/');
		if (_expeditionCharacter.StaticData.Female)
		{
			return array[0];
		}
		return array[1];
	}

	public EventContentDataWrapper Save()
	{
		EventContentDataWrapper result = new EventContentDataWrapper
		{
			ContentType = _contentType,
			ChoiceSprites = new List<string>(),
			Sprite = ((_sprite != null) ? _sprite.name : string.Empty),
			PureText = _pureText,
			IconType = _textIconType,
			Amount = _amount,
			Texts = new List<LocalizedString>(),
			VariableNames = new List<string>(),
			VariableValues = new List<LocalizedString>(),
			CharacterID = ((_character != null) ? _character.ID : string.Empty),
			ExpeditionCharacterID = ((_expeditionCharacter != null) ? _expeditionCharacter.ID : string.Empty),
			ItemID = ((_item != null) ? _item.BaseStaticData.ItemId : string.Empty),
			Variables = _localVariables,
			ResourceID = ((_resource != null) ? _resource.ID : string.Empty),
			SpriteAlign = _spriteAlign,
			Damaged = _damaged
		};
		if (_choiceSprites != null)
		{
			for (int i = 0; i < _choiceSprites.Length; i++)
			{
				string item = ((_choiceSprites[i] == null) ? "null" : _choiceSprites[i].name.Replace("(Clone)", ""));
				result.ChoiceSprites.Add(item);
			}
		}
		if (_texts != null)
		{
			for (int j = 0; j < _texts.Count; j++)
			{
				result.Texts.Add(_texts[j]);
			}
		}
		if (_variableNames != null)
		{
			for (int k = 0; k < _variableNames.Count; k++)
			{
				result.VariableNames.Add(_variableNames[k]);
			}
		}
		if (_variableValues != null)
		{
			for (int l = 0; l < _variableValues.Count; l++)
			{
				result.VariableValues.Add(_variableValues[l]);
			}
		}
		return result;
	}
}
