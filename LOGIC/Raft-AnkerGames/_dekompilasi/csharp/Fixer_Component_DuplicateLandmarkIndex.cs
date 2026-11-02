using UnityEngine;

public class Fixer_Component_DuplicateLandmarkIndex : Fixer_Component_ABS
{
	public override bool DoCheck(Transform target)
	{
		Landmark component = target.GetComponent<Landmark>();
		bool result = true;
		if (component != null)
		{
			Landmark[] array = FindAllLandmarks();
			foreach (Landmark landmark in array)
			{
				if (component != landmark && component.uniqueLandmarkIndex == landmark.uniqueLandmarkIndex)
				{
					Debug.LogError("WARNING: " + component.transform.name + " has the same unique landmark index as " + landmark.transform.name + "Index: " + component.uniqueLandmarkIndex);
					result = false;
				}
			}
		}
		return result;
	}

	public override bool DoFix(Transform target)
	{
		return true;
	}

	public Landmark[] FindAllLandmarks()
	{
		return Object.FindObjectsOfType<Landmark>();
	}
}
