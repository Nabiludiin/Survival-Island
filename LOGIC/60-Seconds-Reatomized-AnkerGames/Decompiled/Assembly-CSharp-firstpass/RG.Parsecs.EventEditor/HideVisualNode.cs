using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Hide Visual Node", new Type[]
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
public class HideVisualNode : EventNode
{
	public const string ID = "EE_hideVisualNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const int VISITOR_INPUT_INDEX = 1;

	private const string NODE_NAME = "Hide visual";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string VISITOR_INPUT_NAME = "Visual";

	[SerializeField]
	private VisualsData _visuals;

	public override string GetID => "EE_hideVisualNode";

	public override Node Create(Vector2 pos)
	{
		HideVisualNode hideVisualNode = ScriptableObject.CreateInstance<HideVisualNode>();
		hideVisualNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		hideVisualNode.name = "Hide visual";
		hideVisualNode.CreateMutliInput("In", "Flow");
		hideVisualNode.CreateInput("Visual", "VisualsData");
		hideVisualNode.CreateOutput("Out", "Flow");
		return hideVisualNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		HideVisualNode hideVisualNode = (HideVisualNode)Create(rect.position + new Vector2(20f, 20f));
		hideVisualNode._visuals = _visuals;
		return hideVisualNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _visuals, canvas);
		_visuals.RuntimeData.VisualToDisplay = null;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
