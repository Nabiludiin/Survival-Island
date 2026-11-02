using System;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class ScavangeItemDataEntry
{
	public ScavangeItemData scavangeItems;

	public IntRange amount;

	public int GetAmount()
	{
		return amount.RandomValue;
	}
}
