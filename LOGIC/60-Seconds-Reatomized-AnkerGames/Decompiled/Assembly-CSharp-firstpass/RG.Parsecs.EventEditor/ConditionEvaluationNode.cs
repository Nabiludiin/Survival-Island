using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Condition Evaluation Node", new Type[] { typeof(ConditionEvent) })]
public class ConditionEvaluationNode : FlowNode
{
	public const string ID = "EE_ConditionEvaluationNode";

	public const string NODE_NAME = "Condition Evaluation";

	public override string GetID => "EE_ConditionEvaluationNode";

	protected override bool IsSingularNode => true;

	public override Node Create(Vector2 pos)
	{
		ConditionEvaluationNode conditionEvaluationNode = ScriptableObject.CreateInstance<ConditionEvaluationNode>();
		conditionEvaluationNode.rect = new Rect(pos.x, pos.y, 150f, 30f);
		conditionEvaluationNode.name = "Condition Evaluation";
		conditionEvaluationNode.CreateOutput("Out", "Flow");
		return conditionEvaluationNode;
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
