using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton_Character : TabButton
{
	[SerializeField]
	private Image lockedImage;

	[SerializeField]
	private Color lockHighlightColor = Color.white;

	[SerializeField]
	private Color lockNormalColor = Color.white;

	[SerializeField]
	private Material lockedMaterial;

	public SkinnedMeshRenderer bodyMesh;

	public SkinnedMeshRenderer[] hairMeshes;

	private Renderer[] renderers;

	private MaterialRendConnection[] materialConnections;

	private bool hasInitializedRenderers;

	public override void OnPointerEnter()
	{
		base.OnPointerEnter();
		lockedImage.color = lockHighlightColor;
	}

	public override void OnPointerExit(bool forceExit = false)
	{
		base.OnPointerExit(forceExit);
		if (forceExit || tabGroup.SelectedTabButton != this)
		{
			lockedImage.color = lockNormalColor;
		}
	}

	public override void SetInteractability(bool interactable)
	{
		base.SetInteractability(interactable);
		text.gameObject.SetActiveSafe(interactable);
		lockedImage.gameObject.SetActiveSafe(!interactable);
		InitializeRenderers();
		if (!interactable)
		{
			MaterialRendConnection[] array = materialConnections;
			foreach (MaterialRendConnection materialRendConnection in array)
			{
				materialRendConnection.SetMaterial(lockedMaterial);
			}
		}
		else
		{
			MaterialRendConnection[] array = materialConnections;
			foreach (MaterialRendConnection materialRendConnection2 in array)
			{
				materialRendConnection2.RestoreMaterials();
			}
		}
	}

	private void InitializeRenderers()
	{
		if (hasInitializedRenderers)
		{
			return;
		}
		hasInitializedRenderers = true;
		List<Renderer> list = new List<Renderer>();
		list.AddRange(tab.GetComponentsInChildren<MeshRenderer>(includeInactive: true));
		list.AddRange(tab.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true));
		renderers = list.ToArray();
		if (renderers != null && renderers.Length != 0)
		{
			materialConnections = new MaterialRendConnection[renderers.Length];
			for (int i = 0; i < renderers.Length; i++)
			{
				materialConnections[i].renderer = renderers[i];
				materialConnections[i].originalMaterials = materialConnections[i].renderer.sharedMaterials;
			}
		}
	}
}
