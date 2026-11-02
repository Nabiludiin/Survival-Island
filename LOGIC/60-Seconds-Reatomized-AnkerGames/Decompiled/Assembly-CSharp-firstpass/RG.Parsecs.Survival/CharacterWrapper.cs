using System.Collections.Generic;
using I2.Loc;

namespace RG.Parsecs.Survival;

internal struct CharacterWrapper
{
	public string ID;

	public bool IsCaptain;

	public List<string> CurrentStatusesID;

	public List<string> PreviousStatusesID;

	public List<NeedWrapper> Needs;

	public bool IsLocked;

	public bool ShowsEndOfGameSlide;

	public List<LocalizedString> History;

	public List<AttributeWrapper> Attributes;
}
