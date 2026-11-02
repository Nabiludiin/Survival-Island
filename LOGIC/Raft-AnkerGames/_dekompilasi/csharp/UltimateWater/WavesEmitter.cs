using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class WavesEmitter : MonoBehaviour
{
	[SerializeField]
	[FormerlySerializedAs("water")]
	private WaveParticlesSystemGPU _Water;

	[SerializeField]
	[FormerlySerializedAs("amplitude")]
	private float _Amplitude = 0.1f;

	[SerializeField]
	[FormerlySerializedAs("wavelength")]
	private float _Wavelength = 10f;

	[SerializeField]
	[FormerlySerializedAs("lifetime")]
	private float _Lifetime = 50f;

	[SerializeField]
	[FormerlySerializedAs("speed")]
	private float _Speed = 3.5f;

	[SerializeField]
	[FormerlySerializedAs("foam")]
	private float _Foam = 1f;

	[SerializeField]
	[FormerlySerializedAs("emissionArea")]
	private float _EmissionArea = 1f;

	[SerializeField]
	[FormerlySerializedAs("emissionInterval")]
	private float _EmissionInterval = 0.15f;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("trailCalming")]
	private float _TrailCalming = 1f;

	[Range(0f, 8f)]
	[SerializeField]
	[FormerlySerializedAs("trailFoam")]
	private float _TrailFoam = 1f;

	[Range(0f, 180f)]
	[SerializeField]
	[FormerlySerializedAs("emissionAngle")]
	private float _EmissionAngle;

	[Header("Advanced")]
	[SerializeField]
	[FormerlySerializedAs("minTextureU")]
	private int _MinTextureU = 4;

	[SerializeField]
	[FormerlySerializedAs("maxTextureU")]
	private int _MaxTextureU = 8;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("initialPower")]
	private float _InitialPower = 1f;

	private float _Power = -1f;

	private float _LastEmitTime;

	private float _FinalEmissionInterval;

	private Water _WaterComponent;

	public float Power
	{
		get
		{
			return _Power;
		}
		set
		{
			_Power = ((value > 0f) ? value : 0f);
			_FinalEmissionInterval = _EmissionInterval / _Power;
			enabled = _Power != 0f;
		}
	}

	private void OnValidate()
	{
		_FinalEmissionInterval = _EmissionInterval / _Power;
	}

	private void LateUpdate()
	{
		float time = Time.time;
		if (time - _LastEmitTime >= _FinalEmissionInterval)
		{
			_LastEmitTime = time;
			Vector2 vector = GetVector2(transform.position);
			Vector2 normalized = GetVector2(transform.forward).normalized;
			Vector2 normalized2 = GetVector2(transform.right).normalized;
			if (normalized.x == 0f && normalized.y == 0f)
			{
				normalized = GetVector2(transform.up).normalized;
			}
			float num = _EmissionAngle * (MathF.PI / 180f);
			FastMath.SinCos2048(UnityEngine.Random.Range(0f - num, num), out var s, out var c);
			Vector2 vector2 = new Vector2(normalized.x * c - normalized.y * s, normalized.x * s + normalized.y * c);
			Vector2 position = vector + normalized2 * UnityEngine.Random.Range(0f - _EmissionArea, _EmissionArea);
			Vector2 horizontalDisplacementAt = _WaterComponent.GetHorizontalDisplacementAt(position.x, position.y, Time.time);
			position.x -= horizontalDisplacementAt.x;
			position.y -= horizontalDisplacementAt.y;
			_Water.EmitParticle(new WaveParticlesSystemGPU.ParticleData
			{
				Position = position,
				Direction = vector2 * (_Speed * _Power),
				Amplitude = _Amplitude * _Power,
				Wavelength = _Wavelength,
				InitialLifetime = _Lifetime * _Power,
				Lifetime = _Lifetime * _Power,
				Foam = _Foam * _Power,
				UvOffsetPack = (float)UnityEngine.Random.Range(0, _Water.FoamAtlasHeight) / (float)_Water.FoamAtlasHeight * 16f + (float)UnityEngine.Random.Range(_MinTextureU, _MaxTextureU) / (float)_Water.FoamAtlasWidth,
				TrailCalming = _TrailCalming,
				TrailFoam = _TrailFoam
			});
		}
	}

	private void Start()
	{
		if (_Water == null)
		{
			_Water = UnityEngine.Object.FindObjectOfType<WaveParticlesSystemGPU>();
		}
		_WaterComponent = _Water.GetComponent<Water>();
		Power = _InitialPower;
	}

	private static Vector2 GetVector2(Vector3 vector3)
	{
		return new Vector2(vector3.x, vector3.z);
	}
}
