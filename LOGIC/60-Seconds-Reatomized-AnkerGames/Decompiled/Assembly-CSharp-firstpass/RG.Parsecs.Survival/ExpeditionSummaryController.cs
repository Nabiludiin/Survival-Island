using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class ExpeditionSummaryController : MonoBehaviour
{
	[SerializeField]
	[Header("Data")]
	[Tooltip("Expedition Data scriptable object holding informations about expedition system.")]
	private ExpeditionData _expeditionData;

	[SerializeField]
	private LengthsRanges _lengthsRanges;

	[SerializeField]
	[Tooltip("Sprite that is used to indicate length of the expedition.")]
	private Sprite _lengthSprite;

	[SerializeField]
	[Tooltip("Prefab that is used to display threat, chances and length icons.")]
	private GameObject _destinationIconPrefab;

	[SerializeField]
	[Tooltip("Prefab that is used to display character attribute.")]
	private ValueIconController _attributeIconPrefab;

	[SerializeField]
	[Header("References")]
	[Tooltip("Field in which destination name will be displayed.")]
	private TextMeshProUGUI _destinationName;

	[SerializeField]
	[Tooltip("Field that is used to display destination image.")]
	private Image _destinationImage;

	[SerializeField]
	[Tooltip("Holder for supplies that can be found in this expedition point.")]
	private Transform _chanceForHolder;

	[SerializeField]
	[Tooltip("Holder for hazards that can be found in this expedition point.")]
	private Transform _hazardsHolder;

	[SerializeField]
	[Tooltip("Holder for lenght of expedition.")]
	private Transform _lengthHolder;

	[SerializeField]
	[Tooltip("Field in which expedition character name will be displayed.")]
	private TextMeshProUGUI _characterName;

	[SerializeField]
	[Tooltip("Field in which expedition character icon will be displayed.")]
	private Image _characterImage;

	[SerializeField]
	[Tooltip("Holder for expedition character attributes.")]
	private Transform _attributesHolder;

	[SerializeField]
	[Tooltip("Expedition item images fields.")]
	private Image[] _itemImages;

	private List<Image> _currentChancesFor = new List<Image>();

	private List<Image> _currentHazards = new List<Image>();

	private List<Image> _currentLengths = new List<Image>();

	public void SetExpeditionDestination(bool temporaryCard = false)
	{
		ExpeditionRuntimeData.OngoingExpData ongoingExpData = GetOngoingExpData(temporaryCard);
		_destinationName.text = ongoingExpData.ChosenDestination.Name;
		_destinationImage.sprite = ongoingExpData.ChosenDestination.DestinationSprite;
		SetIcons(_currentChancesFor, _chanceForHolder, ongoingExpData.ChosenDestination.ResourcesToFind, ongoingExpData.ChosenDestination.DynamicData.Items.ToArray());
		SetIcons(_currentHazards, _hazardsHolder, ongoingExpData.ChosenDestination.Threats.ToArray());
		SetIcons(_currentLengths, _lengthHolder, _lengthSprite, _lengthsRanges.GetAmountOfLengthsIconsForRange(ongoingExpData.ChosenDestination.Event.MinimumDaysAway, ongoingExpData.ChosenDestination.Event.MaximumDaysAway));
	}

	private void SetIcons(List<Image> currentIcons, Transform parent, params Array[] arrays)
	{
		int num = 0;
		for (int i = 0; i < arrays.Length; i++)
		{
			for (int j = 0; j < arrays[i].Length; j++)
			{
				Image image;
				if (currentIcons.Count <= num)
				{
					image = UnityEngine.Object.Instantiate(_destinationIconPrefab, parent, worldPositionStays: false).GetComponent<Image>();
					currentIcons.Add(image);
				}
				else
				{
					image = currentIcons[num];
					image.gameObject.SetActive(value: true);
				}
				num++;
				SetSprite(image, arrays[i].GetValue(j));
			}
		}
		if (currentIcons.Count > num)
		{
			for (int k = num; k < currentIcons.Count; k++)
			{
				currentIcons[k].gameObject.SetActive(value: false);
			}
		}
	}

	private void SetSprite(Image icon, object iconHolder)
	{
		if (iconHolder is Resource)
		{
			Resource resource = (Resource)iconHolder;
			icon.sprite = resource.Icon;
		}
		else if (iconHolder is IItem)
		{
			IItem item = (IItem)iconHolder;
			icon.sprite = item.BaseStaticData.IconJournal;
		}
		else if (iconHolder is ExpeditionThreat)
		{
			ExpeditionThreat expeditionThreat = (ExpeditionThreat)iconHolder;
			icon.sprite = expeditionThreat.StaticData.Icon;
		}
	}

	private void SetIcons(List<Image> currentIcons, Transform parent, Sprite newSprite, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Image image;
			if (currentIcons.Count <= i)
			{
				image = UnityEngine.Object.Instantiate(_destinationIconPrefab, parent, worldPositionStays: false).GetComponent<Image>();
				currentIcons.Add(image);
			}
			else
			{
				image = currentIcons[i];
				image.gameObject.SetActive(value: true);
			}
			image.sprite = newSprite;
		}
		if (currentIcons.Count > amount)
		{
			for (int j = amount; j < currentIcons.Count; j++)
			{
				currentIcons[j].gameObject.SetActive(value: false);
			}
		}
	}

	public void SetExpeditionCharacter(bool temporaryCard = false)
	{
		ExpeditionRuntimeData.OngoingExpData ongoingExpData = GetOngoingExpData(temporaryCard);
		_characterName.text = ongoingExpData.ExpeditionCharacter.StaticData.Name;
		_characterImage.sprite = ongoingExpData.ExpeditionCharacter.StaticData.Icons.JournalIcon;
	}

	public void SetExpeditionEquipment(bool temporaryCard = false)
	{
		ExpeditionRuntimeData.OngoingExpData ongoingExpData = GetOngoingExpData(temporaryCard);
		for (int i = 0; i < _itemImages.Length; i++)
		{
			if (ongoingExpData.ExpeditionItems.Count > i)
			{
				_itemImages[i].sprite = ongoingExpData.ExpeditionItems[i].BaseStaticData.IconJournal;
				_itemImages[i].color = new Color(_itemImages[i].color.r, _itemImages[i].color.g, _itemImages[i].color.b, 1f);
			}
			else
			{
				_itemImages[i].color = new Color(_itemImages[i].color.r, _itemImages[i].color.g, _itemImages[i].color.b, 0f);
			}
		}
	}

	private ExpeditionRuntimeData.OngoingExpData GetOngoingExpData(bool temporaryCard)
	{
		ExpeditionRuntimeData.OngoingExpData ongoingExpData = null;
		if (temporaryCard || _expeditionData.RuntimeData.IsPlanned)
		{
			ongoingExpData = _expeditionData.RuntimeData.PlannedExpeditionData;
		}
		else if (_expeditionData.RuntimeData.IsOngoing)
		{
			ongoingExpData = _expeditionData.RuntimeData.OngoingExpeditionData;
		}
		if (ongoingExpData == null)
		{
			throw new UnityException("OngoingExpData is null in ExpeditionSummaryController.");
		}
		return ongoingExpData;
	}
}
