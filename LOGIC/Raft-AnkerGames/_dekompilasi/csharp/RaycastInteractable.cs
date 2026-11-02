using System.Collections.Generic;
using UnityEngine;

public class RaycastInteractable : MonoBehaviour
{
	private List<IRaycastable> raycastableObjects = new List<IRaycastable>();

	private bool initialized;

	public List<IRaycastable> RaycastableObjects
	{
		get
		{
			if (!initialized)
			{
				InitRaycastables();
			}
			return raycastableObjects;
		}
		private set
		{
			raycastableObjects = value;
		}
	}

	public void InitRaycastables()
	{
		if (!initialized)
		{
			initialized = true;
			IRaycastable[] components = GetComponents<IRaycastable>();
			AddRaycastables(components);
		}
	}

	public void AddRaycastables(IRaycastable[] newRaycastables)
	{
		raycastableObjects.AddRangeUniqueOnly(newRaycastables);
	}
}
