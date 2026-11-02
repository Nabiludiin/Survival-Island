using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RG.Parsecs.Survival;

public class TextIconContent : EventContent
{
	[SerializeField]
	private Image _iconImage;

	[SerializeField]
	private TextMeshProUGUI _textMeshPro;

	[SerializeField]
	private EventContentData.ETextIconContentType _type;

	[SerializeField]
	private float _amount;

	[SerializeField]
	private bool _damaged;

	[SerializeField]
	private Resource _resource;

	[SerializeField]
	private IItem _item;

	public EventContentData.ETextIconContentType Type => _type;

	public float Amount => _amount;

	public Resource Resource => _resource;

	public IItem Item => _item;

	public void SetTextAndIcon(EventContentData.ETextIconContentType type, float amount, Resource resource)
	{
		_resource = resource;
		SetTextAndIcon(type, amount, resource.Icon);
	}

	public void SetTextAndIcon(EventContentData.ETextIconContentType type, float amount, IItem iItem)
	{
		_item = iItem;
		SetTextAndIcon(type, amount, iItem.BaseStaticData.IconJournal);
	}

	public void SetTextAndIcon(bool damaged, IItem iItem)
	{
		Item item = iItem as Item;
		Sprite icon = iItem.BaseStaticData.IconJournal;
		if ((item != null) & damaged)
		{
			icon = item.StaticData.DamagedIcon;
		}
		SetTextAndIcon(damaged, icon);
	}

	private void SetTextAndIcon(bool damaged, Sprite icon)
	{
		_iconImage.sprite = icon;
		_type = EventContentData.ETextIconContentType.NONE;
		_damaged = damaged;
		_textMeshPro.gameObject.SetActive(value: false);
		gameObject.SetActive(value: true);
	}

	private void SetTextAndIcon(EventContentData.ETextIconContentType type, float amount, Sprite icon)
	{
		_iconImage.sprite = icon;
		_type = type;
		_amount = amount;
		string arg = type switch
		{
			EventContentData.ETextIconContentType.NONE => "", 
			EventContentData.ETextIconContentType.ADDITION => "+", 
			EventContentData.ETextIconContentType.SUBTRACTION => "-", 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		_textMeshPro.text = $"{arg}{(((double)Math.Abs(amount - 1f) > 0.001) ? amount.ToString() : string.Empty)}";
		gameObject.SetActive(Math.Abs(amount) > 0.001f);
	}
}
