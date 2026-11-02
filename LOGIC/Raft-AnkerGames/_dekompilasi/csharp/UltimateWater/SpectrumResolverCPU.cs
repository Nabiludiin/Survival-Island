using System.Collections.Generic;
using System.Linq;
using UltimateWater.Internal;
using UnityEngine;

namespace UltimateWater;

public class SpectrumResolverCPU
{
	private WaterTileSpectrum[] _TileSpectra;

	private Vector2 _SurfaceOffset;

	private float _UniformWaterScale;

	private WaterWave[] _DirectWaves;

	private int _TargetDirectWavesCount = -1;

	private int _CachedSeed;

	private bool _CpuWavesDirty;

	private readonly int _NumTiles;

	private readonly Water _Water;

	private readonly WindWaves _WindWaves;

	private readonly List<WaterWavesSpectrumDataBase> _SpectraDataList;

	protected readonly List<WaterWavesSpectrumDataBase> _OverlayedSpectra;

	protected Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData> _SpectraDataCache;

	public WaterWave[] DirectWaves
	{
		get
		{
			lock (this)
			{
				return GetValidatedDirectWavesList();
			}
		}
	}

	public float MaxVerticalDisplacement { get; private set; }

	public float MaxHorizontalDisplacement { get; private set; }

	public Vector2 WindDirection { get; private set; }

	public float LastFrameTime { get; private set; }

	public float StdDev { get; private set; }

	public WaterTileSpectrum GetTile(int index)
	{
		return _TileSpectra[index];
	}

	public SpectrumResolverCPU(Water water, WindWaves windWaves, int numScales)
	{
		_Water = water;
		_WindWaves = windWaves;
		_SpectraDataCache = new Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>();
		_SpectraDataList = new List<WaterWavesSpectrumDataBase>();
		_OverlayedSpectra = new List<WaterWavesSpectrumDataBase>();
		_NumTiles = numScales;
		_CachedSeed = water.Seed;
		CreateSpectraLevels();
	}

	public List<WaterWavesSpectrumDataBase> GetOverlayedSpectraDirect()
	{
		return _OverlayedSpectra;
	}

