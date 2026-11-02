using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Remove Event Tag List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class RemoveEventTagListNode : EventNode
{
	public const string ID = "EE_RemoveEventTagListNode";

	private const string NODE_NAME = "Remove Event Tag List";

	private const string EVENT_TAG_LIST_FIELD_NAME = "Event tag list";

	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const int FLOW_INPUT_INDEX = 0;

	private const int EVENT_TAG_LIST_INPUT_INDEX = 1;

	private const int FLOW_OUTPUT_INDEX = 0;

	[SerializeField]
	private List<EventTag> _tags = new List<EventTag>();

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_RemoveEventTagListNode";

	public override Node Create(Vector2 pos)
	{
		RemoveEventTagListNode removeEventTagListNode = ScriptableObject.CreateInstance<RemoveEventTagListNode>();
		removeEventTagListNode.name = "Remove Event Tag List";
		removeEventTagListNode.rect = new Rect(pos.x, pos.y, 350f, 100f);
		removeEventTagListNode.CreateMutliInput("In", "Flow");
		removeEventTagListNode.CreateInput("Event tag list", "EventTagListConnection");
		removeEventTagListNode.CreateOutput("Out", "Flow");
		return removeEventTagListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveEventTagListNode removeEventTagListNode = (RemoveEventTagListNode)Create(rect.position + new Vector2(20f, 20f));
		removeEventTagListNode._tagList = _tagList;
		removeEventTagListNode._tags = _tags;
		return removeEventTagListNode;
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
		GetInputValue(Inputs[1], ref _tagList, canvas);
		for (int i = 0; i < _tags.Count; i++)
		{
			_tagList.RemoveTag(_tags[i]);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
