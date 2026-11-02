using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

public class WaterQualitySettings : ScriptableObjectSingleton
{
	public WaterRipplesData Ripples;

	[FormerlySerializedAs("qualityLevels")]
	[SerializeField]
	private WaterQualityLevel[] _QualityLevels;

	[SerializeField]
	[FormerlySerializedAs("synchronizeWithUnity")]
	private bool _SynchronizeWithUnity = true;

	[SerializeField]
	[FormerlySerializedAs("savedCustomQualityLevel")]
	private int _SavedCustomQualityLevel;

	private int _WaterQualityIndex;

	private WaterQualityLevel _CurrentQualityLevel;

	private static WaterQualitySettings _Instance;

	public WaterQualityLevel CurrentQualityLevel => _CurrentQualityLevel;

	public string[] Names
	{
		get
		{
			string[] array = new string[_QualityLevels.Length];
			for (int i = 0; i < _QualityLevels.Length; i++)
			{
				array[i] = _QualityLevels[i].Name;
			}
			return array;
		}
	}

	public WaterWavesMode WavesMode
	{
		get
		{
			return _CurrentQualityLevel.WavesMode;
		}
		set
		{
			if (_CurrentQualityLevel.WavesMode != value)
			{
				_CurrentQualityLevel.WavesMode = value;
				OnChange();
			}
		}
	}

	public int MaxSpectrumResolution
	{
		get
		{
			return _CurrentQualityLevel.MaxSpectrumResolution;
		}
		set
		{
			if (_CurrentQualityLevel.MaxSpectrumResolution != value)
			{
				_CurrentQualityLevel.MaxSpectrumResolution = value;
				OnChange();
			}
		}
	}

	public float TileSizeScale
	{
		get
		{
			return _CurrentQualityLevel.TileSizeScale;
		}
		set
		{
			if (_CurrentQualityLevel.TileSizeScale != value)
			{
				_CurrentQualityLevel.TileSizeScale = value;
				OnChange();
			}
		}
	}

	public bool AllowHighPrecisionTextures
	{
		get
		{
			return _CurrentQualityLevel.AllowHighPrecisionTextures;
		}
		set
		{
			if (_CurrentQualityLevel.AllowHighPrecisionTextures != value)
			{
				_CurrentQualityLevel.AllowHighPrecisionTextures = value;
				OnChange();
			}
		}
	}

	public bool AllowHighQualityNormalMaps
	{
		get
		{
			return _CurrentQualityLevel.AllowHighQualityNormalMaps;
		}
		set
		{
			if (_CurrentQualityLevel.AllowHighQualityNormalMaps != value)
			{
				_CurrentQualityLevel.AllowHighQualityNormalMaps = value;
				OnChange();
			}
		}
	}

	public float MaxTesselationFactor
	{
		get
		{
			return _CurrentQualityLevel.MaxTesselationFactor;
		}
		set
		{
			if (_CurrentQualityLevel.MaxTesselationFactor != value)
			{
				_CurrentQualityLevel.MaxTesselationFactor = value;
				OnChange();
			}
		}
	}

	public int MaxVertexCount
	{
		get
		{
			return _CurrentQualityLevel.MaxVertexCount;
		}
		set
		{
			if (_CurrentQualityLevel.MaxVertexCount != value)
			{
				_CurrentQualityLevel.MaxVertexCount = value;
				OnChange();
			}
		}
	}

	public int MaxTesselatedVertexCount
	{
		get
		{
			return _CurrentQualityLevel.MaxTesselatedVertexCount;
		}
		set
		{
			if (_CurrentQualityLevel.MaxTesselatedVertexCount != value)
			{
				_CurrentQualityLevel.MaxTesselatedVertexCount = value;
				OnChange();
			}
		}
	}

	public bool AllowAlphaBlending
	{
		get
		{
			return _CurrentQualityLevel.AllowAlphaBlending;
		}
		set
		{
			if (_CurrentQualityLevel.AllowAlphaBlending != value)
			{
				_CurrentQualityLevel.AllowAlphaBlending = value;
				OnChange();
			}
		}
	}

	public static WaterQualitySettings Instance
	{
		get
		{
			if ((object)_Instance == null || _Instance == null)
			{
				_Instance = ScriptableObjectSingleton.LoadSingleton<WaterQualitySettings>();
				_Instance.Changed = null;
				_Instance._WaterQualityIndex = -1;
				_Instance.SynchronizeQualityLevel();
			}
			return _Instance;
		}
	}

	public bool SynchronizeWithUnity => _SynchronizeWithUnity;

	public event Action Changed;

	public int GetQualityLevel()
	{
		return _WaterQualityIndex;
	}

	public void SetQualityLevel(int index)
	{
		if (!Application.isPlaying)
		{
			_SavedCustomQualityLevel = index;
		}
		_CurrentQualityLevel = _QualityLevels[index];
		_WaterQualityIndex = index;
		OnChange();
	}

	public void SynchronizeQualityLevel()
	{
		int num = -1;
		if (_SynchronizeWithUnity)
		{
			num = FindQualityLevel(QualitySettings.names[QualitySettings.GetQualityLevel()]);
		}
		if (num == -1)
		{
			num = _SavedCustomQualityLevel;
		}
		num = Mathf.Clamp(num, 0, _QualityLevels.Length - 1);
		if (num != _WaterQualityIndex)
		{
			SetQualityLevel(num);
		}
	}

	internal WaterQualityLevel[] GetQualityLevelsDirect()
	{
		return _QualityLevels;
	}

	private void OnChange()
	{
		if (Changed != null)
		{
			Changed();
		}
	}

	private int FindQualityLevel(string levelName)
	{
		for (int i = 0; i < _QualityLevels.Length; i++)
		{
			if (_QualityLevels[i].Name == levelName)
			{
				return i;
			}
		}
		return -1;
	}

	private void SynchronizeLevelNames()
	{
	}
}