	public void DisposeCachedSpectra()
	{
		lock (_SpectraDataCache)
		{
			Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>.Enumerator enumerator = _SpectraDataCache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Dispose(onlyTexture: false);
			}
			enumerator.Dispose();
		}
		SetDirectionalSpectrumDirty();
	}

	public WaterWavesSpectrumData GetSpectrumData(WaterWavesSpectrum spectrum)
	{
		if (!_SpectraDataCache.TryGetValue(spectrum, out var value))
		{
			lock (_SpectraDataCache)
			{
				value = (_SpectraDataCache[spectrum] = new WaterWavesSpectrumData(_Water, _WindWaves, spectrum));
			}
			value.ValidateSpectrumData();
			_CpuWavesDirty = true;
			lock (_SpectraDataList)
			{
				_SpectraDataList.Add(value);
			}
		}
		return value;
	}

	public virtual void AddSpectrum(WaterWavesSpectrumDataBase spectrum)
	{
		lock (_OverlayedSpectra)
		{
			_OverlayedSpectra.Add(spectrum);
		}
	}

	public virtual void RemoveSpectrum(WaterWavesSpectrumDataBase spectrum)
	{
		lock (_OverlayedSpectra)
		{
			_OverlayedSpectra.Remove(spectrum);
		}
	}

	public bool ContainsSpectrum(WaterWavesSpectrumDataBase spectrum)
	{
		return _OverlayedSpectra.Contains(spectrum);
	}

	public void CacheSpectrum(WaterWavesSpectrum spectrum)
	{
		GetSpectrumData(spectrum);
	}

	public Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData> GetCachedSpectraDirect()
	{
		return _SpectraDataCache;
	}

	public Vector3 GetDisplacementAt(float x, float z, float time)
	{
		Vector3 result = default;
		x = 0f - (x + _SurfaceOffset.x);
		z = 0f - (z + _SurfaceOffset.y);
		if (_TargetDirectWavesCount == -1)
		{
			for (int num = _NumTiles - 1; num >= 0; num--)
			{
				if (_TileSpectra[num].ResolveByFFT)
				{
					InterpolationParams(x, z, num, _WindWaves.TileSizes[num], out var fx, out var invFx, out var fy, out var invFy, out var index, out var index2, out var index3, out var index4);
					_TileSpectra[num].GetResults(time, out var da, out var db, out var fa, out var fb, out var p);
					Vector2 vector = FastMath.Interpolate(ref da[index], ref da[index2], ref da[index3], ref da[index4], ref db[index], ref db[index2], ref db[index3], ref db[index4], fx, invFx, fy, invFy, p);
					result.x -= vector.x;
					result.z -= vector.y;
					result.y += FastMath.Interpolate(fa[index].w, fa[index2].w, fa[index3].w, fa[index4].w, fb[index].w, fb[index2].w, fb[index3].w, fb[index4].w, fx, invFx, fy, invFy, p);
				}
			}
		}
		else
		{
			lock (this)
			{
				WaterWave[] validatedDirectWavesList = GetValidatedDirectWavesList();
				if (validatedDirectWavesList.Length != 0)
				{
					Vector3 vector2 = default;
					for (int i = 0; i < validatedDirectWavesList.Length; i++)
					{
						vector2 += validatedDirectWavesList[i].GetDisplacementAt(x, z, time);
					}
					result += vector2;
				}
			}
		}
		float num2 = (0f - _Water.Materials.HorizontalDisplacementScale) * _UniformWaterScale;
		result.x *= num2;
		result.y *= _UniformWaterScale;
		result.z *= num2;
		return result;
	}

	public Vector2 GetHorizontalDisplacementAt(float x, float z, float time)
	{
		Vector2 result = default;
		x = 0f - (x + _SurfaceOffset.x);
		z = 0f - (z + _SurfaceOffset.y);
		if (_TargetDirectWavesCount == -1)
		{
			for (int num = _NumTiles - 1; num >= 0; num--)
			{
				if (_TileSpectra[num].ResolveByFFT)
				{
					InterpolationParams(x, z, num, _WindWaves.TileSizes[num], out var fx, out var invFx, out var fy, out var invFy, out var index, out var index2, out var index3, out var index4);
					_TileSpectra[num].GetResults(time, out var da, out var db, out var _, out var _, out var p);
					result -= FastMath.Interpolate(ref da[index], ref da[index2], ref da[index3], ref da[index4], ref db[index], ref db[index2], ref db[index3], ref db[index4], fx, invFx, fy, invFy, p);
				}
			}
		}
		else
		{
			lock (this)
			{
				WaterWave[] validatedDirectWavesList = GetValidatedDirectWavesList();
				if (validatedDirectWavesList.Length != 0)
				{
					Vector2 vector = default;
					for (int i = 0; i < validatedDirectWavesList.Length; i++)
					{
						vector += validatedDirectWavesList[i].GetRawHorizontalDisplacementAt(x, z, time);
					}
					result += vector;
				}
			}
		}
		float num2 = (0f - _Water.Materials.HorizontalDisplacementScale) * _UniformWaterScale;
		result.x *= num2;
		result.y *= num2;
		return result;
	}

	public Vector4 GetForceAndHeightAt(float x, float z, float time)
	{
		Vector4 result = default;
		x = 0f - (x + _SurfaceOffset.x);
		z = 0f - (z + _SurfaceOffset.y);
		if (_TargetDirectWavesCount == -1)
		{
			for (int num = _NumTiles - 1; num >= 0; num--)
			{
				WaterTileSpectrum waterTileSpectrum = _TileSpectra[num];
				if (waterTileSpectrum.ResolveByFFT)
				{
					InterpolationParams(x, z, num, _WindWaves.TileSizes[num], out var fx, out var invFx, out var fy, out var invFy, out var index, out var index2, out var index3, out var index4);
					waterTileSpectrum.GetResults(time, out var _, out var _, out var fa, out var fb, out var p);
					result += FastMath.Interpolate(fa[index], fa[index2], fa[index3], fa[index4], fb[index], fb[index2], fb[index3], fb[index4], fx, invFx, fy, invFy, p);
				}
			}
		}
		else
		{
			lock (this)
			{
				WaterWave[] validatedDirectWavesList = GetValidatedDirectWavesList();
				if (validatedDirectWavesList.Length != 0)
				{
					Vector4 result2 = default;
					for (int i = 0; i < validatedDirectWavesList.Length; i++)
					{
						validatedDirectWavesList[i].GetForceAndHeightAt(x, z, time, ref result2);
					}
					result += result2;
				}
			}
		}
		float num2 = (0f - _Water.Materials.HorizontalDisplacementScale) * _UniformWaterScale;
		result.x *= num2;
		result.z *= num2;
		result.y *= 0.5f * _UniformWaterScale;
		result.w *= _UniformWaterScale;
		return result;
	}

	public float GetHeightAt(float x, float z, float time)
	{
		float num = 0f;
		x = 0f - (x + _SurfaceOffset.x);
		z = 0f - (z + _SurfaceOffset.y);
		if (_TargetDirectWavesCount == -1)
		{
			for (int num2 = _NumTiles - 1; num2 >= 0; num2--)
			{
				if (_TileSpectra[num2].ResolveByFFT)
				{
					InterpolationParams(x, z, num2, _WindWaves.TileSizes[num2], out var fx, out var invFx, out var fy, out var invFy, out var index, out var index2, out var index3, out var index4);
					_TileSpectra[num2].GetResults(time, out var _, out var _, out var fa, out var fb, out var p);
					float num3 = fa[index].w * fx + fa[index2].w * invFx;
					float num4 = fa[index3].w * fx + fa[index4].w * invFx;
					float num5 = num3 * fy + num4 * invFy;
					float num6 = fb[index].w * fx + fb[index2].w * invFx;
					float num7 = fb[index3].w * fx + fb[index4].w * invFx;
					float num8 = num6 * fy + num7 * invFy;
					num += num5 * (1f - p) + num8 * p;
				}
			}
		}
		else
		{
			lock (this)
			{
				WaterWave[] validatedDirectWavesList = GetValidatedDirectWavesList();
				if (validatedDirectWavesList.Length != 0)
				{
					float num9 = 0f;
					for (int i = 0; i < validatedDirectWavesList.Length; i++)
					{
						num9 += validatedDirectWavesList[i].GetHeightAt(x, z, time);
					}
					num += num9;
				}
			}
		}
		return num * _UniformWaterScale;
	}

	public void SetDirectWaveEvaluationMode(int waveCount)
	{
		lock (this)
		{
			if (_DirectWaves == null)
			{
				_DirectWaves = new WaterWave[0];
			}
			_TargetDirectWavesCount = waveCount;
			_CpuWavesDirty = true;
		}
	}

	public void SetFFTEvaluationMode()
	{
		lock (this)
		{
			_DirectWaves = null;
			_TargetDirectWavesCount = -1;
		}
	}

	public WaterWave[] FindMostMeaningfulWaves(int waveCount)
	{
		Heap<WaterWave> heap = new Heap<WaterWave>();
		for (int num = _SpectraDataList.Count - 1; num >= 0; num--)
		{
			WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = _SpectraDataList[num];
			waterWavesSpectrumDataBase.UpdateSpectralValues(WindDirection, _WindWaves.SpectrumDirectionality);
			lock (waterWavesSpectrumDataBase)
			{
				float weight = waterWavesSpectrumDataBase.Weight;
				WaterWave[] cpuWaves = waterWavesSpectrumDataBase.CpuWaves;
				for (int i = 0; i < cpuWaves.Length; i++)
				{
					WaterWave element = cpuWaves[i];
					element._Amplitude *= weight;
					element._CPUPriority *= weight;
					heap.Insert(element);
					if (heap.Count > waveCount)
					{
						heap.ExtractMax();
					}
				}
			}
		}
		return heap.ToArray();
	}

	public virtual void SetDirectionalSpectrumDirty()
	{
		_CpuWavesDirty = true;
		for (int num = _SpectraDataList.Count - 1; num >= 0; num--)
		{
			_SpectraDataList[num].SetCpuWavesDirty();
		}
		for (int i = 0; i < _NumTiles; i++)
		{
			_TileSpectra[i].SetDirty();
		}
	}

	internal void Update()
	{
		_SurfaceOffset = _Water.SurfaceOffset;
		float num = _Water.Time;
		if (_WindWaves.LoopDuration != 0f)
		{
			num %= _WindWaves.LoopDuration;
		}
		LastFrameTime = num;
		_UniformWaterScale = _Water.UniformWaterScale;
		UpdateCachedSeed();
		bool allowCpuFFT = WaterProjectSettings.Instance.AllowCpuFFT;
		for (int i = 0; i < _NumTiles; i++)
		{
			int num2 = 16;
			int num3 = 0;
			while (true)
			{
				float num4 = 0f;
				for (int num5 = _SpectraDataList.Count - 1; num5 >= 0; num5--)
				{
					WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = _SpectraDataList[num5];
					waterWavesSpectrumDataBase.ValidateSpectrumData();
					float standardDeviation = waterWavesSpectrumDataBase.GetStandardDeviation(i, num3);
					num4 += standardDeviation * waterWavesSpectrumDataBase.Weight;
				}
				for (int num6 = _OverlayedSpectra.Count - 1; num6 >= 0; num6--)
				{
					WaterWavesSpectrumDataBase waterWavesSpectrumDataBase2 = _OverlayedSpectra[num6];
					waterWavesSpectrumDataBase2.ValidateSpectrumData();
					float standardDeviation2 = waterWavesSpectrumDataBase2.GetStandardDeviation(i, num3);
					num4 += standardDeviation2 * waterWavesSpectrumDataBase2.Weight;
				}
				if (num4 < _WindWaves.CpuDesiredStandardError * 0.25f || num2 >= _WindWaves.FinalResolution)
				{
					break;
				}
				num2 <<= 1;
				num3++;
			}
			if (num2 > _WindWaves.FinalResolution)
			{
				num2 = _WindWaves.FinalResolution;
			}
			if (_TileSpectra[i].SetResolveMode((num2 >= 16) & allowCpuFFT, num2))
			{
				_CpuWavesDirty = true;
			}
		}
	}

	internal void SetWindDirection(Vector2 windDirection)
	{
		WindDirection = windDirection;
		SetDirectionalSpectrumDirty();
	}

	private void InterpolationParams(float x, float z, int scaleIndex, float tileSize, out float fx, out float invFx, out float fy, out float invFy, out int index0, out int index1, out int index2, out int index3)
	{
		int resolutionFFT = _TileSpectra[scaleIndex].ResolutionFFT;
		float num = 0.5f / (float)_WindWaves.FinalResolution * tileSize;
		x += num;
		z += num;
		float num2 = (float)resolutionFFT / tileSize;
		fx = x * num2;
		fy = z * num2;
		int num3 = (int)fx;
		if ((float)num3 > fx)
		{
			num3--;
		}
		int num4 = (int)fy;
		if ((float)num4 > fy)
		{
			num4--;
		}
		fx -= num3;
		fy -= num4;
		num3 %= resolutionFFT;
		num4 %= resolutionFFT;
		if (num3 < 0)
		{
			num3 += resolutionFFT;
		}
		if (num4 < 0)
		{
			num4 += resolutionFFT;
		}
		num3 = resolutionFFT - num3 - 1;
		num4 = resolutionFFT - num4 - 1;
		int num5 = num3 + 1;
		int num6 = num4 + 1;
		if (num5 == resolutionFFT)
		{
			num5 = 0;
		}
		if (num6 == resolutionFFT)
		{
			num6 = 0;
		}
		num4 *= resolutionFFT;
		num6 *= resolutionFFT;
		index0 = num4 + num3;
		index1 = num4 + num5;
		index2 = num6 + num3;
		index3 = num6 + num5;
		invFx = 1f - fx;
		invFy = 1f - fy;
	}

	private WaterWave[] GetValidatedDirectWavesList()
	{
		if (_CpuWavesDirty)
		{
			_CpuWavesDirty = false;
			WaterWave[] source = FindMostMeaningfulWaves(_TargetDirectWavesCount);
			_DirectWaves = source.ToArray();
		}
		return _DirectWaves;
	}

	public GerstnerWave[] SelectShorelineWaves(int waveCount, float angle, float coincidenceRange)
	{
		Heap<WaterWave> heap = new Heap<WaterWave>();
		for (int num = _SpectraDataList.Count - 1; num >= 0; num--)
		{
			WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = _SpectraDataList[num];
			waterWavesSpectrumDataBase.UpdateSpectralValues(WindDirection, _WindWaves.SpectrumDirectionality);
			lock (waterWavesSpectrumDataBase)
			{
				float weight = waterWavesSpectrumDataBase.Weight;
				WaterWave[] shorelineCandidates = waterWavesSpectrumDataBase.ShorelineCandidates;
				for (int i = 0; i < shorelineCandidates.Length; i++)
				{
					WaterWave element = shorelineCandidates[i];
					element._Amplitude *= weight;
					element._CPUPriority *= weight;
					if (Mathf.Abs(Mathf.DeltaAngle(Mathf.Atan2(element._Nkx, element._Nky) * 57.29578f, angle)) < coincidenceRange && element._Amplitude > 0.025f)
					{
						heap.Insert(element);
						if (heap.Count > waveCount)
						{
							heap.ExtractMax();
						}
					}
				}
			}
		}
		Vector2[] array = new Vector2[4];
		for (int j = 0; j < 4; j++)
		{
			float num2 = _WindWaves.TileSizes[j];
			array[j].x = num2 + 0.5f / (float)_WindWaves.FinalResolution * num2;
			array[j].y = 0f - num2 + 0.5f / (float)_WindWaves.FinalResolution * num2;
		}
		WaterWave[] array2 = heap.ToArray();
		int num3 = Mathf.Min(heap.Count, waveCount);
		GerstnerWave[] array3 = new GerstnerWave[num3];
		for (int k = 0; k < num3; k++)
		{
			array3[k] = new GerstnerWave(array2[heap.Count - k - 1], array);
		}
		return array3;
	}

	private void UpdateCachedSeed()
	{
		if (_CachedSeed != _Water.Seed)
		{
			_CachedSeed = _Water.Seed;
			DisposeCachedSpectra();
			OnProfilesChanged();
		}
	}

	internal virtual void OnProfilesChanged()
	{
		List<Water.WeightedProfile> profiles = _Water.ProfilesManager.Profiles;
		Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>.Enumerator enumerator = _SpectraDataCache.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Weight = 0f;
		}
		enumerator.Dispose();
		for (int i = 0; i < profiles.Count; i++)
		{
			Water.WeightedProfile weightedProfile = profiles[i];
			if (!(weightedProfile.Weight <= 0.0001f))
			{
				WaterWavesSpectrum spectrum = weightedProfile.Profile.Spectrum;
				if (!_SpectraDataCache.TryGetValue(spectrum, out var value))
				{
					value = GetSpectrumData(spectrum);
				}
				value.Weight = weightedProfile.Weight;
			}
		}
		SetDirectionalSpectrumDirty();
		StdDev = 0f;
		float num = 0f;
		enumerator = _SpectraDataCache.GetEnumerator();
		while (enumerator.MoveNext())
		{
			WaterWavesSpectrumData value2 = enumerator.Current.Value;
			value2.ValidateSpectrumData();
			StdDev += value2.GetStandardDeviation() * value2.Weight;
			if (value2.CpuWaves.Length != 0)
			{
				num += value2.CpuWaves[0]._Amplitude * value2.Weight;
			}
		}
		enumerator.Dispose();
		for (int num2 = _OverlayedSpectra.Count - 1; num2 >= 0; num2--)
		{
			WaterWavesSpectrumDataBase waterWavesSpectrumDataBase = _OverlayedSpectra[num2];
			waterWavesSpectrumDataBase.ValidateSpectrumData();
			StdDev += waterWavesSpectrumDataBase.GetStandardDeviation() * waterWavesSpectrumDataBase.Weight;
			if (waterWavesSpectrumDataBase.CpuWaves.Length != 0)
			{
				num += waterWavesSpectrumDataBase.CpuWaves[0]._Amplitude * waterWavesSpectrumDataBase.Weight;
			}
		}
		MaxVerticalDisplacement = StdDev * 1.6f + num;
		MaxHorizontalDisplacement = MaxVerticalDisplacement * _Water.Materials.HorizontalDisplacementScale;
	}

	private void CreateSpectraLevels()
	{
		_TileSpectra = new WaterTileSpectrum[_NumTiles];
		for (int i = 0; i < _NumTiles; i++)
		{
			_TileSpectra[i] = new WaterTileSpectrum(_Water, _WindWaves, i);
		}
	}

	internal virtual void OnMapsFormatChanged(bool resolution)
	{
		if (_SpectraDataCache != null)
		{
			Dictionary<WaterWavesSpectrum, WaterWavesSpectrumData>.Enumerator enumerator = _SpectraDataCache.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.Dispose(!resolution);
			}
			enumerator.Dispose();
		}
		if (resolution)
		{
			for (int num = _OverlayedSpectra.Count - 1; num >= 0; num--)
			{
				_OverlayedSpectra[num].Dispose(onlyTexture: false);
			}
		}
		SetDirectionalSpectrumDirty();
	}

	internal virtual void OnDestroy()
	{
		OnMapsFormatChanged(resolution: true);
		_SpectraDataCache = null;
		for (int num = _OverlayedSpectra.Count - 1; num >= 0; num--)
		{
			_OverlayedSpectra[num].Dispose(onlyTexture: false);
		}
		_OverlayedSpectra.Clear();
		lock (_SpectraDataList)
		{
			_SpectraDataList.Clear();
		}
	}
}
