using System;
using I2.Loc;
using RG.Core.Base;
using RG.Parsecs.Common;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public abstract class CharacterProperty : RGScriptableObject
{
	[HideInInspector]
	[SerializeField]
	private string _id = string.Empty;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private LocalizedString _description;

	[SerializeField]
	private Sprite _icon;

	public string Id => Guid;

	public LocalizedString Name => _name;

	public LocalizedString Description => _description;

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

	public bool Is(string id)
	{
		return Id.Equals(id);
	}

	public override bool Equals(object other)
	{
		if (other is CharacterProperty)
		{
			return Guid.Equals(((CharacterProperty)other).Guid);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Guid.GetHashCode();
	}
}
