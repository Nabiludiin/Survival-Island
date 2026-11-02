using System;
using I2.Loc;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class CraftingStaticData
{
	[SerializeField]
	private bool _isActive;

	[SerializeField]
	private bool _isRepaired;

	[SerializeField]
	private int _maxLevel;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private SystemEvent _upgradeEvent;

	[SerializeField]
	private CraftingOperation[] _upgradeCosts;

	[SerializeField]
	private IItem[] _requiredItems;

	[SerializeField]
	private GameResource[] _requiredResources;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private bool _availableInCraftingDuringExpedition;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDefaultTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDefaultDescriptionTerm;

	public bool IsActive => _isActive;

	public bool IsRepaired => _isRepaired;

	public int MaxLevel => _maxLevel;

	public Sprite Icon => _icon;

	public SystemEvent UpgradeEvent => _upgradeEvent;

	public CraftingOperation[] UpgradeCosts => _upgradeCosts;

	public IItem[] RequiredItems => _requiredItems;

	public GameResource[] RequiredResources => _requiredResources;

	public LocalizedString BlockedPanelDefaultTitleTerm => _blockedPanelDefaultTitleTerm;

	public LocalizedString BlockedPanelDefaultDescriptionTerm => _blockedPanelDefaultDescriptionTerm;

	public bool AvailableInCraftingDuringExpedition => _availableInCraftingDuringExpedition;

	public LocalizedString Name => _name;
}
