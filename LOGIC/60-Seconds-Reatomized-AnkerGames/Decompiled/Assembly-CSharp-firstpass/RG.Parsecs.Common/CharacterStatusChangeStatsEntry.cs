using System;

namespace RG.Parsecs.Common;

[Serializable]
public class CharacterStatusChangeStatsEntry : StatsEntry
{
	public string CharacterId;

	public bool IsCaptain;

	public bool StatusGained;

	public string ChangedStatusId;

	public override string ToString()
	{
		return $"Status change entry. Day: {Day}, Character: {CharacterId}, Is Captain: {IsCaptain}, Was Status Gained: {StatusGained}, Status: {ChangedStatusId}\n";
	}
}
