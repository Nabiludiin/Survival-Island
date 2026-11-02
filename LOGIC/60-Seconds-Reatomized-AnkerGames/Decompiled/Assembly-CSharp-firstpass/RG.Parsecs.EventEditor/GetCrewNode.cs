using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Crew Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetCrewNode : CharacterNode
{
	public const string Id = "getCrewNode";

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

	private const string CREW_OUTPUT_NAME = "Crew";

	private const string NODE_NAME = "Get Crew";

	[SerializeField]
	private bool _includeCaptain = true;

	[SerializeField]
	private bool _liveOnly = true;

	[SerializeField]
	private bool _excludePreventedFromGoingOnExpedition = true;

	[SerializeField]
	private bool _excludePreventedFromSelectionInEvents = true;

	[SerializeField]
	private bool _excludePreventedFromDisplayingOnShip = true;

	public override string GetID => "getCrewNode";

	public override Node Create(Vector2 pos)
	{
		GetCrewNode getCrewNode = ScriptableObject.CreateInstance<GetCrewNode>();
		getCrewNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		getCrewNode.name = "Get Crew";
		getCrewNode.CreateInput("Include captain", "Bool");
		getCrewNode.CreateInput("Exclude dead", "Bool");
		getCrewNode.CreateInput("Exclude non expeditable", "Bool");
		getCrewNode.CreateInput("Exclude non selectable", "Bool");
		getCrewNode.CreateInput("Exclude not displayed", "Bool");
		getCrewNode.CreateOutput("Crew", "CharacterList");
		return getCrewNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetCrewNode getCrewNode = (GetCrewNode)Create(rect.position + new Vector2(20f, 20f));
		getCrewNode._includeCaptain = _includeCaptain;
		getCrewNode._liveOnly = _liveOnly;
		getCrewNode._excludePreventedFromDisplayingOnShip = _excludePreventedFromDisplayingOnShip;
		getCrewNode._excludePreventedFromGoingOnExpedition = _excludePreventedFromGoingOnExpedition;
		getCrewNode._excludePreventedFromSelectionInEvents = _excludePreventedFromSelectionInEvents;
		return getCrewNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "getCrewNode");
		}
		UpdateInputValues(canvas);
		List<Character> list = new List<Character>();
		List<Character> list2 = new List<Character>();
		if (_liveOnly)
		{
			List<Character> liveCharacters = CharacterManager.Instance.GetCharacterList().GetLiveCharacters();
			for (int i = 0; i < liveCharacters.Count; i++)
			{
				list.Add(liveCharacters[i]);
			}
		}
		else
		{
			List<Character> charactersInGame = CharacterManager.Instance.GetCharacterList().CharactersInGame;
			for (int j = 0; j < charactersInGame.Count; j++)
			{
				list.Add(charactersInGame[j]);
			}
		}
		if (list.Count > 0)
		{
			if (!_includeCaptain && list[0].RuntimeData.IsCaptain)
			{
				list.RemoveAt(0);
			}
			if (_excludePreventedFromDisplayingOnShip)
			{
				list2.Clear();
				for (int k = 0; k < list.Count; k++)
				{
					if (list[k].RuntimeData.IsDrawnOnShip())
					{
						list2.Add(list[k]);
					}
				}
				list.Clear();
				for (int l = 0; l < list2.Count; l++)
				{
					list.Add(list2[l]);
				}
			}
			if (_excludePreventedFromGoingOnExpedition)
			{
				list2.Clear();
				for (int m = 0; m < list.Count; m++)
				{
					if (!list[m].RuntimeData.HasStatusPreventingGoingOnExpeditions())
					{
						list2.Add(list[m]);
					}
				}
				list.Clear();
				for (int n = 0; n < list2.Count; n++)
				{
					list.Add(list2[n]);
				}
			}
			if (_excludePreventedFromSelectionInEvents)
			{
				list2.Clear();
				for (int num = 0; num < list.Count; num++)
				{
					if (!list[num].RuntimeData.HasStatusPreventingSelectionInEventChoices())
					{
						list2.Add(list[num]);
					}
				}
				list.Clear();
				for (int num2 = 0; num2 < list2.Count; num2++)
				{
					list.Add(list2[num2]);
				}
			}
		}
		return CastValue<T>(list);
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
