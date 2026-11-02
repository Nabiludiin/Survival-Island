using System;

[Serializable]
public class Cost
{
	public Item_Base item;

	public int amount;

	public Cost()
	{
	}

	public Cost(Item_Base item, int amount)
	{
		this.item = item;
		this.amount = amount;
	}

	public Cost(Cost copy)
	{
		item = copy.item;
		amount = copy.amount;
	}
}
