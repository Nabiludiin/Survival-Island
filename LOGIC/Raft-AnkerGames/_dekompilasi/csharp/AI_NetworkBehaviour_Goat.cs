public class AI_NetworkBehaviour_Goat : AI_NetworkBehaviour_Domestic_Resource
{
	protected override void ResourceReady()
	{
		base.ResourceReady();
		stateMachine_Domestic.animController.SetFloat("Udder", 1f);
	}

	protected override void ResourceNotReady()
	{
		base.ResourceNotReady();
		stateMachine_Domestic.animController.SetFloat("Udder", 0f);
	}
}
