using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Survival;

[CreateAssetMenu(menuName = "60 Parsecs!/New Threats List", fileName = "New Threats List")]
public class ThreatsList : ScriptableObject
{
	[SerializeField]
	private List<ExpeditionThreat> _expeditionThreats = new List<ExpeditionThreat>();

	public List<ExpeditionThreat> ExpeditionThreats => _expeditionThreats;

	public ExpeditionThreat GetExpeditionThreatOfId(string id)
	{
		for (int i = 0; i < _expeditionThreats.Count; i++)
		{
			if (_expeditionThreats[i].StaticData.Id.Equals(id))
			{
				return _expeditionThreats[i];
			}
		}
		return null;
	}
}
