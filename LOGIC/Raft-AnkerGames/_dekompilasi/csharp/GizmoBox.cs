using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GizmoBox : MonoBehaviour
{
	public Vector3 size = Vector3.one;

	public virtual Vector3 Center => transform.position;

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube(Vector3.zero, size);
	}

	public bool IsColliding(bool includeInactiveObjects, LayerMask mask, float extentsMultiplier, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		IEnumerable<Collider> overlaps = GetOverlaps(includeInactiveObjects, mask, extentsMultiplier, queryTriggerInteraction);
		if (overlaps != null)
		{
			return overlaps.Count() > 0;
		}
		return false;
	}

	public IEnumerable<Collider> GetOverlaps(bool includeInactiveObjects, LayerMask mask, float extentsMultiplier, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 vector = Vector3.Scale(size, transform.lossyScale) * 0.5f;
		Collider[] array = Physics.OverlapBox(Center, vector * extentsMultiplier, transform.rotation, mask, queryTriggerInteraction);
		if (includeInactiveObjects)
		{
			return array;
		}
		List<Collider> list = array.ToList();
		for (int i = 0; i < list.Count; i++)
		{
			Collider collider = list[i];
			if (!collider.enabled || !collider.gameObject.activeSelf)
			{
				list.RemoveAt(i);
				i--;
			}
		}
		return list;
	}

	public List<T> GetOverlaps<T>(int layerMask = -1, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
	{
		Vector3 halfExtents = Vector3.Scale(size, transform.lossyScale) * 0.5f;
		Collider[] array = Physics.OverlapBox(Center, halfExtents, transform.rotation, layerMask, queryTriggerInteraction);
		List<T> list = new List<T>();
		foreach (Collider collider in array)
		{
			if (!(collider == null))
			{
				T component = collider.GetComponent<T>();
				if (component != null)
				{
					list.Add(component);
				}
			}
		}
		return list;
	}
}
