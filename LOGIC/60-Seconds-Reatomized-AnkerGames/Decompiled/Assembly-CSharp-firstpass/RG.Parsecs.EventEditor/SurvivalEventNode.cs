using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Survival Event Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(Goal),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class SurvivalEventNode : EventNode
{
	public const string ID = "EE_SurvivalEventNode";

	private const string NODE_NAME = "Survival Event";

	private const string OUTPUT_NAME = "Event";

	private const int OUTPUT_INDEX = 0;

	[SerializeField]
	private SurvivalEvent _event;

	public override string GetID => "EE_SurvivalEventNode";

	public override Node Create(Vector2 pos)
	{
		SurvivalEventNode survivalEventNode = ScriptableObject.CreateInstance<SurvivalEventNode>();
		survivalEventNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		survivalEventNode.name = "Survival Event";
		survivalEventNode.CreateOutput("Event", "SurvivalEvent");
		return survivalEventNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_SurvivalEventNode");
		}
		return CastValue<T>(_event);
	}
}
