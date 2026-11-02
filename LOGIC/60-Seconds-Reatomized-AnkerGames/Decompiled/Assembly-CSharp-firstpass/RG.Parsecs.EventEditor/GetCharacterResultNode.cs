using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Get Character Result Node", new Type[] { typeof(SurvivalEvent) })]
public class GetCharacterResultNode : PlayerDecisionNode
{
	private const string ID = "EE_GetCharacterResultNode";

	private const string INPUT_RESULT_NAME = "Result";

	private const string INPUT_OUTPUT_CHARACTER_1_NAME = "Character 1";

	private const string INPUT_OUTPUT_CHARACTER_2_NAME = "Character 2";

	private const string INPUT_OUTPUT_CHARACTER_3_NAME = "Character 3";

	private const string INPUT_OUTPUT_CHARACTER_4_NAME = "Character 4";

	private const string OUTPUT_CHARACTER_NAME = "Character";

	private const string NODE_NAME = "Get Character Result";

	private const int INPUT_RESULT_INDEX = 1;

	private const int INPUT_CHARACTER_1_INDEX = 2;

	private const int INPUT_CHARACTER_2_INDEX = 3;

	private const int INPUT_CHARACTER_3_INDEX = 4;

	private const int INPUT_CHARACTER_4_INDEX = 5;

	private const int OUTPUT_CHARACTER_1_INDEX = 0;

	private const int OUTPUT_CHARACTER_2_INDEX = 1;

	private const int OUTPUT_CHARACTER_3_INDEX = 2;

	private const int OUTPUT_CHARACTER_4_INDEX = 3;

	private const int OUTPUT_CHARACTER_INDEX = 4;

	[SerializeField]
	private PlayerDecision _currentDecision;

	[SerializeField]
	private Character _character1;

	[SerializeField]
	private Character _character2;

	[SerializeField]
	private Character _character3;

	[SerializeField]
	private Character _character4;

	public override string GetID => "EE_GetCharacterResultNode";

	public override Node Create(Vector2 pos)
	{
		GetCharacterResultNode getCharacterResultNode = ScriptableObject.CreateInstance<GetCharacterResultNode>();
		getCharacterResultNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		getCharacterResultNode.name = "Get Character Result";
		getCharacterResultNode.CreateMutliInput("In", "Flow");
		getCharacterResultNode.CreateInput("Result", "PlayerDecision");
		getCharacterResultNode.CreateInput("Character 1", "Character");
		getCharacterResultNode.CreateInput("Character 2", "Character");
		getCharacterResultNode.CreateInput("Character 3", "Character");
		getCharacterResultNode.CreateInput("Character 4", "Character");
		getCharacterResultNode.CreateOutput("Character 1", "Flow");
		getCharacterResultNode.CreateOutput("Character 2", "Flow");
		getCharacterResultNode.CreateOutput("Character 3", "Flow");
		getCharacterResultNode.CreateOutput("Character 4", "Flow");
		getCharacterResultNode.CreateOutput("Character", "Character");
		return getCharacterResultNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeEnable()
	{
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
		GetInputValue(Inputs[2], ref _character1, canvas);
		GetInputValue(Inputs[3], ref _character2, canvas);
		GetInputValue(Inputs[4], ref _character3, canvas);
		GetInputValue(Inputs[5], ref _character4, canvas);
		if (ParentEvent is SurvivalEvent)
		{
			((SurvivalEvent)ParentEvent).WasEventSuccessful = result != null;
		}
		if (result == _character1)
		{
			if (!Outputs[0].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 0);
			}
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _character2)
		{
			if (!Outputs[1].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 1);
			}
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _character3)
		{
			if (!Outputs[2].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 2);
			}
			Outputs[2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _character4)
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
