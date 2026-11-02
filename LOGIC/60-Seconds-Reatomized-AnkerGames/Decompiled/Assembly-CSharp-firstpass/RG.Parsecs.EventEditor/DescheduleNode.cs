using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Deschedule Node", new Type[]
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
public class DescheduleNode : EventNode
{
	public const string ID = "EE_DescheduleNode";

	private const string NODE_NAME = "Deschedule Node";

	private const string INPUT_FLOW_NAME = "In";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const string PARSECS_EVENT_INPUT_NAME = "Event";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_EVENT_INDEX = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	[SerializeField]
	private SurvivalEvent _event;

	public override string GetID => "EE_DescheduleNode";

	public override Node Create(Vector2 pos)
	{
		DescheduleNode descheduleNode = ScriptableObject.CreateInstance<DescheduleNode>();
		descheduleNode.rect = new Rect(pos.x, pos.y, 300f, 180f);
		descheduleNode.name = "Deschedule Node";
		descheduleNode.CreateMutliInput("In", "Flow");
		descheduleNode.CreateOutput("Out", "Flow");
		descheduleNode.CreateInput("Event", "ParsecsEvent");
		return descheduleNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DescheduleNode descheduleNode = (DescheduleNode)Create(rect.position + new Vector2(20f, 20f));
		descheduleNode._event = _event;
		return descheduleNode;
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
		GetInputValue(Inputs[1], ref _event, canvas);
		SchedulerManager.Instance.Dechsedule(_event);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
