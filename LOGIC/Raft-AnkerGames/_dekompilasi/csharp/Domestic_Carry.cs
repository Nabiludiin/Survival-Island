public class Domestic_Carry : Carry
{
	public TrophyTextwriter nameTagWriter;

	protected override void HandleBeingCarried()
	{
		base.HandleBeingCarried();
		if (nameTagWriter != null)
		{
			nameTagWriter.DisplayRenamePrompt(value: true, "RenameAnimal", 1, clearOtherText: false, "Game/RenamePet");
			nameTagWriter.CheckForInteractInput("RenameAnimal");
		}
	}

	protected override void HandleCarryInput()
	{
		if (!(nameTagWriter != null) || !nameTagWriter.GetIsBeingEdited())
		{
			base.HandleCarryInput();
		}
	}
}
