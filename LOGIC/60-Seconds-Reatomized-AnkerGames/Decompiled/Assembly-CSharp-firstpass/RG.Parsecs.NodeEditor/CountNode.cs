using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Calculation/Count Node", new Type[]
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
public class CountNode : ParsecsNode
{
	public const string ID = "PE_CountNode";

	private const int OUTPUT_COUNT_INDEX = 0;

	private const int INPUT_LIST_INDEX = 0;

	private const string INPUT_LIST_NAME = "List";

	private const string OUTPUT_COUNT_NAME = "Count";

	private const string NODE_NAME = "Count";

	public override string GetID => "PE_CountNode";

	public override Node Create(Vector2 pos)
	{
		CountNode countNode = ScriptableObject.CreateInstance<CountNode>();
		countNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		countNode.name = "Count";
		countNode.CreateInput("List", ListConnection.ID);
		countNode.CreateOutput("Count", "Int");
		return countNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(GetInputValue<IList>(Inputs[0], canvas).Count);
	}
}
