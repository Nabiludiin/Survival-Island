using System;
using System.Text;
using I2.Loc;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class EventUITooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _decisionName;

	[SerializeField]
	private LocalizedString _yesDecisionName;

	[SerializeField]
	private LocalizedString _noDecisionName;

	[SerializeField]
	private LocalizedString _noChoiceName;

	[SerializeField]
	private LocalizedString _crewAttributeTerm;

	[SerializeField]
	private LocalizedString _captainAttributeTerm;

	[Tooltip("Hex RGB code for color representing negative statuses, like #FF0000")]
	[SerializeField]
	private LocalizedString _negativeColor;

	private string _termString;

	[Tooltip("Panel confirm button.")]
	[SerializeField]
	private Button _confirmButton;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as TextTooltipContent))
		{
			HandleItemTooltipContent((TextTooltipContent)content);
		}
		else if ((bool)(content as EventCardsUITooltipContent))
		{
			HandleItemTooltipContent((EventCardsUITooltipContent)content);
		}
		else if ((bool)(content as UIButtonTooltipContent))
		{
			HandleItemTooltipContent((UIButtonTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(TextTooltipContent content)
	{
		if (string.IsNullOrEmpty(content.TTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.TTerm.Term;
		}
		_text.text = $"{content.TextTerm}{Environment.NewLine}{_termString}";
	}

	private void HandleItemTooltipContent(EventCardsUITooltipContent content)
	{
		if (content.Choice.GetChoiceType() == EPlayerChoice.ITEM)
		{
			_decisionName = content.Choice.GetItemValue().BaseStaticData.Name;
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.ATTRIBUTE)
		{
			Character character = content.Choice.Character;
			CharacterAttribute attributeValue = content.Choice.GetAttributeValue();
			if (character == null)
			{
				float value = CharacterAttribute.CalculateTeamAttributeValue(attributeValue.Id);
				_decisionName = $"{_crewAttributeTerm} {attributeValue.Name} ({attributeValue.GetAttributeLevel(value).Name})";
			}
			else if (character == CharacterManager.Instance.GetCharacterList().CharactersInGame[0])
			{
				float value2 = character.RuntimeData.GetAttributeById(content.Choice.GetAttributeValue().Id).Value;
				_decisionName = $"{_captainAttributeTerm} {attributeValue.Name} ({attributeValue.GetAttributeLevel(value2).Name})";
			}
			else
			{
				float value3 = character.RuntimeData.GetAttributeById(content.Choice.GetAttributeValue().Id).Value;
				_decisionName = $"{attributeValue.Name} ({attributeValue.GetAttributeLevel(value3).Name})";
			}
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.CHARACTER)
		{
			Character characterValue = content.Choice.GetCharacterValue();
			_decisionName = "<b>" + characterValue.StaticData.FullName + "</b>";
			if (characterValue.RuntimeData.HasStatusPreventingSelectionInEventChoices())
			{
				StringBuilder stringBuilder = new StringBuilder(_decisionName);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int i = 0; i < characterValue.RuntimeData.CurrentStatuses.Count; i++)
				{
					if (!characterValue.RuntimeData.CurrentStatuses[i].AllowsSelectionInEventChoice)
					{
						stringBuilder.AppendLine(string.Format("<color={1}>{0}</color>", characterValue.RuntimeData.CurrentStatuses[i].Name, _negativeColor));
					}
				}
				_decisionName = stringBuilder.ToString();
			}
			else
			{
				string decisionName = _decisionName;
				decisionName += Environment.NewLine;
				for (int j = 0; j < characterValue.RuntimeData.CurrentAttributes.Count; j++)
				{
					CharacterAttributeValue characterAttributeValue = characterValue.RuntimeData.CurrentAttributes[j];
					decisionName += Environment.NewLine;
					decisionName = string.Concat(decisionName, characterAttributeValue.Attribute.Name, ": ", characterAttributeValue.Attribute.GetAttributeLevel(characterAttributeValue.Value).Name);
				}
				_decisionName = decisionName;
			}
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.NO_CHOICE)
		{
			_decisionName = _noChoiceName;
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.NO)
		{
			_decisionName = _noDecisionName;
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.YES)
		{
			_decisionName = _yesDecisionName;
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.RESOURCE)
		{
			if (content.Choice.UseResource && content.Choice.GameResource.Amount > 0)
			{
				PlayerResources playerResources = Singleton<ItemManager>.Instance.GetPlayerResources();
				if (!content.Choice.Interactable)
				{
					_decisionName = $"{content.Choice.GameResource.Resource.Name}: <color=red>{playerResources.GetGameResourceById(content.Choice.GameResource.Resource.ID).Amount}/{content.Choice.GameResource.Amount}</color>";
				}
				else
				{
					_decisionName = $"{content.Choice.GameResource.Resource.Name}: <color=green>{content.Choice.GameResource.Amount}/{content.Choice.GameResource.Amount}</color>";
				}
			}
			else
			{
				_decisionName = content.Choice.GameResource.Resource.Name;
			}
		}
		else if (content.Choice.GetChoiceType() == EPlayerChoice.SPRITE)
		{
			if (content.Choice.Interactable)
			{
				_decisionName = content.Choice.ActionCondition.SelectableTerm;
			}
			else
			{
				_decisionName = content.Choice.ActionCondition.NotSelectableTerm;
			}
		}
		if (string.IsNullOrEmpty(content.EventCardsUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.EventCardsUITooltipTerm.Term;
		}
		_text.text = $"{_decisionName}{Environment.NewLine}{_termString}";
	}

	private void HandleItemTooltipContent(UIButtonTooltipContent content)
	{
		string empty = string.Empty;
		empty = ((!_confirmButton.interactable) ? ((string)content.InactiveButtonTerm) : ((string)content.ActiveButtonTerm));
		if (string.IsNullOrEmpty(content.ButtonUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.ButtonUITooltipTerm.Term;
		}
		_text.text = $"{empty}{Environment.NewLine}{_termString}";
	}
}
