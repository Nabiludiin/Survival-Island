using System.Collections.Generic;
using UnityEngine;

public class LightSingularityExternal : MonoBehaviour
{
	private LightSingularityManager lightManager;

	[SerializeField]
	private bool isSingularity;

	[SerializeField]
	private LightSingularity morphedLightSingularity;

	public Transform lightOffsetPosition;

	public Color lightColor;

	public float lightRange = 3f;

	public float intensity = 1.5f;

	private bool hasBeenPlaced;

	public bool IsSingularity
	{
		get
		{
			return isSingularity;
		}
		set
		{
			isSingularity = value;
		}
	}

	public LightSingularity MorphedLightSingularity
	{
		get
		{
			return morphedLightSingularity;
		}
		set
		{
			morphedLightSingularity = value;
		}
	}

	public void OnBlockPlaced()
	{
		lightManager = ComponentManager<LightSingularityManager>.Value;
		hasBeenPlaced = true;
		OnPlaceLight();
	}

	public void OnDestroy()
	{
		if (!hasBeenPlaced)
		{
			return;
		}
		lightManager.RemoveLight(this);
		if (MorphedLightSingularity != null)
		{
			if (MorphedLightSingularity.GetExternals().Count == 1)
			{
				MorphedLightSingularity.RemoveExternalFromList(this);
				lightManager.RemoveLightSingularity(MorphedLightSingularity);
				MorphedLightSingularity = null;
			}
			else
			{
				DisconnectFromSingularity();
			}
		}
	}

	public void SetAsPartOfSingularity(bool p_value, LightSingularity p_singularity)
	{
		IsSingularity = p_value;
		MorphedLightSingularity = p_singularity;
	}

	public void OnPlaceLight()
	{
		lightManager.AddLight(this);
		ChooseWhichLightToBecome();
	}

	public void ChooseWhichLightToBecome()
	{
		List<GameObject> list = lightManager.FindNearbyLights(lightOffsetPosition, lightManager.torchPlaceFindDistance, checkIfObstructed: true);
		List<LightSingularity> list2 = lightManager.FilterOutSingularitiesFromExternals(list);
		if (list2.Count < 1)
		{
			lightManager.CreateNewSingularityFromMultiple(list);
		}
		else
		{
			if (list2.Count < 1)
			{
				return;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				if (!lightManager.MergeLightToSingularity(this, list2) && list.Count >= 2)
				{
					lightManager.CreateNewSingularityFromMultiple(list);
				}
			}
		}
	}

	public void UpdateLightConnectivity()
	{
		List<GameObject> list = lightManager.FindNearbyLights(lightOffsetPosition, lightManager.torchPlaceFindDistance, checkIfObstructed: true);
		List<LightSingularity> list2 = lightManager.FilterOutSingularitiesFromExternals(list);
		if (list2.Count <= 0)
		{
			lightManager.CreateNewSingularityFromMultiple(list);
		}
		else
		{
			if (list2.Count < 0)
			{
				return;
			}
			for (int i = 0; i < list2.Count; i++)
			{
				if (IsSingularity)
				{
					if (!CheckConnectivityToMorphedSingularity())
					{
						DisconnectFromSingularity();
					}
				}
				else if (!lightManager.MergeLightToSingularity(this, list2) && list.Count >= 2)
				{
					lightManager.CreateNewSingularityFromMultiple(list);
				}
			}
		}
	}

	private bool CheckConnectivityToMorphedSingularity()
	{
		if (lightManager.AreLightsInRangeAndInView(lightOffsetPosition.transform, MorphedLightSingularity.transform, lightManager.torchPlaceFindDistance))
		{
			return true;
		}
		return false;
	}

	private void DisconnectFromSingularity()
	{
		MorphedLightSingularity.RemoveExternalFromList(this);
		MorphedLightSingularity.AverageOutSingularityPosition();
		MorphedLightSingularity.UpdateFurthestExternalInfo();
		MorphedLightSingularity.UpdateLightRangeToProper(9f);
		MorphedLightSingularity.UpdateLightIntensity(1.5f);
		MorphedLightSingularity = null;
		IsSingularity = false;
	}
}
