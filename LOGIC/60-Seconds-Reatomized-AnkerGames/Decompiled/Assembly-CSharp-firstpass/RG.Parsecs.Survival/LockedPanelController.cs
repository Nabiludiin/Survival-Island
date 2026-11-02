using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class LockedPanelController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Tooltip("Sprite that is used to show that this item slot was not used.")]
	private Sprite _lockedItemSprite;

	[SerializeField]
	[Tooltip("Term for \"days\" in elapsed days field.")]
	private LocalizedString _daysTerm;

	[SerializeField]
	[Header("References")]
	[Tooltip("Field in which destination name will be displayed.")]
	private TextMeshProUGUI _destinationName;

	[SerializeField]
	[Tooltip("Field that is used to display destination image.")]
	private Image _destinationImage;

	[SerializeField]
	[Tooltip("Field in which expedition character name will be displayed.")]
	private TextMeshProUGUI _characterName;

	[SerializeField]
	[Tooltip("Field in which expedition character icon will be displayed.")]
	private Image _characterImage;

	[SerializeField]
	[Tooltip("Expedition item images fields.")]
	private Image[] _itemImages;

	[SerializeField]
	[Tooltip("Field that is used to display how many days elapsed since beginning of the expedition.")]
	private TextMeshProUGUI _elapsedDays;

	[SerializeField]
	[Tooltip("Field for title explaining why expedition system is blocked.")]
	private TextMeshProUGUI _blockedTitle;

	[SerializeField]
	[Tooltip("Field for description explaining why expedition system is blocked.")]
	private TextMeshProUGUI _blockedDescription;

	[SerializeField]
	[Tooltip("Reference to blocked panel content that displays information about ongoing expedition.")]
	private GameObject _expeditionContent;

	[SerializeField]
	[Tooltip("Reference to blocked panel content that displays text information.")]
	private GameObject _descriptionContent;

	[SerializeField]
	private CraftingData _craftingData;

	[SerializeField]
	private SuitData _suitData;

	public void Initialize()
	{
		_descriptionContent.SetActive(value: true);
		_expeditionContent.SetActive(value: false);
		if (_expeditionData.RuntimeData.CurrentPlanetDeck == null)
		{
			SetBlockedReason(_expeditionData.StaticData.BlockedPanelNoPlanetTitleTerm, _expeditionData.StaticData.BlockedPanelNoPlanetDescriptionTerm);
		}
		if ((_craftingData.RuntimeData.IsOngoing && (_craftingData.RuntimeData.OngoingCraftingActionData.System == _expeditionData || _craftingData.RuntimeData.OngoingCraftingActionData.System == _suitData)) || (_craftingData.RuntimeData.IsPlanned && (_craftingData.RuntimeData.PlannedCraftingActionData.System == _expeditionData || _craftingData.RuntimeData.PlannedCraftingActionData.System == _suitData)))
		{
			SetBlockedReason(_expeditionData.RuntimeData.BlockedPanelTitleTerm, _expeditionData.StaticData.BlockedPanelUpgradeInProgressTerm);
		}
		else if (_expeditionData.RuntimeData.IsOngoing)
		{
			SetBlockedReason(_expeditionData.StaticData.BlockedPanelOngoingTitleTerm, _expeditionData.StaticData.BlockedPanelOngoingDescriptionTerm);
		}
		else if (CharacterManager.Instance.GetCharacterList().GetLiveCharactersCount() == 1)
		{
			SetBlockedReason(_expeditionData.RuntimeData.BlockedPanelTitleTerm, _expeditionData.StaticData.BlockedPanelCaptainAloneTerm);
		}
		else
		{
			SetBlockedReason(_expeditionData.RuntimeData.BlockedPanelTitleTerm, _expeditionData.RuntimeData.BlockedPanelDescriptionTerm);
		}
	}

	private void SetExpeditionCharacter()
	{
		_characterImage.sprite = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.StaticData.Icons.JournalIcon;
		_characterName.text = string.Concat(_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.StaticData.Name, " ", _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionCharacter.StaticData.Surname);
	}

	private void SetExpeditionDestination()
	{
		_destinationName.text = _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.Name;
		_destinationImage.sprite = _expeditionData.RuntimeData.OngoingExpeditionData.ChosenDestination.DestinationSprite;
	}

	private void SetExpeditionEquipment()
	{
		for (int i = 0; i < _itemImages.Length; i++)
		{
			if (_expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems.Count > i)
			{
				_itemImages[i].sprite = _expeditionData.RuntimeData.OngoingExpeditionData.ExpeditionItems[i].BaseStaticData.IconJournal;
				_itemImages[i].color = new Color(_itemImages[i].color.r, _itemImages[i].color.g, _itemImages[i].color.b, 1f);
			}
			else
			{
				_itemImages[i].color = new Color(_itemImages[i].color.r, _itemImages[i].color.g, _itemImages[i].color.b, 0f);
			}
		}
	}

	private void SetElapsedDays()
	{
		_elapsedDays.text = _expeditionData.RuntimeData.ElapsedExpeditionTime + " " + _daysTerm;
	}

	private void SetBlockedReason(LocalizedString title, LocalizedString description)
	{
		if (string.IsNullOrEmpty(title.mTerm))
		{
			title = _expeditionData.StaticData.BlockedPanelDefaultTitleTerm;
		}
		if (string.IsNullOrEmpty(description.mTerm))
		{
			description = _expeditionData.StaticData.BlockedPanelDefaultDescriptionTerm;
		}
		_blockedTitle.text = title;
		_blockedDescription.text = description;
	}
}
