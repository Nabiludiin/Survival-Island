using System;
using I2.Loc;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class StaticSimpleData
{
	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private int _maxLevel;

	[SerializeField]
	private SystemEvent _upgradeEvent;

	[SerializeField]
	private CraftingOperation[] _upgradeCosts;

	[SerializeField]
	private bool _availableInCraftingDuringExpedition;

	public LocalizedString Name => _name;

	public int MaxLevel => _maxLevel;

	public Sprite Icon => _icon;

	public SystemEvent UpgradeEvent => _upgradeEvent;

	public CraftingOperation[] UpgradeCosts => _upgradeCosts;

	public bool AvailableInCraftingDuringExpedition => _availableInCraftingDuringExpedition;
}
