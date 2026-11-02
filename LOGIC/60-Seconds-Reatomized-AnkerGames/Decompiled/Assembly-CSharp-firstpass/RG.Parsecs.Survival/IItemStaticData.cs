using System;
using System.Collections.Generic;
using I2.Loc;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class IItemStaticData
{
	[SerializeField]
	private string _itemId;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private LocalizedString _upgradedName;

	[SerializeField]
	private LocalizedString _description;

	[SerializeField]
	private LocalizedString _expeditionDescription;

	[SerializeField]
	private int _maxLevel = 1;

	[SerializeField]
	private Sprite _buttonIcon;

	[SerializeField]
	private Sprite _iconJournal;

	[SerializeField]
	private LocalizedString _iconTerm;

	[SerializeField]
	private int _level = 1;

	[SerializeField]
	private bool _isAvailable;

	[SerializeField]
	private List<int> _weights;

	[SerializeField]
	private CraftingOperation _createCost;

	[SerializeField]
	private List<CraftingOperation> _upgradeCosts;

	[SerializeField]
	private SystemEvent _createEvent;

	[SerializeField]
	private SystemEvent _interruptCraftingEvent;

	[SerializeField]
	private SystemEvent _upgradeEvent;

	[SerializeField]
	private SystemEvent _recyclingEvent;

	[SerializeField]
	private List<CraftingOperation> _recyclingCosts;

	public int MaxLevel => _maxLevel;

	public string ItemId => _itemId;

	public Sprite ButtonIcon
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_buttonIcon.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _buttonIcon;
		}
	}

	public int Level => _level;

	public bool IsAvailable => _isAvailable;

	public LocalizedString Name => _name;

	public LocalizedString UpgradedName => _upgradedName;

	public LocalizedString Description => _description;

	public LocalizedString ExpeditionDescription => _expeditionDescription;

	public List<int> Weights => _weights;

	public int BaseWeight
	{
		get
		{
			if (_weights.Count == 0)
			{
				Debug.LogError("No weights defined for item " + _name);
			}
			return _weights[0];
		}
	}

	public List<CraftingOperation> RecyclingCosts => _recyclingCosts;

	public SystemEvent RecyclingEvent => _recyclingEvent;

	public SystemEvent UpgradeEvent => _upgradeEvent;

	public SystemEvent CreateEvent => _createEvent;

	public List<CraftingOperation> UpgradeCosts => _upgradeCosts;

	public CraftingOperation CreateCost => _createCost;

	public Sprite IconJournal
	{
		get
		{
			Sprite sprite = ContentManager.GetSprite(_iconJournal.name);
			if (sprite != null)
			{
				return sprite;
			}
			return _iconJournal;
		}
	}

	public SystemEvent InterruptCraftingEvent => _interruptCraftingEvent;

	public LocalizedString IconTerm => _iconTerm;
}
