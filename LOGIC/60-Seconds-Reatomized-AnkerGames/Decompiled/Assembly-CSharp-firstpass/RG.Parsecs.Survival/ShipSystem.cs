using I2.Loc;
using RG.Core.Base;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class ShipSystem : RGScriptableObject
{
	public abstract LocalizedString Name { get; }

	public abstract string Id { get; }

	public abstract int SystemLevel { get; set; }

	public abstract bool IsUpgradable { get; }

	public abstract Sprite Icon { get; }

	public abstract SystemEvent UpgradeEvent { get; }

	public abstract bool IsAvailable { get; set; }

	public abstract bool IsFocusedByDesign { get; }

	public abstract bool AvailableInCraftingDuringExpedition { get; }

	public abstract CraftingOperation GetUpgradeCostForCurrentLevel();

	public abstract void Upgrade();

	public abstract void SetDisabledReason(LocalizedString title, LocalizedString reason);

	public abstract void Downgrade();

	public abstract int MaxLevel();
}
