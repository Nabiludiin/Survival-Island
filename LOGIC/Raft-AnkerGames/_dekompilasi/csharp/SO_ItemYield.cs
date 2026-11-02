using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemYield", menuName = "Raft/Item/ItemYield")]
public class SO_ItemYield : ScriptableObject
{
	public List<Cost> yieldAssets;
}
