using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Is Event Scheduled Node", new Type[]
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
public class IsEventScheduleV2Node : EventNode
{
	public const string ID = "EE_IsEventScheduledV2Node";

	private const string NODE_NAME = "Is Event Scheduled Node";

	private const string INCLUDE_ACTUAL_DAY_NAME = "Actual day";

	private const string INPUT_NAME = "Event";

	private const string OUTPUT_NAME = "Is scheduled";

	private const int INPUT_EVENT_INDEX = 0;

	private const int INPUT_INCLUDE_ACTUAL_DAY = 1;

	private const int OUTPUT_IS_SCHEDULED_INDEX = 0;

	[SerializeField]
	private SurvivalEvent _event;

	[SerializeField]
	private bool _includeActualDay = true;

	public override string GetID => "EE_IsEventScheduledV2Node";

	public override Node Create(Vector2 pos)
	{
		IsEventScheduleV2Node isEventScheduleV2Node = ScriptableObject.CreateInstance<IsEventScheduleV2Node>();
		isEventScheduleV2Node.rect = new Rect(pos.x, pos.y, 300f, 180f);
		isEventScheduleV2Node.name = "Is Event Scheduled Node";
		isEventScheduleV2Node.CreateInput("Event", "ParsecsEvent");
		isEventScheduleV2Node.CreateInput("Actual day", "Bool");
		isEventScheduleV2Node.CreateOutput("Is scheduled", "Bool");
		return isEventScheduleV2Node;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsEventScheduleV2Node isEventScheduleV2Node = (IsEventScheduleV2Node)Create(rect.position + new Vector2(20f, 20f));
		isEventScheduleV2Node._event = _event;
		isEventScheduleV2Node._includeActualDay = _includeActualDay;
		return isEventScheduleV2Node;
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
		GetInputValue(Inputs[1], ref _includeActualDay, canvas);
		return CastValue<T>(SchedulerManager.Instance.IsEventScheduled(_event, _includeActualDay));
	}
}
