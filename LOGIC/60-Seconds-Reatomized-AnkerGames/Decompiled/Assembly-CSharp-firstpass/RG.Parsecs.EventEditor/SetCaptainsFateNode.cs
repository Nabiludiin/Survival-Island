using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "End Game Nodes/Set Captains Fate Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetCaptainsFateNode : EventNode
{
	public const string ID = "EE_setCaptainsFateNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Set captains fate";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	[SerializeField]
	private EndGameCanvas _endGameCanvas;

	public override string GetID => "EE_setCaptainsFateNode";

	public override Node Create(Vector2 pos)
	{
		SetCaptainsFateNode setCaptainsFateNode = ScriptableObject.CreateInstance<SetCaptainsFateNode>();
		setCaptainsFateNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		setCaptainsFateNode.name = "Set captains fate";
		setCaptainsFateNode.CreateMutliInput("In", "Flow");
		setCaptainsFateNode.CreateOutput("Out", "Flow");
		return setCaptainsFateNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetCaptainsFateNode setCaptainsFateNode = (SetCaptainsFateNode)Create(rect.position + new Vector2(20f, 20f));
		setCaptainsFateNode._endGameCanvas = _endGameCanvas;
		return setCaptainsFateNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
		if (!Inputs[0].isConnected)
		{
			LogMessage("No flow input connected to Set captains fate", EMessageType.WARNING);
		}
		if (!Outputs[0].isConnected)
		{
			LogMessage("Flow output not connected in Set captains fate", EMessageType.ERROR);
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
			endGameData.RuntimeData.CaptainsFate = _endGameCanvas;
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
