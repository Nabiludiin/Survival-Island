using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Crew Count Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class GetCrewCountAdditionalFiltersNode : CharacterNode
{
	public const string ID = "getCrewCountAdditionalFiltersNode";

	private const int CAPTAIN_INPUT_INDEX = 0;

	private const string CAPTAIN_INPUT_NAME = "Include captain";

	private const int DEAD_INPUT_INDEX = 1;

	private const string DEAD_INPUT_NAME = "Exclude dead";

	private const int EXPEDITION_INPUT_INDEX = 2;

	private const string EXPEDITION_INPUT_NAME = "Exclude non expeditable";

	private const int SELECTION_INPUT_INDEX = 3;

	private const string SELECTION_INPUT_NAME = "Exclude non selectable";

	private const int DISPLAY_INPUT_INDEX = 4;

	private const string DISPLAY_INPUT_NAME = "Exclude not displayed";

	private const int CREW_OUTPUT_INDEX = 0;

	private const string CREW_OUTPUT_NAME = "Crew Count";

	private const string INDEX_OUTPUT_NAME = "Index";

	private const int INDEX_OUTPUT_INDEX = 1;

	private const string NODE_NAME = "Get Crew Count";

	[SerializeField]
	private bool _includeCaptain = true;

	[SerializeField]
	private bool _liveOnly = true;

	[SerializeField]
	private bool _excludePreventedFromGoingOnExpedition;

	[SerializeField]
	private bool _excludePreventedFromSelectionInEvents;

	[SerializeField]
	private bool _excludePreventedFromDisplayingOnShip = true;

	public override string GetID => "getCrewCountAdditionalFiltersNode";

	public override Node Create(Vector2 pos)
	{
		GetCrewCountAdditionalFiltersNode getCrewCountAdditionalFiltersNode = ScriptableObject.CreateInstance<GetCrewCountAdditionalFiltersNode>();
		getCrewCountAdditionalFiltersNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getCrewCountAdditionalFiltersNode.name = "Get Crew Count";
		getCrewCountAdditionalFiltersNode.CreateInput("Include captain", "Bool");
		getCrewCountAdditionalFiltersNode.CreateInput("Exclude dead", "Bool");
		getCrewCountAdditionalFiltersNode.CreateInput("Exclude non expeditable", "Bool");
		getCrewCountAdditionalFiltersNode.CreateInput("Exclude non selectable", "Bool");
		getCrewCountAdditionalFiltersNode.CreateInput("Exclude not displayed", "Bool");
		getCrewCountAdditionalFiltersNode.CreateOutput("Crew Count", "Int");
		getCrewCountAdditionalFiltersNode.CreateOutput("Index", "Int");
		return getCrewCountAdditionalFiltersNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCrewCountAdditionalFiltersNode getCrewCountAdditionalFiltersNode = (GetCrewCountAdditionalFiltersNode)Create(rect.position + new Vector2(20f, 20f));
		getCrewCountAdditionalFiltersNode._includeCaptain = _includeCaptain;
		getCrewCountAdditionalFiltersNode._liveOnly = _liveOnly;
		getCrewCountAdditionalFiltersNode._excludePreventedFromDisplayingOnShip = _excludePreventedFromDisplayingOnShip;
		getCrewCountAdditionalFiltersNode._excludePreventedFromGoingOnExpedition = _excludePreventedFromGoingOnExpedition;
		getCrewCountAdditionalFiltersNode._excludePreventedFromSelectionInEvents = _excludePreventedFromSelectionInEvents;
		return getCrewCountAdditionalFiltersNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0 && output != 1)
		{
			throw new NodeOutputNotFoundException(output, "getCrewCountAdditionalFiltersNode");
		}
		UpdateInputValues(canvas);
		List<Character> list = ((!_liveOnly) ? CharacterManager.Instance.GetCharacterList().CharactersInGame : CharacterManager.Instance.GetCharacterList().GetLiveCharacters());
		if (list.Count > 0)
		{
			if (!_includeCaptain && list[0].RuntimeData.IsCaptain)
			{
				list.RemoveAt(0);
			}
			if (_excludePreventedFromDisplayingOnShip)
			{
				list = (from x in list
					where x.RuntimeData.IsDrawnOnShip()
					select (x)).ToList();
			}
			if (_excludePreventedFromGoingOnExpedition)
			{
				list = (from x in list
					where !x.RuntimeData.HasStatusPreventingGoingOnExpeditions()
					select (x)).ToList();
			}
			if (_excludePreventedFromSelectionInEvents)
			{
				list = (from x in list
					where !x.RuntimeData.HasStatusPreventingSelectionInEventChoices()
					select (x)).ToList();
			}
		}
		if (output == 0)
		{
			return CastValue<T>(list.Count);
		}
		return CastValue<T>(list.Count - 1);
	}

	private void UpdateInputValues(NodeCanvas canvas)
	{
		GetInputValue(Inputs[0], ref _includeCaptain, canvas);
		GetInputValue(Inputs[1], ref _liveOnly, canvas);
		GetInputValue(Inputs[2], ref _excludePreventedFromGoingOnExpedition, canvas);
		GetInputValue(Inputs[3], ref _excludePreventedFromSelectionInEvents, canvas);
		GetInputValue(Inputs[4], ref _excludePreventedFromDisplayingOnShip, canvas);
	}
}
