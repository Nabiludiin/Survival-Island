using System;
using UnityEngine;

[Serializable]
public class ItemInstance_Buildable
{
	[Serializable]
	public class Upgrade
	{
		public Item_Base woodVersion;

		public Item_Base thatchVersion;

		public Item_Base tier3Version;

		public bool HasUpgrades()
		{
			if (!(woodVersion != null) && !(thatchVersion != null))
			{
				return tier3Version != null;
			}
			return true;
		}

		public Item_Base GetNewItemFromUpgradeItem(Item_Base buildableItem)
		{
			if (buildableItem != null)
			{
				if (buildableItem.UniqueIndex == 546)
				{
					return woodVersion;
				}
				if (buildableItem.UniqueIndex == 547)
				{
					return thatchVersion;
				}
				if (buildableItem.UniqueIndex == 548)
				{
					return tier3Version;
				}
			}
			return null;
		}
	}

	[SerializeField]
	private Block[] blockPrefabs;

	[SerializeField]
	private Item_Base mirroredVersion;

	[SerializeField]
	private Upgrade upgrades;

	[SerializeField]
	private Axis primaryPaintAxis = Axis.None;

	[SerializeField]
	private PaintMode paintMode;

	[SerializeField]
	private bool decorationPaint;

	[SerializeField]
	private bool reselectOnBuild;

	[SerializeField]
	private bool placeable;

	[SerializeField]
	private bool hideWhenOverlapSamePos;

	[SerializeField]
	private bool playParticlesWhenRemoved = true;

	[SerializeField]
	private bool returnPlaceablesWhenUpgraded;

	[SerializeField]
	private bool returnItemWhenRemovedRecursively;

	[Tooltip("Any block within this distance will be checked for stability when this block is removed")]
	[SerializeField]
	private float unstableCheckDistance = 3.5f;

	public bool HasBuildablePrefabs
	{
		get
		{
			if (blockPrefabs != null && blockPrefabs.Length != 0)
			{
				return blockPrefabs[0] != null;
			}
			return false;
		}
	}

	public Item_Base MirroredVersion => mirroredVersion;

	public Upgrade Upgrades => upgrades;

	public bool Paintable => PrimaryPaintAxis != Axis.None;

	public Axis PrimaryPaintAxis => primaryPaintAxis;

	public PaintMode PaintMode => paintMode;

	public bool DecorationPaint => decorationPaint;

	public bool ReselectOnBuild => reselectOnBuild;

	public bool Placeable => placeable;

	public bool HideWhenOverlapSamePos
	{
		get
		{
			return hideWhenOverlapSamePos;
		}
		set
		{
			hideWhenOverlapSamePos = value;
		}
	}

	public bool PlayParticlesWhenRemoved
	{
		get
		{
			return playParticlesWhenRemoved;
		}
		set
		{
			playParticlesWhenRemoved = value;
		}
	}

	public bool ReturnPlaceablesWhenUpgraded => returnPlaceablesWhenUpgraded;

	public bool ReturnItemWhenRemovedRecursively
	{
		get
		{
			return returnItemWhenRemovedRecursively;
		}
		set
		{
			returnItemWhenRemovedRecursively = value;
		}
	}

	public float UnstableCheckDistance => unstableCheckDistance;

	public ItemInstance_Buildable(Block defaultBuildablePrefab, bool reselectOnBuild, bool placeable, bool hideWhenOverlapSamePos)
	{
		blockPrefabs = new Block[1];
		blockPrefabs[0] = defaultBuildablePrefab;
		this.reselectOnBuild = reselectOnBuild;
		this.placeable = placeable;
		this.hideWhenOverlapSamePos = hideWhenOverlapSamePos;
	}

	public ItemInstance_Buildable Clone()
	{
		return (ItemInstance_Buildable)MemberwiseClone();
	}

	public Block GetBlockPrefab(int index)
	{
		if (blockPrefabs == null || index < 0 || index >= blockPrefabs.Length)
		{
			return null;
		}
		return blockPrefabs[index];
	}

	public Block GetBlockPrefab(DPS type)
	{
		if (blockPrefabs == null || blockPrefabs.Length == 0)
		{
			return null;
		}
		if (type == DPS.Default)
		{
			return blockPrefabs[0];
		}
		for (int i = 0; i < blockPrefabs.Length; i++)
		{
			if (blockPrefabs[i] == null)
			{
				Debug.LogError("Blockprefab is null");
			}
			DPS dpsType = blockPrefabs[i].dpsType;
			if (dpsType == type || (type == DPS.FloorAndCeiling && (dpsType == DPS.Ceiling || dpsType == DPS.Floor)))
			{
				return blockPrefabs[i];
			}
		}
		return null;
	}

	public Block[] GetBlockPrefabs()
	{
		return blockPrefabs;
	}
}
