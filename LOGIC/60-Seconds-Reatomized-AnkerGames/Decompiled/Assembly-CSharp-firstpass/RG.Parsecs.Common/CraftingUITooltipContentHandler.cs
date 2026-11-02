using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Common;

public class CraftingUITooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	[Tooltip("Panel confirm button.")]
	[SerializeField]
	private Button _confirmButton;

	[Tooltip("Screen confirm toggle.")]
	[SerializeField]
	private Toggle _reactivateToggle;

	private string _termString;

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
		else if ((bool)(content as CraftingButtonUITooltipContent))
		{
			HandleItemTooltipContent((CraftingButtonUITooltipContent)content);
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
		if (string.IsNullOrEmpty(content.ButtonUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.ButtonUITooltipTerm.Term;
		}
		string empty = string.Empty;
		empty = ((!_reactivateToggle.interactable) ? ((string)content.InactiveButtonTerm) : ((string)content.ActiveButtonTerm));
		_text.text = $"{empty}{Environment.NewLine}{_termString}";
	}

	private void HandleItemTooltipContent(CraftingButtonUITooltipContent content)
	{
		if (string.IsNullOrEmpty(content.CraftingButtonUITooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.CraftingButtonUITooltipTerm.Term;
		}
		string arg = string.Empty;
		if (_confirmButton.interactable)
		{
			arg = content.CraftingActiveButtonTerm;
		}
		else if (!_confirmButton.interactable && !content.Data.RuntimeData.IsOngoing)
		{
			arg = content.CraftingInactiveButtonTerm;
		}
		else if (!_confirmButton.interactable && content.Data.RuntimeData.IsOngoing)
		{
			arg = content.CraftingBusyButtonTerm;
		}
		_text.text = $"{arg}{Environment.NewLine}{_termString}";
	}
}
