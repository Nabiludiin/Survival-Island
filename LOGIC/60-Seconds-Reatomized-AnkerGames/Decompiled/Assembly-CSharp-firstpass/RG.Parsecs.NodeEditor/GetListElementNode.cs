using System;
using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Calculation/Get List Element Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetListElementNode : ParsecsNode
{
	public const string Id = "PE_GetListElementNode";

	private const int OUTPUT_ELEMENT_INDEX = 0;

	private const int INPUT_LIST_INDEX = 0;

	private const int INPUT_ELEMENT_INDEX = 1;

	private const string INPUT_LIST_NAME = "List";

	private const string INPUT_ELEMENT_NAME = "Element index";

	private const string OUTPUT_ELEMENT_NAME = "Element";

	private const string NODE_NAME = "Get List Element";

	[SerializeField]
	private int _elementIndex;

	public override string GetID => "PE_GetListElementNode";

	public override Node Create(Vector2 pos)
	{
		GetListElementNode getListElementNode = ScriptableObject.CreateInstance<GetListElementNode>();
		getListElementNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		getListElementNode.name = "Get List Element";
		getListElementNode.CreateInput("List", ListConnection.ID);
		getListElementNode.CreateInput("Element index", "Int");
		getListElementNode.CreateOutput("Element", "Int");
		return getListElementNode;
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
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[1], ref _elementIndex, canvas);
		return CastValue<T>(GetInputValue<IList>(Inputs[0], canvas)[_elementIndex]);
	}
}
