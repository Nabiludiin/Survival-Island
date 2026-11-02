using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Items/Set Item Enabled Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetItemEnabledNode : ResourceNode
{
	private const string INPUT_NOT_CONNECTED_MESSAGE = "Input is not connected";

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	private const string ITEM_NOT_CONNECTED_MESSAGE = "No item set in SetItemEnabledNode";

	public const string ID = "GE_SetItemEnabledNode";

	private const string NODE_NAME = "Set Item Enabled";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string VALUE_INPUT_NAME = "Value to Set";

	private const string ITEM_INPUT_NAME = "Item";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const int VALUE_INPUT_INDEX = 2;

	private const int ITEM_INPUT_INDEX = 1;

	[SerializeField]
	private bool _valueToSet = true;

	[SerializeField]
	private IItem _item;

	public override string GetID => "GE_SetItemEnabledNode";

	public override Node Create(Vector2 pos)
	{
		SetItemEnabledNode setItemEnabledNode = ScriptableObject.CreateInstance<SetItemEnabledNode>();
		setItemEnabledNode.name = "Set Item Enabled";
		setItemEnabledNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		setItemEnabledNode.CreateMutliInput("In", "Flow");
		setItemEnabledNode.CreateInput("Item", "Item");
		setItemEnabledNode.CreateInput("Value to Set", "Bool");
		setItemEnabledNode.CreateOutput("Out", "Flow");
		return setItemEnabledNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetItemEnabledNode setItemEnabledNode = (SetItemEnabledNode)Create(rect.position + new Vector2(20f, 20f));
		setItemEnabledNode._valueToSet = _valueToSet;
		setItemEnabledNode._item = _item;
		return setItemEnabledNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _item, canvas);
		GetInputValue(Inputs[2], ref _valueToSet, canvas);
		_item.BaseRuntimeData.IsEnabled = _valueToSet;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
