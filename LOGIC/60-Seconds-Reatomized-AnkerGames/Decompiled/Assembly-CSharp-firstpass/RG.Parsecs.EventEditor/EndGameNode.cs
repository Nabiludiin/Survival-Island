using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "End Game Nodes/End Game Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class EndGameNode : EventNode
{
	public const string ID = "EE_EndGameNode";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string RESULT_INPUT_NAME = "Is success";

	private const int FLOW_INPUT_INDEX = 0;

	private const int RESULT_INPUT_INDEX = 1;

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "End Game";

	[SerializeField]
	private bool _result;

	public override string GetID => "EE_EndGameNode";

	public override Node Create(Vector2 pos)
	{
		EndGameNode endGameNode = ScriptableObject.CreateInstance<EndGameNode>();
		endGameNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		endGameNode.name = "End Game";
		endGameNode.CreateMutliInput("In", "Flow");
		endGameNode.CreateInput("Is success", "Bool");
		endGameNode.CreateOutput("Out", "Flow");
		return endGameNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		EndGameNode endGameNode = (EndGameNode)Create(rect.position + new Vector2(20f, 20f));
		endGameNode._result = _result;
		return endGameNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _result, canvas);
		EndGameManager.Instance.EndGame(_result);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
