public class AI_NetworkBehaviour_ButlerBot : AI_NetworkBehaviour_Animal
{
	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		if (ConnectedToSpawner)
		{
			return new RGD_AI_NetworkBehaviour(this);
		}
		return null;
	}

	public override RGD Serialize_Save()
	{
		if (!ConnectedToSpawner)
		{
			return new RGD_AI_NetworkBehaviour(this);
		}
		return null;
	}
}
