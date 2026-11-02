using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/Event Tag List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class EventTagListNode : EventNode
{
	public const string ID = "EE_EventTagListNode";

	private const string NODE_NAME = "Event Tag List";

	private const string EVENT_OUTPUT_NAME = "Event tag List";

	private const int EVENT_OUTPUT_INDEX = 0;

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_EventTagListNode";

	public override Node Create(Vector2 pos)
	{
		EventTagListNode eventTagListNode = ScriptableObject.CreateInstance<EventTagListNode>();
		eventTagListNode.name = "Event Tag List";
		eventTagListNode.rect = new Rect(pos.x, pos.y, 250f, 100f);
		eventTagListNode.CreateOutput("Event tag List", "EventTagListConnection");
		return eventTagListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		EventTagListNode eventTagListNode = (EventTagListNode)Create(rect.position + new Vector2(20f, 20f));
		eventTagListNode._tagList = _tagList;
		return eventTagListNode;
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
			throw new NodeOutputNotFoundException(output, "EE_EventTagListNode");
		}
		return CastValue<T>(_tagList);
	}
}
