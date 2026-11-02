using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_TradingPost_Sorter", menuName = "Raft/TradingPost/Sorter")]
public class SO_TradingPostSorter : ScriptableObject
{
	[SerializeField]
	private List<SO_TradingPost_Buyable> tier1 = new List<SO_TradingPost_Buyable>();

	[SerializeField]
	private List<SO_TradingPost_Buyable> tier2 = new List<SO_TradingPost_Buyable>();

	[SerializeField]
	private List<SO_TradingPost_Buyable> tier3 = new List<SO_TradingPost_Buyable>();
}
