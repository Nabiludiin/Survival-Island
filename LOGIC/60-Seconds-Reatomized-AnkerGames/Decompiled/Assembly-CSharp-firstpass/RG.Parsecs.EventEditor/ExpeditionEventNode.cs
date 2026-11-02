using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Expedition Event Node", new Type[]
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
public class ExpeditionEventNode : EventNode
{
	public const string ID = "EE_ExpeditionEventNode";

	private const string NODE_NAME = "Expedition Event";

	private const string OUTPUT_NAME = "Event";

	private const int OUTPUT_INDEX = 0;

	[SerializeField]
	private ExpeditionEvent _event;

	public override string GetID => "EE_ExpeditionEventNode";

	public override Node Create(Vector2 pos)
	{
		ExpeditionEventNode expeditionEventNode = ScriptableObject.CreateInstance<ExpeditionEventNode>();
		expeditionEventNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		expeditionEventNode.name = "Expedition Event";
		expeditionEventNode.CreateOutput("Event", "ExpeditionEvent");
		return expeditionEventNode;
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
			throw new NodeOutputNotFoundException(output, "EE_ExpeditionEventNode");
		}
		return CastValue<T>(_event);
	}
}
