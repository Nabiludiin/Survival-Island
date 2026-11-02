using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Type Nodes/String Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class StringNode : CalculationNode
{
	public const string ID = "EE_StringNode";

	[SerializeField]
	private string _value = string.Empty;

	private const string NODE_NAME = "String Node";

	private const string OUTPUT_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_StringNode";

	public override Node Create(Vector2 pos)
	{
		StringNode stringNode = ScriptableObject.CreateInstance<StringNode>();
		stringNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		stringNode.name = "String Node";
		stringNode.CreateOutput("Value", "String");
		return stringNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_StringNode");
		}
		return CastValue<T>(_value);
	}
}
