using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Get Item Name Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetItemNameNode : ResourceNode
{
	public const string ID = "EE_GetItemNameNode";

	[SerializeField]
	private IItem _item;

	[SerializeField]
	private bool _upgraded;

	private const int INPUT_ITEM_INDEX = 0;

	private const int INPUT_UPGRADED_INDEX = 1;

	private const int OUTPUT_GET_NAME_INDEX = 0;

	private const string INPUT_ITEM_NAME = "Item";

	private const string INPUT_UPGRADED = "Upgraded";

	private const string OUTPUT_GET_NAME = "Get Name";

	private const string NOT_SET_UPGRADED = "Node does not have the upgraded flag set!";

	public override string GetID => "EE_GetItemNameNode";

	public override Node Create(Vector2 pos)
	{
		GetItemNameNode getItemNameNode = ScriptableObject.CreateInstance<GetItemNameNode>();
		getItemNameNode.rect = new Rect(pos.x, pos.y, 180f, 120f);
		getItemNameNode.name = "Get Item Name";
		getItemNameNode.CreateInput("Item", "Item");
		getItemNameNode.CreateInput("Upgraded", "Bool");
		getItemNameNode.CreateOutput("Get Name", "Int");
		return getItemNameNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetItemNameNode getItemNameNode = (GetItemNameNode)Create(rect.position + new Vector2(20f, 20f));
		getItemNameNode._item = _item;
		getItemNameNode._upgraded = _upgraded;
		return getItemNameNode;
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
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _item, canvas);
		GetInputValue(Inputs[1], ref _upgraded, canvas);
		if (_upgraded)
		{
			return CastValue<T>(_item.BaseStaticData.UpgradedName);
		}
		return CastValue<T>(_item.BaseStaticData.Name);
	}
}
