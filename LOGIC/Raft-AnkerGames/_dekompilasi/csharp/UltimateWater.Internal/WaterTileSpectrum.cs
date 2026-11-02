using UnityEngine;

namespace UltimateWater.Internal;

public class WaterTileSpectrum
{
	public readonly Water Water;

	public readonly WindWaves WindWaves;

	public readonly int TileIndex;

	public Vector2[] DirectionalSpectrum;

	public Vector2[][] Displacements;

	public Vector4[][] ForceAndHeight;

	public float[] ResultsTiming;

	public int RecentResultIndex;

	public bool ResolveByFFT;

	public int DirectionalSpectrumDirty;

	private int _ResolutionFFT;

	private int _MipIndexFFT;

	private float _CachedTime = float.NegativeInfinity;

	private float _CachedTimeProp;

	private Vector2[] _CachedDisplacementsA;

	private Vector2[] _CachedDisplacementsB;

	private Vector4[] _CachedForceAndHeightA;

	private Vector4[] _CachedForceAndHeightB;

	public bool IsResolvedByFFT => ResolveByFFT;

	public int ResolutionFFT => _ResolutionFFT;

	public int MipIndexFFT => _MipIndexFFT;

	public void SetDirty()
	{
		DirectionalSpectrumDirty = 2;
	}

	public bool SetResolveMode(bool resolveByFFT, int resolution)
	{
		if (ResolveByFFT != resolveByFFT || (ResolveByFFT && _ResolutionFFT != resolution))
		{
			if (resolveByFFT)
			{
				lock (this)
				{
					int num = ((WindWaves.LoopDuration != 0f) ? ((int)(WindWaves.LoopDuration * 5f + 0.6f)) : 4);
					_ResolutionFFT = resolution;
					_MipIndexFFT = WaterWavesSpectrumDataBase.GetMipIndex(resolution);
					int num2 = resolution * resolution;
					DirectionalSpectrum = new Vector2[num2];
					Displacements = new Vector2[num][];
					ForceAndHeight = new Vector4[num][];
					ResultsTiming = new float[num];
					SetDirty();
					_CachedTime = float.NegativeInfinity;
					for (int i = 0; i < num; i++)
					{
						Displacements[i] = new Vector2[num2];
						ForceAndHeight[i] = new Vector4[num2];
					}
					if (!ResolveByFFT)
					{
						WaterAsynchronousTasks.Instance.AddFFTComputations(this);
						ResolveByFFT = true;
					}
				}
			}
			else
			{
				WaterAsynchronousTasks.Instance.RemoveFFTComputations(this);
				ResolveByFFT = false;
			}
			return true;
		}
		return false;
	}

	public void GetResults(float time, out Vector2[] da, out Vector2[] db, out Vector4[] fa, out Vector4[] fb, out float p)
	{
		if (WindWaves.LoopDuration != 0f)
		{
			time %= WindWaves.LoopDuration;
		}
		lock (this)
		{
			if (time == _CachedTime)
			{
				da = _CachedDisplacementsA;
				db = _CachedDisplacementsB;
				fa = _CachedForceAndHeightA;
				fb = _CachedForceAndHeightB;
				p = _CachedTimeProp;
				return;
			}
			int recentResultIndex = RecentResultIndex;
			for (int num = recentResultIndex - 1; num >= 0; num--)
			{
				if (ResultsTiming[num] <= time)
				{
					int num2 = num + 1;
					da = Displacements[num];
					db = Displacements[num2];
					fa = ForceAndHeight[num];
					fb = ForceAndHeight[num2];
					float num3 = ResultsTiming[num2] - ResultsTiming[num];
					if (num3 != 0f)
					{
						p = (time - ResultsTiming[num]) / num3;
					}
					else
					{
						p = 0f;
					}
					if (time > _CachedTime)
					{
						_CachedDisplacementsA = da;
						_CachedDisplacementsB = db;
						_CachedForceAndHeightA = fa;
						_CachedForceAndHeightB = fb;
						_CachedTimeProp = p;
						_CachedTime = time;
					}
					return;
				}
			}
			for (int num4 = ResultsTiming.Length - 1; num4 > recentResultIndex; num4--)
			{
				if (ResultsTiming[num4] <= time)
				{
					int num5 = ((num4 != Displacements.Length - 1) ? (num4 + 1) : 0);
					da = Displacements[num4];
					db = Displacements[num5];
					fa = ForceAndHeight[num4];
					fb = ForceAndHeight[num5];
					float num6 = ResultsTiming[num5] - ResultsTiming[num4];
					if (num6 != 0f)
					{
						p = (time - ResultsTiming[num4]) / num6;
					}
					else
					{
						p = 0f;
					}
					return;
				}
			}
			da = Displacements[recentResultIndex];
			db = Displacements[recentResultIndex];
			fa = ForceAndHeight[recentResultIndex];
			fb = ForceAndHeight[recentResultIndex];
			p = 0f;
		}
	}

	public WaterTileSpectrum(Water water, WindWaves windWaves, int index)
	{
		Water = water;
		WindWaves = windWaves;
		TileIndex = index;
	}
}
