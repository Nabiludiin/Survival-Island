using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using UnityEngine;

namespace RG.Parsecs.Survival;

[Node(false, "Flow Nodes/Reschedule Node", new Type[] { typeof(ConditionEvent) })]
public class RescheduleNode : FlowNode
{
	public const string ID = "CE_RescheduleNode";

	private const int INPUT_FLOW_INDEX = 0;

	private const int FORCE_SCHEDULE_INDEX = 1;

	private const int INPUT_MIN_INDEX = 2;

	private const int INPUT_MAX_INDEX = 3;

	private const string NODE_NAME = "Reschedule Node";

	private const string INPUT_FLOW_NAME = "In";

	private const string FORCE_SCHEDULE_INPUT_NAME = "Force Schedule";

	private const string MIN_INPUT_NAME = "Min";

	private const string MAX_INPUT_NAME = "Max";

	[SerializeField]
	private int _min = 1;

	[SerializeField]
	private int _max = 1;

	[SerializeField]
	private bool _forceSchedule;

	private const int MINIMAL_DAY_VALUE = 1;

	public override string GetID => "CE_RescheduleNode";

	public override Node Create(Vector2 pos)
	{
		RescheduleNode rescheduleNode = ScriptableObject.CreateInstance<RescheduleNode>();
		rescheduleNode.rect = new Rect(pos.x, pos.y, 200f, 200f);
		rescheduleNode.name = "Reschedule Node";
		rescheduleNode.CreateMutliInput("In", "Flow");
		rescheduleNode.CreateInput("Force Schedule", "Bool");
		rescheduleNode.CreateInput("Min", "Int");
		rescheduleNode.CreateInput("Max", "Int");
		return rescheduleNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RescheduleNode rescheduleNode = (RescheduleNode)Create(rect.position + new Vector2(20f, 20f));
		rescheduleNode._min = _min;
		rescheduleNode._max = _max;
		rescheduleNode._forceSchedule = _forceSchedule;
		return rescheduleNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		ConditionEvent conditionEvent = (ConditionEvent)canvas;
		conditionEvent.Result = false;
		if (conditionEvent.IsEventScheduled)
		{
			GetInputValue(Inputs[2], ref _min, canvas);
			GetInputValue(Inputs[3], ref _max, canvas);
			GetInputValue(Inputs[1], ref _forceSchedule, canvas);
			SchedulerManager.Instance.RescheduleEvent(_min, _max, _forceSchedule);
		}
	}
}
