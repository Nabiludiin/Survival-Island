using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Get Attribute Result Node", new Type[] { typeof(SurvivalEvent) })]
public class GetAttributeResultNode : PlayerDecisionNode
{
	private const string ID = "EE_GetAttributeResultNode";

	private const string INPUT_RESULT_NAME = "Result";

	private const string NODE_NAME = "Get Attribute Result";

	private const string ATTRIBUTE_ONE_NAME = "Attribute 1";

	private const string ATTRIBUTE_TWO_NAME = "Attribute 2";

	private const string ATTRIBUTE_THREE_NAME = "Attribute 3";

	private const string ATTRIBUTE_FOUR_NAME = "Attribute 4";

	private const string INPUT_SUCCESS_MODIFIER_NAME = "Success Modifier";

	private const string OUTPUT_SUCCES_NAME = "Success";

	private const string OUTPUT_ATTRIBUTE_NAME = "Attribute";

	private const int OUTPUT_ATTRIBUTE_ONE_INDEX = 0;

	private const int OUTPUT_ATTRIBUTE_TWO_INDEX = 1;

	private const int OUTPUT_ATTRIBUTE_THREE_INDEX = 2;

	private const int OUTPUT_ATTRIBUTE_FOUR_INDEX = 3;

	private const int OUTPUT_ATTRIBUTE_INDEX = 4;

	private const int OUTPUT_SUCCES_INDEX = 5;

	private const int INPUT_RESULT_INDEX = 1;

	private const int INPUT_ATTRIBUTE_ONE_INDEX = 2;

	private const int INPUT_ATTRIBUTE_TWO_INDEX = 3;

	private const int INPUT_ATTRIBUTE_THREE_INDEX = 4;

	private const int INPUT_ATTRIBUTE_FOUR_INDEX = 5;

	private const int INPUT_SUCCESS_MODIFIER_INDEX = 6;

	private const string ENTRY_DEFAULT_MODE = "None";

	private const string ENTRY_TEAM_MODE = "Team";

	[SerializeField]
	private CharacterAttribute _attribute1;

	[SerializeField]
	private CharacterAttribute _attribute2;

	[SerializeField]
	private CharacterAttribute _attribute3;

	[SerializeField]
	private CharacterAttribute _attribute4;

	[SerializeField]
	private PlayerDecision _currentDecision;

	[SerializeField]
	private float _successModifier;

	[SerializeField]
	private bool _success;

	public override string GetID => "EE_GetAttributeResultNode";

	public override Node Create(Vector2 pos)
	{
		GetAttributeResultNode getAttributeResultNode = ScriptableObject.CreateInstance<GetAttributeResultNode>();
		getAttributeResultNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		getAttributeResultNode.name = "Get Attribute Result";
		getAttributeResultNode.CreateMutliInput("In", "Flow");
		getAttributeResultNode.CreateInput("Result", "PlayerDecision");
		getAttributeResultNode.CreateInput("Attribute 1", "Attributes");
		getAttributeResultNode.CreateInput("Attribute 2", "Attributes");
		getAttributeResultNode.CreateInput("Attribute 3", "Attributes");
		getAttributeResultNode.CreateInput("Attribute 4", "Attributes");
		getAttributeResultNode.CreateInput("Success Modifier", "Float");
		getAttributeResultNode.CreateOutput("Attribute 1", "Flow");
		getAttributeResultNode.CreateOutput("Attribute 2", "Flow");
		getAttributeResultNode.CreateOutput("Attribute 3", "Flow");
		getAttributeResultNode.CreateOutput("Attribute 4", "Flow");
		getAttributeResultNode.CreateOutput("Attribute", "Attributes");
		getAttributeResultNode.CreateOutput("Success", "Bool");
		return getAttributeResultNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
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
		if (!(_currentDecision is PlayerAttributeDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerAttributeDecision), _currentDecision.GetType());
		}
		CharacterAttribute result = ((PlayerAttributeDecision)_currentDecision).Result;
		GetInputValue(Inputs[2], ref _attribute1, canvas);
		GetInputValue(Inputs[3], ref _attribute2, canvas);
		GetInputValue(Inputs[4], ref _attribute3, canvas);
		GetInputValue(Inputs[5], ref _attribute4, canvas);
		_success = IsSuccessful(canvas);
		if (ParentEvent is SurvivalEvent)
		{
			((SurvivalEvent)ParentEvent).WasEventSuccessful = _success;
		}
		if (result == _attribute1)
		{
			if (!Outputs[0].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 0);
			}
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _attribute2)
		{
			if (!Outputs[1].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 1);
			}
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _attribute3)
		{
			if (!Outputs[2].isConnected)
			{
				throw new NotConnectedOutputException(GetID, 2);
			}
			Outputs[2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
			return;
		}
		if (result == _attribute4)
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

	public bool IsSuccessful(NodeCanvas canvas)
	{
		if (!(_currentDecision is PlayerAttributeDecision))
		{
			throw new WrongDecisionTypeException(typeof(PlayerAttributeDecision), _currentDecision.GetType());
		}
		TestStatsEntry testStatsEntry = new TestStatsEntry();
		testStatsEntry.IsCrewTest = false;
		PlayerAttributeDecision playerAttributeDecision = (PlayerAttributeDecision)_currentDecision;
		float num = 0f;
		if (playerAttributeDecision.IsTeam)
		{
			testStatsEntry.IsCrewTest = true;
			num = CharacterAttribute.CalculateTeamAttributeValue(playerAttributeDecision.Result.Id);
		}
		else if (playerAttributeDecision.Character != null)
		{
			_ = playerAttributeDecision.Character.name;
			testStatsEntry.IsCaptain = playerAttributeDecision.Character.RuntimeData.IsCaptain;
			testStatsEntry.TestedCharacterId = playerAttributeDecision.Character.ID;
			num = playerAttributeDecision.Character.RuntimeData.GetAttributeById(playerAttributeDecision.Result.Id).Value;
		}
		float num2 = UnityEngine.Random.Range(0f, 1f);
		GetInputValue(Inputs[6], ref _successModifier, canvas);
		num = Mathf.Clamp(num + _successModifier, 0f, 1f);
		if (string.IsNullOrEmpty(testStatsEntry.TestedCharacterId) && !testStatsEntry.IsCrewTest)
		{
			testStatsEntry.IsCaptain = true;
			testStatsEntry.TestedCharacterId = CharacterManager.Instance.GetCharacterList().GetCaptain().ID;
		}
		testStatsEntry.TestedAttributeId = playerAttributeDecision.Result.Id;
		testStatsEntry.Day = EndOfDayManager.Instance.AcutalDay - 1;
		if (num >= num2)
		{
			testStatsEntry.IsPassed = true;
			StatsManager.Instance.AddTestStatsEntry(testStatsEntry);
			return true;
		}
		testStatsEntry.IsPassed = false;
		StatsManager.Instance.AddTestStatsEntry(testStatsEntry);
		return false;
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		switch (output)
		{
		case 4:
		{
			if (!(_currentDecision is PlayerAttributeDecision))
			{
				throw new WrongDecisionTypeException(typeof(PlayerAttributeDecision), _currentDecision.GetType());
			}
			CharacterAttribute result = ((PlayerAttributeDecision)_currentDecision).Result;
			return CastValue<T>(result);
		}
		case 5:
			return CastValue<T>(_success);
		default:
			throw new NotExistingOutputException(GetID, output);
		}
	}
}
