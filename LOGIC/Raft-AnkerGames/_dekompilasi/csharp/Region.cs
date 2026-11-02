using System;
using System.Collections.Generic;
using UnityEngine;

public class Region : MonoBehaviour
{
	public AreaZone regionArea;

	public Subregion[] subregions;

	[SerializeField]
	private List<Type> subregionComponentTypes = new List<Type>();

	private void Awake()
	{
		LocateSubregions();
	}

	private void Start()
	{
		RegionManager.AddGroup(this);
	}

	private void OnDestroy()
	{
		RegionManager.RemoveGroup(this);
	}

	private void LocateSubregions()
	{
		subregions = GetComponentsInChildren<Subregion>(includeInactive: true);
		subregionComponentTypes.Clear();
		Subregion[] array = subregions;
		for (int i = 0; i < array.Length; i++)
		{
			SubregionComponent[] components = array[i].GetComponents<SubregionComponent>();
			foreach (SubregionComponent subregionComponent in components)
			{
				subregionComponentTypes.AddUniqueOnly(subregionComponent.GetType());
			}
		}
	}

	public bool ContainsSubRegionComponentType(Type subregionComponentType)
	{
		if (subregionComponentTypes.ContainsItems())
		{
			return subregionComponentTypes.Contains(subregionComponentType);
		}
		return false;
	}

	public List<Subregion> GetSubregionsFromPosition(Vector3 position)
	{
		if (subregions.ContainsItems())
		{
			List<Subregion> list = null;
			for (int i = 0; i < subregions.Length; i++)
			{
				Subregion subregion = subregions[i];
				if (subregion != null && subregion.regionArea.GetNormalizedDistanceToPoint(position) > 0f)
				{
					if (list == null)
					{
						list = new List<Subregion>();
					}
					list.Add(subregion);
				}
			}
			return list;
		}
		return null;
	}
}
