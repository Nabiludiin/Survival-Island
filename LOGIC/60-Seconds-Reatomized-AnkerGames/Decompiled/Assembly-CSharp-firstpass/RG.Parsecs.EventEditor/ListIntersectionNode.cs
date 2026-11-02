using System;
using System.Collections;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/List Intersection Node", new Type[]
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
public class ListIntersectionNode : ParsecsNode
{
	public const string ID = "PE_ListIntersectionNode";

	private const string NODE_NAME = "List Intersection";

	private const string LIST1_INPUT_NAME = "List 1";

	private const int LIST1_INPUT_INDEX = 0;

	private const string LIST2_INPUT_NAME = "List 2";

	private const int LIST2_INPUT_INDEX = 1;

	private const string OUTPUT_NAME = "List";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "PE_ListIntersectionNode";

	public override Node Create(Vector2 pos)
	{
		ListIntersectionNode listIntersectionNode = ScriptableObject.CreateInstance<ListIntersectionNode>();
		listIntersectionNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		listIntersectionNode.name = "List Intersection";
		listIntersectionNode.CreateInput("List 1", ListConnection.ID);
		listIntersectionNode.CreateInput("List 2", ListConnection.ID);
		listIntersectionNode.CreateOutput("List", ListConnection.ID);
		return listIntersectionNode;
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
			if (inputValue2.Contains(inputValue[i]))
			{
				list.Add(inputValue[i]);
			}
		}
		return CastValue<T>(list);
	}
}
