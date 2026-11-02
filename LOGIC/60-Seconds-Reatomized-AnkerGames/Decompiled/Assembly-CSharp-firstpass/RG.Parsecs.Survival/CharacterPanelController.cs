using TMPro;
using UnityEngine;

namespace RG.Parsecs.Survival;

public class CharacterPanelController : MonoBehaviour, IRefreshInteractable
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object that holds data about expeditions.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	[Tooltip("Prefab for attribute displayed in Characters Panel when character is selected.")]
	private ValueIconController _attributeTemplatePrefab;

	[SerializeField]
	[Tooltip("Prefab for need displayed in Characters Panel when character is selected.")]
	private ValueIconController _needTemplatePrefab;

	[SerializeField]
	[Header("References")]
	[Tooltip("Reference to repaired expedition panel controller which is the parent of this object.")]
	private RepairedExpeditionPanelController _repairedExpeditionPanelController;

	[SerializeField]
	[Tooltip("Reference to controller of available characters to choose from when sending expedition.")]
	private ExpeditionCharacterSlotsController _expeditionCharacterSlotsController;

	[SerializeField]
	[Tooltip("Field that is used to display characters name or CHOOSE CHARACTER text.")]
	private TextMeshProUGUI _characterNameText;

	[SerializeField]
	[Tooltip("Field that is used to display active statuses for character.")]
	private TextMeshProUGUI _characterNeedStatusesText;

	[SerializeField]
	[Tooltip("Character Attributes object that will be enabled when character is selected.")]
	private GameObject _characterAttributes;

	[SerializeField]
	[Tooltip("Characters needs object that will be enabled when character is selected.")]
	private GameObject _characterNeeds;

	[SerializeField]
	[Tooltip("Holder for attributes icons for the selected character.")]
	private Transform _characterAttributesHolder;

	[SerializeField]
	[Tooltip("Holder for needs icons for the selected character.")]
	private Transform _characterNeedsHolder;

	public void Initialize()
	{
		_repairedExpeditionPanelController.SetAcceptButtonInteractable(_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter != null);
		SetCharacterInfo(_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter);
		_expeditionCharacterSlotsController.SetAvailableCharacters();
		_expeditionCharacterSlotsController.RefreshCharacters();
	}

	public void RefreshInteractable()
	{
		_expeditionCharacterSlotsController.RefreshCharacters();
	}

	public void ResetCharacters()
	{
		_expeditionCharacterSlotsController.SetAllTogglesOff();
	}

	public void SetCharacter(Character character)
	{
		if (character == null && _expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter != null)
		{
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter.Unlock();
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter = null;
		}
		else if (character != null)
		{
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter = character;
			_expeditionData.RuntimeData.PlannedExpeditionData.ExpeditionCharacter.Lock();
		}
		_repairedExpeditionPanelController.SetAcceptButtonInteractable(character != null);
		SetCharacterInfo(character);
	}

	private void SetCharacterInfo(Character character)
	{
		if (character != null)
		{
			_characterNameText.text = character.StaticData.FullName;
			foreach (Transform item in _characterAttributesHolder)
			{
				Object.Destroy(item.gameObject);
			}
			_characterNeedStatusesText.text = string.Empty;
			for (int i = 0; i < character.RuntimeData.CurrentNeeds.Count; i++)
			{
				CharacterNeedParameters.ECharacterNeedStatus needStatus = character.RuntimeData.CurrentNeeds[i].GetNeedStatus();
				CharacterStatus characterStatusForNeedStatus = character.RuntimeData.CurrentNeeds[i].GetCharacterStatusForNeedStatus(needStatus);
				if (characterStatusForNeedStatus != null && characterStatusForNeedStatus.IsVisibleInUI)
				{
					_characterNeedStatusesText.text += $"{characterStatusForNeedStatus.Name} ";
				}
			}
			for (int j = 0; j < character.RuntimeData.CurrentAttributes.Count; j++)
			{
				ValueIconController valueIconController = Object.Instantiate(_attributeTemplatePrefab, _characterAttributesHolder, worldPositionStays: false);
				CharacterAttribute attribute = character.RuntimeData.CurrentAttributes[j].Attribute;
				CharacterAttributeValue attributeByObject = character.RuntimeData.GetAttributeByObject(attribute);
				valueIconController.SetSprite(attribute.GetAttributeLevel(attributeByObject.Value).Icon);
			}
			_characterAttributes.SetActive(value: true);
		}
		else
		{
			_characterNameText.text = string.Empty;
			_characterAttributes.SetActive(value: false);
		}
	}
}
