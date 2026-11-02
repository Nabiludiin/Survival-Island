using I2.Loc;

namespace RG.Parsecs.Survival;

internal struct CraftingDataWrapper
{
	public bool IsActive;

	public bool IsRepaired;

	public bool IsPlanned;

	public bool IsOngoing;

	public bool ShouldReactive;

	public bool IsFocusedByDesign;

	public int CraftingLevel;

	public int CraftingTotalTime;

	public int CraftingCurrentTime;

	public LocalizedString BlockedPanelTitleTerm;

	public LocalizedString BlockedPanelDescriptionTerm;

	public CraftingOperationWrapper PlannedAction;

	public CraftingOperationWrapper OngoingAction;
}
