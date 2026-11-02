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
	typeof(Goal)
})]
public class ScheduleNode : EventNode
{
	public const string ID = "EE_ScheduleNode";

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

	private const int FORCE_SCHEDULE_INDEX = 1;

	private const int INPUT_MIN_INDEX = 2;

	private const int INPUT_MAX_INDEX = 3;

	private const int OFFSET = 4;

	private const string NODE_NAME = "Schedule Node";

	private const string INPUT_FLOW_NAME = "In";

	private const string FORCE_SCHEDULE_INPUT_NAME = "Force Schedule";

	private const string MIN_INPUT_NAME = "Min";

	private const string MAX_INPUT_NAME = "Max";

	private const string OUTPUT_FLOW_NAME = "Out";

	public override string GetID => "EE_ScheduleNode";

	public override Node Create(Vector2 pos)
	{
		ScheduleNode scheduleNode = ScriptableObject.CreateInstance<ScheduleNode>();
		scheduleNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		scheduleNode.name = "Schedule Node";
		scheduleNode.CreateMutliInput("In", "Flow");
		scheduleNode.CreateInput("Force Schedule", "Bool");
		scheduleNode.CreateInput("Min", "Int");
		scheduleNode.CreateInput("Max", "Int");
		scheduleNode.CreateOutput("Out", "Flow");
		_previousEvent = _event;
		_charactersParameters = new List<CharacterParameter>();
		_itemsParameters = new List<ItemParameter>();
		return scheduleNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ScheduleNode scheduleNode = (ScheduleNode)Create(rect.position + new Vector2(20f, 20f));
		scheduleNode._min = _min;
		scheduleNode._max = _max;
		scheduleNode._forceSchedule = _forceSchedule;
		scheduleNode._event = _event;
		scheduleNode._previousEvent = _previousEvent;
		scheduleNode._charactersParameters = _charactersParameters;
		scheduleNode._itemsParameters = _itemsParameters;
		return scheduleNode;
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
			GetInputValue(Inputs[4 + i], ref _charactersParameters[i].Character, canvas);
			list.Add(_charactersParameters[i]);
		}
		int num = 4 + _charactersParameters.Count;
		List<ItemParameter> list2 = new List<ItemParameter>();
		for (int j = 0; j < _itemsParameters.Count; j++)
		{
			GetInputValue(Inputs[num + j], ref _itemsParameters[j].Item, canvas);
			list2.Add(_itemsParameters[j]);
		}
		GetInputValue(Inputs[2], ref _min, canvas);
		GetInputValue(Inputs[3], ref _max, canvas);
		GetInputValue(Inputs[1], ref _forceSchedule, canvas);
		SchedulerManager.Instance.Schedule(_event, _min, _max, list2, list, _forceSchedule);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
