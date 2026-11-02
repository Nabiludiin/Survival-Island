using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Condition Nodes/Is List Empty Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class IsListEmptyNode : ConditionNode
{
	public const string ID = "PE_IsListEmptyNode";

	[SerializeField]
	private bool _value;

	private const int INPUT_VALUE_INDEX = 0;

	private const int OUTPUT_RESULT_INDEX = 0;

	public override string GetID => "PE_IsListEmptyNode";

	public override Node Create(Vector2 pos)
	{
		IsListEmptyNode isListEmptyNode = ScriptableObject.CreateInstance<IsListEmptyNode>();
		isListEmptyNode.rect = new Rect(pos.x, pos.y, 150f, 70f);
		isListEmptyNode.name = "Is List Empty";
		isListEmptyNode.CreateInput("List", ListConnection.ID);
		isListEmptyNode.CreateOutput("Result", "Bool");
		return isListEmptyNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsListEmptyNode isListEmptyNode = (IsListEmptyNode)Create(rect.position + new Vector2(20f, 20f));
		isListEmptyNode._value = _value;
		return isListEmptyNode;
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		IList inputValue = GetInputValue<IList>(Inputs[0], canvas);
		return CastValue<T>(inputValue == null || inputValue.Count == 0);
	}
}
