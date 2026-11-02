using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public class ProfilesManager
{
	[SerializeField]
	[FormerlySerializedAs("initialProfile")]
	private WaterProfile _InitialProfile;

	[SerializeField]
	[FormerlySerializedAs("changed")]
	private Water.WaterEvent _Changed = new Water.WaterEvent();

	private Water _Water;

	private bool _ProfilesDirty;

	private WaterProfile _InitialProfileCopy;

	public List<Water.WeightedProfile> Profiles { get; private set; }

	public Water.WaterEvent Changed => _Changed;

	public void CacheProfiles(params WaterProfileData[] profiles)
	{
		WindWaves windWaves = _Water.WindWaves;
		if (windWaves != null)
		{
			for (int i = 0; i < profiles.Length; i++)
			{
				windWaves.SpectrumResolver.CacheSpectrum(profiles[i].Spectrum);
			}
		}
	}

	public float EvaluateProfilesParameter(Func<WaterProfileData, float> func)
	{
		float num = 0f;
		List<Water.WeightedProfile> profiles = Profiles;
		for (int num2 = profiles.Count - 1; num2 >= 0; num2--)
		{
			num += func(profiles[num2].Profile) * profiles[num2].Weight;
		}
		return num;
	}

	public void SetProfiles(List<Water.WeightedProfile> profiles)
	{
		for (int i = 0; i < profiles.Count; i++)
		{
			CacheProfiles(profiles[i].Profile);
		}
		CheckProfiles(profiles);
		Profiles = profiles;
		_ProfilesDirty = true;
	}

	public void ValidateProfiles()
	{
		bool flag = false;
		foreach (Water.WeightedProfile profile in Profiles)
		{
			flag |= profile.Profile.Dirty;
		}
		if (flag || _ProfilesDirty)
		{
			_ProfilesDirty = false;
			_Changed.Invoke(_Water);
		}
	}

	internal void Awake(Water water)
	{
		_Water = water;
		if (_Changed == null)
		{
			_Changed = new Water.WaterEvent();
		}
		if (Profiles == null)
		{
			if (_InitialProfile != null)
			{
				List<Water.WeightedProfile> list = new List<Water.WeightedProfile>();
				list.Add(new Water.WeightedProfile(_InitialProfile, 1f));
				SetProfiles(list);
			}
			else
			{
				Profiles = new List<Water.WeightedProfile>();
			}
		}
		WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
		WaterQualitySettings.Instance.Changed += OnQualitySettingsChanged;
	}

	internal void OnEnable()
	{
		_ProfilesDirty = true;
	}

	internal void OnDisable()
	{
		_ProfilesDirty = false;
	}

	internal void OnDestroy()
	{
		WaterQualitySettings.Instance.Changed -= OnQualitySettingsChanged;
	}

	internal void Update()
	{
		ValidateProfiles();
	}

	internal void OnValidate()
	{
		if (Profiles != null && Profiles.Count != 0 && (_InitialProfileCopy == _InitialProfile || _InitialProfileCopy == null))
		{
			_InitialProfileCopy = _InitialProfile;
			_ProfilesDirty = true;
		}
		else if (_InitialProfile != null)
		{
			_InitialProfileCopy = _InitialProfile;
			Profiles = new List<Water.WeightedProfile>();
			Profiles.Add(new Water.WeightedProfile(_InitialProfile, 1f));
			_ProfilesDirty = true;
		}
	}

	private void OnQualitySettingsChanged()
	{
		_ProfilesDirty = true;
	}

	private static void CheckProfiles(IList<Water.WeightedProfile> profiles)
	{
		if (profiles == null)
		{
			return;
		}
		if (profiles.Count == 0)
		{
			throw new ArgumentException("Water has to use at least one profile.");
		}
		float tileSize = profiles[0].Profile.TileSize;
		for (int i = 1; i < profiles.Count; i++)
		{
			if (profiles[i].Profile.TileSize != tileSize)
			{
				Debug.LogError("TileSize varies between used water profiles. It is the only parameter that you should keep equal on all profiles used at a time.");
				break;
			}
		}
	}
}
