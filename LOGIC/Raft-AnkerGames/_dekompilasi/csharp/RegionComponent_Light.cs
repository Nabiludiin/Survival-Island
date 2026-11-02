using System;
using UnityEngine;

public class RegionComponent_Light : MonoBehaviour
{
	[Serializable]
	public class LightData
	{
		public Light light;

		[HideInInspector]
		public float initialIntensity = 1f;

		public bool disableIntensityAtStart;

		public float lerpSpeed = 1f;
	}

	[Serializable]
	public class LightRegionData
	{
		public LightData[] lightDatas;

		public Subregion[] subregions;
	}

	public LightRegionData[] lightRegionData;

	private void Awake()
	{
		LightRegionData[] array = this.lightRegionData;
		foreach (LightRegionData lightRegionData in array)
		{
			if (lightRegionData == null)
			{
				continue;
			}
			LightData[] lightDatas = lightRegionData.lightDatas;
			foreach (LightData lightData in lightDatas)
			{
				lightData.initialIntensity = lightData.light.intensity;
				if (lightData.disableIntensityAtStart)
				{
					lightData.light.intensity = 0f;
				}
			}
		}
	}

	public void Update()
	{
		LightRegionData[] array = this.lightRegionData;
		foreach (LightRegionData lightRegionData in array)
		{
			bool flag = false;
			Subregion[] subregions = lightRegionData.subregions;
			for (int j = 0; j < subregions.Length; j++)
			{
				if (subregions[j].playerIsWithinSubregion)
				{
					flag = true;
					break;
				}
			}
			if (lightRegionData == null)
			{
				continue;
			}
			LightData[] lightDatas = lightRegionData.lightDatas;
			foreach (LightData lightData in lightDatas)
			{
				if (lightData == null || !(lightData.light != null))
				{
					continue;
				}
				Light light = lightData.light;
				if (light.enabled)
				{
					float num = (flag ? lightData.initialIntensity : 0f);
					if (light.intensity != num)
					{
						light.intensity = Mathf.MoveTowards(light.intensity, num, Time.deltaTime * lightData.lerpSpeed);
					}
				}
			}
		}
	}
}
