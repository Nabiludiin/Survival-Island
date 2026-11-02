public class Buff_Heal_Player : Buff_Heal
{
	private PlayerBuffManager playerBuffManager;

	private Network_Player networkPlayer;

	public override void OnBuffApplied(BuffManager buffmanager, SO_Buff buffAsset)
	{
		base.OnBuffApplied(buffmanager, buffAsset);
		playerBuffManager = buffmanager as PlayerBuffManager;
		networkPlayer = playerBuffManager.networkPlayer;
	}

	protected override void OnTick()
	{
		base.OnTick();
	}
}
