using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Fuel : MonoBehaviour
{
	public delegate void OnFuelRemoved();

	public Item_Base fuelItem;

	private float burnTimePerFuel = 50f;

	[SerializeField]
	private int maxFuel = 1;

	[SerializeField]
	private List<GameObject> fuelModels;

	[SerializeField]
	private ParticleSystem particleSystem;

	[SerializeField]
	private ParticleController particleController;

	[SerializeField]
	private StudioEventEmitter eventEmitter;

	private int fuelCount;

	private bool burningFuel;

	private float burnTimer;

	public int MaxFuel => maxFuel;

	public event OnFuelRemoved OnFuelRemovedEvent;

	private void Awake()
	{
		if (maxFuel < 0)
		{
			fuelCount = maxFuel;
		}
		RefreshFuelModels();
	}

	public void UpdateFuel()
	{
		if (particleSystem != null)
		{
			if (burningFuel)
			{
				particleSystem.PlaySafe();
			}
			else
			{
				particleSystem.StopSafe();
			}
		}
		else if (particleController != null)
		{
			if (burningFuel)
			{
				particleController.PlayParticles();
			}
			else
			{
				particleController.StopParticles();
			}
		}
		if (eventEmitter != null)
		{
			if (burningFuel)
			{
				eventEmitter.PlaySafe();
			}
			else
			{
				eventEmitter.StopSafe();
			}
		}
		if (!Raft_Network.IsHost || fuelCount <= 0 || !burningFuel)
		{
			return;
		}
		burnTimer += Time.deltaTime;
		if (burnTimer >= burnTimePerFuel)
		{
			burnTimer -= burnTimePerFuel;
			fuelCount--;
			if (fuelCount <= 0)
			{
				burnTimer = 0f;
				StopBurning();
			}
			RefreshFuelModels();
			if (OnFuelRemovedEvent != null)
			{
				OnFuelRemovedEvent();
			}
		}
	}

	private void OnValidate()
	{
		if (fuelModels != null && fuelModels.Count > 0)
		{
			while (fuelModels.Count > maxFuel)
			{
				fuelModels.RemoveAt(fuelModels.Count - 1);
			}
		}
	}

	public void SetFuelCount(int count)
	{
		fuelCount = count;
	}

	public void ForceSetTimer(float value)
	{
		burnTimer = value;
		while (burnTimer >= burnTimePerFuel)
		{
			burnTimer -= burnTimePerFuel;
			fuelCount--;
		}
		RefreshFuelModels();
	}

	public void IncrementTimer(float amount)
	{
		burnTimer += amount;
	}

	public float GetTimer()
	{
		return burnTimer;
	}

	public float GetBurningNormal()
	{
		return burnTimer / burnTimePerFuel;
	}

	public int GetFuelCount()
	{
		if (fuelCount < 0)
		{
			return int.MaxValue;
		}
		return fuelCount;
	}

	public float GetBurnSecondsLeft()
	{
		return burnTimePerFuel * (float)(fuelCount - 1) + (burnTimePerFuel - burnTimer);
	}

	public void AddFuel(int amount)
	{
		fuelCount = Mathf.Clamp(fuelCount + amount, 0, maxFuel);
		if (fuelCount <= 0)
		{
			burnTimer = 0f;
			StopBurning();
		}
		RefreshFuelModels();
	}

	public bool HasMaxFuel()
	{
		if (MaxFuel < 0)
		{
			return true;
		}
		if (fuelCount < 0)
		{
			return true;
		}
		return fuelCount == maxFuel;
	}

	public bool IsBurning()
	{
		return burningFuel;
	}

	public void StartBurning()
	{
		if (!burningFuel && fuelCount != 0)
		{
			burningFuel = true;
		}
	}

	public void StopBurning()
	{
		burningFuel = false;
	}

	private void RefreshFuelModels()
	{
		if (fuelModels == null || fuelModels.Count == 0)
		{
			return;
		}
		for (int i = 0; i < fuelModels.Count; i++)
		{
			if (i < fuelCount)
			{
				fuelModels[i].SetActive(value: true);
			}
			else
			{
				fuelModels[i].SetActive(value: false);
			}
		}
	}
}
