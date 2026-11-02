using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Resources/Get Crafting Operation Resources Values Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetCraftingOperationResourcesValues : ResourceNode
{
	public const string ID = "EE_GetCraftingOperationResourcesValues";

	private const string INPUT_ITEM_NAME = "Item";

	private const string OUTPUT_VALUE_NAME = "Value";

	private const string NODE_NAME = "Get Crafting Operation Resources Values";

	private const int OUTPUT_VALUE_INDEX = 0;

	private const int INPUT_ITEM_INDEX = 0;

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private EPlannedCraftingAction _craftingAction;

	public override string GetID => "EE_GetCraftingOperationResourcesValues";

	public override Node Create(Vector2 pos)
	{
		GetCraftingOperationResourcesValues getCraftingOperationResourcesValues = ScriptableObject.CreateInstance<GetCraftingOperationResourcesValues>();
		getCraftingOperationResourcesValues.rect = new Rect(pos.x, pos.y, 180f, 40f);
		getCraftingOperationResourcesValues.name = "Get Crafting Operation Resources Values";
		getCraftingOperationResourcesValues.CreateInput("Item", "Item");
		getCraftingOperationResourcesValues.CreateOutput("Resource 1", "Int");
		getCraftingOperationResourcesValues.CreateOutput("Resource 2", "Int");
		getCraftingOperationResourcesValues.CreateOutput("Resource 3", "Int");
		getCraftingOperationResourcesValues.CreateOutput("Resource 1", "Resources");
		getCraftingOperationResourcesValues.CreateOutput("Resource 2", "Resources");
		getCraftingOperationResourcesValues.CreateOutput("Resource 3", "Resources");
		return getCraftingOperationResourcesValues;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCraftingOperationResourcesValues getCraftingOperationResourcesValues = (GetCraftingOperationResourcesValues)Create(rect.position + new Vector2(20f, 20f));
		getCraftingOperationResourcesValues._item = _item;
		getCraftingOperationResourcesValues._craftingAction = _craftingAction;
		return getCraftingOperationResourcesValues;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output > 5)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _item, canvas);
		CraftingOperation craftingOperation = null;
		switch (_craftingAction)
		{
		case EPlannedCraftingAction.CRAFT:
			craftingOperation = _item.GetCreateCost();
			break;
		case EPlannedCraftingAction.RECYCLE:
			craftingOperation = _item.GetRecyclingCostForCurrentLevel();
			break;
		case EPlannedCraftingAction.REPAIR:
			craftingOperation = ((Item)_item).GetRepairCostsForCurrentLevel();
			break;
		case EPlannedCraftingAction.UPGRADE:
			craftingOperation = _item.GetUpgradeCostForCurrentLevel();
			break;
		default:
			throw new ArgumentOutOfRangeException();
		case EPlannedCraftingAction.NONE:
			break;
		}
		if (craftingOperation == null)
		{
			return CastValue<T>(0);
		}
		switch (output)
		{
		case 0:
			if (craftingOperation.Resources.Count > 0)
			{
				return CastValue<T>(craftingOperation.Resources[0].Amount);
			}
			return CastValue<T>(0);
		case 1:
			if (craftingOperation.Resources.Count > 1)
			{
				return CastValue<T>(craftingOperation.Resources[1].Amount);
			}
			return CastValue<T>(0);
		case 2:
			if (craftingOperation.Resources.Count > 2)
			{
				return CastValue<T>(craftingOperation.Resources[2].Amount);
			}
			return CastValue<T>(0);
		case 3:
			if (craftingOperation.Resources.Count > 0)
			{
				return CastValue<T>(craftingOperation.Resources[0].Resource);
			}
			return CastValue<T>(null);
		case 4:
			if (craftingOperation.Resources.Count > 1)
			{
				return CastValue<T>(craftingOperation.Resources[1].Resource);
			}
			return CastValue<T>(null);
		case 5:
			if (craftingOperation.Resources.Count > 2)
			{
				return CastValue<T>(craftingOperation.Resources[2].Resource);
			}
			return CastValue<T>(null);
		default:
			if (output < 3)
			{
				return CastValue<T>(0);
			}
			return CastValue<T>(null);
		}
	}
}
