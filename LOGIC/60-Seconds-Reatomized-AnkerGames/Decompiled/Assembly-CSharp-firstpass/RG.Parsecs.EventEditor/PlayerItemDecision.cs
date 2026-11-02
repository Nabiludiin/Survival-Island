using RG.Parsecs.Survival;

namespace RG.Parsecs.EventEditor;

public class PlayerItemDecision : PlayerDecision
{
	public IItem Result;

	public override string ToString()
	{
		return Result.ToString();
	}
}
