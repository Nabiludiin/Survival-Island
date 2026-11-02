using RG.Parsecs.Survival;
using RG.SecondsRemaster.Survival;
using UnityEngine;

namespace RG.SecondsRemaster.Core;

[CreateAssetMenu(menuName = "60 Seconds Remaster!/Crafting/New consumable Remedium", fileName = "New Consumable Remedium")]
public class SecondsConsumableRemedium : ConsumableRemedium
{
	private const float WHOLE_CONSUMABLE_AMOUNT = 1f;

	[SerializeField]
	private IconSizeDefinition _iconSizeDefinition;

	public IconSizeDefinition IconSizeDefinition => _iconSizeDefinition;

	public override void Use()
	{
		RuntimeData.Amount--;
		if (RuntimeData.Amount < 0f)
		{
			RuntimeData.Amount = 0f;
		}
	}
}
