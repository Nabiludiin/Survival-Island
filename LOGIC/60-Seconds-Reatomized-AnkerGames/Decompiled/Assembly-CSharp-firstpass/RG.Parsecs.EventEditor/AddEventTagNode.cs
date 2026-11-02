using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Add Event Tag Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class AddEventTagNode : EventNode
{
	public const string ID = "EE_AddEventTagNode";

	private const string NODE_NAME = "Add event tag";

	private const string FLOW_INPUT_NAME = "In";

	private const string FLOW_OUTPUT_NAME = "Out";

	private const string EVENT_TAG_FIELD_NAME = "Event tag";

	private const string EVENT_TAG_LIST_FIELD_NAME = "Event tag list";

	private const int FLOW_INPUT_INDEX = 0;

	private const int EVENT_TAG_LIST_INPUT_INDEX = 1;

	private const int EVENT_TAG_INPUT_INDEX = 2;

	private const int FLOW_OUTPUT_INDEX = 0;

	[SerializeField]
	private EventTag _tag;

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_AddEventTagNode";

	public override Node Create(Vector2 pos)
	{
		AddEventTagNode addEventTagNode = ScriptableObject.CreateInstance<AddEventTagNode>();
		addEventTagNode.name = "Add event tag";
		addEventTagNode.rect = new Rect(pos.x, pos.y, 250f, 100f);
		addEventTagNode.CreateMutliInput("In", "Flow");
		addEventTagNode.CreateInput("Event tag list", "EventTagListConnection");
		addEventTagNode.CreateInput("Event tag", "EventTagConection");
		addEventTagNode.CreateOutput("Out", "Flow");
		return addEventTagNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddEventTagNode addEventTagNode = (AddEventTagNode)Create(rect.position + new Vector2(20f, 20f));
		addEventTagNode._tag = _tag;
		addEventTagNode._tagList = _tagList;
		return addEventTagNode;
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
		GetInputValue(Inputs[2], ref _tag, canvas);
		GetInputValue(Inputs[1], ref _tagList, canvas);
		_tagList.AddTag(_tag);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
