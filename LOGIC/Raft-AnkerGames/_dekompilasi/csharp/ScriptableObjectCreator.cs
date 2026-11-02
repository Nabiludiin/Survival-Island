using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectCreator : MonoBehaviour
{
	public List<PickupItem> objectsToCreateSos = new List<PickupItem>();

	public void CreateSOFromObjects()
	{
		foreach (PickupItem objectsToCreateSo in objectsToCreateSos)
		{
			CreateSOFromItem(objectsToCreateSo);
		}
	}

	private void CreateSOFromItem(PickupItem item)
	{
	}
}
