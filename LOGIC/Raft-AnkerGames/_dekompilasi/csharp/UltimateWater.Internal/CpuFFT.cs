using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater.Internal;

public class CpuFFT
{
	public class FFTBuffers
	{
		public readonly float[] Timed;

		public readonly float[] PingPongA;

		public readonly float[] PingPongB;

		public readonly int[][] Indices;

		public readonly float[][] Weights;

		public readonly int NumButterflies;

		private readonly int _Resolution;

		private readonly Dictionary<float, Vector3[]> _PrecomputedKMap = new Dictionary<float, Vector3[]>(new FloatEqualityComparer());

		public FFTBuffers(int resolution)
		{
			_Resolution = resolution;
			Timed = new float[resolution * resolution * 12];
			PingPongA = new float[resolution * 12];
			PingPongB = new float[resolution * 12];
			NumButterflies = (int)(Mathf.Log(resolution) / Mathf.Log(2f));
			ButterflyFFTUtility.ComputeButterfly(resolution, NumButterflies, out Indices, out Weights);
			for (int i = 0; i < Indices.Length; i++)
			{
				int[] array = Indices[i];
				for (int j = 0; j < array.Length; j++)
				{
					array[j] *= 12;
				}
			}
		}

		public Vector3[] GetPrecomputedK(float tileSize)
		{
			if (!_PrecomputedKMap.TryGetValue(tileSize, out var value))
			{
				int num = _Resolution >> 1;
				float num2 = MathF.PI * 2f / tileSize;
				value = new Vector3[_Resolution * _Resolution];
				int num3 = 0;
				for (int i = 0; i < _Resolution; i++)
				{
					int num4 = (i + num) % _Resolution;
					float num5 = num2 * (float)(num4 - num);
					for (int j = 0; j < _Resolution; j++)
					{
						int num6 = (j + num) % _Resolution;
						float num7 = num2 * (float)(num6 - num);
						float num8 = Mathf.Sqrt(num7 * num7 + num5 * num5);
						value[num3++] = new Vector3((num8 != 0f) ? (num7 / num8) : 0f, (num8 != 0f) ? (num5 / num8) : 0f, num8);
					}
				}
				_PrecomputedKMap[tileSize] = value;
			}
			return value;
		}
	}

	private WaterTileSpectrum _TargetSpectrum;

	private float _Time;

	private int _Resolution;

	private static readonly Dictionary<int, FFTBuffers> _BuffersCache = new Dictionary<int, FFTBuffers>();

	public void Compute(WaterTileSpectrum targetSpectrum, float time, int outputBufferIndex)
	{
		_TargetSpectrum = targetSpectrum;
		_Time = time;
		Vector2[] directionalSpectrum;
		Vector2[] displacements;
		Vector4[] forceAndHeight;
		lock (targetSpectrum)
		{
			_Resolution = targetSpectrum.ResolutionFFT;
			directionalSpectrum = targetSpectrum.DirectionalSpectrum;
			displacements = targetSpectrum.Displacements[outputBufferIndex];
			forceAndHeight = targetSpectrum.ForceAndHeight[outputBufferIndex];
		}
		if (!_BuffersCache.TryGetValue(_Resolution, out var value))
		{
			value = (_BuffersCache[_Resolution] = new FFTBuffers(_Resolution));
		}
		float tileSize = targetSpectrum.WindWaves.UnscaledTileSizes[targetSpectrum.TileIndex];
		Vector3[] precomputedK = value.GetPrecomputedK(tileSize);
		if (targetSpectrum.DirectionalSpectrumDirty > 0)
		{
			ComputeDirectionalSpectra(targetSpectrum.TileIndex, directionalSpectrum, precomputedK);
			targetSpectrum.DirectionalSpectrumDirty--;
		}
		ComputeTimedSpectra(directionalSpectrum, value.Timed, precomputedK);
		ComputeFFT(value.Timed, displacements, forceAndHeight, value.Indices, value.Weights, value.PingPongA, value.PingPongB);
	}

