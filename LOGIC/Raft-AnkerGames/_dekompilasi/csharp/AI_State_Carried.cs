public class AI_State_Carried : AI_State
{
	private AI_NetworkBehaviour_Domestic domesticBehaviour;

	protected override void Awake()
	{
		base.Awake();
		domesticBehaviour = GetComponentInParent<AI_NetworkBehaviour_Domestic>();
	}

	public override void EnterState()
	{
		base.EnterState();
		domesticBehaviour.IsTamed = true;
		if (domesticBehaviour.connectedSpawner != null)
		{
			domesticBehaviour.connectedSpawner.DisconnectCurrentEntityFromSpawner();
		}
	}
}
