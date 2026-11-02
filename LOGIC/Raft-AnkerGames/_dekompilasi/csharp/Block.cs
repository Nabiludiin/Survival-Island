using System;
using UnityEngine;

[RequireComponent(typeof(OccupyingComponent))]
public class Block : MonoBehaviour_ID
{
	public Action<Block> Action_OnHealthChange;

	public RGDSortingOrder rgdSortingOrder = RGDSortingOrder.Block;

	public DPS dpsType;

	public Axis perpendicularAxis;

	public Item_Base buildableItem;

	public SO_BlockCollisionMask blockCollisionMask;

	public NetworkType networkType;

	public MonoBehaviour_Network networkedBehaviour;

	public MonoBehaviour_ID_Network networkedIDBehaviour;

	[SerializeField]
	private int health = 100;

	[SerializeField]
	private int maxHealth = 100;

	public bool canBeRemoved = true;

	public bool snapsToQuads = true;

	public bool snapsToTopCollider;

	public bool isRotateable;

	public bool canRotateFreely;

	public bool forceRotationToQuadDirection;

	public float rotateAmount = 90f;

	[Tooltip("When placing a block it will snap to this rotation on its perpendicular axis")]
	public float snapRotateAmount;

	public float currentRotationY;

	public Vector3 pivotOffset = Vector3.zero;

	public bool flipPivot;

	[HideInInspector]
	public SO_ColorValue currentColorA;

	[HideInInspector]
	public SO_ColorValue currentColorB;

	[HideInInspector]
	public SO_ColorValue currentPatternColorA;

	[HideInInspector]
	public SO_ColorValue currentPatternColorB;

	[HideInInspector]
	public uint currentPatternIndex1;

	[HideInInspector]
	public uint currentPatternIndex2;

	[Tooltip("Unique item name of item to return when the block is destroyed, leave empty if no item should be returned")]
	public Item_Base itemToReturnOnDestroy;

	public Collider[] onoffColliders;

	public BoxCollider[] blockColliders;

	[Space(20f)]
	[HideInInspector]
	public OccupyingComponent occupyingComponent;

	public StableComponent stableComponent;

	public GameObject colliderPrefab;

	[HideInInspector]
	public Transform activeColliderPrefab;

	[HideInInspector]
	public bool hasBeenPlaced;

	protected MaterialPropertyBlock matPropBlock;

	protected static ColliderPrefabEnabler colliderPrefabEnabler;

