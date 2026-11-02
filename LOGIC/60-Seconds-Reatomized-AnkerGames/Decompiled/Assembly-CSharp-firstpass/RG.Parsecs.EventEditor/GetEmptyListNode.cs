using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Calculation/Get Empty List Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetEmptyListNode : ParsecsNode
{
	public const string ID = "PE_GetEmptyListNode";

	private const string NODE_NAME = "Get empty list";

	private const string OUTPUT_NAME = "List";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "PE_GetEmptyListNode";

	public override Node Create(Vector2 pos)
	{
		GetEmptyListNode getEmptyListNode = ScriptableObject.CreateInstance<GetEmptyListNode>();
		getEmptyListNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		getEmptyListNode.name = "Get empty list";
		getEmptyListNode.CreateOutput("List", ListConnection.ID);
		return getEmptyListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(new List<object>());
	}
}
