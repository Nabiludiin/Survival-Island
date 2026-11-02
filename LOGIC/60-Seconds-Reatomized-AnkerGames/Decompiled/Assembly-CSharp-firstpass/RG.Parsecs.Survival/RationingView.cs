using System;
using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class RationingView : WindowView
{
	[Serializable]
	private struct SRationNeedEntry
	{
		[SerializeField]
		private CharacterNeed _need;

		[SerializeField]
		private TextMeshProUGUI _needNameLabel;

		[SerializeField]
		private TextMeshProUGUI _needStatusLabel;

		public CharacterNeed Need => _need;

		public TextMeshProUGUI NeedNameLabel => _needNameLabel;

		public TextMeshProUGUI NeedStatusLabel => _needStatusLabel;
	}

	[Serializable]
	private struct SItemValueLabel
	{
		[SerializeField]
		private IItem _item;

		[SerializeField]
		private TextMeshProUGUI _valueLabel;

		public IItem Item => _item;

		public TextMeshProUGUI ValueLabel => _valueLabel;
	}

	[Header("Data")]
	[Space]
	[SerializeField]
	private CharacterList _characterList;

	[SerializeField]
	private RationingData _rationingData;

	[SerializeField]
	private EndOfDayListenerList _endOfDayListenerList;

	[SerializeField]
	private RationingListenerList _rationingListenerList;

	[SerializeField]
	private int _characterIndex;

	[Header("Rationing")]
	[Space]
	[SerializeField]
	private RationingManager _rationingManager;

	[SerializeField]
	private List<RationingViewRationToggleController> _rationToggles;

	[SerializeField]
	private SItemValueLabel[] _itemValueLabels;

	[Header("Character Info")]
	[Space]
	[SerializeField]
	private TextMeshProUGUI _characterNameField;

	[SerializeField]
	private SRationNeedEntry[] _needEntries;

	[SerializeField]
	private LocalizedString _needStatusDefaultText;

	[SerializeField]
	private Color _positiveStatusColor;

	[SerializeField]
	private Color _neutralStatusColor;

	[SerializeField]
	private Color _negativeStatusColor;

	[SerializeField]
	private Color _warningStatusColor;

	[SerializeField]
	private Image _characterIconField;

	private static int RESET_LISTENER_LIST_PRIORITY = 30;

	private Character Character => _characterList.CharactersInGame[_characterIndex];

	public override void OnShow()
	{
		RefreshRationItemCounts();
		ShowToggles();
		SetCharacterInfo();
	}

	private void ShowToggles()
	{
		for (int i = 0; i < _rationToggles.Count; i++)
		{
			_rationToggles[i].OnShow();
		}
	}

	public void RefreshRationItemCounts()
	{
		for (int i = 0; i < _itemValueLabels.Length; i++)
		{
			if (_itemValueLabels[i].Item is ConsumableRemedium)
			{
				ConsumableRemedium consumableRemedium = (ConsumableRemedium)_itemValueLabels[i].Item;
				float num = consumableRemedium.RuntimeData.Amount - consumableRemedium.RuntimeData.PlannedConsumption;
				_itemValueLabels[i].ValueLabel.text = $"{num}";
			}
			else
			{
				int num2 = (_rationingManager.IsItemAvailableForRationing(_itemValueLabels[i].Item) ? 1 : 0);
				_itemValueLabels[i].ValueLabel.text = $"{num2}";
			}
		}
	}

	public void Start()
	{
		_endOfDayListenerList.RegisterOnEndOfDay(ResetToggles, "Reset", RESET_LISTENER_LIST_PRIORITY, this, forceOrder: true);
		_rationingListenerList.Register(RefreshRationItemCounts);
	}

	public void OnDestroy()
	{
		_endOfDayListenerList.UnregisterOnEndOfDay(ResetToggles, "Reset");
		_rationingListenerList.Unregister(RefreshRationItemCounts);
	}

	public bool CanBeRationedToCharacter(IItem item)
	{
		return _rationingManager.CanItemBeRationedToCharacter(item, _characterIndex);
	}

	public bool CanBeRationedToAll(IItem item)
	{
		return _rationingManager.IsThereEnoughRations(item);
	}

	public bool WasRationedToAll(IItem item)
	{
		return _rationingManager.GetEligibleCharacterCountForRation(item) - _rationingManager.GetEligibleCharacterRationedCount(item) == 0;
	}

	public bool CanRationItem(IItem item)
	{
		if (!_rationingManager.IsItemAvailableForRationing(item))
		{
			return _rationingData.IsItemRationedForCharacter(_characterIndex, item.BaseStaticData.ItemId);
		}
		return true;
	}

	private void ResetToggles()
	{
		for (int i = 0; i < _rationToggles.Count; i++)
		{
			_rationToggles[i].SetToggleWithoutInvokingValueChange(value: false);
		}
	}

	public void RationItem(IItem rationingItem)
	{
		_rationingManager.CharacterRationed(_characterIndex, rationingItem);
		ShowToggles();
	}

	public void RationToAll(IItem rationingItem)
	{
		if (_rationingManager.AreAllEligibleCharactersRationed(rationingItem))
		{
			_rationingManager.UnrationAllCharacters(rationingItem);
		}
		else if (_rationingManager.IsItemAvailableForRationing(rationingItem))
		{
			_rationingManager.RationAllCharacters(rationingItem);
		}
		ShowToggles();
	}

	public float GetNeedValueForCharacter(CharacterNeed need)
	{
		CharacterNeedParameters needById = Character.RuntimeData.GetNeedById(need.Id);
		if (needById != null)
		{
			return needById.Value;
		}
		Debug.LogWarning($"Need {need.Id} not defined for character {Character.ID}!");
		return 0f;
	}

	public bool IsItemRationedForThisCharacter(IItem item)
	{
		return _rationingData.IsItemRationedForCharacter(_characterIndex, item.BaseStaticData.ItemId);
	}

	private void SetCharacterInfo()
	{
		_characterNameField.text = Character.StaticData.FullName;
		_characterIconField.sprite = Character.StaticData.Icons.JournalIcon;
		if (_needEntries == null)
		{
			return;
		}
		for (int i = 0; i < _needEntries.Length; i++)
		{
			_needEntries[i].NeedNameLabel.text = string.Empty;
			_needEntries[i].NeedStatusLabel.text = string.Empty;
			CharacterNeedParameters needById = Character.RuntimeData.GetNeedById(_needEntries[i].Need.Id);
			if (needById == null)
			{
				continue;
			}
			string text = _needStatusDefaultText;
			Color color = _neutralStatusColor;
			_needEntries[i].NeedNameLabel.text = _needEntries[i].Need.Name;
			CharacterStatus currentCharacterStatusForNeedStatus = needById.GetCurrentCharacterStatusForNeedStatus();
			if (currentCharacterStatusForNeedStatus != null)
			{
				text = currentCharacterStatusForNeedStatus.Name;
				if (currentCharacterStatusForNeedStatus.IsPositive && currentCharacterStatusForNeedStatus.IsNegative)
				{
					color = _warningStatusColor;
				}
				else if (currentCharacterStatusForNeedStatus.IsPositive)
				{
					color = _positiveStatusColor;
				}
				else if (currentCharacterStatusForNeedStatus.IsNegative)
				{
					color = _negativeStatusColor;
				}
			}
			_needEntries[i].NeedStatusLabel.text = text;
			_needEntries[i].NeedStatusLabel.color = color;
		}
	}
}
