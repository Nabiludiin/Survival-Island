using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Flow Nodes/For Each Node", new Type[]
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
public class ForeachNode : FlowNode
{
	public const string ID = "PE_ForEachNode";

	private const int INPUT_LIST_INDEX = 1;

	private const int OUTPUT_LOOP_INDEX = 0;

	private const int OUTPUT_ITEM_INDEX = 1;

	private const int OUTPUT_END_INDEX = 2;

	private const string INPUT_LIST_NAME = "List";

	private const string OUTPUT_LOOP_NAME = "Loop";

	private const string OUTPUT_ITEM_NAME = "Element";

	private const string OUTPUT_END_NAME = "End Loop";

	private const string NODE_NAME = "For Each";

	private object _currentItem;

	public override string GetID => "PE_ForEachNode";

	public override Node Create(Vector2 pos)
	{
		ForeachNode foreachNode = ScriptableObject.CreateInstance<ForeachNode>();
		foreachNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		foreachNode.name = "For Each";
		foreachNode.CreateMutliInput("In", "Flow");
		foreachNode.CreateInput("List", ListConnection.ID);
		foreachNode.CreateOutput("Loop", "Flow");
		foreachNode.CreateOutput("Element", ObjectConnection.ID);
		foreachNode.CreateOutput("End Loop", "Flow");
		return foreachNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		IList currentValue = null;
		GetInputValue(Inputs[1], ref currentValue, canvas);
		for (int i = 0; i < currentValue.Count; i++)
		{
			_currentItem = currentValue[i];
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		Outputs[2].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 1)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_currentItem);
	}
}
