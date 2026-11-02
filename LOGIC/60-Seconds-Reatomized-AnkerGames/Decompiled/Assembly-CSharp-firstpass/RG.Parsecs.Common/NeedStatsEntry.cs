using System;

namespace RG.Parsecs.Common;

[Serializable]
public class NeedStatsEntry : StatsEntry
{
	public float Value;

	public string CharacterId;

	public string NeedId;

	public override string ToString()
	{
		return $"Need entry. Day: {Day}, Character: {CharacterId}, Need: {NeedId}, New Value: {Value}\n";
	}
}
