using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Debug Nodes/Pause Editor Execution Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ReportEvent)
})]
public class PauseEditorExecutionNode : ParsecsNode
{
	public const string ID = "EE_PauseEditorExecutionNode";

	public override string GetID => "EE_PauseEditorExecutionNode";

	public override Node Create(Vector2 pos)
	{
		PauseEditorExecutionNode pauseEditorExecutionNode = ScriptableObject.CreateInstance<PauseEditorExecutionNode>();
		pauseEditorExecutionNode.rect = new Rect(pos.x, pos.y, 300f, 105f);
		pauseEditorExecutionNode.name = "Pause Editor Execution";
		pauseEditorExecutionNode.CreateMutliInput("In", "Flow");
		pauseEditorExecutionNode.CreateOutput("Out", "Flow");
		return pauseEditorExecutionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (PauseEditorExecutionNode)Create(rect.position + new Vector2(20f, 20f));
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
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
