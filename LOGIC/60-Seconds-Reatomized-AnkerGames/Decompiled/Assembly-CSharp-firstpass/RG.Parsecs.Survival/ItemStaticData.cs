using System;
using System.Collections.Generic;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ItemStaticData : IItemStaticData
{
	[SerializeField]
	private int _durability;

	[SerializeField]
	private bool _isDamaged;

	[SerializeField]
	private List<CraftingOperation> _repairCost;

	[SerializeField]
	private SystemEvent _repairEvent;

	[SerializeField]
	private List<int> _durabilites;

	[SerializeField]
	private Sprite _damageIcon;

	public int Durability => _durability;

	public bool IsDamaged => _isDamaged;

	public List<CraftingOperation> RepairCost => _repairCost;

	public SystemEvent RepairEvent => _repairEvent;

	public List<int> Durabilites => _durabilites;

	public Sprite DamagedIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_damageIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _damageIcon;
		}
	}
}
