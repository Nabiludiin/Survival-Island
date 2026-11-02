using System.Collections.Generic;
using UnityEngine;

public class JellyFishScareBox : MonoBehaviour
{
	[SerializeField]
	private List<JellyFish> jellyFishes = new List<JellyFish>();

	private void OnEnable()
	{
		SetJellyFishScared(scared: true);
	}

	private void OnDisable()
	{
		SetJellyFishScared(scared: false);
	}

	private void OnDrawGizmos()
	{
		if (jellyFishes == null)
		{
			return;
		}
		foreach (JellyFish jellyFish in jellyFishes)
		{
			if (!(jellyFish == null))
			{
				Gizmos.DrawLine(transform.position, jellyFish.transform.position);
			}
		}
	}

	private void SetJellyFishScared(bool scared)
	{
		if (jellyFishes == null)
		{
			return;
		}
		foreach (JellyFish jellyFish in jellyFishes)
		{
			if (!(jellyFish == null))
			{
				jellyFish.scared = scared;
			}
		}
	}
}
