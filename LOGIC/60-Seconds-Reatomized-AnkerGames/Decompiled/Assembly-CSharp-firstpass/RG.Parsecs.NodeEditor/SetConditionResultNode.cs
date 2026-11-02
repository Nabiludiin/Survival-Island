using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Flow Nodes/Set Condition Result Node", new Type[] { typeof(ConditionEvent) })]
public class SetConditionResultNode : FlowNode
{
	public const string ID = "CE_SetConditionResultNode";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_CONDITION_INDEX = 1;

	private const string INPUT_FLOW_NAME = "In";

	private const string CONDITION_FLOW_NAME = "Result";

	private const string NODE_NAME = "Set Condition Result";

	[SerializeField]
	private bool _conditionResult;

	public override string GetID => "CE_SetConditionResultNode";

	public override Node Create(Vector2 pos)
	{
		SetConditionResultNode setConditionResultNode = ScriptableObject.CreateInstance<SetConditionResultNode>();
		setConditionResultNode.rect = new Rect(pos.x, pos.y, 150f, 125f);
		setConditionResultNode.name = "Set Condition Result";
		setConditionResultNode.CreateMutliInput("In", "Flow");
		setConditionResultNode.CreateInput("Result", "Bool");
		return setConditionResultNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetConditionResultNode setConditionResultNode = (SetConditionResultNode)Create(rect.position + new Vector2(20f, 20f));
		setConditionResultNode._conditionResult = _conditionResult;
		return setConditionResultNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _conditionResult, canvas);
		((ConditionEvent)canvas).Result = _conditionResult;
	}
}
