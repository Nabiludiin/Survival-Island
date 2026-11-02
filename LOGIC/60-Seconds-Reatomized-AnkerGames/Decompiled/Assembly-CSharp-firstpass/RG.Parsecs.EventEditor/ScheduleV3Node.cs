using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Schedule Node", new Type[]
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
public class ScheduleV3Node : EventNode
{
	public const string ID = "EE_ScheduleV3Node";

	[SerializeField]
	private ParsecsEvent _event;

	private ParsecsEvent _previousEvent;

	[SerializeField]
	private int _min = 1;

	[SerializeField]
	private int _max = 1;

	[SerializeField]
	private List<CharacterParameter> _charactersParameters;

	[SerializeField]
	private List<ItemParameter> _itemsParameters;

	[SerializeField]
	private bool _forceSchedule;

	[SerializeField]
	private bool _ignoreCondition;

	private const int MINIMAL_DAY_VALUE = 0;

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_EVENT_INDEX = 1;

	private const int FORCE_SCHEDULE_INDEX = 2;

	private const int IGNORE_CONDITION_INDEX = 3;

	private const int INPUT_MIN_INDEX = 4;

	private const int INPUT_MAX_INDEX = 5;

	private const int OFFSET = 6;

	private const string NODE_NAME = "Schedule Node";

	private const string INPUT_FLOW_NAME = "In";

	private const string INPUT_EVENT_NAME = "Event";

	private const string FORCE_SCHEDULE_INPUT_NAME = "Force Schedule";

	private const string IGNORE_CONDITION_INPUT_NAME = "Ignore conditiom";

	private const string MIN_INPUT_NAME = "Min";

	private const string MAX_INPUT_NAME = "Max";

	private const string OUTPUT_FLOW_NAME = "Out";

	public override string GetID => "EE_ScheduleV3Node";

	public override Node Create(Vector2 pos)
	{
		ScheduleV3Node scheduleV3Node = ScriptableObject.CreateInstance<ScheduleV3Node>();
		scheduleV3Node.rect = new Rect(pos.x, pos.y, 300f, 200f);
		scheduleV3Node.name = "Schedule Node";
		scheduleV3Node.CreateMutliInput("In", "Flow");
		scheduleV3Node.CreateInput("Event", "ParsecsEvent");
		scheduleV3Node.CreateInput("Force Schedule", "Bool");
		scheduleV3Node.CreateInput("Ignore conditiom", "Bool");
		scheduleV3Node.CreateInput("Min", "Int");
		scheduleV3Node.CreateInput("Max", "Int");
		scheduleV3Node.CreateOutput("Out", "Flow");
		_previousEvent = _event;
		_charactersParameters = new List<CharacterParameter>();
		_itemsParameters = new List<ItemParameter>();
		return scheduleV3Node;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ScheduleV3Node scheduleV3Node = (ScheduleV3Node)Create(rect.position + new Vector2(20f, 20f));
		scheduleV3Node._min = _min;
		scheduleV3Node._max = _max;
		scheduleV3Node._forceSchedule = _forceSchedule;
		scheduleV3Node._event = _event;
		scheduleV3Node._previousEvent = _previousEvent;
		scheduleV3Node._charactersParameters = _charactersParameters;
		scheduleV3Node._itemsParameters = _itemsParameters;
		scheduleV3Node._ignoreCondition = _ignoreCondition;
		return scheduleV3Node;
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
		List<CharacterParameter> list = new List<CharacterParameter>();
		for (int i = 0; i < _charactersParameters.Count; i++)
		{
			GetInputValue(Inputs[6 + i], ref _charactersParameters[i].Character, canvas);
			list.Add(_charactersParameters[i]);
		}
		int num = 6 + _charactersParameters.Count;
		List<ItemParameter> list2 = new List<ItemParameter>();
		for (int j = 0; j < _itemsParameters.Count; j++)
		{
			GetInputValue(Inputs[num + j], ref _itemsParameters[j].Item, canvas);
			list2.Add(_itemsParameters[j]);
		}
		GetInputValue(Inputs[4], ref _min, canvas);
		GetInputValue(Inputs[5], ref _max, canvas);
		GetInputValue(Inputs[2], ref _forceSchedule, canvas);
		GetInputValue(Inputs[1], ref _event, canvas);
		GetInputValue(Inputs[3], ref _ignoreCondition, canvas);
		SchedulerManager.Instance.Schedule(_event, _min, _max, list2, list, _forceSchedule, _ignoreCondition);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
