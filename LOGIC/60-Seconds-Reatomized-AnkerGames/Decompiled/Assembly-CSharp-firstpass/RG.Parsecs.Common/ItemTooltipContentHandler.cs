using System;
using RG.Parsecs.Survival;
using TMPro;
using UnityEngine;

namespace RG.Parsecs.Common;

public class ItemTooltipContentHandler : TooltipContentHandler
{
	[Tooltip("Text handler of tooltip.")]
	[SerializeField]
	private TextMeshProUGUI _text;

	private string _nameInfo;

	private string _itemInfo;

	private string _termString;

	public override void HandleContent(TooltipContent content)
	{
		if ((bool)(content as ItemTooltipContent))
		{
			HandleItemTooltipContent((ItemTooltipContent)content);
		}
	}

	private void HandleItemTooltipContent(ItemTooltipContent content)
	{
		if (!(_text != null) || !(content.Item != null))
		{
			return;
		}
		if (content.Item is Item)
		{
			Item item = (Item)content.Item;
			if (item.RuntimeData.IsDamaged && !string.IsNullOrEmpty(content.DamagedItemName))
			{
				_itemInfo = $" <color=red>{content.DamagedItemName}</color=red>";
			}
			else
			{
				_itemInfo = string.Empty;
			}
			if (content.ItemNamesWithLevel != null && item.RuntimeData.Level - 1 < content.ItemNamesWithLevel.Count)
			{
				_nameInfo = content.ItemNamesWithLevel[item.RuntimeData.Level - 1];
			}
		}
		else if (content.Item is ConsumableRemedium)
		{
			ConsumableRemedium consumableRemedium = (ConsumableRemedium)content.Item;
			_itemInfo = string.Empty;
			if (content.ItemNamesWithLevel != null && consumableRemedium.RuntimeData.Level - 1 < content.ItemNamesWithLevel.Count)
			{
				_nameInfo = string.Format(content.ItemNamesWithLevel[consumableRemedium.RuntimeData.Level - 1], consumableRemedium.RuntimeData.Amount.ToString());
			}
		}
		else if (content.Item is Remedium)
		{
			Remedium remedium = (Remedium)content.Item;
			_itemInfo = string.Empty;
			if (content.ItemNamesWithLevel != null && remedium.RuntimeData.Level - 1 < content.ItemNamesWithLevel.Count)
			{
				_nameInfo = content.ItemNamesWithLevel[remedium.RuntimeData.Level - 1];
			}
		}
		if (string.IsNullOrEmpty(content.TooltipTerm.Term))
		{
			_termString = string.Empty;
		}
		else
		{
			_termString = content.TooltipTerm.Term;
		}
		_text.text = $"{_nameInfo} {_itemInfo}{Environment.NewLine}{_termString}";
	}
}
