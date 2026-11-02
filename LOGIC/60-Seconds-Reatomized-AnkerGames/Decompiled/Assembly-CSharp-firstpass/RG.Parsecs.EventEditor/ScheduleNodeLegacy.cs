using System;
using System.Collections.Generic;
using NodeEditorFramework;
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
	typeof(ExpeditionEvent)
})]
public class ScheduleNodeLegacy : EventNode
{
	public const string ID = "EE_LegacyScheduleNode";

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

	private const int MINIMAL_DAY_VALUE = 0;

	private const int INPUT_MIN_INDEX = 1;

	private const int INPUT_MAX_INDEX = 2;

	private const int OFFSET = 3;

	public override string GetID => "EE_LegacyScheduleNode";

	public override Node Create(Vector2 pos)
	{
		ScheduleNodeLegacy scheduleNodeLegacy = ScriptableObject.CreateInstance<ScheduleNodeLegacy>();
		scheduleNodeLegacy.rect = new Rect(pos.x, pos.y, 300f, 200f);
		scheduleNodeLegacy.name = "Schedule node Legacy";
		scheduleNodeLegacy.CreateMutliInput("In", "Flow");
		scheduleNodeLegacy.CreateInput("Min", "Int");
		scheduleNodeLegacy.CreateInput("Max", "Int");
		scheduleNodeLegacy.CreateOutput("Out", "Flow");
		_previousEvent = _event;
		_charactersParameters = new List<CharacterParameter>();
		_itemsParameters = new List<ItemParameter>();
		return scheduleNodeLegacy;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (ScheduleNodeLegacy)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void CreateDataLists()
	{
		_charactersParameters.Clear();
		_itemsParameters.Clear();
		for (int i = 0; i < _event.CharacterParameters.Count; i++)
		{
			CharacterParameter item = new CharacterParameter
			{
				Name = _event.CharacterParameters[i].Name
			};
			_charactersParameters.Add(item);
		}
		for (int j = 0; j < _event.ItemParameters.Count; j++)
		{
			ItemParameter item2 = new ItemParameter
			{
				Name = _event.ItemParameters[j].Name
			};
			_itemsParameters.Add(item2);
		}
	}

	private void UpdateInputs()
	{
	}

	private void ShowItemsAndCharsInputs()
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
			GetInputValue(Inputs[3 + i], ref _charactersParameters[i].Character, canvas);
			list.Add(_charactersParameters[i]);
		}
		int num = 3 + _charactersParameters.Count;
		List<ItemParameter> list2 = new List<ItemParameter>();
		for (int j = 0; j < _itemsParameters.Count; j++)
		{
			GetInputValue(Inputs[num + j], ref _itemsParameters[j].Item, canvas);
			list2.Add(_itemsParameters[j]);
		}
		GetInputValue(Inputs[1], ref _min, canvas);
		GetInputValue(Inputs[2], ref _max, canvas);
		SchedulerManager.Instance.Schedule(_event, _min, _max, list2, list);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