	public float NormalizedHealth => Mathf.Clamp((float)health / (float)maxHealth, 0f, 1f);

	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}
		set
		{
			maxHealth = value;
		}
	}

	public int Health => health;

	public virtual bool Reinforced
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	protected virtual void Awake()
	{
		stableComponent = GetComponent<StableComponent>();
		occupyingComponent = GetComponent<OccupyingComponent>();
	}

	protected virtual void Start()
	{
		if (colliderPrefabEnabler == null)
		{
			colliderPrefabEnabler = ComponentManager<ColliderPrefabEnabler>.Value;
		}
	}

	public static bool IsBlockIndexFoundation(int uniqueBlockIndex)
	{
		if (uniqueBlockIndex == 2 || uniqueBlockIndex == 382)
		{
			return true;
		}
		return IsBlockIndexFoundationTriangular(uniqueBlockIndex);
	}

	public static bool IsBlockIndexFoundationTriangular(int uniqueBlockIndex)
	{
		if (uniqueBlockIndex == 189 || uniqueBlockIndex == 383 || uniqueBlockIndex == 387)
		{
			return true;
		}
		return false;
	}

	public static bool IsBlockIndexFloor(int uniqueBlockIndex)
	{
		switch (uniqueBlockIndex)
		{
		case 1:
		case 191:
		case 384:
		case 385:
		case 388:
			return true;
		default:
			return false;
		}
	}

	public static bool IsBlockIndexRaisedFloor(int uniqueBlockIndex)
	{
		switch (uniqueBlockIndex)
		{
		case 143:
		case 193:
		case 396:
		case 397:
		case 465:
			return true;
		default:
			return false;
		}
	}

	public static bool IsBlockIndexCollectionNet(int uniqueBlockIndex)
	{
		if (uniqueBlockIndex == 8 || uniqueBlockIndex == 503)
		{
			return true;
		}
		return false;
	}

	public void OnStartingPlacement()
	{
		SetOnOffColliderState(state: false);
	}

	public virtual void OnFinishedPlacement()
	{
		hasBeenPlaced = true;
		SetOnOffColliderState(state: true);
		if (networkType == NetworkType.NetworkIDBehaviour && networkedIDBehaviour != null)
		{
			networkedIDBehaviour.ObjectIndex = ObjectIndex;
		}
		SendMessage("OnBlockPlaced", SendMessageOptions.DontRequireReceiver);
		IBlockPlaced[] componentsInChildren = GetComponentsInChildren<IBlockPlaced>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].OnBlockPlacedInterface();
		}
		if (ComponentManager<BlockCollisionConsolidator>.Value != null && blockColliders.Length != 0)
		{
			ComponentManager<BlockCollisionConsolidator>.Value.AddBlock(this);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (ComponentManager<BlockCollisionConsolidator>.Value != null && blockColliders.Length != 0)
		{
			ComponentManager<BlockCollisionConsolidator>.Value.RemoveBlock(this);
		}
	}

	public void Damage(int damage)
	{
		SetHealth(health - damage);
	}

	public void SetHealth(int newHealth)
	{
		health = newHealth;
		if (Action_OnHealthChange != null)
		{
			Action_OnHealthChange(this);
		}
		if (health <= 0)
		{
			BlockCreator.RemoveBlockNetwork(this, null, updateRaftBounds: true);
		}
	}

	public OverlappType IsOverlapping()
	{
		if (occupyingComponent == null)
		{
			return OverlappType.None;
		}
		return occupyingComponent.IsCollidingWithOtherBlocks(blockCollisionMask, this);
	}

	public void Repair(int amount)
	{
		int num = health + amount;
		if (num > maxHealth)
		{
			num = maxHealth;
		}
		SetHealth(num);
		BlockCreator.UpdateHealthNetwork(this);
	}

	public bool CanBeRepaired()
	{
		return health < maxHealth;
	}

	public bool CanBeReinforced()
	{
		return !Reinforced;
	}

	public virtual void SetInstanceColorAndPattern(SO_ColorValue primary, SO_ColorValue secondary, int paintSide, uint patternIndex)
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		if (primary != null && primary.uniqueColorIndex == 0)
		{
			primary = null;
		}
		if (secondary != null && secondary.uniqueColorIndex == 0)
		{
			secondary = null;
		}
		for (int i = 0; i < occupyingComponent.renderers.Length; i++)
		{
			Renderer obj = occupyingComponent.renderers[i];
			obj.GetPropertyBlock(matPropBlock);
			if (paintSide == 1)
			{
				currentColorA = primary;
				currentPatternColorA = secondary;
				currentPatternIndex1 = patternIndex;
				matPropBlock.SetColor("_Side1_Base", (currentColorA != null) ? currentColorA.paintColor : Color.clear);
				matPropBlock.SetColor("_Side1_Pattern", (currentPatternColorA != null) ? currentPatternColorA.paintColor : Color.clear);
				matPropBlock.SetFloat("_Pattern_Index1", currentPatternIndex1);
				matPropBlock.SetFloat("_Pattern1_Masked", (currentColorA == null || currentPatternColorA == null) ? 1f : 0f);
				matPropBlock.SetFloat("_Pattern1_MaskFlip", (currentColorA != null && currentPatternColorA == null) ? 1f : 0f);
			}
			if (paintSide == 2)
			{
				currentColorB = primary;
				currentPatternColorB = secondary;
				currentPatternIndex2 = patternIndex;
				matPropBlock.SetColor("_Side2_Base", (currentColorB != null) ? currentColorB.paintColor : Color.clear);
				matPropBlock.SetColor("_Side2_Pattern", (currentPatternColorB != null) ? currentPatternColorB.paintColor : Color.clear);
				matPropBlock.SetFloat("_Pattern_Index2", currentPatternIndex2);
				matPropBlock.SetFloat("_Pattern2_Masked", (currentColorB == null || currentPatternColorB == null) ? 1f : 0f);
				matPropBlock.SetFloat("_Pattern2_MaskFlip", (currentColorB != null && currentPatternColorB == null) ? 1f : 0f);
			}
			if (paintSide == 3)
			{
				if (buildableItem.settings_buildable.DecorationPaint)
				{
					currentColorA = primary;
					currentColorB = secondary;
					currentPatternColorA = currentColorA;
					currentPatternColorB = currentColorB;
				}
				else
				{
					currentColorA = primary;
					currentColorB = primary;
					currentPatternColorA = secondary;
					currentPatternColorB = secondary;
				}
				currentPatternIndex1 = (currentPatternIndex2 = patternIndex);
				matPropBlock.SetColor("_Side1_Base", (currentColorA != null) ? currentColorA.paintColor : Color.clear);
				matPropBlock.SetColor("_Side1_Pattern", (currentPatternColorA != null) ? currentPatternColorA.paintColor : Color.clear);
				matPropBlock.SetFloat("_Pattern_Index1", currentPatternIndex1);
				matPropBlock.SetFloat("_Pattern1_Masked", (currentColorA == null || currentPatternColorA == null) ? 1f : 0f);
				matPropBlock.SetFloat("_Pattern1_MaskFlip", (currentColorA != null && currentPatternColorA == null) ? 1f : 0f);
				matPropBlock.SetColor("_Side2_Base", (currentColorB != null) ? currentColorB.paintColor : Color.clear);
				matPropBlock.SetColor("_Side2_Pattern", (currentPatternColorB != null) ? currentPatternColorB.paintColor : Color.clear);
				matPropBlock.SetFloat("_Pattern_Index2", currentPatternIndex2);
				matPropBlock.SetFloat("_Pattern2_Masked", (currentColorB == null || currentPatternColorB == null) ? 1f : 0f);
				matPropBlock.SetFloat("_Pattern2_MaskFlip", (currentColorB != null && currentPatternColorB == null) ? 1f : 0f);
			}
			int num = 0;
			if ((currentColorA != null || currentPatternColorA != null) && currentColorB == null && currentPatternColorB == null)
			{
				num = 1;
			}
			if ((currentColorB != null || currentPatternColorB != null) && currentColorA == null && currentPatternColorA == null)
			{
				num = 2;
			}
			if ((currentColorA != null || currentPatternColorA != null) && (currentColorB != null || currentPatternColorB != null))
			{
				num = 3;
			}
			if (buildableItem.settings_buildable.DecorationPaint)
			{
				matPropBlock.SetFloat("_PaintSide", 0f);
				matPropBlock.SetFloat("_DecoPaintSelect", num);
			}
			else
			{
				matPropBlock.SetFloat("_PaintSide", num);
				matPropBlock.SetFloat("_DecoPaintSelect", 0f);
			}
			obj.SetPropertyBlock(matPropBlock);
		}
	}

	public void SetInstanceOutline(bool outlineState)
	{
		if (matPropBlock == null)
		{
			matPropBlock = new MaterialPropertyBlock();
		}
		for (int i = 0; i < occupyingComponent.renderers.Length; i++)
		{
			Renderer obj = occupyingComponent.renderers[i];
			obj.GetPropertyBlock(matPropBlock);
			matPropBlock.SetFloat("_Destroy_Overlay", outlineState ? 1f : 0f);
			obj.SetPropertyBlock(matPropBlock);
		}
	}

	public virtual bool HasColor(SO_ColorValue primary, SO_ColorValue secondary, int side, uint patternIndex)
	{
		switch (side)
		{
		case 1:
			if (primary.IsSameColor(currentColorA) && secondary.IsSameColor(currentPatternColorA))
			{
				return currentPatternIndex1 == patternIndex;
			}
			return false;
		case 2:
			if (primary.IsSameColor(currentColorB) && secondary.IsSameColor(currentPatternColorB))
			{
				return currentPatternIndex2 == patternIndex;
			}
			return false;
		case 3:
			if (buildableItem.settings_buildable.DecorationPaint)
			{
				if (primary.IsSameColor(currentColorA) && primary.IsSameColor(currentPatternColorA) && secondary.IsSameColor(currentColorB) && secondary.IsSameColor(currentPatternColorB) && currentPatternIndex1 == patternIndex)
				{
					return currentPatternIndex2 == patternIndex;
				}
				return false;
			}
			if (primary.IsSameColor(currentColorA) && secondary.IsSameColor(currentPatternColorA) && primary.IsSameColor(currentColorB) && secondary.IsSameColor(currentPatternColorB) && currentPatternIndex1 == patternIndex)
			{
				return currentPatternIndex2 == patternIndex;
			}
			return false;
		default:
			return false;
		}
	}

	public bool IsValidSerialization()
	{
		return hasBeenPlaced;
	}

	public virtual bool IsStable()
	{
		if (stableComponent != null)
		{
			return stableComponent.IsStable();
		}
		if (colliderPrefabEnabler == null)
		{
			colliderPrefabEnabler = ComponentManager<ColliderPrefabEnabler>.Value;
		}
		bool flag = activeColliderPrefab != null;
		if (!flag)
		{
			colliderPrefabEnabler.AttachColliderPrefab(this);
		}
		bool flag2 = false;
		flag2 = stableComponent == null || stableComponent.IsStable();
		if (!flag)
		{
			colliderPrefabEnabler.DetachColliderPrefab(this);
		}
		return flag2;
	}

	public bool IsWalkable()
	{
		switch (buildableItem.UniqueIndex)
		{
		case 1:
		case 2:
		case 8:
		case 189:
		case 191:
		case 382:
		case 383:
		case 384:
		case 385:
		case 387:
		case 388:
			return true;
		default:
			return false;
		}
	}

	public void SetOnOffColliderState(bool state)
	{
		Collider[] array = onoffColliders;
		foreach (Collider collider in array)
		{
			if (collider != null)
			{
				collider.enabled = state;
			}
			else
			{
				Debug.Log("OnOffCollider is null on " + transform.name);
			}
		}
	}

	public override RGD Serialize_Save()
	{
		if (IsValidSerialization() && networkType == NetworkType.None && buildableItem != null)
		{
			return new RGD_Block(RGDType.Block, this);
		}
		return null;
	}

	public virtual RGD_Block GetBlockCreationData()
	{
		return new RGD_Block(RGDType.Block, this);
	}
}
