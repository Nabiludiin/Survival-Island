using FMODUnity;
using UnityEngine;

public class MeshPathBase_Igloo : MeshPathBase_Electricity
{
	[SerializeField]
	private Animator iglooAnimator;

	[SerializeField]
	private Renderer iglooRenderer;

	[SerializeField]
	private Light[] iglooLights;

	[SerializeField]
	private StudioEventEmitter emitter_openDoor;

	[SerializeField]
	private StudioEventEmitter emitter_closeDoor;

	private MaterialPropertyBlock emissionBlock;

	protected override void RefreshElectricityVisuals()
	{
		base.RefreshElectricityVisuals();
		if (iglooAnimator != null)
		{
			iglooAnimator.SetInteger("StateIndex", HasElectricity ? 1 : 0);
			if (HasElectricity)
			{
				if (emitter_openDoor != null)
				{
					emitter_openDoor.Play();
				}
			}
			else if (emitter_closeDoor != null)
			{
				emitter_closeDoor.Play();
			}
		}
		if (iglooRenderer != null)
		{
			SetIglooEmission(HasElectricity ? 12 : 0);
		}
		SetLampsEnabled(hasElectricity);
	}

	private void SetIglooEmission(int emission)
	{
		if (emissionBlock == null)
		{
			emissionBlock = new MaterialPropertyBlock();
		}
		iglooRenderer.GetPropertyBlock(emissionBlock, 0);
		emissionBlock.SetFloat("_EmissionIntensity", emission);
		iglooRenderer.SetPropertyBlock(emissionBlock);
	}

	private void SetLampsEnabled(bool enabled)
	{
		if (iglooLights != null)
		{
			Light[] array = iglooLights;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].enabled = enabled;
			}
		}
	}
}
