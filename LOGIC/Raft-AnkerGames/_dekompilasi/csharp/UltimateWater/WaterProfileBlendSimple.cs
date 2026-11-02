using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Profile Blend (simple)")]
public class WaterProfileBlendSimple : MonoBehaviour
{
	[SerializeField]
	private Water _Water;

	[Header("Profiles")]
	[SerializeField]
	private WaterProfile _First;

	[SerializeField]
	private WaterProfile _Second;

	[Range(0f, 1f)]
	[SerializeField]
	[Header("Blend:")]
	private float _Factor;

	private readonly List<Water.WeightedProfile> _Profiles = new List<Water.WeightedProfile>();

	public Water Water
	{
		get
		{
			return _Water;
		}
		set
		{
			_Water = value;
			UpdateProfiles();
		}
	}

	public WaterProfile First
	{
		get
		{
			return _First;
		}
		set
		{
			_First = value;
			UpdateProfiles();
		}
	}

	public WaterProfile Second
	{
		get
		{
			return _Second;
		}
		set
		{
			_Second = value;
			UpdateProfiles();
		}
	}

	public float Factor
	{
		get
		{
			return _Factor;
		}
		set
		{
			_Factor = Mathf.Clamp01(value);
			UpdateProfiles();
		}
	}

	private void Awake()
	{
		if (Water == null)
		{
			Water = Utilities.GetWaterReference();
		}
	}

	private void Start()
	{
		UpdateProfiles();
	}

	private void OnValidate()
	{
		if (Application.isPlaying && !(Water == null) && Water.WindWaves != null)
		{
			UpdateProfiles();
		}
	}

	private void Reset()
	{
		if (Water == null)
		{
			Water = Utilities.GetWaterReference();
		}
	}

	private void UpdateProfiles()
	{
		if (!(Water == null) && !(First == null) && !(Second == null))
		{
			if (_Profiles.Count == 0)
			{
				_Profiles.Add(new Water.WeightedProfile(First, 1f - Factor));
				_Profiles.Add(new Water.WeightedProfile(Second, Factor));
			}
			Water.ProfilesManager.SetProfiles(_Profiles);
		}
	}
}
