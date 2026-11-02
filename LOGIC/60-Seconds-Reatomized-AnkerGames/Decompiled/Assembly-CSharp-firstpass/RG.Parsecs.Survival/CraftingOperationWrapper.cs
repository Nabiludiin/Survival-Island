using System;

namespace RG.Parsecs.Survival;

[Serializable]
internal struct CraftingOperationWrapper
{
	public EPlannedCraftingAction CraftingAction;

	public string ItemID;

	public string SystemID;
}
