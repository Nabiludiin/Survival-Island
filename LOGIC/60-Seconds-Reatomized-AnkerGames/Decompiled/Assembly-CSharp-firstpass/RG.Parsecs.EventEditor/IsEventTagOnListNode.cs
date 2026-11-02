using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Event Tags/Is Event Tag On List Node", new Type[]
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
public class IsEventTagOnListNode : EventNode
{
	public const string ID = "EE_IsEventTagOnListNode";

	private const string NODE_NAME = "Is Event Tag On List";

	private const string EVENT_INPUT_NAME = "Event tag";

	private const string RESULT_OUTPUT_NAME = "Is On List";

	private const string EVENT_TAG_LIST_FIELD_NAME = "Event tag list";

	private const int RESULT_OUTPUT_INDEX = 0;

	private const int EVENT_TAG_LIST_INPUT_INDEX = 1;

	private const int EVENT_INPUT_INDEX = 0;

	[SerializeField]
	private EventTag _tag;

	[SerializeField]
	private EventTagList _tagList;

	public override string GetID => "EE_IsEventTagOnListNode";

	public override Node Create(Vector2 pos)
	{
		IsEventTagOnListNode isEventTagOnListNode = ScriptableObject.CreateInstance<IsEventTagOnListNode>();
		isEventTagOnListNode.name = "Is Event Tag On List";
		isEventTagOnListNode.rect = new Rect(pos.x, pos.y, 250f, 100f);
		isEventTagOnListNode.CreateInput("Event tag", "EventTagConection");
		isEventTagOnListNode.CreateInput("Event tag list", "EventTagListConnection");
		isEventTagOnListNode.CreateOutput("Is On List", "Bool");
		return isEventTagOnListNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsEventTagOnListNode isEventTagOnListNode = (IsEventTagOnListNode)Create(rect.position + new Vector2(20f, 20f));
		isEventTagOnListNode._tag = _tag;
		isEventTagOnListNode._tagList = _tagList;
		return isEventTagOnListNode;
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
			throw new NodeOutputNotFoundException(output, "EE_IsEventTagOnListNode");
		}
		GetInputValue(Inputs[0], ref _tag, canvas);
		GetInputValue(Inputs[1], ref _tagList, canvas);
		return CastValue<T>(_tagList.IsTagOnList(_tag));
	}
}
