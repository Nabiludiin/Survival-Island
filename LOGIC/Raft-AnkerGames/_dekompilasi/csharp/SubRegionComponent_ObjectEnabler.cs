using System;
using System.Collections.Generic;
using UnityEngine;

public class SubRegionComponent_ObjectEnabler : SubregionComponent
{
	[Serializable]
	private class ObjectConnection
	{
		public GameObject obj;

		public bool enableInsideRegion = true;
	}

	[SerializeField]
	private List<ObjectConnection> objects = new List<ObjectConnection>();

	public override void AddSubregionComponent()
	{
		GetComponent<Subregion>().subregionComponents.Add(typeof(SubRegionComponent_ObjectEnabler), this);
	}

	public override void LateUpdateSubRegionComponent(bool playerIsWithinSubregion)
	{
		base.LateUpdateSubRegionComponent(playerIsWithinSubregion);
		foreach (ObjectConnection @object in objects)
		{
			bool value = (@object.enableInsideRegion & playerIsWithinSubregion) || (!@object.enableInsideRegion && !playerIsWithinSubregion);
			@object.obj.SetActiveSafe(value);
		}
	}
}
