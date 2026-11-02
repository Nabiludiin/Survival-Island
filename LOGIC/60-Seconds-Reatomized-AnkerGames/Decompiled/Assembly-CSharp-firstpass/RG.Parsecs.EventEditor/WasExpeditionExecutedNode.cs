using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Was Expedition Executed Node", new Type[]
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
public class WasExpeditionExecutedNode : EventNode
{
	public const string ID = "EE_WasExpeditionExecuted";

	[SerializeField]
	private ExpeditionEvent _expeditionEvent;

	private const string NODE_NAME = "Was Expedition Executed";

	private const string OUTPUT_RESULT_NAME = "Was Executed";

	private const string OUTPUT_SUCCESS_NAME = "Was Successful";

	private const string INPUT_EVENT_NAME = "Expedition Event";

	private const int INPUT_EVENT_INDEX = 0;

	private const int OUTPUT_RESULT_INDEX = 0;

	private const int OUTPUT_SUCCESS_INDEX = 1;

	public override string GetID => "EE_WasExpeditionExecuted";

	public override Node Create(Vector2 pos)
	{
		WasExpeditionExecutedNode wasExpeditionExecutedNode = ScriptableObject.CreateInstance<WasExpeditionExecutedNode>();
		wasExpeditionExecutedNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		wasExpeditionExecutedNode.name = "Was Expedition Executed";
		wasExpeditionExecutedNode.CreateInput("Expedition Event", "ExpeditionEvent");
		wasExpeditionExecutedNode.CreateOutput("Was Executed", "Bool");
		wasExpeditionExecutedNode.CreateOutput("Was Successful", "Bool");
		return wasExpeditionExecutedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		WasExpeditionExecutedNode wasExpeditionExecutedNode = (WasExpeditionExecutedNode)Create(rect.position + new Vector2(20f, 20f));
		wasExpeditionExecutedNode._expeditionEvent = _expeditionEvent;
		return wasExpeditionExecutedNode;
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
		GetInputValue(Inputs[0], ref _expeditionEvent, canvas);
		return output switch
		{
			0 => CastValue<T>(_expeditionEvent.WasEventExecuted), 
			1 => CastValue<T>(_expeditionEvent.WasEventExecuted && _expeditionEvent.WasEventSuccessful), 
			_ => throw new NotExistingOutputException(GetID, output), 
		};
	}
}
