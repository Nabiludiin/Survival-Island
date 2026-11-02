using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

[Node(false, "Expedition Nodes/On Evaluate Character Return Node", new Type[] { typeof(ExpeditionEvent) })]
public class OnEvaluateCharacterReturnNode : FlowNode
{
	public const string ID = "EE_OnEvaluateCharacterReturnNode";

	public const string NODE_NAME = "On Evaluate Character Return";

	public override string GetID => "EE_OnEvaluateCharacterReturnNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		OnEvaluateCharacterReturnNode onEvaluateCharacterReturnNode = ScriptableObject.CreateInstance<OnEvaluateCharacterReturnNode>();
		onEvaluateCharacterReturnNode.rect = new Rect(pos.x, pos.y, 200f, 30f);
		onEvaluateCharacterReturnNode.name = "On Evaluate Character Return";
		onEvaluateCharacterReturnNode.CreateOutput("Out", "Flow");
		return onEvaluateCharacterReturnNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override void Execute(NodeCanvas canvas)
	{
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
