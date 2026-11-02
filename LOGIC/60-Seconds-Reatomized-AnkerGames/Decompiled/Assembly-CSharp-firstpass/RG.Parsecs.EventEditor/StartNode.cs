using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Start Node", new Type[]
{
	typeof(Goal),
	typeof(EndGameCanvas),
	typeof(NodeFunction)
})]
public class StartNode : FlowNode
{
	public const string ID = "GE_StartNode";

	private const string NODE_NAME = "Start";

	public override string GetID => "GE_StartNode";

	public override Node Create(Vector2 pos)
	{
		StartNode startNode = ScriptableObject.CreateInstance<StartNode>();
		startNode.rect = new Rect(pos.x, pos.y, 150f, 50f);
		startNode.name = "Start";
		startNode.CreateOutput("Out", "Flow");
		return startNode;
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}
}
