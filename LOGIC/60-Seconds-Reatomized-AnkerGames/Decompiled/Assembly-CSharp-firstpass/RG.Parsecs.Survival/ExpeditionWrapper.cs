using System.Collections.Generic;
using I2.Loc;

namespace RG.Parsecs.Survival;

internal struct ExpeditionWrapper
{
	public bool IsActive;

	public bool IsPlanned;

	public bool IsOngoing;

	public bool IsRepaired;

	public bool IsFocusedByDesign;

	public int ElapsedExpeditionTime;

	public int Level;

	public bool ShouldReactivate;

	public string CurrentPlanetDeck;

	public ExpeditionDataWrapper PlannedExpedition;

	public ExpeditionDataWrapper OngoingExpedition;

	public LocalizedString BlockedPanelTitleTerm;

	public LocalizedString BlockedPanelDescriptionTerm;

	public List<string> Destinations;
}
