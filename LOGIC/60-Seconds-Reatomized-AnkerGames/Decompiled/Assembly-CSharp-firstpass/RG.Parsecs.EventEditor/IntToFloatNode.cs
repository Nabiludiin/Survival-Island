using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Casting/Int To Float Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class IntToFloatNode : CalculationNode
{
	public const string ID = "GE_IntToFloatNode";

	private const string INPUT_VALUE_NAME = "Value";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const string NODE_NAME = "Int To Float";

	private const int INPUT_VALUE_INDEX = 0;

	public override string GetID => "GE_IntToFloatNode";

	public override Node Create(Vector2 pos)
	{
		IntToFloatNode intToFloatNode = ScriptableObject.CreateInstance<IntToFloatNode>();
		intToFloatNode.rect = new Rect(pos.x, pos.y, 150f, 60f);
		intToFloatNode.name = "Int To Float";
		intToFloatNode.CreateInput("Value", "Int");
		intToFloatNode.CreateOutput("Result", "Float");
		return intToFloatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
		if (!Inputs[0].isConnected)
		{
			LogMessage("Input not connected. Result will be null.", EMessageType.WARNING);
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		int inputValue = GetInputValue<int>(Inputs[0], canvas);
		return CastValue<T>(Convert.ToSingle(inputValue));
	}
}
