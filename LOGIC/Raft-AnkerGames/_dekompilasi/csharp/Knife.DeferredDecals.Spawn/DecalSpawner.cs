using System.Collections;
using UnityEngine;

namespace Knife.DeferredDecals.Spawn;

public abstract class DecalSpawner : MonoBehaviour
{
	public abstract Decal Spawn();

	public abstract void DestroyDecal(Decal decal);

	public void DestroyDecal(Decal decal, float delay)
	{
		StartCoroutine(DestroyDecalWithDelay(decal, delay));
	}

	private IEnumerator DestroyDecalWithDelay(Decal decal, float delay)
	{
		yield return new WaitForSeconds(delay);
		if (!(decal == null))
		{
			DestroyDecal(decal);
			decal.gameObject.SetActive(value: false);
		}
	}
}
