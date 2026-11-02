public class AI_NetworkBehaviour_Bear : AI_NetworkBehaviour_Animal
{
	public override RGD_AI_NetworkBehaviour Create_SaveData()
	{
		return new RGD_AI_NetworkBehaviour(this);
	}
}
