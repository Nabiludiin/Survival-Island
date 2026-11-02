using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Is Event Scheduled Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class IsEventScheduledNode : EventNode
{
	public const string ID = "EE_IsEventScheduledNode";

	private const string NODE_NAME = "Is Event Scheduled Node";

	private const string INPUT_NAME = "Event";

	private const string OUTPUT_NAME = "Is scheduled";

	private const int INPUT_EVENT_INDEX = 0;

	private const int OUTPUT_IS_SCHEDULED_INDEX = 0;

	[SerializeField]
	private SurvivalEvent _event;

	public override string GetID => "EE_IsEventScheduledNode";

	public override Node Create(Vector2 pos)
	{
		IsEventScheduledNode isEventScheduledNode = ScriptableObject.CreateInstance<IsEventScheduledNode>();
		isEventScheduledNode.rect = new Rect(pos.x, pos.y, 300f, 180f);
		isEventScheduledNode.name = "Is Event Scheduled Node";
		isEventScheduledNode.CreateInput("Event", "ParsecsEvent");
		isEventScheduledNode.CreateOutput("Is scheduled", "Bool");
		return isEventScheduledNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsEventScheduledNode isEventScheduledNode = (IsEventScheduledNode)Create(rect.position + new Vector2(20f, 20f));
		isEventScheduledNode._event = _event;
		return isEventScheduledNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _event, canvas);
		return CastValue<T>(SchedulerManager.Instance.IsEventScheduled(_event, includeActualDay: true));
	}
}
