using System;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ExpeditionTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _systemStatusString;

	private string _levelNameString;

	private string _termString;

	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _statusText;

	private string _suitLevelNameString;

	private string _suitTermString;

	[SerializeField]
	private bool _displaySuitInfo;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as ExpeditionTooltipContent))
		{
			HandleItemTooltipContent((ExpeditionTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(ExpeditionTooltipContent content)
	{
		if (content.Data.RuntimeData.IsRepaired && content.Data.IsAvailable && content.Data.RuntimeData.CurrentPlanetDeck != null && !content.Data.RuntimeData.IsOngoing)
		{
			_systemStatusString = string.Empty;
		}
		else if (!content.Data.RuntimeData.IsRepaired)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=red>{content.Damaged}</color=red>";
		}
		else if (content.Data.RuntimeData.CurrentPlanetDeck == null)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=yellow>{content.Unavailable}</color=yellow>";
		}
		else if ((content.Data.RuntimeData.IsRepaired && !content.Data.IsAvailable) || content.Data.RuntimeData.IsOngoing)
		{
			_systemStatusString = $"{Environment.NewLine}{content.Status}: <color=yellow>{content.Busy}</color=yellow>";
		}
		if (content.ExpeditionNamesWithLevel != null && content.Data.SystemLevel - 1 < content.ExpeditionNamesWithLevel.Count)
		{
			_levelNameString = content.ExpeditionNamesWithLevel[content.Data.SystemLevel - 1];
		}
		if (string.IsNullOrEmpty(content.ExpeditionTooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.ExpeditionTooltipTerm.Term;
		}
		if (_displaySuitInfo && content.SuitLevelName != null && content.SuitData.SystemLevel - 1 < content.SuitLevelName.Count)
		{
			_suitLevelNameString = content.SuitLevelName[content.SuitData.SystemLevel - 1];
		}
		if (_displaySuitInfo)
		{
			if (string.IsNullOrEmpty(content.SuitTooltipTerm.Term))
			{
				_suitTermString = string.Empty;
			}
			else
			{
				_suitTermString = content.SuitTooltipTerm.Term;
			}
		}
		if (_displaySuitInfo)
		{
			_text.text = $"{_levelNameString}{Environment.NewLine}{_termString}{Environment.NewLine}{_suitLevelNameString}{Environment.NewLine}{_suitTermString}";
			_statusText.text = $"{_systemStatusString}{Environment.NewLine}";
		}
		else
		{
			_text.text = $"{_levelNameString}{_systemStatusString}";
			_statusText.text = string.Empty;
		}
	}
}
