using UnityEngine;

[CreateAssetMenu(fileName = "SO_TradingPost_Buyable", menuName = "Raft/TradingPost/Buyable")]
public class SO_TradingPost_Buyable : ScriptableObject
{
	public class Instance
	{
		public TradingPost.Tier tier;

		public int stock = 1;

		public Cost reward;

		public Cost[] cost;
	}

	public int sortingOrder;

	public int startStock = 1;

	public TradingPost.Tier tier;

	public Cost reward;

	public Cost[] cost;

	public Instance CreateInstance()
	{
		Instance instance = new Instance();
		instance.reward = new Cost(reward.item, reward.amount);
		instance.cost = new Cost[cost.Length];
		cost.CopyTo(instance.cost, 0);
		instance.stock = startStock;
		instance.tier = tier;
		return instance;
	}
}
