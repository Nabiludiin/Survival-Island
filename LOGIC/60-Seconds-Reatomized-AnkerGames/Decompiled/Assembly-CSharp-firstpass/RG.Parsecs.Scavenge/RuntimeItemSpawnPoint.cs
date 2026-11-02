using System.Collections;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class RuntimeItemSpawnPoint : ItemSpawnPoint
{
	protected override IEnumerator Start()
	{
		SpriteRenderer component = GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.enabled = false;
		}
		yield return null;
	}
}
