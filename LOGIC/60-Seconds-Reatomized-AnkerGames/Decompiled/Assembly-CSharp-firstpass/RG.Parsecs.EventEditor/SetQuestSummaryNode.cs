using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "End Game Nodes/Set Quest Summary Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetQuestSummaryNode : EventNode
{
	public const string ID = "EE_setQuestSummaryNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Set quest summary";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	[SerializeField]
	private EndGameCanvas _endGameCanvas;

	public override string GetID => "EE_setQuestSummaryNode";

	public override Node Create(Vector2 pos)
	{
		SetQuestSummaryNode setQuestSummaryNode = ScriptableObject.CreateInstance<SetQuestSummaryNode>();
		setQuestSummaryNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		setQuestSummaryNode.name = "Set quest summary";
		setQuestSummaryNode.CreateMutliInput("In", "Flow");
		setQuestSummaryNode.CreateOutput("Out", "Flow");
		return setQuestSummaryNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetQuestSummaryNode setQuestSummaryNode = (SetQuestSummaryNode)Create(rect.position + new Vector2(20f, 20f));
		setQuestSummaryNode._endGameCanvas = _endGameCanvas;
		return setQuestSummaryNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
		if (!Inputs[0].isConnected)
		{
			LogMessage("No flow input connected to Set quest summary", EMessageType.WARNING);
		}
		if (!Outputs[0].isConnected)
		{
			LogMessage("Flow output not connected in Set quest summary", EMessageType.ERROR);
		}
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		EndGameData endGameData = EndGameManager.Instance.GetEndGameData();
		if (endGameData != null)
		{
			endGameData.RuntimeData.MainQuestSummary = _endGameCanvas;
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
