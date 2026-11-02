using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Test Attribute Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent),
	typeof(ConditionEvent)
})]
public class TestAttributeNode : FlowNode
{
	private const string ID = "EE_TestAttributeNode";

	private const string NODE_NAME = "Test Attribute";

	private const string INPUT_ATTRIBUTE_NAME = "Attribute";

	private const string INPUT_SUCCESS_MODIFIER_NAME = "Success Modifier";

	private const string INPUT_CHARACTER_NAME = "Character";

	private const string INPUT_IS_TEAM_NAME = "Is Team";

	private const string OUTPUT_SUCCES_NAME = "Success";

	private const string OUTPUT_FAILURE_NAME = "Failure";

	private const string OUTPUT_ATTRIBUTE_NAME = "Attribute";

	private const int OUTPUT_SUCCES_INDEX = 0;

	private const int OUTPUT_FAILURE_INDEX = 1;

	private const int OUTPUT_ATTRIBUTE_INDEX = 2;

	private const int INPUT_ATTRIBUTE_INDEX = 1;

	private const int INPUT_SUCCESS_MODIFIER_INDEX = 2;

	private const int INPUT_CHARACTER_INDEX = 3;

	private const int INPUT_IS_TEAM_INDEX = 4;

	[SerializeField]
	private CharacterAttribute _attribute;

	[SerializeField]
	private float _successModifier;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private bool _isTeam;

	public override string GetID => "EE_TestAttributeNode";

	public override Node Create(Vector2 pos)
	{
		TestAttributeNode testAttributeNode = ScriptableObject.CreateInstance<TestAttributeNode>();
		testAttributeNode.rect = new Rect(pos.x, pos.y, 200f, 160f);
		testAttributeNode.name = "Test Attribute";
		testAttributeNode.CreateMutliInput("In", "Flow");
		testAttributeNode.CreateInput("Attribute", "Attributes");
		testAttributeNode.CreateInput("Success Modifier", "Float");
		testAttributeNode.CreateInput("Character", "Character");
		testAttributeNode.CreateInput("Is Team", "Bool");
		testAttributeNode.CreateOutput("Success", "Flow");
		testAttributeNode.CreateOutput("Failure", "Flow");
		testAttributeNode.CreateOutput("Attribute", "Attributes");
		return testAttributeNode;
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
		if (_attribute == null && !Inputs[1].isConnected)
		{
			LogMessage($"Attribute value is null or not connected: {GetID}", EMessageType.ERROR);
		}
		if (!_isTeam && _character == null && !Inputs[3].isConnected && _successModifier <= 0f)
		{
			LogMessage($"This will always be FAILURE:, {GetID}", EMessageType.WARNING);
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[3], ref _character, canvas);
		GetInputValue(Inputs[4], ref _isTeam, canvas);
		GetInputValue(Inputs[1], ref _attribute, canvas);
		float num = 0f;
		string text = "None";
		if (_isTeam)
		{
			text = "Team";
			num = CharacterAttribute.CalculateTeamAttributeValue(_attribute.Id);
		}
		else if (_character != null)
		{
			text = _character.name;
			num = _character.RuntimeData.GetAttributeById(_attribute.Id).Value;
		}
		float num2 = UnityEngine.Random.Range(0f, 1f);
		GetInputValue(Inputs[2], ref _successModifier, canvas);
		float num3 = num;
		num = Mathf.Clamp(num + _successModifier, 0f, 1f);
		Debug.Log($"Mode: {text} | Original Value: {num3} | Modifier: {_successModifier} | Value: {num} | Random: {num2} | Result: {num >= num2}");
		if (num >= num2)
		{
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		else
		{
			Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		GetInputValue(Inputs[3], ref _character, canvas);
		GetInputValue(Inputs[4], ref _isTeam, canvas);
		GetInputValue(Inputs[1], ref _attribute, canvas);
		if (output == 2)
		{
			return CastValue<T>(_attribute);
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
