using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Event Tag Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class EventTagNode : EventNode
{
	public const string ID = "EE_EventTagNode";

	private const string NODE_NAME = "Event Tag";

	private const string EVENT_OUTPUT_NAME = "Event tag";

	private const int EVENT_OUTPUT_INDEX = 0;

	[SerializeField]
	private EventTag _tag;

	public override string GetID => "EE_EventTagNode";

	public override Node Create(Vector2 pos)
	{
		EventTagNode eventTagNode = ScriptableObject.CreateInstance<EventTagNode>();
		eventTagNode.name = "Event Tag";
		eventTagNode.rect = new Rect(pos.x, pos.y, 250f, 100f);
		eventTagNode.CreateOutput("Event tag", "EventTagConection");
		return eventTagNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		EventTagNode eventTagNode = (EventTagNode)Create(rect.position + new Vector2(20f, 20f));
		eventTagNode._tag = _tag;
		return eventTagNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_EventTagNode");
		}
		return CastValue<T>(_tag);
	}
}
