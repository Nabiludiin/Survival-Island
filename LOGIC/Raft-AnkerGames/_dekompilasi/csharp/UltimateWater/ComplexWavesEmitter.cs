using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class ComplexWavesEmitter : MonoBehaviour, IWavesParticleSystemPlugin
{
	public enum WavesSource
	{
		CustomWaveFrequency,
		WindWavesSpectrum,
		Shoaling,
		Vehicle
	}

	private class SpawnPoint
	{
		public readonly Vector2 Position;

		public Vector2 Direction;

		public readonly float Frequency;

		public readonly float Amplitude;

		public readonly float TimeInterval;

		public float TimeLeft;

		public SpawnPoint(Vector2 position, Vector2 direction, float frequency, float amplitude, float speed)
		{
			Position = position;
			Direction = direction;
			Frequency = frequency;
			Amplitude = amplitude;
			TimeInterval = MathF.PI * 2f / speed * UnityEngine.Random.Range(1f, 8f);
			TimeLeft = UnityEngine.Random.Range(0f, TimeInterval);
		}
	}

	[SerializeField]
	[FormerlySerializedAs("wavesParticleSystem")]
	private WaveParticleSystem _WavesParticleSystem;

	[SerializeField]
	[FormerlySerializedAs("wavesSource")]
	private WavesSource _WavesSource = WavesSource.Shoaling;

	[SerializeField]
	[FormerlySerializedAs("wavelength")]
	private float _Wavelength = 120f;

	[SerializeField]
	[FormerlySerializedAs("amplitude")]
	private float _Amplitude = 0.6f;

	[SerializeField]
	[FormerlySerializedAs("emissionRate")]
	private float _EmissionRate = 2f;

	[SerializeField]
	[FormerlySerializedAs("width")]
	private int _Width = 8;

	[Range(0f, 180f)]
	[SerializeField]
	[FormerlySerializedAs("spectrumCoincidenceRange")]
	private float _SpectrumCoincidenceRange = 20f;

	[Range(0f, 100f)]
	[SerializeField]
	[FormerlySerializedAs("spectrumWavesCount")]
	private int _SpectrumWavesCount = 30;

	[Tooltip("Affects both waves and emission area width.")]
	[SerializeField]
	[FormerlySerializedAs("span")]
	private float _Span = 1000f;

	[Range(1f, 3.5f)]
	[SerializeField]
	[FormerlySerializedAs("waveShapeIrregularity")]
	private float _WaveShapeIrregularity = 2f;

	[SerializeField]
	[FormerlySerializedAs("lifetime")]
	private float _Lifetime = 200f;

	[SerializeField]
	[FormerlySerializedAs("shoreWaves")]
	private bool _ShoreWaves = true;

	[SerializeField]
	[FormerlySerializedAs("boundsSize")]
	private Vector2 _BoundsSize = new Vector2(500f, 500f);

	[SerializeField]
	[FormerlySerializedAs("spawnDepth")]
	[Range(3f, 80f)]
	private float _SpawnDepth = 8f;

	[FormerlySerializedAs("emissionFrequencyScale")]
	[Range(0.01f, 2f)]
	[SerializeField]
	private float _EmissionFrequencyScale = 1f;

	[SerializeField]
	[FormerlySerializedAs("spawnPointsDensity")]
	private float _SpawnPointsDensity = 1f;

	private SpawnPoint[] _SpawnPoints;

	private WindWaves _WindWaves;

	private float _NextSpawnTime;

	private float _TimeStep;

	public void UpdateParticles(float time, float deltaTime)
	{
		if (!isActiveAndEnabled)
		{
			return;
		}
		switch (_WavesSource)
		{
		case WavesSource.CustomWaveFrequency:
			if (time > _NextSpawnTime)
			{
				Vector3 position = transform.position;
				Vector3 forward = transform.forward;
				WaveParticle waveParticle = WaveParticle.Create(new Vector2(position.x, position.z), new Vector2(forward.x, forward.z).normalized, MathF.PI * 2f / _Wavelength, _Amplitude, _Lifetime, _ShoreWaves);
				if (waveParticle != null)
				{
					_WavesParticleSystem.Spawn(waveParticle, _Width, _WaveShapeIrregularity);
					waveParticle.Destroy();
					waveParticle.AddToCache();
				}
				_NextSpawnTime += _TimeStep;
			}
			break;
		case WavesSource.WindWavesSpectrum:
			if (_SpawnPoints == null)
			{
				CreateSpectralWavesSpawnPoints();
			}
			UpdateSpawnPoints(deltaTime);
			break;
		case WavesSource.Shoaling:
			if (_SpawnPoints == null)
			{
				CreateShoalingSpawnPoints();
			}
			UpdateSpawnPoints(deltaTime);
			break;
		}
	}

	private void Awake()
	{
		_WindWaves = _WavesParticleSystem.GetComponent<Water>().WindWaves;
		OnValidate();
		_WavesParticleSystem.RegisterPlugin(this);
	}

	private void OnEnable()
	{
		OnValidate();
		_NextSpawnTime = Time.time + UnityEngine.Random.Range(0f, _TimeStep);
	}

	private void OnValidate()
	{
		_TimeStep = _Wavelength / _EmissionRate;
	}

	private void UpdateSpawnPoints(float deltaTime)
	{
		deltaTime *= _EmissionFrequencyScale;
		for (int i = 0; i < _SpawnPoints.Length; i++)
		{
			SpawnPoint spawnPoint = _SpawnPoints[i];
			spawnPoint.TimeLeft -= deltaTime;
			if (spawnPoint.TimeLeft < 0f)
			{
				float num = MathF.PI * 2f / spawnPoint.Frequency;
				float num2 = _Span * 0.3f / num;
				int minInclusive = Mathf.Max(2, Mathf.RoundToInt(num2 * 0.7f));
				int maxExclusive = Mathf.Max(2, Mathf.RoundToInt(num2 * 1.429f));
				spawnPoint.TimeLeft += spawnPoint.TimeInterval;
				WaveParticle waveParticle = WaveParticle.Create(spawnPoint.Position + new Vector2(spawnPoint.Direction.y, 0f - spawnPoint.Direction.x) * UnityEngine.Random.Range((0f - _Span) * 0.35f, _Span * 0.35f), spawnPoint.Direction, spawnPoint.Frequency, spawnPoint.Amplitude, _Lifetime, _ShoreWaves);
				if (waveParticle != null)
				{
					_WavesParticleSystem.Spawn(waveParticle, UnityEngine.Random.Range(minInclusive, maxExclusive), _WaveShapeIrregularity);
					waveParticle.Destroy();
					waveParticle.AddToCache();
				}
			}
		}
	}

	private void CreateShoalingSpawnPoints()
	{
		Bounds bounds = new Bounds(transform.position, new Vector3(_BoundsSize.x, 0f, _BoundsSize.y));
		float x = bounds.min.x;
		float z = bounds.min.z;
		float x2 = bounds.max.x;
		float z2 = bounds.max.z;
		float num = Mathf.Sqrt(_SpawnPointsDensity);
		float num2 = Mathf.Max(35f, bounds.size.x / 256f) / num;
		float num3 = Mathf.Max(35f, bounds.size.z / 256f) / num;
		bool[,] array = new bool[32, 32];
		List<SpawnPoint> list = new List<SpawnPoint>();
		GerstnerWave[] array2 = _WindWaves.SpectrumResolver.SelectShorelineWaves(50, 0f, 360f);
		if (array2.Length == 0)
		{
			_SpawnPoints = new SpawnPoint[0];
			return;
		}
		float num4 = _SpawnDepth * 0.85f;
		float num5 = _SpawnDepth * 1.18f;
		Vector2 vector = default;
		for (float num6 = z; num6 < z2; num6 += num3)
		{
			for (float num7 = x; num7 < x2; num7 += num2)
			{
				int num8 = Mathf.FloorToInt(32f * (num7 - x) / (x2 - x));
				int num9 = Mathf.FloorToInt(32f * (num6 - z) / (z2 - z));
				if (array[num8, num9])
				{
					continue;
				}
				float totalDepthAt = StaticWaterInteraction.GetTotalDepthAt(num7, num6);
				if (!(totalDepthAt > num4) || !(totalDepthAt < num5) || !(UnityEngine.Random.value < 0.06f))
				{
					continue;
				}
				array[num8, num9] = true;
				vector.x = StaticWaterInteraction.GetTotalDepthAt(num7 - 3f, num6) - StaticWaterInteraction.GetTotalDepthAt(num7 + 3f, num6);
				vector.y = StaticWaterInteraction.GetTotalDepthAt(num7, num6 - 3f) - StaticWaterInteraction.GetTotalDepthAt(num7, num6 + 3f);
				vector.Normalize();
				GerstnerWave gerstnerWave = array2[0];
				float num10 = Vector2.Dot(vector, array2[0].Direction);
				for (int i = 1; i < array2.Length; i++)
				{
					float num11 = Vector2.Dot(vector, array2[i].Direction);
					if (num11 > num10)
					{
						num10 = num11;
						gerstnerWave = array2[i];
					}
				}
				list.Add(new SpawnPoint(new Vector2(num7, num6), vector, gerstnerWave.Frequency, Mathf.Abs(gerstnerWave.Amplitude), gerstnerWave.Speed));
			}
		}
		_SpawnPoints = list.ToArray();
	}

	private void CreateSpectralWavesSpawnPoints()
	{
		Vector3 normalized = transform.forward.normalized;
		float num = Mathf.Atan2(normalized.x, normalized.z);
		GerstnerWave[] array = _WindWaves.SpectrumResolver.SelectShorelineWaves(_SpectrumWavesCount, num * 57.29578f, _SpectrumCoincidenceRange);
		_SpectrumWavesCount = array.Length;
		Vector3 vector = new Vector3(transform.position.x + _Span * 0.5f, 0f, transform.position.z + _Span * 0.5f);
		Vector2 vector2 = new Vector2(vector.x, vector.z);
		List<SpawnPoint> list = new List<SpawnPoint>();
		for (int i = 0; i < _SpectrumWavesCount; i++)
		{
			GerstnerWave gerstnerWave = array[i];
			if (gerstnerWave.Amplitude != 0f)
			{
				Vector2 position = vector2 - gerstnerWave.Direction * _Span * 0.5f;
				list.Add(new SpawnPoint(position, gerstnerWave.Direction, gerstnerWave.Frequency, Mathf.Abs(gerstnerWave.Amplitude), gerstnerWave.Speed));
			}
		}
		_SpawnPoints = list.ToArray();
	}
}
