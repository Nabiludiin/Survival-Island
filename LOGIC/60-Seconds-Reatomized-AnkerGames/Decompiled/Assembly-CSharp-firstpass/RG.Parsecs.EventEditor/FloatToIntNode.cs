using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Casting/Float To Int Node", new Type[]
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
public class FloatToIntNode : CalculationNode
{
	public const string ID = "EE_FloatToIntNode";

	private const string INPUT_VALUE_NAME = "Value";

	private const string OUTPUT_RESULT_NAME = "Result";

	private const string NODE_NAME = "Float To Int";

	private const int INPUT_VALUE_INDEX = 0;

	public override string GetID => "EE_FloatToIntNode";

	public override Node Create(Vector2 pos)
	{
		FloatToIntNode floatToIntNode = ScriptableObject.CreateInstance<FloatToIntNode>();
		floatToIntNode.rect = new Rect(pos.x, pos.y, 150f, 60f);
		floatToIntNode.name = "Float To Int";
		floatToIntNode.CreateInput("Value", "Float");
		floatToIntNode.CreateOutput("Result", "Int");
		return floatToIntNode;
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
		float inputValue = GetInputValue<float>(Inputs[0], canvas);
		return CastValue<T>(Convert.ToInt32(inputValue));
	}
}
