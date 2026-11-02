namespace RG.Parsecs.EventEditor;

public class PlayerYesNoDecision : PlayerDecision
{
	public bool Result { get; set; }

	public override string ToString()
	{
		return Result.ToString();
	}
}
