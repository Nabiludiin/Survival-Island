using System;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace UltimateWater.Internal;

public abstract class WaterWavesSpectrumDataBase
{
	private float[][] _StandardDeviationData;

	private bool _CpuWavesDirty = true;

	private Vector2 _LastWindDirection;

	private float _StdDev;

	private Vector2 _WindDirection = new Vector2(1f, 0f);

	private Texture2D _Texture;

	private readonly float _TileSize;

	private readonly float _Gravity;

	protected readonly Water _Water;

	protected readonly WindWaves _WindWaves;

	private static Color[] _Colors = new Color[262144];

	public WaterWave[] CpuWaves { get; private set; }

	public WaterWave[] ShorelineCandidates { get; private set; }

	public Vector3[][] SpectrumValues { get; private set; }

	public Texture2D Texture
	{
		get
		{
			if (_Texture == null)
			{
				CreateSpectrumTexture();
			}
			return _Texture;
		}
	}

	public float Weight { get; set; }

	public float Gravity => _Gravity;

	public Vector2 WeatherSystemOffset { get; set; }

	public float WeatherSystemRadius { get; set; }

	public Vector2 WindDirection
	{
		get
		{
			return _WindDirection;
		}
		set
		{
			_WindDirection = value;
		}
	}

	public static int GetMipIndex(int i)
	{
		if (i == 0)
		{
			return 0;
		}
		int num = (int)Mathf.Log(i, 2f) - 3;
		if (num < 0)
		{
			return 0;
		}
		return num;
	}

	public float GetStandardDeviation()
	{
		return _StdDev;
	}

	public float GetStandardDeviation(int scaleIndex, int mipLevel)
	{
		float[] array = _StandardDeviationData[scaleIndex];
		if (mipLevel >= array.Length)
		{
			return 0f;
		}
		return array[mipLevel];
	}

	public void SetCpuWavesDirty()
	{
		_CpuWavesDirty = true;
	}

	public void ValidateSpectrumData()
	{
		if (CpuWaves != null)
		{
			return;
		}
		lock (this)
		{
			if (CpuWaves != null)
			{
				return;
			}
			if (SpectrumValues == null)
			{
				SpectrumValues = new Vector3[4][];
				_StandardDeviationData = new float[4][];
			}
			int finalResolution = _WindWaves.FinalResolution;
			int num = finalResolution * finalResolution;
			int num2 = Mathf.RoundToInt(Mathf.Log(finalResolution, 2f)) - 4;
			if (SpectrumValues[0] == null || SpectrumValues[0].Length != num)
			{
				for (int i = 0; i < 4; i++)
				{
					SpectrumValues[i] = new Vector3[num];
					_StandardDeviationData[i] = new float[num2 + 1];
				}
			}
			GenerateContents(SpectrumValues);
			AnalyzeSpectrum();
		}
	}

	public void UpdateSpectralValues(Vector2 windDirection, float directionality)
	{
		ValidateSpectrumData();
		if (!_CpuWavesDirty)
		{
			return;
		}
		lock (this)
		{
			if (CpuWaves == null || !_CpuWavesDirty)
			{
				return;
			}
			lock (CpuWaves)
			{
				_CpuWavesDirty = false;
				float directionalityInv = 1f - directionality;
				float horizontalDisplacementScale = _Water.Materials.HorizontalDisplacementScale;
				int finalResolution = _WindWaves.FinalResolution;
				bool mostlySorted = Vector2.Dot(_LastWindDirection, windDirection) >= 0.97f;
				WaterWave[] cpuWaves = CpuWaves;
				for (int i = 0; i < cpuWaves.Length; i++)
				{
					cpuWaves[i].UpdateSpectralValues(SpectrumValues, windDirection, directionalityInv, finalResolution, horizontalDisplacementScale);
				}
				SortCpuWaves(cpuWaves, mostlySorted);
				WaterWave[] shorelineCandidates = ShorelineCandidates;
				for (int j = 0; j < shorelineCandidates.Length; j++)
				{
					shorelineCandidates[j].UpdateSpectralValues(SpectrumValues, windDirection, directionalityInv, finalResolution, horizontalDisplacementScale);
				}
				_LastWindDirection = windDirection;
			}
		}
	}

	public static void SortCpuWaves(WaterWave[] windWaves, bool mostlySorted)
	{
		if (!mostlySorted)
		{
			Array.Sort(windWaves, (WaterWave a, WaterWave b) =>
			{
				if (a._CPUPriority > b._CPUPriority)
				{
					return -1;
				}
				return (a._CPUPriority != b._CPUPriority) ? 1 : 0;
			});
			return;
		}
		int num = windWaves.Length;
		int num2 = 0;
		for (int num3 = 1; num3 < num; num3++)
		{
			if (windWaves[num2]._CPUPriority < windWaves[num3]._CPUPriority)
			{
				WaterWave waterWave = windWaves[num2];
				windWaves[num2] = windWaves[num3];
				windWaves[num3] = waterWave;
				if (num3 != 1)
				{
					num3 -= 2;
				}
			}
			num2 = num3;
		}
	}

