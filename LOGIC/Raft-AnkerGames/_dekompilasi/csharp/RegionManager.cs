using System;
using System.Collections.Generic;
using UnityEngine;

public class RegionManager : MonoBehaviour, SceneEventInterface
{
	public static Action OnRegionUpdate;

	public static List<Region> CurrentRegions = new List<Region>();

	public static List<Subregion> CurrentSubregions = new List<Subregion>();

	public static List<Region> AllRegions = new List<Region>();

	[SerializeField]
	private float updateCurrentRegionInterval = 1f;

	private Network_Player localPlayer;

	private float updateCurrentRegionTimer;

	private static bool forceUpdate;

	private void LateUpdate()
	{
		if (localPlayer == null)
		{
			localPlayer = ComponentManager<Network_Player>.Value;
		}
		if (localPlayer == null)
		{
			return;
		}
		if (localPlayer != null)
		{
			updateCurrentRegionTimer += Time.deltaTime;
			if (updateCurrentRegionTimer >= updateCurrentRegionInterval || forceUpdate)
			{
				forceUpdate = false;
				updateCurrentRegionTimer = 0f;
				Vector3 zero = Vector3.zero;
				zero = ((!localPlayer.currentModel.thirdPersonSettings.ThirdPersonState) ? localPlayer.CameraTransform.position : localPlayer.PlayerHeadBone.position);
				if (localPlayer.PlayerHeadBone != null)
				{
					UpdateCurrentRegionAndSubregions(zero);
				}
			}
		}
		foreach (Region currentRegion in CurrentRegions)
		{
			if (currentRegion == null)
			{
				continue;
			}
			Subregion[] subregions = currentRegion.subregions;
			foreach (Subregion subregion in subregions)
			{
				if (!(subregion == null))
				{
					subregion.LateUpdateSubRegion();
				}
			}
		}
	}

	void SceneEventInterface.OnSceneEvent(SceneEvent sceneEvent)
	{
		if (sceneEvent == SceneEvent.LeaveGame)
		{
			OnRegionUpdate = null;
			if (CurrentRegions != null)
			{
				CurrentRegions.Clear();
			}
			if (CurrentSubregions != null)
			{
				CurrentSubregions.Clear();
			}
		}
	}

	public static void UpdateCurrentRegionAndSubregions(Vector3 position)
	{
		if (CurrentRegions == null)
		{
			CurrentRegions = new List<Region>();
		}
		else
		{
			CurrentRegions.Clear();
		}
		if (CurrentSubregions == null)
		{
			CurrentSubregions = new List<Subregion>();
		}
		else
		{
			foreach (Subregion currentSubregion in CurrentSubregions)
			{
				currentSubregion.playerIsWithinSubregion = false;
			}
			CurrentSubregions.Clear();
		}
		if (AllRegions.ContainsItems())
		{
			foreach (Region allRegion in AllRegions)
			{
				if (allRegion == null)
				{
					continue;
				}
				if (allRegion.regionArea == null)
				{
					Debug.LogError("Region " + allRegion.gameObject.name + " does not have a area variable assigned", allRegion.gameObject);
				}
				else
				{
					if (!allRegion.gameObject.activeInHierarchy || !(allRegion.regionArea.GetNormalizedDistanceToPoint(position) > 0f))
					{
						continue;
					}
					CurrentRegions.Add(allRegion);
					for (int i = 0; i < allRegion.subregions.Length; i++)
					{
						Subregion subregion = allRegion.subregions[i];
						if (subregion != null)
						{
							subregion.playerIsWithinSubregion = subregion.regionArea.GetNormalizedDistanceToPoint(position) > 0f;
							if (subregion.playerIsWithinSubregion)
							{
								CurrentSubregions.Add(subregion);
							}
						}
					}
				}
			}
		}
		if (OnRegionUpdate != null)
		{
			OnRegionUpdate();
		}
	}

	public static void AddGroup(Region group)
	{
		if (!AllRegions.Contains(group))
		{
			AllRegions.Add(group);
		}
	}

	public static void RemoveGroup(Region group)
	{
		AllRegions.Remove(group);
	}

	public static List<Region> GetRegionsFromPosition(Vector3 position)
	{
		if (AllRegions == null)
		{
			return null;
		}
		List<Region> list = null;
		foreach (Region allRegion in AllRegions)
		{
			if (allRegion == null)
			{
				continue;
			}
			if (allRegion.regionArea == null)
			{
				Debug.LogError("Region " + allRegion.gameObject.name + " does not have a area variable assigned", allRegion.gameObject);
			}
			else if (allRegion.gameObject.activeInHierarchy && allRegion.regionArea.GetNormalizedDistanceToPoint(position) > 0f)
			{
				if (list == null)
				{
					list = new List<Region>();
				}
				list.AddUniqueOnly(allRegion);
			}
		}
		return list;
	}

	public static List<Region> GetRegionsFromPosition(Vector3 position, Type subRegionComponentType)
	{
		List<Region> regionsFromPosition = GetRegionsFromPosition(position);
		if (regionsFromPosition.ContainsItems())
		{
			for (int i = 0; i < regionsFromPosition.Count; i++)
			{
				if (!regionsFromPosition[i].ContainsSubRegionComponentType(subRegionComponentType))
				{
					regionsFromPosition.RemoveAt(i);
					i--;
				}
			}
		}
		return regionsFromPosition;
	}

	public static void ForceUpdate()
	{
		forceUpdate = true;
	}
}
