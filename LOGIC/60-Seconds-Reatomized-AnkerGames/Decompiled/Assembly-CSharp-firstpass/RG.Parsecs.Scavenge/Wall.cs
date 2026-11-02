using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

public class Wall : MonoBehaviour
{
	public GenericEvent<Wall> OnTransparent = new GenericEvent<Wall>();

	public GenericEvent<Wall> OnMaterialsSetUp = new GenericEvent<Wall>();

	public bool FlipFace;

	public MeshRenderer MeshRenderer;

	public CameraTransparencyWall Transparency;

	public bool CanTrimSideMaterials = true;

	private Material[] _transparentMaterials = new Material[3];

	private Material[] _opaqueMaterials = new Material[3];

	public bool AreMaterialsSetUp { get; set; }

	public virtual bool CanBeTransparent { get; set; }

	public Material[] TransparentMaterials
	{
		get
		{
			return _transparentMaterials;
		}
		set
		{
			_transparentMaterials = value;
		}
	}

	public Material[] OpaqueMaterials
	{
		get
		{
			return _opaqueMaterials;
		}
		set
		{
			_opaqueMaterials = value;
		}
	}

	public void SetFrontMaterial(Material transparentMaterial, Material material)
	{
		Material[] transparentMaterials = _transparentMaterials;
		Material[] opaqueMaterials = _opaqueMaterials;
		if (FlipFace)
		{
			transparentMaterials[0] = transparentMaterial;
			opaqueMaterials[0] = material;
		}
		else
		{
			transparentMaterials[1] = transparentMaterial;
			opaqueMaterials[1] = material;
		}
		_transparentMaterials = transparentMaterials;
		_opaqueMaterials = opaqueMaterials;
		MeshRenderer.sharedMaterials = opaqueMaterials;
	}

	public void SetBackMaterial(Material transparentMaterial, Material material)
	{
		Material[] transparentMaterials = _transparentMaterials;
		Material[] opaqueMaterials = _opaqueMaterials;
		if (FlipFace)
		{
			transparentMaterials[1] = transparentMaterial;
			opaqueMaterials[1] = material;
		}
		else
		{
			transparentMaterials[0] = transparentMaterial;
			opaqueMaterials[0] = material;
		}
		_transparentMaterials = transparentMaterials;
		_opaqueMaterials = opaqueMaterials;
		MeshRenderer.sharedMaterials = opaqueMaterials;
	}

	internal void RemoveSides()
	{
		if (CanTrimSideMaterials)
		{
			Array.Resize(ref _transparentMaterials, 3);
			Array.Resize(ref _opaqueMaterials, 3);
		}
	}

	internal void SetupMaterials()
	{
		if (!AreMaterialsSetUp)
		{
			_transparentMaterials = MeshRenderer.sharedMaterials;
			_opaqueMaterials = MeshRenderer.sharedMaterials;
			if (_transparentMaterials.Length > 2)
			{
				_transparentMaterials[2] = ShipGenerator.I.RoomTransparentMaterialsDatabase.Tops[0];
				_opaqueMaterials[2] = ShipGenerator.I.RoomMaterialsDatabase.Tops[0];
			}
			if (_transparentMaterials.Length > 3)
			{
				_transparentMaterials[3] = ShipGenerator.I.RoomTransparentMaterialsDatabase.Tops[0];
				_opaqueMaterials[3] = ShipGenerator.I.RoomMaterialsDatabase.Tops[0];
			}
			MeshRenderer.sharedMaterials = _opaqueMaterials;
			AreMaterialsSetUp = true;
			OnMaterialsSetUp.Invoke(this);
		}
	}

	public void SetFrontMaterial(Material material)
	{
		Material[] transparentMaterials = _transparentMaterials;
		Material[] opaqueMaterials = _opaqueMaterials;
		if (FlipFace)
		{
			transparentMaterials[0] = material;
			opaqueMaterials[0] = material;
		}
		else
		{
			transparentMaterials[1] = material;
			opaqueMaterials[1] = material;
		}
		MeshRenderer.sharedMaterials = opaqueMaterials;
	}

	public void SetBackMaterial(Material material)
	{
		Material[] transparentMaterials = _transparentMaterials;
		Material[] opaqueMaterials = _opaqueMaterials;
		if (FlipFace)
		{
			transparentMaterials[1] = material;
			opaqueMaterials[1] = material;
		}
		else
		{
			transparentMaterials[0] = material;
			opaqueMaterials[0] = material;
		}
		MeshRenderer.sharedMaterials = opaqueMaterials;
	}

	private void OnDestroy()
	{
		OnTransparent.Clear();
	}
}
