using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(true, "Legacy/Schedule Node", new Type[]
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
public class ScheduleV2Node : EventNode
{
	public const string ID = "EE_ScheduleV2Node";

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

	private const int MINIMAL_DAY_VALUE = 0;

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_EVENT_INDEX = 1;

	private const int FORCE_SCHEDULE_INDEX = 2;

	private const int INPUT_MIN_INDEX = 3;

	private const int INPUT_MAX_INDEX = 4;

	private const int OFFSET = 5;

	private const string NODE_NAME = "Schedule Node";

	private const string INPUT_FLOW_NAME = "In";

	private const string INPUT_EVENT_NAME = "Event";

	private const string FORCE_SCHEDULE_INPUT_NAME = "Force Schedule";

	private const string MIN_INPUT_NAME = "Min";

	private const string MAX_INPUT_NAME = "Max";

	private const string OUTPUT_FLOW_NAME = "Out";

	public override string GetID => "EE_ScheduleV2Node";

	public override Node Create(Vector2 pos)
	{
		ScheduleV2Node scheduleV2Node = ScriptableObject.CreateInstance<ScheduleV2Node>();
		scheduleV2Node.rect = new Rect(pos.x, pos.y, 300f, 200f);
		scheduleV2Node.name = "Schedule Node";
		scheduleV2Node.CreateMutliInput("In", "Flow");
		scheduleV2Node.CreateInput("Event", "ParsecsEvent");
		scheduleV2Node.CreateInput("Force Schedule", "Bool");
		scheduleV2Node.CreateInput("Min", "Int");
		scheduleV2Node.CreateInput("Max", "Int");
		scheduleV2Node.CreateOutput("Out", "Flow");
		_previousEvent = _event;
		_charactersParameters = new List<CharacterParameter>();
		_itemsParameters = new List<ItemParameter>();
		return scheduleV2Node;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ScheduleV2Node scheduleV2Node = (ScheduleV2Node)Create(rect.position + new Vector2(20f, 20f));
		scheduleV2Node._min = _min;
		scheduleV2Node._max = _max;
		scheduleV2Node._forceSchedule = _forceSchedule;
		scheduleV2Node._event = _event;
		scheduleV2Node._previousEvent = _previousEvent;
		scheduleV2Node._charactersParameters = _charactersParameters;
		scheduleV2Node._itemsParameters = _itemsParameters;
		return scheduleV2Node;
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
			GetInputValue(Inputs[5 + i], ref _charactersParameters[i].Character, canvas);
			list.Add(_charactersParameters[i]);
		}
		int num = 5 + _charactersParameters.Count;
		List<ItemParameter> list2 = new List<ItemParameter>();
		for (int j = 0; j < _itemsParameters.Count; j++)
		{
			GetInputValue(Inputs[num + j], ref _itemsParameters[j].Item, canvas);
			list2.Add(_itemsParameters[j]);
		}
		GetInputValue(Inputs[3], ref _min, canvas);
		GetInputValue(Inputs[4], ref _max, canvas);
		GetInputValue(Inputs[2], ref _forceSchedule, canvas);
		GetInputValue(Inputs[1], ref _event, canvas);
		SchedulerManager.Instance.Schedule(_event, _min, _max, list2, list, _forceSchedule);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
