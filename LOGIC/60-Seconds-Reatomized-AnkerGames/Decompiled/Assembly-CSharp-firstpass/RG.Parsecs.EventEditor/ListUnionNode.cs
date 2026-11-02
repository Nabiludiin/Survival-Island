using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/List Union Node", new Type[]
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
public class ListUnionNode : ParsecsNode
{
	public const string ID = "PE_ListUnionNode";

	private const string NODE_NAME = "List Union";

	private const string LIST1_INPUT_NAME = "List 1";

	private const int LIST1_INPUT_INDEX = 0;

	private const string LIST2_INPUT_NAME = "List 2";

	private const int LIST2_INPUT_INDEX = 1;

	private const string OUTPUT_NAME = "List";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "PE_ListUnionNode";

	public override Node Create(Vector2 pos)
	{
		ListUnionNode listUnionNode = ScriptableObject.CreateInstance<ListUnionNode>();
		listUnionNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		listUnionNode.name = "List Union";
		listUnionNode.CreateInput("List 1", ListConnection.ID);
		listUnionNode.CreateInput("List 2", ListConnection.ID);
		listUnionNode.CreateOutput("List", ListConnection.ID);
		return listUnionNode;
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
			list.Add(inputValue[i]);
		}
		for (int j = 0; j < inputValue2.Count; j++)
		{
			if (!list.Contains(inputValue2[j]))
			{
				list.Add(inputValue2[j]);
			}
		}
		return CastValue<T>(list);
	}
}
