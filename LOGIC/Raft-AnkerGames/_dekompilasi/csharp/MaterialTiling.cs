using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialTiling : MonoBehaviour
{
	[SerializeField]
	private string tagToSearch = "MaterialTiling";

	private MaterialPropertyBlock matPropBlock;

	private void Awake()
	{
		RefreshChildrenMaterials();
		if (Application.isPlaying)
		{
			Object.Destroy(this);
		}
	}

	private void RefreshChildrenMaterials()
	{
		List<Transform> children = new List<Transform>();
		base.transform.FindChildrenByTag(ref children, tagToSearch);
		if (!children.ContainsItems())
		{
			return;
		}
		for (int i = 0; i < children.Count; i++)
		{
			Transform transform = children[i];
			if (transform != null)
			{
				RefreshTransform(transform);
			}
		}
	}

	private void RefreshTransform(Transform transform)
	{
		if (!transform.tag.Equals(tagToSearch))
		{
			return;
		}
		Renderer componentInChildren = transform.GetComponentInChildren<Renderer>();
		if (componentInChildren != null)
		{
			if (matPropBlock == null)
			{
				matPropBlock = new MaterialPropertyBlock();
			}
			componentInChildren.GetPropertyBlock(matPropBlock);
			matPropBlock.SetVector("_Scale_Tiling", new Vector2(transform.localScale.x, transform.localScale.z));
			componentInChildren.SetPropertyBlock(matPropBlock);
		}
	}
}
