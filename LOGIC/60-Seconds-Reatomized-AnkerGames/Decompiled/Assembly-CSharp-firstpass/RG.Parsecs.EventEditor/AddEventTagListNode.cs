using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Add Event Tag List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class AddEventTagListNode : EventNode
{
	public const string ID = "EE_AddEventTagListNode";

	private const string NODE_NAME = "Add Event Tag List";

	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string EVENT_TAG_LIST_FIELD_NAME = "Event tag list";

	private const int FLOW_INPUT_INDEX = 0;

	private const int EVENT_TAG_LIST_INPUT_INDEX = 1;

	private const int FLOW_OUTPUT_INDEX = 0;

	[SerializeField]
	private List<EventTag> _tags = new List<EventTag>();

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_AddEventTagListNode";

	public override Node Create(Vector2 pos)
	{
		AddEventTagListNode addEventTagListNode = ScriptableObject.CreateInstance<AddEventTagListNode>();
		addEventTagListNode.name = "Add Event Tag List";
		addEventTagListNode.rect = new Rect(pos.x, pos.y, 350f, 100f);
		addEventTagListNode.CreateMutliInput("In", "Flow");
		addEventTagListNode.CreateInput("Event tag list", "EventTagListConnection");
		addEventTagListNode.CreateOutput("Out", "Flow");
		return addEventTagListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddEventTagListNode addEventTagListNode = (AddEventTagListNode)Create(rect.position + new Vector2(20f, 20f));
		addEventTagListNode._tagList = _tagList;
		addEventTagListNode._tags = _tags;
		return addEventTagListNode;
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
			_tagList.AddTag(_tags[i]);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
