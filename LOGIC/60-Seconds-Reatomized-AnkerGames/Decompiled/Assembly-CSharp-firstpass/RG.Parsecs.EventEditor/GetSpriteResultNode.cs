using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Get Sprite Result Node", new Type[] { typeof(SurvivalEvent) })]
public class GetSpriteResultNode : PlayerDecisionNode
{
	private const string ID = "EE_GetSpriteResultNode";

	private const string INPUT_RESULT_NAME = "Result";

	private const string INPUT_OUTPUT_CHOICE_1_NAME = "Choice 1";

	private const string INPUT_OUTPUT_CHOICE_2_NAME = "Choice 2";

	private const string INPUT_OUTPUT_CHOICE_3_NAME = "Choice 3";

	private const string INPUT_OUTPUT_CHOICE_4_NAME = "Choice 4";

	private const string OUTPUT_NO_CHOICE_NAME = "No Choice";

	private const string OUTPUT_RES_NAME = "Choice";

	private const string NODE_NAME = "Get Sprite Result";

	private const int INPUT_RESULT_INDEX = 1;

	private const int OUTPUT_CHOICE_1_INDEX = 0;

	private const int OUTPUT_CHOICE_2_INDEX = 1;

	private const int OUTPUT_CHOICE_3_INDEX = 2;

	private const int OUTPUT_CHOICE_4_INDEX = 3;

	private const int OUTPUT_NO_CHOICE_INDEX = 4;

	private const int OUTPUT_RES_INDEX = 5;

	private const int CHOICE_1_CARD_ID = 0;

	private const int CHOICE_2_CARD_ID = 1;

	private const int CHOICE_3_CARD_ID = 2;

	private const int CHOICE_4_CARD_ID = 3;

	private const int NO_CHOICE_ID = -1;

	[SerializeField]
	private PlayerDecision _currentDecision;

	public override string GetID => "EE_GetSpriteResultNode";

	public override Node Create(Vector2 pos)
	{
		GetSpriteResultNode getSpriteResultNode = ScriptableObject.CreateInstance<GetSpriteResultNode>();
		getSpriteResultNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		getSpriteResultNode.name = "Get Sprite Result";
		getSpriteResultNode.CreateMutliInput("In", "Flow");
		getSpriteResultNode.CreateInput("Result", "PlayerDecision");
		getSpriteResultNode.CreateOutput("Choice 1", "Flow");
		getSpriteResultNode.CreateOutput("Choice 2", "Flow");
		getSpriteResultNode.CreateOutput("Choice 3", "Flow");
		getSpriteResultNode.CreateOutput("Choice 4", "Flow");
		getSpriteResultNode.CreateOutput("No Choice", "Flow");
		getSpriteResultNode.CreateOutput("Choice", "Int");
		return getSpriteResultNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _currentDecision, canvas);
		if (!(_currentDecision is PlayerSpriteDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerSpriteDecision), _currentDecision.GetType());
		}
		PlayerSpriteDecision playerSpriteDecision = (PlayerSpriteDecision)_currentDecision;
		if (ParentEvent is SurvivalEvent)
		{
			((SurvivalEvent)ParentEvent).WasEventSuccessful = playerSpriteDecision.ChoosenNumber != -1;
		}
		if (playerSpriteDecision.ChoosenNumber == 0)
		{
			if (!Outputs[0].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 0);
			}
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (playerSpriteDecision.ChoosenNumber == 1)
		{
			if (!Outputs[1].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 1);
			}
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (playerSpriteDecision.ChoosenNumber == 2)
		{
			if (!Outputs[2].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 2);
			}
			Outputs[2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (playerSpriteDecision.ChoosenNumber == 3)
		{
			if (!Outputs[3].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 3);
			}
			Outputs[3].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (playerSpriteDecision.ChoosenNumber == -1)
		{
			if (!Outputs[4].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 4);
			}
			Outputs[4].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		throw new WrongDecisionConnectionExcpetion(GetID);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 5)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (!(_currentDecision is PlayerSpriteDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerSpriteDecision), _currentDecision.GetType());
		}
		int choosenNumber = ((PlayerSpriteDecision)_currentDecision).ChoosenNumber;
		return CastValue<T>(choosenNumber);
	}
}
