public class AI_State_PufferFish_CirculateWater : AI_State_CirculateSpawn_Water
{
	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override bool PlayerIsWithinRange()
	{
		if (GameModeValueManager.GetCurrentGameModeValue().pufferfishVariables.isTame)
		{
			return false;
		}
		return base.PlayerIsWithinRange();
	}

	public override void UpdateState()
	{
		Circulate();
		if (!GameModeValueManager.GetCurrentGameModeValue().pufferfishVariables.isTame)
		{
			Network_Player network_Player = Helper.ClosestPlayerInWaterToPoint(stateMachine.transform.position, playerDistance, includeDeadPlayers: false);
			if (network_Player != null && nextState != null && Helper.IsValidWaterTarget(network_Player))
			{
				stateMachine.ChangeState(nextState);
			}
		}
	}
}