	private void ComputeDirectionalSpectra(int scaleIndex, Vector2[] directional, Vector3[] kMap)
	{
		float num = 1f - _TargetSpectrum.WindWaves.SpectrumDirectionality;
		Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData> cachedSpectraDirect = _TargetSpectrum.WindWaves.SpectrumResolver.GetCachedSpectraDirect();
		int num2 = _Resolution * _Resolution;
		int num3 = _Resolution >> 1;
		int finalResolution = _TargetSpectrum.WindWaves.FinalResolution;
		Vector2 windDirection = _TargetSpectrum.WindWaves.SpectrumResolver.WindDirection;
		for (int i = 0; i < num2; i++)
		{
			directional[i].x = 0f;
			directional[i].y = 0f;
		}
		lock (cachedSpectraDirect)
		{
			Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>.ValueCollection.Enumerator enumerator = cachedSpectraDirect.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				WaterWavesSpectrumData current = enumerator.Current;
				if (current == null)
				{
					continue;
				}
				float weight = current.Weight;
				if (current.GetStandardDeviation() * weight <= 0.003f)
				{
					continue;
				}
				int num4 = 0;
				int num5 = 0;
				Vector3[] array = current.SpectrumValues[scaleIndex];
				for (int j = 0; j < _Resolution; j++)
				{
					if (j == num3)
					{
						num5 += (finalResolution - _Resolution) * finalResolution;
					}
					for (int k = 0; k < _Resolution; k++)
					{
						if (k == num3)
						{
							num5 += finalResolution - _Resolution;
						}
						float x = kMap[num4].x;
						float y = kMap[num4].y;
						if (x == 0f && y == 0f)
						{
							x = windDirection.x;
							y = windDirection.y;
						}
						float num6 = windDirection.x * x + windDirection.y * y;
						float num7 = Mathf.Sqrt(1f + array[num5].z * (2f * num6 * num6 - 1f));
						if (num6 < 0f)
						{
							num7 *= num;
						}
						float num8 = num7 * weight;
						directional[num4].x += array[num5].x * num8;
						directional[num4++].y += array[num5++].y * num8;
					}
				}
			}
			enumerator.Dispose();
		}
		List<WaterWavesSpectrumDataBase> overlayedSpectraDirect = _TargetSpectrum.WindWaves.SpectrumResolver.GetOverlayedSpectraDirect();
		lock (overlayedSpectraDirect)
		{
			for (int num9 = overlayedSpectraDirect.Count - 1; num9 >= 0; num9--)
			{
				WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = overlayedSpectraDirect[num9];
				float weight2 = waterWavesSpectrumDataBase.Weight;
				windDirection = waterWavesSpectrumDataBase.WindDirection;
				if (!(waterWavesSpectrumDataBase.GetStandardDeviation() * weight2 <= 0.003f))
				{
					float x2 = waterWavesSpectrumDataBase.WeatherSystemOffset.x;
					float y2 = waterWavesSpectrumDataBase.WeatherSystemOffset.y;
					float weatherSystemRadius = waterWavesSpectrumDataBase.WeatherSystemRadius;
					float num10 = Mathf.Sqrt(x2 * x2 + y2 * y2);
					float num11 = 0.84f * Mathf.Pow((float)Math.Tanh(Mathf.Pow(num10 / 22000f, 0.4f)), -0.75f);
					float num12 = waterWavesSpectrumDataBase.Gravity * FastMath.Pow2(num11 / 10f);
					int num13 = 0;
					int num14 = 0;
					Vector3[] array2 = waterWavesSpectrumDataBase.SpectrumValues[scaleIndex];
					for (int l = 0; l < _Resolution; l++)
					{
						if (l == num3)
						{
							num14 += (finalResolution - _Resolution) * finalResolution;
						}
						for (int m = 0; m < _Resolution; m++)
						{
							if (m == num3)
							{
								num14 += finalResolution - _Resolution;
							}
							float x3 = kMap[num13].x;
							float y3 = kMap[num13].y;
							if (x3 == 0f && y3 == 0f)
							{
								x3 = windDirection.x;
								y3 = windDirection.y;
							}
							float num15 = windDirection.x * x3 + windDirection.y * y3;
							float num16 = Mathf.Sqrt(1f + array2[num14].z * (2f * num15 * num15 - 1f));
							if (num15 < 0f)
							{
								num16 *= num;
							}
							float num17 = num16 * weight2;
							if (weatherSystemRadius != 0f)
							{
								float z = kMap[num13].z;
								float num18 = -2f * x3 * x2 + -2f * y3 * y2;
								float num19 = x2 * x2 + y2 * y2 - weatherSystemRadius * weatherSystemRadius;
								float num20 = num18 * num18 - 4f * num19;
								if (num20 < 0f)
								{
									directional[num13].x = 0f;
									directional[num13++].y = 0f;
									num14++;
									continue;
								}
								float num21 = Mathf.Sqrt(num20);
								float num22 = (num21 - num18) * 0.5f;
								float num23 = (0f - num21 - num18) * 0.5f;
								if (num22 > 0f && num23 > 0f)
								{
									directional[num13].x = 0f;
									directional[num13++].y = 0f;
									num14++;
									continue;
								}
								Vector2 a = new Vector2(x3 * num22, y3 * num22);
								Vector2 b = new Vector2(x3 * num23, y3 * num23);
								float num24 = Vector2.Distance(a, b) / (weatherSystemRadius * 2f);
								num17 *= num24;
								if (num22 * num23 > 0f)
								{
									float num25 = Mathf.Min(0f - num22, 0f - num23);
									float num26 = z / num12;
									float num27 = Mathf.Exp(-1E-06f * num25 * num26 * num26);
									num17 *= num27;
								}
							}
							directional[num13].x += array2[num14].x * num17;
							directional[num13++].y += array2[num14++].y * num17;
						}
					}
				}
			}
		}
	}

	private void ComputeTimedSpectra(Vector2[] directional, float[] timed, Vector3[] kMap)
	{
		Vector2 windDirection = _TargetSpectrum.WindWaves.SpectrumResolver.WindDirection;
		float gravity = _TargetSpectrum.Water.Gravity;
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < _Resolution; i++)
		{
			for (int j = 0; j < _Resolution; j++)
			{
				float x = kMap[num].x;
				float y = kMap[num].y;
				float z = kMap[num].z;
				if (x == 0f && y == 0f)
				{
					x = windDirection.x;
					y = windDirection.y;
				}
				int num3 = _Resolution * ((_Resolution - i) % _Resolution) + (_Resolution - j) % _Resolution;
				Vector2 vector = directional[num];
				Vector2 vector2 = directional[num3];
				float f = _Time * Mathf.Sqrt(gravity * z);
				float num4 = Mathf.Sin(f);
				float num5 = Mathf.Cos(f);
				float num6 = (vector.x + vector2.x) * num5 - (vector.y + vector2.y) * num4;
				float num7 = (vector.x - vector2.x) * num4 + (vector.y - vector2.y) * num5;
				timed[num2++] = num7 * x;
				timed[num2++] = num7 * y;
				timed[num2++] = 0f - num6;
				timed[num2++] = num7;
				timed[num2++] = num6 * x;
				timed[num2++] = num6 * y;
				timed[num2++] = num7;
				timed[num2++] = num6;
				timed[num2++] = num7 * x;
				timed[num2++] = (0f - num6) * x;
				timed[num2++] = num7 * y;
				timed[num2++] = (0f - num6) * y;
				num++;
			}
		}
	}

	private void ComputeFFT(float[] data, Vector2[] displacements, Vector4[] forceAndHeight, int[][] indices, float[][] weights, float[] pingPongA, float[] pingPongB)
	{
		int num = pingPongA.Length;
		int num2 = 0;
		for (int num3 = _Resolution - 1; num3 >= 0; num3--)
		{
			Array.Copy(data, num2, pingPongA, 0, num);
			FFT(indices, weights, ref pingPongA, ref pingPongB);
			Array.Copy(pingPongA, 0, data, num2, num);
			num2 += num;
		}
		num2 = _Resolution * (_Resolution + 1) * 12;
		for (int num4 = _Resolution - 1; num4 >= 0; num4--)
		{
			num2 -= 12;
			int num5 = num2;
			for (int num6 = num - 12; num6 >= 0; num6 -= 12)
			{
				num5 -= num;
				for (int i = 0; i < 12; i++)
				{
					pingPongA[num6 + i] = data[num5 + i];
				}
			}
			FFT(indices, weights, ref pingPongA, ref pingPongB);
			num5 = num2 / 12;
			for (int num7 = num - 12; num7 >= 0; num7 -= 12)
			{
				num5 -= _Resolution;
				forceAndHeight[num5] = new Vector4(pingPongA[num7], pingPongA[num7 + 2], pingPongA[num7 + 1], pingPongA[num7 + 7]);
				displacements[num5] = new Vector2(pingPongA[num7 + 8], pingPongA[num7 + 10]);
			}
		}
	}

	private void FFT(int[][] indices, float[][] weights, ref float[] pingPongA, ref float[] pingPongB)
	{
		int num = weights.Length;
		for (int i = 0; i < num; i++)
		{
			int[] array = indices[num - i - 1];
			float[] array2 = weights[i];
			int num2 = (_Resolution - 1) * 12;
			for (int num3 = array.Length - 2; num3 >= 0; num3 -= 2)
			{
				int num4 = array[num3];
				int num5 = array[num3 + 1];
				float num6 = array2[num3];
				float num7 = array2[num3 + 1];
				int num8 = num5 + 4;
				pingPongB[num2++] = pingPongA[num4++] + num7 * pingPongA[num8++] + num6 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num7 * pingPongA[num8++] + num6 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num7 * pingPongA[num8++] + num6 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num7 * pingPongA[num8] + num6 * pingPongA[num5++];
				num8 = num5;
				num5 -= 4;
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5++];
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5];
				num5 = num8;
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5 + 1];
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] + num7 * pingPongA[num5];
				pingPongB[num2++] = pingPongA[num4++] + num6 * pingPongA[num8++] - num7 * pingPongA[num5 + 3];
				pingPongB[num2] = pingPongA[num4] + num6 * pingPongA[num8] + num7 * pingPongA[num5 + 2];
				num2 -= 23;
			}
			float[] array3 = pingPongA;
			pingPongA = pingPongB;
			pingPongB = array3;
		}
	}
}
