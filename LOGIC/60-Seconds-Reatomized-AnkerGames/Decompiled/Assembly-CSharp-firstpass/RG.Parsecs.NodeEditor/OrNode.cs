using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Condition Nodes/Or Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class OrNode : ConditionNode
{
	public const string ID = "PE_OrNode";

	private readonly char[] _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

	[SerializeField]
	private List<bool> _values;

	private const int OUTPUT_RESULT_INDEX = 0;

	private const string RESULT_OUTPUT_NAME = "Result";

	public override string GetID => "PE_OrNode";

	public override Node Create(Vector2 pos)
	{
		OrNode orNode = ScriptableObject.CreateInstance<OrNode>();
		orNode.rect = new Rect(pos.x, pos.y, 100f, 85f);
		orNode.name = "Or";
		orNode.CreateInput("A", "Bool");
		orNode.CreateInput("B", "Bool");
		orNode.CreateOutput("Result", "Bool");
		orNode._values = new List<bool> { false, false };
		return orNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		OrNode orNode = (OrNode)Create(rect.position + new Vector2(20f, 20f));
		orNode._values = new List<bool>(_values);
		for (int i = 2; i < _values.Count; i++)
		{
			orNode.CreateInput(_alphabet[i].ToString(), "Bool");
		}
		orNode.rect = new Rect(rect.x, rect.y, 100f, 45 + 20 * Inputs.Count);
		return orNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateInputsCount()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		bool flag = Calculate(canvas);
		return CastValue<T>(flag);
	}

	private bool Calculate(NodeCanvas canvas)
	{
		for (int i = 0; i < Inputs.Count - 1; i++)
		{
			if (GetInputValue(Inputs[i], _values[i], canvas))
			{
				return true;
			}
		}
		return false;
	}
}
