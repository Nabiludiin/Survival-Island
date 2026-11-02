using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/New Destinations List", fileName = "New Destinations List")]
public class DestinationsList : ScriptableObject
{
	[SerializeField]
	private List<ExpeditionDestination> _expeditonDestinations = new List<ExpeditionDestination>();

	public List<ExpeditionDestination> ExpeditonDestinations => _expeditonDestinations;

	public ExpeditionDestination GetExpeditionDestinationOfId(string id)
	{
		for (int i = 0; i < _expeditonDestinations.Count; i++)
		{
			if (_expeditonDestinations[i].ID.Equals(id))
			{
				return _expeditonDestinations[i];
			}
		}
		return null;
	}
}
