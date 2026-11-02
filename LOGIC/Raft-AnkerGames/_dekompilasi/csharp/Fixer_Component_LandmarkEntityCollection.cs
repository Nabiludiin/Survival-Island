using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Fixer_Component_LandmarkEntityCollection : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		bool result = true;
		LandmarkEntityCollection[] componentsInChildren = target.GetComponentsInChildren<LandmarkEntityCollection>(includeInactive: true);
		List<LandmarkEntitySpawner> list = new List<LandmarkEntitySpawner>();
		List<LandmarkEntitySpawner> list2 = target.GetComponentsInChildren<LandmarkEntitySpawner>().ToList();
		if (componentsInChildren != null)
		{
			LandmarkEntityCollection[] array = componentsInChildren;
			foreach (LandmarkEntityCollection landmarkEntityCollection in array)
			{
				LandmarkEntitySpawner[] spawners = landmarkEntityCollection.spawners;
				foreach (LandmarkEntitySpawner landmarkEntitySpawner in spawners)
				{
					list.Add(landmarkEntitySpawner);
					foreach (LandmarkEntitySpawner landmarkEntitySpawner2 in spawners)
					{
						if (!(landmarkEntitySpawner == landmarkEntitySpawner2) && landmarkEntitySpawner.spawnerIndex == landmarkEntitySpawner2.spawnerIndex)
						{
							Debug.LogError("Spawners have the same spawner Index:" + landmarkEntitySpawner.transform.name + ":" + landmarkEntitySpawner2.transform.name + "Index: " + landmarkEntitySpawner.spawnerIndex);
							result = false;
						}
					}
				}
				if (!DoesCollectionHaveAllSpawners(target, landmarkEntityCollection))
				{
					result = false;
				}
			}
		}
		if (list.Count != list2.Count)
		{
			result = false;
			Debug.Log(list.Count + " " + list2.Count);
			string text = "Some spawners underneath " + target.transform.name + " is not connected to a landmark entity spawner collection";
			foreach (LandmarkEntitySpawner item in list2)
			{
				if (!list.Contains(item))
				{
					text += item.name;
				}
			}
			Debug.Log(text);
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}

	private bool DoesCollectionHaveAllSpawners(Transform parentSceneRoot, LandmarkEntityCollection collection)
	{
		int num = collection.spawners.Length;
		int num2 = GetChildSpawners(collection.transform).Length;
		if (num != num2)
		{
			Debug.LogError(parentSceneRoot.name + " : " + collection.transform.name + " has unconnected spawners spawner count: " + num + "collection children count : " + num2);
			return false;
		}
		return true;
	}

	private LandmarkEntitySpawner[] GetChildSpawners(Transform target)
	{
		return target.GetComponentsInChildren<LandmarkEntitySpawner>();
	}
}
