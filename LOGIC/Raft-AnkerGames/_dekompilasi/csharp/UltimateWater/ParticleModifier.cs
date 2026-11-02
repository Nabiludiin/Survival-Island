using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater;

public class ParticleModifier
{
	private struct InitialData
	{
		public ParticleSystem.MinMaxCurve EmissionRate;

		public List<Vector2> BurstRates;

		public float Speed;
	}

	private ParticleSystem[] _Systems;

	private List<InitialData> _InitialData;

	private static readonly ParticleSystem.Burst[] _Bursts = new ParticleSystem.Burst[4];

	public float Emission
	{
		set
		{
			for (int i = 0; i < _Systems.Length; i++)
			{
				InitialData initialData = _InitialData[i];
				ParticleSystem.EmissionModule emission = _Systems[i].emission;
				ParticleSystem.MinMaxCurve emissionRate = initialData.EmissionRate;
				emission.rateOverTime = MultiplyMinMaxCurve(value, emissionRate);
				int bursts = emission.GetBursts(_Bursts);
				for (int j = 0; j < bursts; j++)
				{
					_Bursts[j].minCount = (short)(initialData.BurstRates[j].x * value);
					_Bursts[j].maxCount = (short)(initialData.BurstRates[j].y * value);
				}
				emission.SetBursts(_Bursts, bursts);
			}
		}
	}

	public float Speed
	{
		set
		{
			for (int i = 0; i < _Systems.Length; i++)
			{
				InitialData initialData = _InitialData[i];
				ParticleSystem.MainModule main = _Systems[i].main;
				main.startSpeedMultiplier = initialData.Speed * value;
			}
		}
	}

	public bool Active
	{
		set
		{
			if (value)
			{
				for (int i = 0; i < _Systems.Length; i++)
				{
					_Systems[i].Play();
				}
			}
			else
			{
				for (int j = 0; j < _Systems.Length; j++)
				{
					_Systems[j].Stop();
				}
			}
		}
	}

	public bool IsInitialized => _Systems != null;

	public void Initialize(ParticleSystem[] particleSystems)
	{
		_Systems = particleSystems;
		_InitialData = new List<InitialData>(_Systems.Length);
		ParticleSystem[] systems = _Systems;
		InitialData item = default;
		foreach (ParticleSystem particleSystem in systems)
		{
			item.Speed = particleSystem.main.startSpeedMultiplier;
			item.EmissionRate = particleSystem.emission.rateOverTime;
			item.BurstRates = new List<Vector2>();
			int bursts = particleSystem.emission.GetBursts(_Bursts);
			for (int j = 0; j < bursts; j++)
			{
				item.BurstRates.Add(new Vector2(_Bursts[j].minCount, _Bursts[j].maxCount));
			}
			_InitialData.Add(item);
		}
	}

	private static ParticleSystem.MinMaxCurve MultiplyMinMaxCurve(float value, ParticleSystem.MinMaxCurve result)
	{
		switch (result.mode)
		{
		case ParticleSystemCurveMode.Constant:
			result.constant *= value;
			break;
		case ParticleSystemCurveMode.Curve:
		case ParticleSystemCurveMode.TwoCurves:
			result.curveMultiplier = value;
			break;
		case ParticleSystemCurveMode.TwoConstants:
			result.constantMin *= value;
			result.constantMax *= value;
			break;
		}
		return result;
	}
}
