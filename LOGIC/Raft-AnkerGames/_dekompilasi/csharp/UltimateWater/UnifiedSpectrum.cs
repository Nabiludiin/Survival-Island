using System;
using UnityEngine;

namespace UltimateWater;

public class UnifiedSpectrum : WaterWavesSpectrum
{
	private readonly float _Fetch;

	private readonly float _FreqScale;

	public UnifiedSpectrum(float tileSize, float gravity, float windSpeed, float amplitude, float freqScale, float fetch)
		: base(tileSize, gravity, windSpeed, amplitude)
	{
		_Fetch = fetch;
		_FreqScale = freqScale;
	}

	public override void ComputeSpectrum(Vector3[] spectrum, float tileSizeMultiplier, int maxResolution, System.Random random)
	{
		int num = Mathf.RoundToInt(Mathf.Sqrt(spectrum.Length));
		int num2 = num / 2;
		int num3 = (maxResolution - num) / 2;
		if (num3 < 0)
		{
			num3 = 0;
		}
		float num4 = MathF.PI * 2f / (TileSize * tileSizeMultiplier * _FreqScale);
		float windSpeed = _WindSpeed;
		float num5 = 0.84f * Mathf.Pow((float)Math.Tanh(Mathf.Pow(_Fetch / 22000f, 0.4f)), -0.75f);
		float f = ((num5 <= 1f) ? 1.7f : (1.7f + 6f * Mathf.Log(num5)));
		float num6 = 2f * _Gravity / 0.0529f;
		float num7 = _Gravity * FastMath.Pow2(num5 / windSpeed);
		float num8 = PhaseSpeed(num7, num6);
		float num9 = windSpeed / num8;
		float num10 = 0.006f * Mathf.Sqrt(num9);
		float num11 = (0f - num9) / Mathf.Sqrt(10f);
		float num12 = 0.08f * (1f + 4f * Mathf.Pow(num5, -3f));
		float num13 = 1f / (2f * num12 * num12);
		float num14 = 3.7E-05f * windSpeed * windSpeed / _Gravity * Mathf.Pow(windSpeed / num8, 0.9f);
		float num15 = windSpeed * 0.41f / Mathf.Log(10f / num14);
		float num16 = Mathf.Log(2f) / 4f;
		float num17 = 4f;
		float num18 = 0.13f * num15 / 0.23f;
		float num19 = 0.01f * ((num15 < 0.23f) ? (1f + Mathf.Log(num15 / 0.23f)) : (1f + 3f * Mathf.Log(num15 / 0.23f)));
		for (int i = 0; i < num3; i++)
		{
			for (int j = 0; j < maxResolution; j++)
			{
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
			}
		}
		for (int k = 0; k < num; k++)
		{
			float num20 = num4 * (float)(k - num2);
			float num21 = num20 * num20;
			int num22 = (k + num2) % num * num;
			for (int l = 0; l < num3; l++)
			{
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
			}
			for (int m = 0; m < num; m++)
			{
				float num23 = num4 * (float)(m - num2);
				float num24 = Mathf.Sqrt(num21 + num23 * num23);
				float num25 = PhaseSpeed(num24, num6);
				float num26 = Mathf.Exp(-1.25f * FastMath.Pow2(num7 / num24));
				float num27 = Mathf.Sqrt(num24 / num7) - 1f;
				float p = Mathf.Exp((0f - FastMath.Pow2(num27)) * num13);
				float num28 = Mathf.Pow(f, p);
				float num29 = num26 * num28 * Mathf.Exp(num11 * num27);
				float num30 = 0.5f * num10 * (num8 / num25) * num29;
				float num31 = Mathf.Exp(-0.25f * FastMath.Pow2(num24 / num6 - 1f));
				float num32 = 0.5f * num19 * (0.23f / num25) * num31 * num26;
				float z = (float)Math.Tanh(num16 + num17 * Mathf.Pow(num25 / num8, 2.5f) + num18 * Mathf.Pow(0.23f / num25, 2.5f));
				float num33 = _Amplitude * (num30 + num32) / (num24 * num24 * num24 * num24 * 2f * MathF.PI);
				num33 = ((!(num33 > 0f)) ? 0f : (Mathf.Sqrt(num33) * num4 * 0.5f));
				float x = FastMath.Gauss01() * num33;
				float y = FastMath.Gauss01() * num33;
				int num34 = (m + num2) % num;
				if (k == num2 && m == num2)
				{
					x = 0f;
					y = 0f;
					z = 0f;
				}
				spectrum[num22 + num34] = new Vector3(x, y, z);
			}
			for (int n = 0; n < num3; n++)
			{
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
			}
		}
		for (int num35 = 0; num35 < num3; num35++)
		{
			for (int num36 = 0; num36 < maxResolution; num36++)
			{
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
				UnityEngine.Random.Range(1E-06f, 1f);
				_ = UnityEngine.Random.value;
			}
		}
	}

	private float PhaseSpeed(float k, float km)
	{
		return Mathf.Sqrt(_Gravity / k * (1f + FastMath.Pow2(k / km)));
	}
}
