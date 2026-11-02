using System.Collections.Generic;
using UnityEngine;

namespace UltimateWater;

[AddComponentMenu("Ultimate Water/Water Profile Blend")]
public class WaterProfileBlend : MonoBehaviour
{
	public class Cache
	{
		public List<Water.WeightedProfile> profiles = new List<Water.WeightedProfile>();

		public List<float> normalizedWeight = new List<float>();

		public Stack<Water.WeightedProfile> profileCache = new Stack<Water.WeightedProfile>();

		public Water.WeightedProfile GetProfile()
		{
			if (profileCache.Count > 0)
			{
				return profileCache.Pop();
			}
			return default;
		}

		public void ReturnProfile(Water.WeightedProfile value)
		{
			profileCache.Push(value);
		}
	}

	public Water Water;

	public List<WaterProfile> _Profiles = new List<WaterProfile> { null };

	public List<float> _Weights = new List<float> { 1f };

	private Cache cache = new Cache();

	private void Awake()
	{
		if (Water == null)
		{
			Water = Utilities.GetWaterReference();
			if (Water.IsNullReference(this))
			{
				return;
			}
		}
		_Profiles.RemoveAll((WaterProfile x) => x == null);
		if (_Profiles.Count == 0)
		{
			Debug.LogError("[WaterProfileBlend] : no valid profiles found");
			enabled = false;
		}
	}

	private void Start()
	{
		CreateProfilesNoAlloc(_Profiles, _Weights, cache);
		Water.ProfilesManager.SetProfiles(cache.profiles);
	}

	public void OnValidate()
	{
		if (Application.isPlaying && Water != null && Water.WindWaves != null)
		{
			CreateProfilesNoAlloc(_Profiles, _Weights, cache);
			Water.ProfilesManager.SetProfiles(cache.profiles);
		}
		if (WeightSum(_Weights) == 0f)
		{
			_Weights[0] = 1f;
		}
	}

	private void Reset()
	{
		if (Water == null)
		{
			Water = Utilities.GetWaterReference();
		}
	}

	private static Water.WeightedProfile[] CreateProfiles(List<WaterProfile> profiles, List<float> weights)
	{
		List<float> list = NormalizeWeights(weights);
		if (WeightSum(weights) == 0f)
		{
			list[0] = 1f;
		}
		int count = profiles.Count;
		Water.WeightedProfile[] array = new Water.WeightedProfile[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new Water.WeightedProfile(profiles[i], list[i]);
		}
		return array;
	}

	private static void CreateProfilesNoAlloc(List<WaterProfile> profiles, List<float> weights, Cache cache)
	{
		NormalizeWeights(weights, cache);
		if (WeightSum(weights) == 0f)
		{
			cache.normalizedWeight[0] = 1f;
		}
		cache.profiles.Clear();
		int count = profiles.Count;
		for (int i = 0; i < count; i++)
		{
			Water.WeightedProfile profile = cache.GetProfile();
			profile.SetNonCopy(profiles[i].Data, cache.normalizedWeight[i]);
			cache.profiles.Add(profile);
		}
	}

	private static float WeightSum(List<float> weights)
	{
		float num = 0f;
		for (int i = 0; i < weights.Count; i++)
		{
			num += weights[i];
		}
		return num;
	}

	private static List<float> NormalizeWeights(List<float> weights)
	{
		List<float> list = new List<float>(weights.Count);
		float num = WeightSum(weights);
		for (int i = 0; i < weights.Count; i++)
		{
			float item = ((num > 0f) ? (weights[i] / num) : 0f);
			list.Add(item);
		}
		return list;
	}

	private static void NormalizeWeights(List<float> weights, Cache cache)
	{
		cache.normalizedWeight.Clear();
		float num = WeightSum(weights);
		for (int i = 0; i < weights.Count; i++)
		{
			float item = ((num > 0f) ? (weights[i] / num) : 0f);
			cache.normalizedWeight.Add(item);
		}
	}
}
