using RG.Parsecs.Survival;

namespace RG.Parsecs.EventEditor;

public class PlayerAttributeDecision : PlayerDecision
{
	public CharacterAttribute Result { get; set; }

	public Character Character { get; set; }

	public bool IsTeam { get; set; }

	public override string ToString()
	{
		return Result.Name.ToString();
	}
}
