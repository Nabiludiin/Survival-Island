using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class WaveParticlesGroup
{
	[FormerlySerializedAs("lastUpdateTime")]
	public float LastUpdateTime;

	[FormerlySerializedAs("lastCostlyUpdateTime")]
	public float LastCostlyUpdateTime;

	[FormerlySerializedAs("leftParticle")]
	public WaveParticle LeftParticle;

	private readonly int _Id;

	private static int _NextId;

	public int ParticleCount
	{
		get
		{
			int num = 0;
			for (WaveParticle waveParticle = LeftParticle; waveParticle != null; waveParticle = waveParticle.RightNeighbour)
			{
				num++;
			}
			return num;
		}
	}

	public int Id => _Id;

	public WaveParticle LastParticle
	{
		get
		{
			if (LeftParticle == null)
			{
				return null;
			}
			WaveParticle waveParticle = LeftParticle;
			while (waveParticle.RightNeighbour != null)
			{
				waveParticle = waveParticle.RightNeighbour;
			}
			return waveParticle;
		}
	}

	public WaveParticlesGroup(float startTime)
	{
		_Id = ++_NextId;
		LastUpdateTime = (LastCostlyUpdateTime = startTime);
	}

	public void CostlyUpdate(WaveParticlesQuadtree quadtree, float time)
	{
		WaveParticle waveParticle = LeftParticle;
		float deltaTime = time - LastCostlyUpdateTime;
		LastCostlyUpdateTime = time;
		int num = 0;
		do
		{
			WaveParticle waveParticle2 = waveParticle;
			waveParticle = waveParticle.RightNeighbour;
			num += waveParticle2.CostlyUpdate((num < 30) ? quadtree : null, deltaTime);
		}
		while (waveParticle != null);
		waveParticle = LeftParticle;
		if (waveParticle == null)
		{
			return;
		}
		WaveParticle waveParticle3 = waveParticle;
		int num2 = 0;
		do
		{
			WaveParticle waveParticle4 = waveParticle;
			waveParticle = waveParticle.RightNeighbour;
			num2++;
			if (waveParticle4 != waveParticle3 && (waveParticle4.DisallowSubdivision || waveParticle == null))
			{
				if (num2 > 3)
				{
					FilterRefractedDirections(waveParticle3, num2);
				}
				waveParticle3 = waveParticle;
				num2 = 0;
			}
		}
		while (waveParticle != null);
	}

	public void Update(float time)
	{
		WaveParticle waveParticle = LeftParticle;
		float num = time - LastUpdateTime;
		LastUpdateTime = time;
		float num2 = ((num < 1f) ? num : 1f);
		float invStep = 1f - num2;
		do
		{
			WaveParticle waveParticle2 = waveParticle;
			waveParticle = waveParticle.RightNeighbour;
			waveParticle2.Update(num, num2, invStep);
		}
		while (waveParticle != null);
	}

	private static void FilterRefractedDirections(WaveParticle left, int waveLength)
	{
		WaveParticle waveParticle = left;
		int num = waveLength / 2;
		Vector2 a = default;
		for (int i = 0; i < num; i++)
		{
			a += waveParticle.Direction;
			waveParticle = waveParticle.RightNeighbour;
		}
		Vector2 b = default;
		for (int j = num; j < waveLength; j++)
		{
			b += waveParticle.Direction;
			waveParticle = waveParticle.RightNeighbour;
		}
		a.Normalize();
		b.Normalize();
		waveParticle = left;
		for (int k = 0; k < waveLength; k++)
		{
			waveParticle.Direction = Vector2.Lerp(a, b, (float)k / (float)(waveLength - 1));
			waveParticle = waveParticle.RightNeighbour;
		}
	}
}
