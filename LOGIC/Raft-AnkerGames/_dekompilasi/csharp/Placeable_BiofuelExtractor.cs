using System;
using FMODUnity;
using UnityEngine;

public class Placeable_BiofuelExtractor : Placeable_Extractor
{
	public Tank honeyTank;

	[SerializeField]
	private FillContainerComponent fillContainer_honey;

	[SerializeField]
	private int honeyDrainAtMerge = 1;

	[SerializeField]
	private ParticleSystem smokeParticles;

	[SerializeField]
	private ParticleSystem dripParticles;

	[SerializeField]
	private ParticleSystem smokeParticlesExhaust;

	[SerializeField]
	private Renderer rendererAnimatedHose;

	[SerializeField]
	private StudioEventEmitter emitter_drip;

	[SerializeField]
	private float dripRate = 0.5f;

	[SerializeField]
	private float dripTimer;

	private MaterialPropertyBlock animatedHoseProperty;

	protected override void OnBlockPlaced()
	{
		if (!hasBeenPlaced)
		{
			allTanks[2] = honeyTank;
			Tank tank = honeyTank;
			tank.OnTankAmountChange = (Action)Delegate.Combine(tank.OnTankAmountChange, new Action(HoneyTankChangedCall));
		}
		base.OnBlockPlaced();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		Tank tank = honeyTank;
		tank.OnTankAmountChange = (Action)Delegate.Remove(tank.OnTankAmountChange, new Action(HoneyTankChangedCall));
	}

	protected override void Update()
	{
		base.Update();
		bool flag = CanProcessTanks();
		HandleParticles(flag);
		HandleSounds(flag);
	}

	private void HoneyTankChangedCall()
	{
		if (fillContainer_honey != null)
		{
			fillContainer_honey.SetNormalizedValue(honeyTank.NormalizedTankAmount, instant: false);
		}
	}

	protected override bool CanProcessTanks()
	{
		if (honeyTank.CurrentTankAmount >= (float)honeyDrainAtMerge && !outputTank.IsFull)
		{
			return base.CanProcessTanks();
		}
		return false;
	}

	protected override void ProcessTanks()
	{
		base.ProcessTanks();
		honeyTank.ModifyTank(null, -honeyDrainAtMerge);
	}

	protected override void HandleSounds(bool isActive)
	{
		base.HandleSounds(isActive);
		if (isActive && dripParticles != null && emitter_drip != null)
		{
			dripTimer += Time.deltaTime;
			if (dripTimer >= dripRate)
			{
				dripTimer = 0f;
				dripParticles.Emit(1);
				emitter_drip.Play();
			}
		}
	}

	private void HandleParticles(bool particleState)
	{
		if (particleState)
		{
			if (smokeParticles != null)
			{
				smokeParticles.PlaySafe();
			}
			if (smokeParticlesExhaust != null)
			{
				smokeParticlesExhaust.PlaySafe();
			}
			if (rendererAnimatedHose != null)
			{
				if (animatedHoseProperty == null)
				{
					animatedHoseProperty = new MaterialPropertyBlock();
				}
				rendererAnimatedHose.GetPropertyBlock(animatedHoseProperty);
				animatedHoseProperty.SetFloat("_Waves", 3.71f);
				rendererAnimatedHose.SetPropertyBlock(animatedHoseProperty);
			}
			return;
		}
		if (smokeParticles != null)
		{
			smokeParticles.StopSafe();
		}
		if (smokeParticlesExhaust != null)
		{
			smokeParticlesExhaust.StopSafe();
		}
		if (rendererAnimatedHose != null)
		{
			if (animatedHoseProperty == null)
			{
				animatedHoseProperty = new MaterialPropertyBlock();
			}
			rendererAnimatedHose.GetPropertyBlock(animatedHoseProperty);
			animatedHoseProperty.SetFloat("_Waves", 0f);
			rendererAnimatedHose.SetPropertyBlock(animatedHoseProperty);
		}
	}

	public override RGD Serialize_Save()
	{
		if (!hasBeenPlaced)
		{
			return null;
		}
		return new RGD_Block_BiofuelExtractor(this);
	}

	public override RGD_Extractor GetSave()
	{
		if (!hasBeenPlaced)
		{
			return null;
		}
		return new RGD_BiofuelExtactor(this);
	}
}
