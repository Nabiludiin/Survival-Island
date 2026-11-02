using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Execute If List Empty Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class ExecuteIfListEmpty : FlowNode
{
	public const string ID = "GE_ExecuteIfListEmptyNode";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_CONDITION_INDEX = 1;

	private const int OUTPUT_TRUE_INDEX = 0;

	private const int OUTPUT_FALSE_INDEX = 1;

	private const int OUTPUT_RESULT_INDEX = 2;

	public override string GetID => "GE_ExecuteIfListEmptyNode";

	public override Node Create(Vector2 pos)
	{
		ExecuteIfListEmpty executeIfListEmpty = ScriptableObject.CreateInstance<ExecuteIfListEmpty>();
		executeIfListEmpty.rect = new Rect(pos.x, pos.y, 150f, 125f);
		executeIfListEmpty.name = "Execute If List Empty";
		executeIfListEmpty.CreateMutliInput("In", "Flow");
		executeIfListEmpty.CreateInput("List", ListConnection.ID);
		executeIfListEmpty.CreateOutput("Out Empty", "Flow");
		executeIfListEmpty.CreateOutput("Out Not Empty", "Flow");
		executeIfListEmpty.CreateOutput("Is Empty Bool", "Bool");
		return executeIfListEmpty;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (ExecuteIfListEmpty)Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		IList inputValue = GetInputValue<IList>(Inputs[1], canvas);
		if (inputValue == null || inputValue.Count == 0)
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
		if (output != 2)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		IList inputValue = GetInputValue<IList>(Inputs[1], canvas);
		return CastValue<T>(inputValue == null || inputValue.Count == 0);
	}
}
