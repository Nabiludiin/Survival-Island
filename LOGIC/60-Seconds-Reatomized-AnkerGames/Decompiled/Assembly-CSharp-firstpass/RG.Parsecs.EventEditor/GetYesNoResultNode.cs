using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Get Yes-No Result Node", new Type[] { typeof(SurvivalEvent) })]
public class GetYesNoResultNode : PlayerDecisionNode
{
	private const string ID = "EE_GetYesNoResultNode";

	private const string INPUT_RESULT_NAME = "Result";

	private const string OUTPUT_YES_NAME = "Yes";

	private const string OUTPUT_NO_NAME = "No";

	private const string OUTPUT_CHOICE_NAME = "Choice";

	private const string NODE_NAME = "Get Yes/No Result";

	private const int INPUT_RESULT_INDEX = 1;

	private const int OUTPUT_YES_INDEX = 0;

	private const int OUTPUT_NO_INDEX = 1;

	private const int OUTPUT_CHOICE_INDEX = 2;

	[SerializeField]
	private PlayerDecision _currentDecision;

	public override string GetID => "EE_GetYesNoResultNode";

	public override Node Create(Vector2 pos)
	{
		GetYesNoResultNode getYesNoResultNode = ScriptableObject.CreateInstance<GetYesNoResultNode>();
		getYesNoResultNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		getYesNoResultNode.name = "Get Yes/No Result";
		getYesNoResultNode.CreateMutliInput("In", "Flow");
		getYesNoResultNode.CreateInput("Result", "PlayerDecision");
		getYesNoResultNode.CreateOutput("Yes", "Flow");
		getYesNoResultNode.CreateOutput("No", "Flow");
		getYesNoResultNode.CreateOutput("Choice", "Bool");
		return getYesNoResultNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
		if (!Inputs[1].isConnected)
		{
			LogMessage($"Result input is not connected: {GetID}", EMessageType.ERROR);
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _currentDecision, canvas);
		if (!(_currentDecision is PlayerYesNoDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerYesNoDecision), _currentDecision.GetType());
		}
		PlayerYesNoDecision playerYesNoDecision = (PlayerYesNoDecision)_currentDecision;
		if (ParentEvent is SurvivalEvent)
		{
			((SurvivalEvent)ParentEvent).WasEventSuccessful = playerYesNoDecision.Result;
		}
		CheckAreAllFlowOutputsConnected();
		YesNoDecisionStatsEntry yesNoDecisionStatsEntry = new YesNoDecisionStatsEntry();
		yesNoDecisionStatsEntry.YesDecision = playerYesNoDecision.Result;
		if (playerYesNoDecision.Result)
		{
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		else
		{
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		StatsManager.Instance.AddYesNoDecisionStatsEntry(yesNoDecisionStatsEntry);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 2)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (!(_currentDecision is PlayerYesNoDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerYesNoDecision), _currentDecision.GetType());
		}
		bool result = ((PlayerYesNoDecision)_currentDecision).Result;
		return CastValue<T>(result);
	}
}
