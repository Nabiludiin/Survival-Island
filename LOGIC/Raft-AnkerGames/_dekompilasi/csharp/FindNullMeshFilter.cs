using System;
using System.Collections.Generic;
using UnityEngine;

public class FindNullMeshFilter : MonoBehaviour
{
	[SerializeField]
	private new string name;

	[SerializeField]
	private string filterName;

	[SerializeField]
	private Mesh mesh;

	[SerializeField]
	private Material material;

	private void Find()
	{
		MeshFilter[] componentsInChildren = GetComponentsInChildren<MeshFilter>(includeInactive: true);
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			if (meshFilter.sharedMesh == null)
			{
				Debug.Log(meshFilter.gameObject.name, meshFilter.gameObject);
			}
		}
	}

	private void Replace()
	{
		List<Transform> list = new List<Transform>();
		FindAllChildren(base.transform, list);
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (transform == null || !transform.name.Equals(name, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			Transform transform2 = ((filterName != null) ? transform.FindChildRecursively(filterName) : transform);
			if (transform2 != null)
			{
				MeshFilter component = transform2.GetComponent<MeshFilter>();
				MeshRenderer component2 = transform2.GetComponent<MeshRenderer>();
				if (component != null && mesh != null)
				{
					component.sharedMesh = mesh;
				}
				if (component2 != null && material != null)
				{
					component2.material = material;
				}
			}
		}
	}

	private void FindAllChildren(Transform transform, List<Transform> children)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (!(child == null))
			{
				if (child.childCount > 0)
				{
					FindAllChildren(child, children);
				}
				if (!children.Contains(child))
				{
					children.Add(child);
				}
			}
		}
	}
}
