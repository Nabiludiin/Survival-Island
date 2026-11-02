using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/List Difference Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas)
})]
public class ListDifferenceNode : ParsecsNode
{
	public const string ID = "PE_ListDifferenceNode";

	private const string NODE_NAME = "List Difference";

	private const string LIST1_INPUT_NAME = "List 1";

	private const int LIST1_INPUT_INDEX = 0;

	private const string LIST2_INPUT_NAME = "List 2";

	private const int LIST2_INPUT_INDEX = 1;

	private const string OUTPUT_NAME = "List";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "PE_ListDifferenceNode";

	public override Node Create(Vector2 pos)
	{
		ListDifferenceNode listDifferenceNode = ScriptableObject.CreateInstance<ListDifferenceNode>();
		listDifferenceNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		listDifferenceNode.name = "List Difference";
		listDifferenceNode.CreateInput("List 1", ListConnection.ID);
		listDifferenceNode.CreateInput("List 2", ListConnection.ID);
		listDifferenceNode.CreateOutput("List", ListConnection.ID);
		return listDifferenceNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		IList inputValue = GetInputValue<IList>(Inputs[0], canvas);
		IList inputValue2 = GetInputValue<IList>(Inputs[1], canvas);
		List<object> list = new List<object>();
		for (int i = 0; i < inputValue.Count; i++)
		{
			if (!inputValue2.Contains(inputValue[i]))
			{
				list.Add(inputValue[i]);
			}
		}
		return CastValue<T>(list);
	}
}
