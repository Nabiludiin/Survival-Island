using System.Collections.Generic;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class SpawnMultipleObjectsAction : EventAction
{
	public List<GameObject> Prefabs;

	public IntRange HowMuch;

	public List<Transform> Targets;

	protected override void Act(EventActionArgs args = null)
	{
		List<GameObject> list = new List<GameObject>(Prefabs);
		List<Transform> list2 = new List<Transform>(Targets);
		int randomValue = HowMuch.RandomValue;
		for (int i = 0; i < randomValue; i++)
		{
			GameObject gameObject = list.RandomElement();
			list.Remove(gameObject);
			GameObject gameObject2 = Object.Instantiate(gameObject);
			Transform transform = list2.RandomElement();
			list2.Remove(transform);
			gameObject2.transform.SetParent(transform);
			gameObject2.transform.localPosition = Vector3.zero;
		}
	}
}
