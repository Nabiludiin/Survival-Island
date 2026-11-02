using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Random Crew Member Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetRandomCrewMemberNode : CharacterNode
{
	private const int FLOW_INPUT_INDEX = 0;

	private const string FLOW_INPUT_NAME = "Randomize selection";

	private const int CAPTAIN_INPUT_INDEX = 1;

	private const string CAPTAIN_INPUT_NAME = "Include captain";

	private const int DEAD_INPUT_INDEX = 2;

	private const string DEAD_INPUT_NAME = "Exclude dead";

	private const int EXPEDITION_INPUT_INDEX = 3;

	private const string EXPEDITION_INPUT_NAME = "Exclude non expeditable";

	private const int SELECTION_INPUT_INDEX = 4;

	private const string SELECTION_INPUT_NAME = "Exclude non selectable";

	private const int DISPLAY_INPUT_INDEX = 5;

	private const string DISPLAY_INPUT_NAME = "Exclude not displayed";

	private const int CHARACTER_OUTPUT_INDEX = 1;

	private const string CHARACTER_OUTPUT_NAME = "Crew member";

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string SUCCESS_OUTPUT_NAME = "Member matching criteria found";

	private const int SUCCESS_OUTPUT_INDEX = 2;

	private const string NODE_NAME = "Get Random Crew Member";

	public const string ID = "EE_getRandomCrewMemberNode";

	private Character _chosenCharacter;

	[SerializeField]
	private bool _includeCaptain = true;

	[SerializeField]
	private bool _excludeDeadCharacters = true;

	[SerializeField]
	private bool _excludePreventedFromGoingOnExpedition = true;

	[SerializeField]
	private bool _excludePreventedFromSelectionInEvents = true;

	[SerializeField]
	private bool _excludePreventedFromDisplayingOnShip = true;

	public override string GetID => "EE_getRandomCrewMemberNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomCrewMemberNode getRandomCrewMemberNode = ScriptableObject.CreateInstance<GetRandomCrewMemberNode>();
		getRandomCrewMemberNode.rect = new Rect(pos.x, pos.y, 225f, 50f);
		getRandomCrewMemberNode.name = "Get Random Crew Member";
		getRandomCrewMemberNode.CreateMutliInput("Randomize selection", "Flow");
		getRandomCrewMemberNode.CreateInput("Include captain", "Bool");
		getRandomCrewMemberNode.CreateInput("Exclude dead", "Bool");
		getRandomCrewMemberNode.CreateInput("Exclude non expeditable", "Bool");
		getRandomCrewMemberNode.CreateInput("Exclude non selectable", "Bool");
		getRandomCrewMemberNode.CreateInput("Exclude not displayed", "Bool");
		getRandomCrewMemberNode.CreateOutput("Out", "Flow");
		getRandomCrewMemberNode.CreateOutput("Crew member", "Character");
		getRandomCrewMemberNode.CreateOutput("Member matching criteria found", "Bool");
		return getRandomCrewMemberNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomCrewMemberNode getRandomCrewMemberNode = (GetRandomCrewMemberNode)Create(rect.position + new Vector2(20f, 20f));
		getRandomCrewMemberNode._excludePreventedFromDisplayingOnShip = _excludePreventedFromDisplayingOnShip;
		getRandomCrewMemberNode._excludePreventedFromGoingOnExpedition = _excludePreventedFromGoingOnExpedition;
		getRandomCrewMemberNode._excludePreventedFromSelectionInEvents = _excludePreventedFromSelectionInEvents;
		getRandomCrewMemberNode._excludeDeadCharacters = _excludeDeadCharacters;
		getRandomCrewMemberNode._includeCaptain = _includeCaptain;
		return getRandomCrewMemberNode;
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
		if (_chosenCharacter == null)
		{
			ChooseCharacter(canvas);
		}
		switch (output)
		{
		case 1:
			return CastValue<T>(_chosenCharacter);
		case 2:
		{
			bool flag = _chosenCharacter != null;
			return CastValue<T>(flag);
		}
		default:
			throw new NodeOutputNotFoundException(output, "EE_getRandomCrewMemberNode");
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		ChooseCharacter(canvas);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	private void ChooseCharacter(NodeCanvas canvas)
	{
		UpdateInputValues(canvas);
		List<Character> list = CharacterManager.Instance.GetCharacterList().CharactersInGame;
		if (_excludeDeadCharacters)
		{
			list = (from x in list
				where x.RuntimeData.IsAlive()
				select (x)).ToList();
		}
		if (!_includeCaptain)
		{
			list = (from x in list
				where !x.RuntimeData.IsCaptain
				select (x)).ToList();
		}
		if (_excludePreventedFromSelectionInEvents)
		{
			list = (from x in list
				where !x.RuntimeData.HasStatusPreventingSelectionInEventChoices()
				select (x)).ToList();
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
		if (list.Count == 0)
		{
			_chosenCharacter = null;
		}
		else
		{
			_chosenCharacter = list[UnityEngine.Random.Range(0, list.Count)];
		}
	}

	private void UpdateInputValues(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _includeCaptain, canvas);
		GetInputValue(Inputs[2], ref _excludeDeadCharacters, canvas);
		GetInputValue(Inputs[3], ref _excludePreventedFromGoingOnExpedition, canvas);
		GetInputValue(Inputs[4], ref _excludePreventedFromSelectionInEvents, canvas);
		GetInputValue(Inputs[5], ref _excludePreventedFromDisplayingOnShip, canvas);
	}
}
