using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSingularityManager : MonoBehaviour
{
	[SerializeField]
	public ObjectPool lightSingularityPool;

	[SerializeField]
	private List<LightSingularityExternal> lightSources = new List<LightSingularityExternal>();

	private List<LightSingularity> lightSingularites = new List<LightSingularity>();

	[SerializeField]
	public float singularityMergeRange = 5f;

	[SerializeField]
	public float baseSingularityRange = 6f;

	public float torchPlaceFindDistance = 3f;

	private Transform lockedPivotParent;

	public float lightIntensityDividedByAmountMultiplier = 25f;

	private IEnumerator singularityNullCheck;

	private void Awake()
	{
		lightSingularityPool.Initialize();
		ComponentManager<LightSingularityManager>.Value = this;
		lockedPivotParent = SingletonGeneric<GameManager>.Singleton.lockedPivot;
	}

	private void Start()
	{
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Combine(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnBlockIsCreated));
	}

	private void OnDisable()
	{
		BlockCreator.PlaceBlockCallStack = (Action<Block, Network_Player, bool, int>)Delegate.Remove(BlockCreator.PlaceBlockCallStack, new Action<Block, Network_Player, bool, int>(OnBlockIsCreated));
	}

	private void OnBlockIsCreated(Block p_block, Network_Player p_player, bool isReplicating, int hotSlotIndex)
	{
		_ = p_block.transform.position;
		List<GameObject> list = FindNearbyLights(p_block.transform, torchPlaceFindDistance, checkIfObstructed: false);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].GetComponent<LightSingularityExternal>().UpdateLightConnectivity();
		}
		List<LightSingularity> list2 = FilterOutSingularitiesFromExternals(list);
		for (int j = 0; j < list2.Count; j++)
		{
			list2[j].RecheckExternalConnection();
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].GetComponent<LightSingularityExternal>().UpdateLightConnectivity();
		}
	}

	public void AddLight(LightSingularityExternal p_lightExternal)
	{
		lightSources.Add(p_lightExternal);
	}

	public void RemoveLight(LightSingularityExternal p_lightExternal)
	{
		lightSources.Remove(p_lightExternal);
	}

	public void AddLight(LightSingularity p_lightSingularity)
	{
		lightSingularites.Add(p_lightSingularity);
	}

	public void RemoveLightSingularity(LightSingularity p_lightSingularity)
	{
		lightSingularites.Remove(p_lightSingularity);
		PoolManager.ReturnObject(p_lightSingularity.gameObject);
	}

	public List<GameObject> FindNearbyLights(LightSingularityExternal from, float p_distance, bool checkIfObstructed)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < lightSources.Count; i++)
		{
			if (!(lightSources[i] != null) || from.gameObject.GetInstanceID() == lightSources[i].gameObject.GetInstanceID())
			{
				continue;
			}
			if (checkIfObstructed)
			{
				if (AreLightsInRangeAndInView(from.lightOffsetPosition, lightSources[i].lightOffsetPosition, p_distance))
				{
					list.Add(lightSources[i].gameObject);
				}
			}
			else if (Vector3.Distance(from.lightOffsetPosition.position, lightSources[i].transform.position) < p_distance)
			{
				list.Add(lightSources[i].gameObject);
			}
		}
		return list;
	}

	public List<GameObject> FindNearbyLights(Transform from, float p_distance, bool checkIfObstructed)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < lightSources.Count; i++)
		{
			if (!(lightSources[i] != null))
			{
				continue;
			}
			if (checkIfObstructed)
			{
				if (AreLightsInRangeAndInView(from, lightSources[i].lightOffsetPosition, p_distance))
				{
					list.Add(lightSources[i].gameObject);
				}
			}
			else if (Vector3.Distance(from.transform.position, lightSources[i].lightOffsetPosition.position) < p_distance)
			{
				list.Add(lightSources[i].gameObject);
			}
		}
		return list;
	}

	private GameObject CreateLightSingularity()
	{
		GameObject gameObject = lightSingularityPool.GetObject(returnObjectActive: true);
		gameObject.transform.SetParent(lockedPivotParent);
		return gameObject;
	}

	public bool MergeLightToSingularity(LightSingularityExternal p_external, List<LightSingularity> p_singularities)
	{
		LightSingularity lightSingularity = FindClosestViableSingularity(p_singularities, p_external);
		if (lightSingularity != null)
		{
			p_external.SetAsPartOfSingularity(p_value: true, lightSingularity);
			lightSingularity.AddExternalToExternalList(p_external);
			lightSingularity.UpdateFurthestExternalInfo();
			lightSingularity.SetLightRange(baseSingularityRange + lightSingularity.GetDistanceToFurthestAwayExternal() + (float)lightSingularity.GetExternals().Count);
			lightSingularity.AverageOutSingularityPosition();
			return true;
		}
		return false;
	}

	public LightSingularity FindClosestViableSingularity(List<LightSingularity> p_singularities, LightSingularityExternal p_to)
	{
		float num = float.PositiveInfinity;
		LightSingularity result = null;
		for (int i = 0; i < p_singularities.Count; i++)
		{
			if (p_singularities[i].LightComponent.enabled)
			{
				float num2 = Vector3.Distance(p_singularities[i].transform.position, p_to.transform.position);
				if (num2 < num && num2 < singularityMergeRange)
				{
					num = num2;
					result = p_singularities[i];
				}
			}
		}
		return result;
	}

	public void CreateNewSingularityFromMultiple(List<GameObject> p_lightGameObjects)
	{
		if (p_lightGameObjects.Count >= 1)
		{
			LightSingularity component = CreateLightSingularity().GetComponent<LightSingularity>();
			component.GetExternals().Clear();
			SetLightDefaults(component);
			lightSingularites.Add(component);
			SetExternalsAsPartOfSingularity(component, p_lightGameObjects);
			component.AverageOutSingularityPosition();
			ModifySingularitysLight(p_lightGameObjects, component);
			CheckToMergeSingularityWithOtherSingularities(component);
		}
	}

	public List<LightSingularity> FilterOutSingularitiesFromExternals(List<GameObject> p_lightGameObjects)
	{
		List<LightSingularity> list = new List<LightSingularity>();
		for (int i = 0; i < p_lightGameObjects.Count; i++)
		{
			LightSingularityExternal component = p_lightGameObjects[i].GetComponent<LightSingularityExternal>();
			if (component.IsSingularity && !list.Contains(component.MorphedLightSingularity))
			{
				list.Add(component.MorphedLightSingularity);
			}
		}
		return list;
	}

	private void SetLightDefaults(LightSingularity singularity)
	{
		singularity.LightComponent.intensity = 1.5f;
		singularity.LightComponent.enabled = true;
		singularity.LightComponent.range = 2f;
	}

	private void SetExternalsAsPartOfSingularity(LightSingularity p_singularity, List<GameObject> p_lightGameObjects)
	{
		for (int i = 0; i < p_lightGameObjects.Count; i++)
		{
			LightSingularityExternal component = p_lightGameObjects[i].GetComponent<LightSingularityExternal>();
			component.SetAsPartOfSingularity(p_value: true, p_singularity);
			p_singularity.AddExternalToExternalList(component);
		}
	}

	private void ModifySingularitysLight(List<GameObject> otherLights, LightSingularity p_singualrityLight)
	{
		p_singualrityLight.UpdateFurthestExternalInfo();
		p_singualrityLight.SetLightRange(baseSingularityRange + p_singualrityLight.GetDistanceToFurthestAwayExternal() + (float)p_singualrityLight.GetExternals().Count);
	}

	public void CheckToMergeSingularityWithOtherSingularities(LightSingularity p_singularity)
	{
		for (int i = 0; i < lightSingularites.Count; i++)
		{
			if (p_singularity != lightSingularites[i] && AreLightsInRangeAndInView(p_singularity.LightComponent, lightSingularites[i].LightComponent, singularityMergeRange))
			{
				p_singularity.MergeWithOtherSingularity(lightSingularites[i]);
			}
		}
	}

	public bool AreLightsInRangeAndInView(Light p_from, Light p_to, float p_range)
	{
		if (Vector3.Distance(p_from.transform.position, p_to.transform.position) < p_range)
		{
			if (AreLightsInView(p_from, p_to))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool AreLightsInRangeAndInView(Transform p_from, Transform p_to, float p_range)
	{
		if (Vector3.Distance(p_from.position, p_to.position) < p_range)
		{
			if (AreLightsInView(p_from, p_to))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public bool AreLightsInView(Light p_from, Light p_to)
	{
		if (Physics.Linecast(p_from.transform.position, p_to.transform.position, out var hitInfo, LayerMasks.MASK_Block, QueryTriggerInteraction.Collide))
		{
			Debug.DrawLine(p_from.transform.position, hitInfo.point, Color.red, 6f);
			return false;
		}
		return true;
	}

	public bool AreLightsInView(Transform p_from, Transform p_to)
	{
		if (Physics.Linecast(p_from.position, p_to.position, out var hitInfo, LayerMasks.MASK_Block, QueryTriggerInteraction.Collide))
		{
			Debug.DrawLine(p_from.position, hitInfo.point, Color.red, 6f);
			return false;
		}
		return true;
	}
}
