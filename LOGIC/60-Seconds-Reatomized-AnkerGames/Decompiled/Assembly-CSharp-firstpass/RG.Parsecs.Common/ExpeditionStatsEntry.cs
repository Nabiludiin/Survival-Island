using System;
using System.Collections.Generic;
using System.Text;

namespace RG.Parsecs.Common;

[Serializable]
public class ExpeditionStatsEntry : StatsEntry
{
	public bool IsSuccessful;

	public string DestinationId;

	public string CharacterId;

	public List<string> IdsOfItemsTaken;

	public override string ToString()
	{
		string text = string.Empty;
		if (IdsOfItemsTaken != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < IdsOfItemsTaken.Count; i++)
			{
				stringBuilder.Append(IdsOfItemsTaken[i]);
				if (i != IdsOfItemsTaken.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			text = stringBuilder.ToString();
		}
		return $"Expedition entry. Day: {Day}, Destination: {DestinationId}, Successful: {IsSuccessful}, Character: {CharacterId}, Items taken: [{text}]\n";
	}
}
