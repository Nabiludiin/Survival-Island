using System;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class ExpeditionUITooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	[Tooltip("Panel confirm button.")]
	[SerializeField]
	private Button _confirmButton;

	[Tooltip("Screen confirm toggle.")]
	[SerializeField]
	private Toggle _reactivateToogle;

	private string _termString;

	private string _reasonString;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as TextTooltipContent))
		{
			HandleItemTooltipContent((TextTooltipContent)content);
		}
		else if ((bool)(content as UIButtonTooltipContent))
		{
			HandleItemTooltipContent((UIButtonTooltipContent)content);
		}
		else if ((bool)(content as ExpeditionCharacterUITooltipContent))
		{
			HandleItemTooltipContent((ExpeditionCharacterUITooltipContent)content);
		}
		else if ((bool)(content as ExpeditionItemUITooltipContent))
		{
			HandleItemTooltipContent((ExpeditionItemUITooltipContent)content);
		}
		else if ((bool)(content as ExpeditionButtonUITooltipContent))
		{
			HandleItemTooltipContent((ExpeditionButtonUITooltipContent)content);
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

	private void HandleItemTooltipContent(UIButtonTooltipContent content)
	{
		string empty = string.Empty;
		empty = ((!_reactivateToogle.interactable) ? ((string)content.InactiveButtonTerm) : ((string)content.ActiveButtonTerm));
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

	private void HandleItemTooltipContent(ExpeditionCharacterUITooltipContent content)
	{
		if (!content.CharacterSlot.Character.RuntimeData.HasStatusPreventingGoingOnExpeditions())
		{
			if (content.RegularTooltipTerm.IsNullOrEmpty())
			{
				_termString = string.Empty;
			}
			else
			{
				_termString = content.RegularTooltipTerm.ToString();
			}
			_text.text = $"{content.CharacterSlot.Character.StaticData.FullName}{Environment.NewLine}{_termString}";
			return;
		}
		_reasonString = string.Empty;
		for (int i = 0; i < content.TermPairs.Count; i++)
		{
			if (content.CharacterSlot.Character.RuntimeData.HasStatus(content.TermPairs[i].Status.Id))
			{
				_reasonString = content.TermPairs[i].Term.ToString();
			}
		}
		if (!content.CharacterSlot.Character.RuntimeData.IsAlive() && (string)content.DeadTooltipTerm != null && !content.DeadTooltipTerm.IsNullOrEmpty())
		{
			_reasonString = content.DeadTooltipTerm.ToString();
		}
		_text.text = $"{content.CharacterSlot.Character.StaticData.FullName}{Environment.NewLine}{_termString}{_reasonString}";
	}

	private void HandleItemTooltipContent(ExpeditionItemUITooltipContent content)
	{
		int currentItemIndex = content.ItemSlot.CurrentItemIndex;
		string empty = string.Empty;
		if (currentItemIndex >= 0)
		{
			empty = content.ItemList.Items[currentItemIndex].BaseStaticData.Name;
			content.ExpeditionItemUITooltipTerm.Term = content.ItemList.Items[currentItemIndex].BaseStaticData.ExpeditionDescription;
		}
		else
		{
			empty = content.NoItem;
			content.ExpeditionItemUITooltipTerm.Term = string.Empty;
		}
		if (string.IsNullOrEmpty(content.ExpeditionItemUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.ExpeditionItemUITooltipTerm.Term;
		}
		_text.text = $"{empty}{Environment.NewLine}{_termString}";
	}

	private void HandleItemTooltipContent(ExpeditionButtonUITooltipContent content)
	{
		string arg = string.Empty;
		if (_confirmButton.interactable && content.ExpeditionPanel.GetPanelStatus() != RepairedExpeditionPanelController.EExpeditionPanels.LOCKED && content.ExpeditionPanel.GetPanelStatus() != RepairedExpeditionPanelController.EExpeditionPanels.EQUIPMENT)
		{
			arg = content.ActiveConfirmButton;
		}
		else if (!_confirmButton.interactable && content.ExpeditionPanel.GetPanelStatus() == RepairedExpeditionPanelController.EExpeditionPanels.LOCKED)
		{
			arg = content.Locked;
		}
		else if (!_confirmButton.interactable && content.ExpeditionPanel.GetPanelStatus() == RepairedExpeditionPanelController.EExpeditionPanels.DESTINATION)
		{
			arg = content.ChooseDestiantion;
		}
		else if (!_confirmButton.interactable && content.ExpeditionPanel.GetPanelStatus() == RepairedExpeditionPanelController.EExpeditionPanels.CHARACTER)
		{
			arg = content.ChooseCharacter;
		}
		else if (_confirmButton.interactable && content.ExpeditionPanel.GetPanelStatus() == RepairedExpeditionPanelController.EExpeditionPanels.EQUIPMENT)
		{
			arg = content.SendExpedition;
		}
		if (string.IsNullOrEmpty(content.ExpeditionButtonUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.ExpeditionButtonUITooltipTerm.Term;
		}
		_text.text = $"{arg}{Environment.NewLine}{_termString}";
	}
}
