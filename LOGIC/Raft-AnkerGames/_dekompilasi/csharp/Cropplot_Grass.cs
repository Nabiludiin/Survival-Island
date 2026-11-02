using System;
using UnityEngine;

public class Cropplot_Grass : Cropplot
{
	[SerializeField]
	private BitmaskTile bitmaskTile;

	[SerializeField]
	private Renderer grassPlotRenderer;

	private MaterialPropertyBlock materialProp;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		TileBitmaskManager.RemoveTile(this.bitmaskTile);
		if (this.bitmaskTile != null)
		{
			this.bitmaskTile.RefreshNeighbours();
			BitmaskTile bitmaskTile = this.bitmaskTile;
			bitmaskTile.OnMeshChange = (Action)Delegate.Remove(bitmaskTile.OnMeshChange, new Action(OnBitmaskTileMeshChange));
		}
	}

	private void OnBitmaskTileMeshChange()
	{
		if (materialProp == null)
		{
			materialProp = new MaterialPropertyBlock();
		}
		grassPlotRenderer.GetPropertyBlock(materialProp, 1);
		materialProp.SetFloat("_UVRotation", transform.localEulerAngles.y);
		grassPlotRenderer.SetPropertyBlock(materialProp, 1);
	}

	private void OnSlotWaterChange(bool hasWater)
	{
		if (materialProp == null)
		{
			materialProp = new MaterialPropertyBlock();
		}
		grassPlotRenderer.GetPropertyBlock(materialProp, 1);
		materialProp.SetFloat("_Dry", hasWater ? 0f : 1f);
		grassPlotRenderer.SetPropertyBlock(materialProp, 1);
	}

	public override Plant PlantSeed(Plant plantPrefab, uint plantObjectIndex, bool waterCropAswell, int slotIndex = -1)
	{
		Plant plant = base.PlantSeed(plantPrefab, plantObjectIndex, waterCropAswell, slotIndex);
		PlantationSlot plantationSlot = plantationSlots[plant.plantationSlotIndex];
		plantationSlot.OnWaterChange = (Action<bool>)Delegate.Combine(plantationSlot.OnWaterChange, new Action<bool>(OnSlotWaterChange));
		MeshFilter component = plant.transform.GetChild(0).GetComponent<MeshFilter>();
		bitmaskTile.meshFilters[1] = component;
		return plant;
	}

	public override void OnFinishPlacementLate()
	{
		base.OnFinishPlacementLate();
		TileBitmaskManager.AddTile(this.bitmaskTile);
		BitmaskTile bitmaskTile = this.bitmaskTile;
		bitmaskTile.OnMeshChange = (Action)Delegate.Combine(bitmaskTile.OnMeshChange, new Action(OnBitmaskTileMeshChange));
		this.bitmaskTile.Refresh(refreshNeighbour: true);
	}
}
