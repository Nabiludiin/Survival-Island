using System.Collections.Generic;
using UltimateWater.Utils;
using UnityEngine;

namespace UltimateWater;

[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Ultimate Water/Dynamic/Water Particles")]
public class WaterParticleDisplacement : MonoBehaviour
{
	[Tooltip("What water to use to check particle collisions")]
	public Water Water;

	[Tooltip("Force that each particle inflicts on water")]
	public float Force = 1f;

	[Range(0f, 1f)]
	[Tooltip("Percentage of particles causing wave effects")]
	public float UsedParticles = 1f;

	[Tooltip("Particle force modifier based on particle speed")]
	public AnimationCurve ForceOverSpeed = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(10f, 0f));

	[Tooltip("Particle force modifier based on particle size")]
	public float SizeMultipier;

	[Tooltip("Calculate only a subset of all particles per frame")]
	[Range(0f, 16f)]
	public int FrameSplit = 2;

	private ParticleSystem _System;

	private WaterSample _Sampler;

	private ParticleSystem.Particle[] _Particles;

	private int _CurrentFrame;

	private static readonly List<WaterForce.Data> _Data = new List<WaterForce.Data>(256);

	private const int _SplitThreshold = 64;

	[InspectorWarning("Validation", InspectorWarningAttribute.InfoType.Warning)]
	[SerializeField]
	private string _Validation;

	public bool Initialized => Water != null;

	public void Initialize(Water water)
	{
		if (!Initialized)
		{
			Water = water;
			Start();
		}
	}

	private void Awake()
	{
		Water = Utilities.GetWaterReference();
		if (!Water.IsNullReference(this))
		{
			_System = GetComponent<ParticleSystem>();
			if (!_System.IsNullReference(this))
			{
				ParticleSystem.MainModule main = _System.main;
				main.simulationSpace = ParticleSystemSimulationSpace.World;
			}
		}
	}

	private void Start()
	{
		if (Initialized)
		{
			_Particles = new ParticleSystem.Particle[_System.main.maxParticles];
			_Sampler = new WaterSample(Water);
		}
	}

	private void Update()
	{
		if (!Initialized || _System.particleCount == 0)
		{
			return;
		}
		_Data.Clear();
		int particles = _System.GetParticles(_Particles);
		int num = 0;
		int num2 = 0;
		int num3 = particles;
		if (particles > 64)
		{
			num2 = (int)((float)particles * ((float)_CurrentFrame + 0f) / ((float)FrameSplit + 1f));
			num3 = (int)((float)particles * ((float)_CurrentFrame + 1f) / ((float)FrameSplit + 1f));
			NextBatch();
		}
		WaterForce.Data item = default;
		for (int i = num2; i < num3; i++)
		{
			if (_Particles[i].remainingLifetime <= 0f)
			{
				continue;
			}
			float x = _Particles[i].position.x;
			float z = _Particles[i].position.z;
			float y = _Particles[i].position.y;
			Vector3 andReset = _Sampler.GetAndReset(x, z);
			if (y < andReset.y && Mathf.Abs(y - andReset.y) < 0.1f)
			{
				if (!(_Particles[i].startLifetime - _Particles[i].remainingLifetime < 0.1f) && UsedParticles > Random.Range(0f, 1f))
				{
					float magnitude = _Particles[i].velocity.magnitude;
					float num4 = ForceOverSpeed.Evaluate(magnitude);
					float num5 = Force * (1f + magnitude * num4 * 0.1f);
					item.Position = new Vector3(x, 0f, z);
					item.Force = num5 * (1f + SizeMultipier * _Particles[i].GetCurrentSize(_System));
					_Data.Add(item);
				}
				_Particles[i].remainingLifetime = 0f;
				num++;
			}
		}
		if (_Data.Count != 0)
		{
			WaterRipples.AddForce(_Data);
		}
		if (num != 0)
		{
			_System.SetParticles(_Particles, particles);
		}
	}

	private void NextBatch()
	{
		if (_CurrentFrame < FrameSplit)
		{
			_CurrentFrame++;
		}
		else
		{
			_CurrentFrame = 0;
		}
	}

	private string Validation()
	{
		string text = string.Empty;
		if (Water == null)
		{
			text += "warning: assign water component";
		}
		return text;
	}
}
