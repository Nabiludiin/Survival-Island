using System;
using UnityEngine;

[Serializable]
public class Buff
{
	[SerializeField]
	protected SO_Buff buffAsset;

	protected BuffManager buffManager;

	private float durationTimer;

	private BuffDisplayObject buffDisplayObject;

	public SO_Buff Asset => buffAsset;

	public float NormalizedTimer => durationTimer / buffAsset.BuffDuration;

	public virtual void OnBuffApplied(BuffManager buffmanager, SO_Buff buffAsset)
	{
		buffManager = buffmanager;
		this.buffAsset = buffAsset;
		PlayerBuffManager playerBuffManager = buffmanager as PlayerBuffManager;
		if (playerBuffManager != null && playerBuffManager.player.IsLocalPlayer)
		{
			if (buffAsset.BuffDisplayType != BuffDisplayType.None)
			{
				buffDisplayObject = ComponentManager<BuffDisplayer>.Value.DisplayBuff(buffAsset.BuffDisplayType, buffAsset.BuffDuration, 0f);
			}
			if (buffAsset.ModifyMovementSpeed)
			{
				playerBuffManager.player.PersonController.GroundVelocityModifier += buffAsset.GroundMovementModifier;
				playerBuffManager.player.PersonController.WaterVelocityModifier += buffAsset.WaterMovementModifier;
			}
			if (buffAsset.ApplyInvurnerability)
			{
				playerBuffManager.player.Stats.IsInvurnerable = true;
			}
			if (buffAsset.ApplyDeathPrevention)
			{
				playerBuffManager.player.Stats.hasDeathPrevention = true;
			}
			if (buffAsset.ModifyOxygen)
			{
				float oxygenLostPerSecond = playerBuffManager.player.Stats.stat_oxygen.GetOxygenLostPerSecond();
				playerBuffManager.player.Stats.stat_oxygen.SetOxygenLostPerSecond(oxygenLostPerSecond * buffAsset.OxygenModifier);
			}
		}
	}

	public virtual void OnBuffRemoved(BuffManager buffmanager)
	{
		PlayerBuffManager playerBuffManager = buffmanager as PlayerBuffManager;
		if (!(playerBuffManager != null))
		{
			return;
		}
		if (buffAsset.BuffDisplayType != BuffDisplayType.None && buffDisplayObject != null)
		{
			buffDisplayObject.Expire();
		}
		if (playerBuffManager.player.IsLocalPlayer)
		{
			if (buffAsset.ModifyMovementSpeed)
			{
				playerBuffManager.player.PersonController.GroundVelocityModifier -= buffAsset.GroundMovementModifier;
				playerBuffManager.player.PersonController.WaterVelocityModifier -= buffAsset.WaterMovementModifier;
			}
			if (buffAsset.ApplyInvurnerability)
			{
				playerBuffManager.player.Stats.IsInvurnerable = false;
			}
			if (buffAsset.ApplyDeathPrevention)
			{
				playerBuffManager.player.Stats.hasDeathPrevention = false;
			}
			if (buffAsset.ModifyOxygen)
			{
				float oxygenLostPerSecond = playerBuffManager.player.Stats.stat_oxygen.GetOxygenLostPerSecond();
				playerBuffManager.player.Stats.stat_oxygen.SetOxygenLostPerSecond(oxygenLostPerSecond / buffAsset.OxygenModifier);
			}
		}
	}

	public virtual void UpdateBuff(BuffUpdateResult result)
	{
	}

	public bool UpdateTimer()
	{
		durationTimer += Time.deltaTime;
		return durationTimer < buffAsset.BuffDuration + 0.1f;
	}

	public void ResetDuration()
	{
		durationTimer = 0f;
		RefreshBuffUI();
	}

	public void SetDurationComplete()
	{
		durationTimer = buffAsset.BuffDuration + 0.15f;
		RefreshBuffUI();
	}

	public void SetNormalizedTimer(float normalizedTime)
	{
		durationTimer = buffAsset.BuffDuration * Mathf.Clamp01(normalizedTime);
		RefreshBuffUI();
	}

	private void RefreshBuffUI()
	{
		if (buffAsset.BuffDisplayType != BuffDisplayType.None && buffDisplayObject != null)
		{
			buffDisplayObject.SetNewDurationAndProgress(buffAsset.BuffDuration, durationTimer);
		}
	}
}
