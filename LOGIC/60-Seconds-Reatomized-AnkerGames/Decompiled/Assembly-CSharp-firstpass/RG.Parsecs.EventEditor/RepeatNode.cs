using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Repeat Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent),
	typeof(ConditionNode)
})]
public class RepeatNode : FlowNode
{
	public const string ID = "EE_RepeatNode";

	private const string NODE_NAME = "Repeat";

	private new const string INPUT_IN_NAME = "In";

	private const string INPUT_REPEATS_AMOUNT_NAME = "Repeats Amount";

	private const string OUTPUT_LOOP_NAME = "Loop";

	private new const string OUTPUT_OUT_NAME = "Out";

	private new const int INPUT_IN_INDEX = 0;

	private const int INPUT_REPEAT_TIME_INDEX = 1;

	private const int OUTPUT_LOOP_INDEX = 0;

	private new const int OUTPUT_OUT_INDEX = 1;

	private const int MINIMAL_REPEAT_VALUE = 1;

	[SerializeField]
	private int _repeatsCounter = 2;

	public override string GetID => "EE_RepeatNode";

	public override Node Create(Vector2 pos)
	{
		RepeatNode repeatNode = ScriptableObject.CreateInstance<RepeatNode>();
		repeatNode.name = "Repeat";
		repeatNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		repeatNode.CreateMutliInput("In", "Flow");
		repeatNode.CreateInput("Repeats Amount", "Int");
		repeatNode.CreateOutput("Loop", "Flow");
		repeatNode.CreateOutput("Out", "Flow");
		return repeatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RepeatNode repeatNode = (RepeatNode)Create(rect.position + new Vector2(20f, 20f));
		repeatNode._repeatsCounter = _repeatsCounter;
		return repeatNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _repeatsCounter, canvas);
		_repeatsCounter = Mathf.Clamp(_repeatsCounter, 1, int.MaxValue);
		for (int i = 0; i < _repeatsCounter; i++)
		{
			Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
		}
		Outputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
