using System;
using I2.Loc;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public struct AttributeLevel
{
	[SerializeField]
	private SRange _range;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private Sprite _teamIcon;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private float _scavengeValue;

	public SRange Range => _range;

	public Sprite Icon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_icon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _icon;
		}
	}

	public Sprite TeamIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_teamIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _teamIcon;
		}
	}

	public LocalizedString Name => _name;

	public float ScavengeValue => _scavengeValue;
}
