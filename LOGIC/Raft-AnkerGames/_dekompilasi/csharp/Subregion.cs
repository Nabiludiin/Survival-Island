using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaZone))]
public class Subregion : MonoBehaviour
{
	public AreaZone regionArea;

	public Dictionary<Type, SubregionComponent> subregionComponents = new Dictionary<Type, SubregionComponent>();

	[HideInInspector]
	public bool playerIsWithinSubregion;

	private void Awake()
	{
		SubregionComponent[] components = GetComponents<SubregionComponent>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].AddSubregionComponent();
		}
	}

	private void OnValidate()
	{
		if (regionArea == null)
		{
			regionArea = GetComponent<AreaZone>();
		}
	}

	public void LateUpdateSubRegion()
	{
		foreach (SubregionComponent value in subregionComponents.Values)
		{
			if (value != null)
			{
				value.LateUpdateSubRegionComponent(playerIsWithinSubregion);
			}
		}
	}

	public T GetSubRegionComponent<T>()
	{
		Type typeFromHandle = typeof(T);
		if (subregionComponents.ContainsItems() && subregionComponents.ContainsKey(typeFromHandle))
		{
			return (T)(object)subregionComponents[typeFromHandle];
		}
		return default;
	}
}