	public void Dispose(bool onlyTexture)
	{
		if (_Texture != null)
		{
			_Texture.Destroy();
			_Texture = null;
		}
		if (!onlyTexture)
		{
			lock (this)
			{
				SpectrumValues = null;
				CpuWaves = null;
				_CpuWavesDirty = true;
			}
		}
	}

	protected WaterWavesSpectrumDataBase(Water water, WindWaves windWaves, float tileSize, float gravity)
	{
		_Water = water;
		_WindWaves = windWaves;
		_TileSize = tileSize;
		_Gravity = gravity;
	}

	private void CreateSpectrumTexture()
	{
		ValidateSpectrumData();
		int resolution = _WindWaves.FinalResolution;
		int width = resolution << 1;
		int num = resolution << 1;
		if (_Colors.Length < width * num)
		{
			_Colors = new Color[width * num];
		}
		Thread[] array = new Thread[4];
		for (int i = 0; i < 4; i++)
		{
			int index = i;
			array[i] = new Thread(() =>
			{
				Calculate(index, resolution, width);
			});
		}
		for (int num2 = 0; num2 < 4; num2++)
		{
			array[num2].Start();
		}
		_Texture = new Texture2D(width, num, TextureFormat.RGBAFloat, mipChain: false, linear: true)
		{
			hideFlags = HideFlags.DontSave,
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Repeat
		};
		for (int num3 = 0; num3 < 4; num3++)
		{
			array[num3].Join();
		}
		_Texture.SetPixels(0, 0, width, num, _Colors);
		_Texture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
	}

	private void Calculate(int scaleIndex, int resolution, int width)
	{
		Vector3[] array = SpectrumValues[scaleIndex];
		int num = (((scaleIndex & 1) != 0) ? resolution : 0);
		int num2 = (((scaleIndex & 2) != 0) ? resolution : 0) * width + num;
		for (int i = 0; i < resolution; i++)
		{
			int num3 = i * resolution;
			int num4 = i * width + num2;
			for (int j = 0; j < resolution; j++)
			{
				Vector3 vector = array[num3 + j];
				int num5 = num4 + j;
				_Colors[num5].r = vector.x;
				_Colors[num5].g = vector.y;
				_Colors[num5].b = vector.z;
			}
		}
	}

	private void AnalyzeSpectrum()
	{
		int finalResolution = _WindWaves.FinalResolution;
		int num = finalResolution >> 1;
		int num2 = Mathf.RoundToInt(Mathf.Log(finalResolution >> 1, 2f)) - 4;
		Heap<WaterWave> heap = new Heap<WaterWave>();
		Heap<WaterWave> heap2 = new Heap<WaterWave>();
		_StdDev = 0f;
		for (byte b = 0; b < 4; b++)
		{
			Vector3[] array = SpectrumValues[b];
			float[] array2 = (_StandardDeviationData[b] = new float[num2 + 1]);
			float num3 = MathF.PI * 2f / _TileSize;
			float offsetX = _TileSize + 0.5f / (float)finalResolution * _TileSize;
			float offsetZ = 0f - _TileSize + 0.5f / (float)finalResolution * _TileSize;
			for (int i = 0; i < finalResolution; i++)
			{
				float num4 = num3 * (float)(i - num);
				ushort num5 = (ushort)((i + num) % finalResolution);
				ushort num6 = (ushort)(num5 * finalResolution);
				for (int j = 0; j < finalResolution; j++)
				{
					float num7 = num3 * (float)(j - num);
					ushort num8 = (ushort)((j + num) % finalResolution);
					Vector3 vector = array[num6 + num8];
					float num9 = vector.x * vector.x + vector.y * vector.y;
					float num10 = Mathf.Sqrt(num9);
					float num11 = Mathf.Sqrt(num4 * num4 + num7 * num7);
					float w = Mathf.Sqrt(_Gravity * num11);
					if (num10 >= 0.0025f)
					{
						heap2.Insert(new WaterWave(b, offsetX, offsetZ, num5, num8, num4, num7, num11, w, num10));
						if (heap2.Count > 100)
						{
							heap2.ExtractMax();
						}
					}
					if (num10 > 0.025f)
					{
						heap.Insert(new WaterWave(b, offsetX, offsetZ, num5, num8, num4, num7, num11, w, num10));
						if (heap.Count > 200)
						{
							heap.ExtractMax();
						}
					}
					int mipIndex = GetMipIndex(Mathf.Max(Mathf.Min(num5, finalResolution - num5 - 1), Mathf.Min(num8, finalResolution - num8 - 1)));
					array2[mipIndex] += num9;
				}
			}
			for (int k = 0; k < array2.Length; k++)
			{
				array2[k] = Mathf.Sqrt(2f * array2[k]);
				_StdDev += array2[k];
			}
		}
		CpuWaves = heap2.ToArray();
		SortCpuWaves(CpuWaves, mostlySorted: false);
		ShorelineCandidates = heap.ToArray();
		Array.Sort(ShorelineCandidates);
	}

	protected abstract void GenerateContents(Vector3[][] spectrumValues);
}
