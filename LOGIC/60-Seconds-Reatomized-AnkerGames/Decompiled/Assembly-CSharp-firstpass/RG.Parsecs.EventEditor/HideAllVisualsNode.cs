using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Hide All Visuals Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class HideAllVisualsNode : EventNode
{
	public const string ID = "EE_hideAllVisualsNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Hide all visuals";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	public override string GetID => "EE_hideAllVisualsNode";

	public override Node Create(Vector2 pos)
	{
		HideAllVisualsNode hideAllVisualsNode = ScriptableObject.CreateInstance<HideAllVisualsNode>();
		hideAllVisualsNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		hideAllVisualsNode.name = "Hide all visuals";
		hideAllVisualsNode.CreateMutliInput("In", "Flow");
		hideAllVisualsNode.CreateOutput("Out", "Flow");
		return hideAllVisualsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		VisualsManager.Instance.HideAllVisuals();
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
