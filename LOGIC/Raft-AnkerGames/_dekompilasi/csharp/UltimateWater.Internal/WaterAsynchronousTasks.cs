using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace UltimateWater.Internal;

public sealed class WaterAsynchronousTasks : MonoBehaviour
{
	private static WaterAsynchronousTasks _Instance;

	private bool _Run;

	private readonly List<WaterTileSpectrum> _FFTSpectra = new List<WaterTileSpectrum>();

	private int _FFTSpectrumIndex;

	private float _FFTTimeStep = 0.2f;

	private readonly List<WaterSample> _Computations = new List<WaterSample>();

	private int _ComputationIndex;

	private Exception _ThreadException;

	public static WaterAsynchronousTasks Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = UnityEngine.Object.FindObjectOfType<WaterAsynchronousTasks>();
				if (_Instance == null)
				{
					_Instance = new GameObject("Ultimate Water Spectrum Sampler")
					{
						hideFlags = HideFlags.HideInHierarchy
					}.AddComponent<WaterAsynchronousTasks>();
				}
			}
			return _Instance;
		}
	}

	public static bool HasInstance => _Instance != null;

	public void AddWaterSampleComputations(WaterSample computation)
	{
		lock (_Computations)
		{
			_Computations.Add(computation);
		}
	}

	public void RemoveWaterSampleComputations(WaterSample computation)
	{
		lock (_Computations)
		{
			int num = _Computations.IndexOf(computation);
			if (num != -1)
			{
				if (num < _ComputationIndex)
				{
					_ComputationIndex--;
				}
				_Computations.RemoveAt(num);
			}
		}
	}

	public void AddFFTComputations(WaterTileSpectrum scale)
	{
		lock (_FFTSpectra)
		{
			_FFTSpectra.Add(scale);
		}
	}

	public void RemoveFFTComputations(WaterTileSpectrum scale)
	{
		lock (_FFTSpectra)
		{
			int num = _FFTSpectra.IndexOf(scale);
			if (num != -1)
			{
				if (num < _FFTSpectrumIndex)
				{
					_FFTSpectrumIndex--;
				}
				_FFTSpectra.RemoveAt(num);
			}
		}
	}

	private void Awake()
	{
		_Run = true;
		if (_Instance == null)
		{
			_Instance = this;
		}
		else if (_Instance != this)
		{
			gameObject.Destroy();
			return;
		}
		if (Application.isPlaying)
		{
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			for (int i = 0; i < WaterProjectSettings.Instance.PhysicsThreads; i++)
			{
				Thread thread = new Thread(RunSamplingTask);
				thread.Priority = WaterProjectSettings.Instance.PhysicsThreadsPriority;
				thread.Start();
			}
			Thread thread2 = new Thread(RunFFTTask);
			thread2.Priority = WaterProjectSettings.Instance.PhysicsThreadsPriority;
			thread2.Start();
		}
	}

	private void OnDisable()
	{
		_Run = false;
		if (_ThreadException != null)
		{
			UnityEngine.Debug.LogException(_ThreadException);
		}
	}

	private void RunSamplingTask()
	{
		try
		{
			while (_Run)
			{
				WaterSample waterSample = null;
				lock (_Computations)
				{
					if (_Computations.Count != 0)
					{
						if (_ComputationIndex >= _Computations.Count)
						{
							_ComputationIndex = 0;
						}
						waterSample = _Computations[_ComputationIndex++];
					}
				}
				if (waterSample == null)
				{
					Thread.Sleep(2);
					continue;
				}
				lock (waterSample)
				{
					waterSample.ComputationStep();
				}
			}
		}
		catch (Exception threadException)
		{
			_ThreadException = threadException;
		}
	}

	private void RunFFTTask()
	{
		try
		{
			CpuFFT cpuFFT = new CpuFFT();
			Stopwatch stopwatch = new Stopwatch();
			bool flag = false;
			while (_Run)
			{
				WaterTileSpectrum waterTileSpectrum = null;
				lock (_FFTSpectra)
				{
					if (_FFTSpectra.Count != 0)
					{
						if (_FFTSpectrumIndex >= _FFTSpectra.Count)
						{
							_FFTSpectrumIndex = 0;
						}
						if (_FFTSpectrumIndex == 0)
						{
							if ((float)stopwatch.ElapsedMilliseconds > _FFTTimeStep * 900f)
							{
								if (flag)
								{
									_FFTTimeStep += 0.05f;
								}
								else
								{
									flag = true;
								}
							}
							else
							{
								flag = false;
								if (_FFTTimeStep > 0.2f)
								{
									_FFTTimeStep -= 0.001f;
								}
							}
							stopwatch.Reset();
							stopwatch.Start();
						}
						waterTileSpectrum = _FFTSpectra[_FFTSpectrumIndex++];
					}
				}
				if (waterTileSpectrum == null)
				{
					stopwatch.Reset();
					Thread.Sleep(6);
					continue;
				}
				bool flag2 = false;
				SpectrumResolver spectrumResolver = waterTileSpectrum.WindWaves.SpectrumResolver;
				if (spectrumResolver == null)
				{
					continue;
				}
				int recentResultIndex = waterTileSpectrum.RecentResultIndex;
				int num = (recentResultIndex + 2) % waterTileSpectrum.ResultsTiming.Length;
				int num2 = (recentResultIndex + 1) % waterTileSpectrum.ResultsTiming.Length;
				float num3 = waterTileSpectrum.ResultsTiming[recentResultIndex];
				float num4 = waterTileSpectrum.ResultsTiming[num];
				float num5 = waterTileSpectrum.ResultsTiming[num2];
				float lastFrameTime = spectrumResolver.LastFrameTime;
				if (num4 <= lastFrameTime || num5 > lastFrameTime)
				{
					float loopDuration = waterTileSpectrum.WindWaves.LoopDuration;
					float num6;
					if (loopDuration != 0f)
					{
						num6 = Mathf.Round((num3 % loopDuration + 0.2f) / 0.2f) * 0.2f;
					}
					else
					{
						num6 = ((!(num5 > lastFrameTime)) ? (Mathf.Max(num3, lastFrameTime) + _FFTTimeStep) : (lastFrameTime + _FFTTimeStep));
					}
					if (num6 != num5)
					{
						cpuFFT.Compute(waterTileSpectrum, num6, num2);
						waterTileSpectrum.ResultsTiming[num2] = num6;
						flag2 = true;
					}
					waterTileSpectrum.RecentResultIndex = num2;
				}
				if (!flag2)
				{
					stopwatch.Reset();
					Thread.Sleep(3);
				}
			}
		}
		catch (Exception threadException)
		{
			_ThreadException = threadException;
		}
	}
}
