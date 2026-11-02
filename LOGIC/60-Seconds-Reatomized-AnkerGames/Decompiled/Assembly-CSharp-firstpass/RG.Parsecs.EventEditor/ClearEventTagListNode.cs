using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Clear Event Tag List Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class ClearEventTagListNode : EventNode
{
	public const string ID = "EE_ClearEventTagListNode";

	private const string NODE_NAME = "Clear Event Tag List";

	private const string INPUT_NAME = "In";

	private const string OUTPUT_NAME = "Out";

	private const string EVENT_TAG_LIST_FIELD_NAME = "Event tag list";

	private const int INPUT_INDEX = 0;

	private const int OUTPUT_INDEX = 0;

	private const int EVENT_TAG_LIST_INPUT_INDEX = 1;

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_ClearEventTagListNode";

	public override Node Create(Vector2 pos)
	{
		ClearEventTagListNode clearEventTagListNode = ScriptableObject.CreateInstance<ClearEventTagListNode>();
		clearEventTagListNode.name = "Clear Event Tag List";
		clearEventTagListNode.rect = new Rect(pos.x, pos.y, 200f, 100f);
		clearEventTagListNode.CreateMutliInput("In", "Flow");
		clearEventTagListNode.CreateInput("Event tag list", "EventTagListConnection");
		clearEventTagListNode.CreateOutput("Out", "Flow");
		return clearEventTagListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ClearEventTagListNode clearEventTagListNode = (ClearEventTagListNode)Create(rect.position + new Vector2(20f, 20f));
		clearEventTagListNode._tagList = _tagList;
		return clearEventTagListNode;
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
		_tagList.ClearTagList();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
