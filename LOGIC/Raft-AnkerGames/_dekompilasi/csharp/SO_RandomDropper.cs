using UnityEngine;

[CreateAssetMenu(fileName = "RandomDropper", menuName = "Raft/Item/RandomDropper")]
public class SO_RandomDropper : ScriptableObject
{
	public Randomizer randomizer;

	public Item_Base[] GetRandomItems(int amountOfItems)
	{
		if (randomizer != null)
		{
			return randomizer.GetRandomItems<Item_Base>(amountOfItems);
		}
		return new Item_Base[amountOfItems];
	}

	public Item_Base GetRandomItem()
	{
		if (randomizer != null)
		{
			return randomizer.GetRandomItem<Item_Base>();
		}
		return null;
	}
}
