namespace RG.Parsecs.Scavenge;

public class BottomCorridorWall : Wall
{
	public override bool CanBeTransparent
	{
		get
		{
			return true;
		}
		set
		{
		}
	}

	private void Start()
	{
		ShipGenerator.I.RegisterCorridorWall(this);
	}
}
