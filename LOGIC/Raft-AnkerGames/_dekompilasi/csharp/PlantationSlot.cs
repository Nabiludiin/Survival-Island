using System;
using UnityEngine;

[Serializable]
public class PlantationSlot
{
	public Action<bool> OnWaterChange;

	[HideInInspector]
	public bool busy;

	[HideInInspector]
	public Plant plant;

	[HideInInspector]
	public bool hasWater;

	public Transform transform;

	public Renderer renderer;

	private MaterialPropertyBlock materialProp;

	public bool CanGrow
	{
		get
		{
			if (busy && plant != null)
			{
				return hasWater;
			}
			return false;
		}
	}

	public void AddWater(bool playEffects = true)
	{
		hasWater = true;
		if (renderer != null)
		{
			if (materialProp == null)
			{
				materialProp = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(materialProp);
			materialProp.SetFloat("_Wet", 1f);
			renderer.SetPropertyBlock(materialProp);
		}
		if (playEffects)
		{
			ParticleManager.PlaySystem("WaterPlant", transform.position, Quaternion.LookRotation(transform.up));
		}
		if (OnWaterChange != null)
		{
			OnWaterChange(hasWater);
		}
	}

	public void RemoveWater()
	{
		hasWater = false;
		if (renderer != null)
		{
			if (materialProp == null)
			{
				materialProp = new MaterialPropertyBlock();
			}
			renderer.GetPropertyBlock(materialProp);
			materialProp.SetFloat("_Wet", 0f);
			renderer.SetPropertyBlock(materialProp);
		}
		if (OnWaterChange != null)
		{
			OnWaterChange(hasWater);
		}
	}
}
