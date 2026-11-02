public class AI_Component_ApplyInvurnerability : AI_Component
{
	public Network_Entity networkEntity;

	public override void OnEnterState()
	{
		base.OnEnterState();
		networkEntity.IsInvurnerable = true;
	}

	public override void OnExitState()
	{
		base.OnExitState();
		networkEntity.IsInvurnerable = false;
	}
}
