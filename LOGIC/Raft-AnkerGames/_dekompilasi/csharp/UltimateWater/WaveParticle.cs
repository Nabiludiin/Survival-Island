using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public sealed class WaveParticle : IPoint2D
{
	[FormerlySerializedAs("direction")]
	public Vector2 Direction;

	[FormerlySerializedAs("speed")]
	public float Speed;

	[FormerlySerializedAs("targetSpeed")]
	public float TargetSpeed = 1f;

	[FormerlySerializedAs("baseFrequency")]
	public float BaseFrequency;

	[FormerlySerializedAs("frequency")]
	public float Frequency;

	[FormerlySerializedAs("baseAmplitude")]
	public float BaseAmplitude;

	[FormerlySerializedAs("amplitude")]
	public float Amplitude;

	[FormerlySerializedAs("fadeFactor")]
	public float FadeFactor;

	[FormerlySerializedAs("energyBalance")]
	public float EnergyBalance;

	[FormerlySerializedAs("targetEnergyBalance")]
	public float TargetEnergyBalance;

	[FormerlySerializedAs("shoaling")]
	public float Shoaling;

	[FormerlySerializedAs("invkh")]
	public float Invkh = 1f;

	[FormerlySerializedAs("targetInvKh")]
	public float TargetInvKh = 1f;

	[FormerlySerializedAs("baseSpeed")]
	public float BaseSpeed;

	[FormerlySerializedAs("lifetime")]
	public float Lifetime;

	[FormerlySerializedAs("amplitudeModifiers")]
	public float AmplitudeModifiers;

	[FormerlySerializedAs("amplitudeModifiers2")]
	public float AmplitudeModifiers2 = 1f;

	[FormerlySerializedAs("expansionEnergyLoss")]
	public float ExpansionEnergyLoss;

	[FormerlySerializedAs("isShoreWave")]
	public bool IsShoreWave;

	[FormerlySerializedAs("isAlive")]
	public bool IsAlive = true;

	[FormerlySerializedAs("disallowSubdivision")]
	public bool DisallowSubdivision;

	[FormerlySerializedAs("leftNeighbour")]
	public WaveParticle LeftNeighbour;

	[FormerlySerializedAs("rightNeighbour")]
	public WaveParticle RightNeighbour;

	[FormerlySerializedAs("group")]
	public WaveParticlesGroup Group;

	private Vector2 _Position;

	private static readonly Stack<WaveParticle> _WaveParticlesCache;

	private static readonly float[] _AmplitudeFuncPrecomp;

	private static readonly float[] _FrequencyFuncPrecomp;

	public Vector2 Position => _Position;

	public Vector4 PackedParticleData => new Vector4(Direction.x * 2f * MathF.PI / Frequency, Direction.y * 2f * MathF.PI / Frequency, Shoaling, Speed);

	public Vector3 VertexData => new Vector3(_Position.x, _Position.y, Amplitude);

	public Vector3 DebugData => new Vector3(Group.Id, 0f, 0f);

	static WaveParticle()
	{
		_WaveParticlesCache = new Stack<WaveParticle>();
		_AmplitudeFuncPrecomp = new float[2048];
		_FrequencyFuncPrecomp = new float[2048];
		for (int i = 0; i < 2048; i++)
		{
			double num = ((float)i + 0.49f) / 2047f;
			double kh = 4.0 * (1.0 - Math.Pow(1.0 - num, 0.33333333));
			_AmplitudeFuncPrecomp[i] = ComputeAmplitudeAtShore(kh);
			_FrequencyFuncPrecomp[i] = Mathf.Sqrt(1f / ComputeWavelengthAtShore(kh));
		}
	}

	public static WaveParticle Create(Vector3 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
	{
		return Create(new Vector2(position.x, position.z), direction, baseFrequency, baseAmplitude, lifetime, isShoreWave);
	}

	public static WaveParticle Create(Vector2 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
	{
		WaveParticle waveParticle;
		if (_WaveParticlesCache.Count != 0)
		{
			waveParticle = _WaveParticlesCache.Pop();
			waveParticle._Position = position;
			waveParticle.Direction = direction;
			waveParticle.BaseFrequency = baseFrequency;
			waveParticle.BaseAmplitude = baseAmplitude;
			waveParticle.FadeFactor = 0f;
			waveParticle.IsShoreWave = isShoreWave;
			waveParticle.BaseSpeed = 2.2f * Mathf.Sqrt(9.81f / baseFrequency);
			waveParticle.Amplitude = baseAmplitude;
			waveParticle.Frequency = baseFrequency;
			waveParticle.TargetSpeed = 1f;
			waveParticle.Invkh = 1f;
			waveParticle.TargetInvKh = 1f;
			waveParticle.EnergyBalance = 0f;
			waveParticle.Shoaling = 0f;
			waveParticle.Speed = 0f;
			waveParticle.TargetEnergyBalance = 0f;
			waveParticle.Lifetime = lifetime;
			waveParticle.AmplitudeModifiers = 0f;
			waveParticle.AmplitudeModifiers2 = 1f;
			waveParticle.ExpansionEnergyLoss = 0f;
			waveParticle.IsAlive = true;
			waveParticle.DisallowSubdivision = false;
			if (waveParticle.LeftNeighbour != null || waveParticle.RightNeighbour != null)
			{
				waveParticle.LeftNeighbour = null;
				waveParticle.RightNeighbour = null;
			}
			waveParticle.CostlyUpdate(null, 0.1f);
		}
		else
		{
			waveParticle = new WaveParticle(position, direction, baseFrequency, baseAmplitude, lifetime, isShoreWave);
		}
		if (waveParticle.BaseAmplitude == 0f)
		{
			return null;
		}
		return waveParticle;
	}

	public void Destroy()
	{
		BaseAmplitude = (Amplitude = 0f);
		IsAlive = false;
		if (LeftNeighbour != null)
		{
			LeftNeighbour.RightNeighbour = RightNeighbour;
			LeftNeighbour.DisallowSubdivision = true;
		}
		if (RightNeighbour != null)
		{
			RightNeighbour.LeftNeighbour = LeftNeighbour;
			RightNeighbour.DisallowSubdivision = true;
		}
		if (Group != null && Group.LeftParticle == this)
		{
			Group.LeftParticle = RightNeighbour;
		}
		LeftNeighbour = null;
		RightNeighbour = null;
	}

	public void DelayedDestroy()
	{
		BaseAmplitude = (Amplitude = 0f);
		IsAlive = false;
	}

	public void AddToCache()
	{
		_WaveParticlesCache.Push(this);
	}

	public WaveParticle Clone(Vector2 position)
	{
		WaveParticle waveParticle = Create(position, Direction, BaseFrequency, BaseAmplitude, Lifetime, IsShoreWave);
		if (waveParticle == null)
		{
			return null;
		}
		waveParticle.Amplitude = Amplitude;
		waveParticle.Frequency = Frequency;
		waveParticle.Speed = Speed;
		waveParticle.TargetSpeed = TargetSpeed;
		waveParticle.EnergyBalance = EnergyBalance;
		waveParticle.Shoaling = Shoaling;
		waveParticle.Group = Group;
		return waveParticle;
	}

	public void Update(float deltaTime, float step, float invStep)
	{
		if (Lifetime > 0f)
		{
			if (FadeFactor != 1f)
			{
				FadeFactor += deltaTime;
				if (FadeFactor > 1f)
				{
					FadeFactor = 1f;
				}
			}
		}
		else
		{
			FadeFactor -= deltaTime;
			if (FadeFactor <= 0f)
			{
				Destroy();
				return;
			}
		}
		if (TargetEnergyBalance < EnergyBalance)
		{
			float num = step * 0.005f;
			EnergyBalance = EnergyBalance * (1f - num) + TargetEnergyBalance * num;
		}
		else
		{
			float num2 = step * 0.0008f;
			EnergyBalance = EnergyBalance * (1f - num2) + TargetEnergyBalance * num2;
		}
		BaseAmplitude += deltaTime * EnergyBalance;
		BaseAmplitude *= step * ExpansionEnergyLoss + 1f;
		if (BaseAmplitude <= 0.01f)
		{
			Destroy();
			return;
		}
		Speed = invStep * Speed + step * TargetSpeed;
		float num3 = Speed + EnergyBalance * -20f;
		Invkh = invStep * Invkh + step * TargetInvKh;
		int num4 = (int)(2047f * (1f - Invkh * Invkh * Invkh) - 0.49f);
		float num5 = ((num4 >= 2048) ? 1f : _FrequencyFuncPrecomp[num4]);
		Frequency = BaseFrequency * num5;
		Amplitude = FadeFactor * BaseAmplitude * ((num4 >= 2048) ? 1f : _AmplitudeFuncPrecomp[num4]);
		Shoaling = AmplitudeModifiers * 0.004f * (0f - EnergyBalance) / Amplitude;
		Amplitude *= AmplitudeModifiers;
		float num6 = num3 * deltaTime;
		_Position.x += Direction.x * num6;
		_Position.y += Direction.y * num6;
	}

	public int CostlyUpdate(WaveParticlesQuadtree quadtree, float deltaTime)
	{
		float num;
		if (Frequency < 0.025f)
		{
			float x = _Position.x + Direction.x / Frequency;
			float z = _Position.y + Direction.y / Frequency;
			num = Mathf.Max(StaticWaterInteraction.GetTotalDepthAt(_Position.x, _Position.y), StaticWaterInteraction.GetTotalDepthAt(x, z));
		}
		else
		{
			num = StaticWaterInteraction.GetTotalDepthAt(_Position.x, _Position.y);
		}
		if (num <= 0.001f)
		{
			Destroy();
			return 0;
		}
		UpdateWaveParameters(deltaTime, num);
		int numSubdivisions = 0;
		if (quadtree != null && !DisallowSubdivision)
		{
			if (LeftNeighbour != null)
			{
				Subdivide(quadtree, LeftNeighbour, this, ref numSubdivisions);
			}
			if (RightNeighbour != null)
			{
				Subdivide(quadtree, this, RightNeighbour, ref numSubdivisions);
			}
		}
		return numSubdivisions;
	}

	private WaveParticle(Vector2 position, Vector2 direction, float baseFrequency, float baseAmplitude, float lifetime, bool isShoreWave)
	{
		_Position = position;
		Direction = direction;
		BaseFrequency = baseFrequency;
		BaseAmplitude = baseAmplitude;
		FadeFactor = 0f;
		Frequency = baseFrequency;
		Amplitude = baseAmplitude;
		IsShoreWave = isShoreWave;
		BaseSpeed = 2.5f * Mathf.Sqrt(9.81f / baseFrequency);
		Lifetime = lifetime;
		CostlyUpdate(null, 0.1f);
	}

	private void UpdateWaveParameters(float deltaTime, float depth)
	{
		Lifetime -= deltaTime;
		TargetInvKh = 1f - 0.25f * BaseFrequency * depth;
		if (TargetInvKh < 0f)
		{
			TargetInvKh = 0f;
		}
		int num = (int)(BaseFrequency * depth * 512f);
		TargetSpeed = BaseSpeed * ((num >= 2048) ? 1f : FastMath.PositiveTanhSqrtNoZero[num]);
		if (TargetSpeed < 0.5f)
		{
			TargetSpeed = 0.5f;
		}
		if (0.135f / Frequency < Amplitude)
		{
			TargetEnergyBalance = (0f - Amplitude) * 5f;
		}
		if (LeftNeighbour != null && RightNeighbour != null && !DisallowSubdivision)
		{
			Vector2 vector = new Vector2(RightNeighbour._Position.y - LeftNeighbour._Position.y, LeftNeighbour._Position.x - RightNeighbour._Position.x);
			float num2 = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
			if (num2 > 0.001f)
			{
				if (vector.x * Direction.x + vector.y * Direction.y < 0f)
				{
					num2 = 0f - num2;
				}
				vector.x /= num2;
				vector.y /= num2;
				float num3 = 0.6f * deltaTime;
				if (num3 > 0.6f)
				{
					num3 = 0.6f;
				}
				Direction.x = Direction.x * (1f - num3) + vector.x * num3;
				Direction.y = Direction.y * (1f - num3) + vector.y * num3;
				float num4 = Mathf.Sqrt(Direction.x * Direction.x + Direction.y * Direction.y);
				Direction.x /= num4;
				Direction.y /= num4;
			}
			ExpansionEnergyLoss = -1f + 0.5f * (Direction.x * (LeftNeighbour.Direction.x + RightNeighbour.Direction.x) + Direction.y * (LeftNeighbour.Direction.y + RightNeighbour.Direction.y));
			if (ExpansionEnergyLoss < -1f)
			{
				ExpansionEnergyLoss = -1f;
			}
			if (LeftNeighbour.DisallowSubdivision)
			{
				LeftNeighbour.ExpansionEnergyLoss = ExpansionEnergyLoss;
			}
			if (RightNeighbour.DisallowSubdivision)
			{
				RightNeighbour.ExpansionEnergyLoss = ExpansionEnergyLoss;
			}
		}
		AmplitudeModifiers = 1f;
		if (IsShoreWave)
		{
			int num5 = (int)(depth * 5.12f);
			if (num5 < 2048)
			{
				AmplitudeModifiers *= 1f - FastMath.PositiveTanhSqrtNoZero[num5];
			}
		}
		AmplitudeModifiers *= AmplitudeModifiers2;
	}

	private void Subdivide(WaveParticlesQuadtree quadtree, WaveParticle left, WaveParticle right, ref int numSubdivisions)
	{
		Vector2 vector = left._Position - right._Position;
		float magnitude = vector.magnitude;
		if (!(magnitude * Frequency > 1f) || !(magnitude > 1f) || quadtree.FreeSpace == 0)
		{
			return;
		}
		WaveParticle waveParticle = Create(right._Position + vector * 0.5f, (left.Direction + right.Direction) * 0.5f, (left.BaseFrequency + right.BaseFrequency) * 0.5f, (left.BaseAmplitude + right.BaseAmplitude) * 0.5f, (left.Lifetime + right.Lifetime) * 0.5f, left.IsShoreWave);
		if (waveParticle != null)
		{
			waveParticle.Group = left.Group;
			waveParticle.Amplitude = (left.Amplitude + right.Amplitude) * 0.5f;
			waveParticle.Frequency = (left.Frequency + right.Frequency) * 0.5f;
			waveParticle.Speed = (left.Speed + right.Speed) * 0.5f;
			waveParticle.TargetSpeed = (left.TargetSpeed + right.TargetSpeed) * 0.5f;
			waveParticle.EnergyBalance = (left.EnergyBalance + right.EnergyBalance) * 0.5f;
			waveParticle.Shoaling = (left.Shoaling + right.Shoaling) * 0.5f;
			waveParticle.TargetInvKh = (left.TargetInvKh + right.TargetInvKh) * 0.5f;
			waveParticle.Lifetime = (left.Lifetime + right.Lifetime) * 0.5f;
			waveParticle.TargetEnergyBalance = (left.TargetEnergyBalance + right.TargetEnergyBalance) * 0.5f;
			waveParticle.AmplitudeModifiers = (left.AmplitudeModifiers + right.AmplitudeModifiers) * 0.5f;
			waveParticle.AmplitudeModifiers2 = (left.AmplitudeModifiers2 + right.AmplitudeModifiers2) * 0.5f;
			waveParticle.Invkh = (left.Invkh + right.Invkh) * 0.5f;
			waveParticle.BaseSpeed = (left.BaseSpeed + right.BaseSpeed) * 0.5f;
			waveParticle.ExpansionEnergyLoss = (left.ExpansionEnergyLoss + right.ExpansionEnergyLoss) * 0.5f;
			waveParticle.Direction = left.Direction;
			if (quadtree.AddElement(waveParticle))
			{
				waveParticle.LeftNeighbour = left;
				waveParticle.RightNeighbour = right;
				left.RightNeighbour = waveParticle;
				right.LeftNeighbour = waveParticle;
			}
			numSubdivisions++;
		}
	}

	private static float ComputeAmplitudeAtShore(double kh)
	{
		double num = Math.Cosh(kh);
		return (float)Math.Sqrt(2.0 * num * num / (Math.Sinh(2.0 * kh) + 2.0 * kh));
	}

	private static float ComputeWavelengthAtShore(double kh)
	{
		return (float)Math.Pow(Math.Tanh(Math.Pow(kh * Math.Tanh(kh), 0.75)), 0.666666);
	}
}
