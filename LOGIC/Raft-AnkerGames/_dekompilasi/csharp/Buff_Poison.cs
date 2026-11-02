using System;
using UnityEngine;

[Serializable]
public class Buff_Poison : Buff
{
	private SO_Buff_DOT buffAssetDot;

	private float tickTimer;

	public override void OnBuffApplied(BuffManager buffmanager, SO_Buff buffAsset)
	{
		base.OnBuffApplied(buffmanager, buffAsset);
		buffAssetDot = buffAsset as SO_Buff_DOT;
	}

	public override void UpdateBuff(BuffUpdateResult result)
	{
		base.UpdateBuff(result);
		if (!(buffAssetDot == null))
		{
			tickTimer += Time.deltaTime;
			if (tickTimer >= buffAssetDot.TickRate)
			{
				tickTimer = 0f;
				result.damageTaken += buffAssetDot.TickDamage;
				TickDamage();
			}
		}
	}

	protected virtual void TickDamage()
	{
	}
}
