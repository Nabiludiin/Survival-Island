using System;

[Serializable]
public class Buff_Poison_Player : Buff_Poison
{
	private PlayerBuffManager playerBuffManager;

	private Network_Player networkPlayer;

	public override void OnBuffApplied(BuffManager buffmanager, SO_Buff buffAsset)
	{
		base.OnBuffApplied(buffmanager, buffAsset);
		playerBuffManager = buffmanager as PlayerBuffManager;
		networkPlayer = playerBuffManager.networkPlayer;
	}

	protected override void TickDamage()
	{
		base.TickDamage();
		if (networkPlayer != null && networkPlayer.IsLocalPlayer)
		{
			ScreenEffectManager screenEffectManager = playerBuffManager.screenEffectManager;
			switch (buffAsset.BuffType)
			{
			case BuffType.Poison:
				screenEffectManager.screenImage_Poison.Enable(playerBuffManager, screenEffectManager.screenEffect_Poison);
				break;
			case BuffType.Radiation:
				screenEffectManager.screenImage_Radiation.Enable(playerBuffManager, screenEffectManager.screenEffect_Radiation);
				break;
			}
		}
		AudioHelper.PlayOneShotWithParameters(buffAsset.er_sound, networkPlayer.transform.position, ("damage", 1f));
	}
}
