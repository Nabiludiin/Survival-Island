using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class RandomPrefabSpawner : MonoBehaviour
{
	public GameObject[] Range;

	public GameObject Instance;

	public bool InheritRotation;

	public bool Scale111;

	public GenericEvent<RandomPrefabSpawner, GameObject> Spawned = new GenericEvent<RandomPrefabSpawner, GameObject>();

	private void Start()
	{
		StartCoroutine(Show());
	}

	public IEnumerator Show()
	{
		if (Instance != null)
		{
			Object.Destroy(Instance.gameObject);
			Instance = null;
		}
		yield return null;
		if (Range.Length != 0)
		{
			GameObject gameObject = Range.RandomElement();
			if (gameObject == null)
			{
				yield break;
			}
			RectTransform component = GetComponent<RectTransform>();
			Instance = Object.Instantiate(gameObject);
			Instance.transform.SetParent(transform);
			if (component == null)
			{
				Instance.transform.localPosition = Vector3.zero;
				if (InheritRotation)
				{
					Instance.transform.rotation = transform.rotation;
				}
				if (Scale111)
				{
					Instance.transform.localScale = Vector3.one;
				}
			}
			else
			{
				Instance.transform.localPosition = Vector3.zero;
				RectTransform component2 = Instance.GetComponent<RectTransform>();
				component2.anchoredPosition = Vector2.zero;
				if (InheritRotation)
				{
					component2.rotation = component.rotation;
				}
				component2.localScale = Vector3.one;
			}
			Spawned.Invoke(this, Instance);
		}
		else
		{
			Debug.LogErrorFormat("Prop: {0} has no sprites assigned.", base.gameObject.name);
		}
	}
}
