using UnityEngine;

namespace RG.Parsecs.Survival;

public abstract class EventContent : MonoBehaviour
{
	public void SetParent(Transform parent)
	{
		transform.SetParent(parent, worldPositionStays: false);
	}
}
