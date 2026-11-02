using System;
using System.Collections.Generic;

namespace RG.Parsecs.Survival;

[Serializable]
public class CraftingOperation
{
	public int MinDays = 1;

	public int MaxDays = 1;

	public List<GameResource> Resources;

	public int RequiredSystemLevel = 1;

	public bool AvailableOnlyOnRequiredSystemLevel;
}
