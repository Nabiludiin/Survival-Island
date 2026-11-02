using UnityEngine;

[CreateAssetMenu(fileName = "SO_TradingPost_Sellable", menuName = "Raft/TradingPost/Sellable")]
public class SO_TradingPost_Sellable : ScriptableObject
{
	public int sortingOrder;

	public Item_Base sellableItem;

	public int reputationReward;

	public Cost reward;
}
