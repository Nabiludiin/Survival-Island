using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "End Game Nodes/Set Session Summary Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetSessionSummaryNode : EventNode
{
	public const string ID = "EE_setSessionSummaryNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Set session summary";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	[SerializeField]
	private EndGameCanvas _endGameCanvas;

	public override string GetID => "EE_setSessionSummaryNode";

	public override Node Create(Vector2 pos)
	{
		SetSessionSummaryNode setSessionSummaryNode = ScriptableObject.CreateInstance<SetSessionSummaryNode>();
		setSessionSummaryNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		setSessionSummaryNode.name = "Set session summary";
		setSessionSummaryNode.CreateMutliInput("In", "Flow");
		setSessionSummaryNode.CreateOutput("Out", "Flow");
		return setSessionSummaryNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetSessionSummaryNode setSessionSummaryNode = (SetSessionSummaryNode)Create(rect.position + new Vector2(20f, 20f));
		setSessionSummaryNode._endGameCanvas = _endGameCanvas;
		return setSessionSummaryNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
		if (!Inputs[0].isConnected)
		{
			LogMessage("No flow input connected to Set session summary", EMessageType.WARNING);
		}
		if (!Outputs[0].isConnected)
		{
			LogMessage("Flow output not connected in Set session summary", EMessageType.ERROR);
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
			Debug.Log("Setting session summary");
			endGameData.RuntimeData.SessionSummary = _endGameCanvas;
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
