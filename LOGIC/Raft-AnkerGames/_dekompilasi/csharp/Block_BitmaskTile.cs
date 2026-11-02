using UnityEngine;

public class Block_BitmaskTile : Block
{
	[SerializeField]
	protected BitmaskTile bitmaskTile;

	protected new bool hasBeenPlaced;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (bitmaskTile != null && !GameManager.IsLeavingGame && hasBeenPlaced)
		{
			TileBitmaskManager.RemoveTile(bitmaskTile);
			bitmaskTile.RefreshNeighbours();
		}
	}

	public override void OnFinishedPlacement()
	{
		base.OnFinishedPlacement();
		Invoke("OnFinishPlacementLate", 0.1f);
	}

	protected virtual void OnFinishPlacementLate()
	{
		if (bitmaskTile != null)
		{
			TileBitmaskManager.AddTile(bitmaskTile);
			bitmaskTile.Refresh(refreshNeighbour: true);
		}
		hasBeenPlaced = true;
	}
}
