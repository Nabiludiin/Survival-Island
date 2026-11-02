public class AI_NetworkBehaviour_MamaBear : AI_NetworkBehaviour_Bear
{
	private AI_StateMachine_MamaBear stateMachineMamaBear;

	protected override void Awake()
	{
		base.Awake();
		stateMachineMamaBear = stateMachineAnimal as AI_StateMachine_MamaBear;
	}

	protected override void Start()
	{
		base.Start();
		stateMachineMamaBear.FindObjectOfInterests();
	}

	public override void OnRestoreStart(RGD_AI_NetworkBehaviour rgdNetworkBehaviour)
	{
		stateMachineMamaBear.FindObjectOfInterests();
		base.OnRestoreStart(rgdNetworkBehaviour);
	}
}
