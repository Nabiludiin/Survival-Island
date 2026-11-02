using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Get Character Result List Node", new Type[] { typeof(SurvivalEvent) })]
public class GetCharacterResultListNode : PlayerDecisionNode
{
	private const string ID = "EE_GetCharacterResultListNode";

	private const string INPUT_RESULT_NAME = "Result";

	private const string INPUT_OUTPUT_CHARACTER_1_NAME = "Character 1";

	private const string INPUT_OUTPUT_CHARACTER_2_NAME = "Character 2";

	private const string INPUT_OUTPUT_CHARACTER_3_NAME = "Character 3";

	private const string INPUT_OUTPUT_CHARACTER_4_NAME = "Character 4";

	private const string INPUT_CHARACTER_LIST_NAME = "CharacterList";

	private const string OUTPUT_CHARACTER_NAME = "Character";

	private const string NODE_NAME = "Get Character List Result";

	private const int INPUT_RESULT_INDEX = 1;

	private const int INPUT_CHARACTER_LIST_INDEX = 2;

	private const int OUTPUT_CHARACTER_1_INDEX = 0;

	private const int OUTPUT_CHARACTER_2_INDEX = 1;

	private const int OUTPUT_CHARACTER_3_INDEX = 2;

	private const int OUTPUT_CHARACTER_4_INDEX = 3;

	private const int OUTPUT_CHARACTER_INDEX = 4;

	[SerializeField]
	private PlayerDecision _currentDecision;

	public override string GetID => "EE_GetCharacterResultListNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterResultListNode getCharacterResultListNode = ScriptableObject.CreateInstance<GetCharacterResultListNode>();
		getCharacterResultListNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		getCharacterResultListNode.name = "Get Character List Result";
		getCharacterResultListNode.CreateMutliInput("In", "Flow");
		getCharacterResultListNode.CreateInput("Result", "PlayerDecision");
		getCharacterResultListNode.CreateInput("CharacterList", "CharacterList");
		getCharacterResultListNode.CreateOutput("Character 1", "Flow");
		getCharacterResultListNode.CreateOutput("Character 2", "Flow");
		getCharacterResultListNode.CreateOutput("Character 3", "Flow");
		getCharacterResultListNode.CreateOutput("Character 4", "Flow");
		getCharacterResultListNode.CreateOutput("Character", "Character");
		return getCharacterResultListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _currentDecision, canvas);
		if (!(_currentDecision is PlayerCharacterDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerCharacterDecision), _currentDecision.GetType());
		}
		Character result = ((PlayerCharacterDecision)_currentDecision).Result;
		List<Character> inputValue = GetInputValue<List<Character>>(Inputs[2], canvas);
		if (ParentEvent is SurvivalEvent)
		{
			((SurvivalEvent)ParentEvent).WasEventSuccessful = result != null;
		}
		if (result == inputValue[0])
		{
			if (!Outputs[0].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 0);
			}
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == inputValue[1])
		{
			if (!Outputs[1].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 1);
			}
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == inputValue[2])
		{
			if (!Outputs[2].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 2);
			}
			Outputs[2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == inputValue[3])
		{
			if (!Outputs[3].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 3);
			}
			Outputs[3].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		throw new WrongDecisionConnectionExcpetion(GetID);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 4)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (!(_currentDecision is PlayerCharacterDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerCharacterDecision), _currentDecision.GetType());
		}
		Character result = ((PlayerCharacterDecision)_currentDecision).Result;
		return CastValue<T>(result);
	}
}
