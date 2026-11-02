using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Remove Event Tag Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class RemoveEventTagNode : EventNode
{
	private const string ID = "EE_RemoveEventTagNode";

	private const string NODE_NAME = "Remove event tag";

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

	public override string GetID => "EE_RemoveEventTagNode";

	public override Node Create(Vector2 pos)
	{
		RemoveEventTagNode removeEventTagNode = ScriptableObject.CreateInstance<RemoveEventTagNode>();
		removeEventTagNode.name = "Remove event tag";
		removeEventTagNode.rect = new Rect(pos.x, pos.y, 250f, 100f);
		removeEventTagNode.CreateMutliInput("In", "Flow");
		removeEventTagNode.CreateInput("Event tag list", "EventTagListConnection");
		removeEventTagNode.CreateInput("Event tag", "EventTagConection");
		removeEventTagNode.CreateOutput("Out", "Flow");
		return removeEventTagNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveEventTagNode removeEventTagNode = (RemoveEventTagNode)Create(rect.position + new Vector2(20f, 20f));
		removeEventTagNode._tag = _tag;
		removeEventTagNode._tagList = _tagList;
		return removeEventTagNode;
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
		_tagList.RemoveTag(_tag);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
