using System;
using I2.Loc;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Serializable]
public class ExpeditionStaticData
{
	[SerializeField]
	[Tooltip("Resources required to unlock Expeditions.")]
	private GameResource[] _requiredResources;

	[SerializeField]
	[Tooltip("Items required to unlock Expeditions.")]
	private IItem[] _requiredItems;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content that says that Player is not on the planet.")]
	private LocalizedString _blockedPanelNoPlanetTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content that says that Player is not on the planet.")]
	private LocalizedString _blockedPanelNoPlanetDescriptionTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDefaultTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelDefaultDescriptionTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelOngoingTitleTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content for current block reason.")]
	private LocalizedString _blockedPanelOngoingDescriptionTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content if expedition or suit is currently upgraded.")]
	private LocalizedString _blockedPanelUpgradeInProgressTerm;

	[SerializeField]
	[Tooltip("Term containing description for blocked expedition panel content if captain is alone.")]
	private LocalizedString _blockedPanelCaptainAloneTerm;

	[SerializeField]
	[Tooltip("Status to set for character when it's away on expedition.")]
	private CharacterStatus _awayOnExpeditionStatus;

	[SerializeField]
	[Tooltip("Status to set for character if it doesn't return from expedition.")]
	private CharacterStatus _missingOnExpeditionStatus;

	[SerializeField]
	private LocalizedString _name;

	[SerializeField]
	private int _maxLevel;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private SystemEvent _upgradeEvent;

	[SerializeField]
	private CraftingOperation[] _upgradeCosts;

	[SerializeField]
	private bool _availableForCraftingDuringExpedition;

	public LocalizedString BlockedPanelUpgradeInProgressTerm => _blockedPanelUpgradeInProgressTerm;

	public LocalizedString BlockedPanelCaptainAloneTerm => _blockedPanelCaptainAloneTerm;

	public GameResource[] RequiredResources => _requiredResources;

	public IItem[] RequiredItems => _requiredItems;

	public LocalizedString BlockedPanelNoPlanetTitleTerm => _blockedPanelNoPlanetTitleTerm;

	public LocalizedString BlockedPanelNoPlanetDescriptionTerm => _blockedPanelNoPlanetDescriptionTerm;

	public CharacterStatus AwayOnExpeditionStatus => _awayOnExpeditionStatus;

	public CharacterStatus MissingOnExpeditionStatus => _missingOnExpeditionStatus;

	public Sprite Icon => _icon;

	public SystemEvent UpgradeEvent => _upgradeEvent;

	public int MaxLevel => _maxLevel;

	public CraftingOperation[] UpgradeCosts => _upgradeCosts;

	public LocalizedString BlockedPanelDefaultTitleTerm => _blockedPanelDefaultTitleTerm;

	public LocalizedString BlockedPanelDefaultDescriptionTerm => _blockedPanelDefaultDescriptionTerm;

	public bool AvailableForCraftingDuringExpedition => _availableForCraftingDuringExpedition;

	public LocalizedString Name => _name;

	public LocalizedString BlockedPanelOngoingTitleTerm => _blockedPanelOngoingTitleTerm;

	public LocalizedString BlockedPanelOngoingDescriptionTerm => _blockedPanelOngoingDescriptionTerm;
}
