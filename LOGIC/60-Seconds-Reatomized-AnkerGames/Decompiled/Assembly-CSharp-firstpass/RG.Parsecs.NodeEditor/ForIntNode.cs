using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Flow Nodes/For Int Node", new Type[]
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
public class ForIntNode : FlowNode
{
	public const string ID = "PE_ForIntNode";

	private const int INPUT_START_INDEX_INDEX = 1;

	private const int INPUT_END_INDEX_INDEX = 2;

	private const int OUTPUT_LOOP_INDEX = 0;

	private const int OUTPUT_CURRENT_INDEX_INDEX = 1;

	private const int OUTPUT_END_INDEX = 2;

	private const string INPUT_START_INDEX_NAME = "Start Index";

	private const string INPUT_END_INDEX_NAME = "End Index";

	private const string OUTPUT_LOOP_NAME = "Loop";

	private const string OUTPUT_CURRENT_INDEX_NAME = "Current Index";

	private const string OUTPUT_END_NAME = "End Loop";

	private const string NODE_NAME = "For Int";

	private int _currentIndex;

	public override string GetID => "PE_ForIntNode";

	public override Node Create(Vector2 pos)
	{
		ForIntNode forIntNode = ScriptableObject.CreateInstance<ForIntNode>();
		forIntNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		forIntNode.name = "For Int";
		forIntNode.CreateMutliInput("In", "Flow");
		forIntNode.CreateInput("Start Index", "Int");
		forIntNode.CreateInput("End Index", "Int");
		forIntNode.CreateOutput("Loop", "Flow");
		forIntNode.CreateOutput("Current Index", "Int");
		forIntNode.CreateOutput("End Loop", "Flow");
		return forIntNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		int inputValue = GetInputValue<int>(Inputs[1], canvas);
		int inputValue2 = GetInputValue<int>(Inputs[2], canvas);
		for (int i = inputValue; i < inputValue2; i++)
		{
			_currentIndex = i;
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
		return CastValue<T>(_currentIndex);
	}
}
