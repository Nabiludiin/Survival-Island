using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Was Event Executed Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class WasEventExecutedNode : EventNode
{
	public const string ID = "EE_WasEventExecuted";

	[SerializeField]
	private SurvivalEvent _survivalEvent;

	private const string NODE_NAME = "Was Event Executed";

	private const string OUTPUT_RESULT_NAME = "Was Executed";

	private const string OUTPUT_SUCCESS_NAME = "Was Successful";

	private const string INPUT_EVENT_NAME = "Survival Event";

	private const int INPUT_EVENT_INDEX = 0;

	private const int OUTPUT_RESULT_INDEX = 0;

	private const int OUTPUT_SUCCESS_INDEX = 1;

	public override string GetID => "EE_WasEventExecuted";

	public override Node Create(Vector2 pos)
	{
		WasEventExecutedNode wasEventExecutedNode = ScriptableObject.CreateInstance<WasEventExecutedNode>();
		wasEventExecutedNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		wasEventExecutedNode.name = "Was Event Executed";
		wasEventExecutedNode.CreateInput("Survival Event", "SurvivalEvent");
		wasEventExecutedNode.CreateOutput("Was Executed", "Bool");
		wasEventExecutedNode.CreateOutput("Was Successful", "Bool");
		return wasEventExecutedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		WasEventExecutedNode wasEventExecutedNode = (WasEventExecutedNode)Create(rect.position + new Vector2(20f, 20f));
		wasEventExecutedNode._survivalEvent = _survivalEvent;
		return wasEventExecutedNode;
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
		GetInputValue(Inputs[0], ref _survivalEvent, canvas);
		return output switch
		{
			0 => CastValue<T>(_survivalEvent.WasEventExecuted), 
			1 => CastValue<T>(_survivalEvent.WasEventExecuted && _survivalEvent.WasEventSuccessful), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
