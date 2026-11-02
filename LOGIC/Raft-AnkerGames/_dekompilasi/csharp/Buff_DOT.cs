using UnityEngine;

public class Buff_DOT : Buff
{
	protected SO_Buff_DOT buffAssetDot;

	private float tickTimer;

	private int elapsedTicks;

	public int TotalDamageDealt => elapsedTicks * buffAssetDot.TickDamage;

	public int TotalDamageRemaining => buffAssetDot.TotalDotDamage - TotalDamageDealt;

	public override void OnBuffApplied(BuffManager buffmanager, SO_Buff buffAsset)
	{
		base.OnBuffApplied(buffmanager, buffAsset);
		buffAssetDot = buffAsset as SO_Buff_DOT;
	}

	public override void UpdateBuff(BuffUpdateResult result)
	{
		base.UpdateBuff(result);
		tickTimer += Time.deltaTime;
		if (tickTimer >= buffAssetDot.TickRate)
		{
			tickTimer = 0f;
			result.damageTaken += buffAssetDot.TickDamage;
			OnTick();
		}
		if (TotalDamageRemaining < 0)
		{
			result.estimatedDamage += TotalDamageRemaining;
		}
	}

	protected virtual void OnTick()
	{
		elapsedTicks++;
	}
}
