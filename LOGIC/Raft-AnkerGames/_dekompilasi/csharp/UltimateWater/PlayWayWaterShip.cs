using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class PlayWayWaterShip : MonoBehaviour
{
	public class ParticleSystemData
	{
		public float RateOverTime;

		public float StartSpeed;

		public bool UseAlphaGradient;

		public Color Color;

		public Gradient Gradient;

		public ParticleSystemData(ParticleSystem particleSystem)
		{
			RateOverTime = particleSystem.emission.rateOverTimeMultiplier;
			StartSpeed = particleSystem.main.startSpeedMultiplier;
			switch (particleSystem.main.startColor.mode)
			{
			case ParticleSystemGradientMode.Color:
				UseAlphaGradient = false;
				Color = particleSystem.main.startColor.color;
				break;
			case ParticleSystemGradientMode.Gradient:
			case ParticleSystemGradientMode.RandomColor:
				UseAlphaGradient = true;
				Gradient = particleSystem.main.startColor.gradient;
				break;
			default:
				throw new ArgumentException("Unsupported startColor mode: " + particleSystem.main.startColor.mode);
			}
		}
	}

	[SerializeField]
	[FormerlySerializedAs("rigidBody")]
	private Rigidbody _RigidBody;

	[SerializeField]
	[FormerlySerializedAs("mainCollider")]
	private Collider _MainCollider;

	[SerializeField]
	[FormerlySerializedAs("sternEffects")]
	private ParticleSystem[] _SternEffects;

	[SerializeField]
	[FormerlySerializedAs("sternWaveEmitters")]
	private WavesEmitter[] _SternWaveEmitters;

	[SerializeField]
	[FormerlySerializedAs("bowWavesEmitter")]
	private ShipBowWavesEmitter _BowWavesEmitter;

	[SerializeField]
	[FormerlySerializedAs("bowSprayEmitters")]
	private ParticleSystem[] _BowSprayEmitters;

	[SerializeField]
	[FormerlySerializedAs("maxVelocity")]
	private float _MaxVelocity = 7.5f;

	private ParticleSystemData[] _PropellerEffectsData;

	private ParticleSystemData[] _BowSprayEmittersData;

	private float _PreviousSternEffectsIntensity = float.NaN;

	private float _PreviousBowEffectsIntensity = float.NaN;

	private void Start()
	{
		_PropellerEffectsData = new ParticleSystemData[_SternEffects.Length];
		for (int num = _PropellerEffectsData.Length - 1; num >= 0; num--)
		{
			_PropellerEffectsData[num] = new ParticleSystemData(_SternEffects[num]);
		}
		_BowSprayEmittersData = new ParticleSystemData[_BowSprayEmitters.Length];
		for (int num2 = _BowSprayEmittersData.Length - 1; num2 >= 0; num2--)
		{
			_BowSprayEmittersData[num2] = new ParticleSystemData(_BowSprayEmitters[num2]);
		}
	}

	private void OnEnable()
	{
		SetEnabled(enable: true);
	}

	private void OnDisable()
	{
		SetEnabled(enable: false);
	}

	private void Update()
	{
		float num = _RigidBody.velocity.magnitude / _MaxVelocity;
		float num2 = Mathf.Clamp01(1f - (0f - _BowWavesEmitter.transform.position.y - 1f) * 0.5f);
		float num3 = ClampIntensity(num * num2);
		if (num3 != _PreviousBowEffectsIntensity)
		{
			_PreviousBowEffectsIntensity = num3;
			_BowWavesEmitter.enabled = num3 > 0f;
			for (int num4 = _BowSprayEmitters.Length - 1; num4 >= 0; num4--)
			{
				SetEffectIntensity(_BowSprayEmitters[num4], _BowSprayEmittersData[num4], num3);
			}
		}
		float num5 = Mathf.Clamp01(1f - (0f - _MainCollider.bounds.max.y - 1f) * 0.5f);
		float num6 = ClampIntensity(num * num5);
		if (num6 != _PreviousSternEffectsIntensity)
		{
			_PreviousSternEffectsIntensity = num6;
			for (int num7 = _SternWaveEmitters.Length - 1; num7 >= 0; num7--)
			{
				_SternWaveEmitters[num7].Power = num6;
			}
			for (int i = 0; i < _SternEffects.Length; i++)
			{
				SetEffectIntensity(_SternEffects[i], _PropellerEffectsData[i], num6);
			}
		}
	}

	private static void SetEffectIntensity(ParticleSystem particleSystem, ParticleSystemData data, float intensity)
	{
		float num = ((intensity == 0f) ? 0f : (0.5f + intensity * 0.5f));
		float num2 = intensity * intensity;
		ParticleSystem.EmissionModule emission = particleSystem.emission;
		emission.rateOverTimeMultiplier = data.RateOverTime * num;
		ParticleSystem.MainModule main = particleSystem.main;
		main.startSpeedMultiplier = data.StartSpeed * num;
		if (data.UseAlphaGradient)
		{
			Gradient gradient = data.Gradient;
			GradientAlphaKey[] alphaKeys = gradient.alphaKeys;
			for (int i = 0; i < alphaKeys.Length; i++)
			{
				alphaKeys[i].alpha *= num2;
			}
			main.startColor = gradient;
		}
		else
		{
			Color color = data.Color;
			color.a *= num2;
			main.startColor = color;
		}
	}

	private static float ClampIntensity(float x)
	{
		if (!(x > 1f))
		{
			if (!(x < 0.2f))
			{
				return x;
			}
			return 0f;
		}
		return 1f;
	}

	private void SetEnabled(bool enable)
	{
		for (int i = 0; i < _SternWaveEmitters.Length; i++)
		{
			_SternWaveEmitters[i].enabled = enable;
		}
		if (_BowWavesEmitter != null)
		{
			_BowWavesEmitter.enabled = enable;
		}
	}
}
