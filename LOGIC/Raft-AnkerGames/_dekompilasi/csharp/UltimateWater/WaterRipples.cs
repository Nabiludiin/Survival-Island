using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public sealed class WaterRipples : SceneSingleton<WaterRipples>
{
	private readonly List<WaterSimulationArea> _Areas = new List<WaterSimulationArea>();

	public static void Register(WaterSimulationArea area)
	{
		WaterRipples instance = SceneSingleton<WaterRipples>.Instance;
		if (!(instance == null) && !instance._Areas.Contains(area))
		{
			instance._Areas.Add(area);
		}
	}

	public static void Unregister(WaterSimulationArea area)
	{
		WaterRipples instance = SceneSingleton<WaterRipples>.Instance;
		if (!(instance == null))
		{
			instance._Areas.Remove(area);
		}
	}

	public static void AddForce(List<WaterForce.Data> data, float radius = 1f)
	{
		WaterRipples instance = SceneSingleton<WaterRipples>.Instance;
		if (!(instance == null) && Time.timeScale != 0f)
		{
			for (int num = instance._Areas.Count - 1; num >= 0; num--)
			{
				instance._Areas[num].AddForce(data, radius);
			}
		}
	}

	private void FixedUpdate()
	{
		int iterations = WaterQualitySettings.Instance.Ripples.Iterations;
		for (int i = 0; i < iterations; i++)
		{
			for (int j = 0; j < _Areas.Count; j++)
			{
				_Areas[j].Simulate();
			}
			for (int k = 0; k < _Areas.Count; k++)
			{
				_Areas[k].Smooth();
			}
			for (int l = 0; l < _Areas.Count; l++)
			{
				_Areas[l].Swap();
			}
		}
	}

	private void OnDisable()
	{
		_Areas.Clear();
	}
}
