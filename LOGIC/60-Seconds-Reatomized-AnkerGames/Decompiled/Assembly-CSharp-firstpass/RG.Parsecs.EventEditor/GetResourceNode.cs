using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Resources/Get Resource Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(Goal),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetResourceNode : CalculationNode
{
	public const string ID = "EE_GetResourceNode";

	[SerializeField]
	private Resource _value;

	private const string NODE_NAME = "Get Resource Node";

	private const string OUTPUT_NAME = "Resource";

	private const int OUTPUT_INDEX = 0;

	public override string GetID => "EE_GetResourceNode";

	public override Node Create(Vector2 pos)
	{
		GetResourceNode getResourceNode = ScriptableObject.CreateInstance<GetResourceNode>();
		getResourceNode.rect = new Rect(pos.x, pos.y, 180f, 100f);
		getResourceNode.name = "Get Resource Node";
		getResourceNode.CreateOutput("Resource", "Resources");
		return getResourceNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetResourceNode getResourceNode = (GetResourceNode)Create(rect.position + new Vector2(20f, 20f));
		getResourceNode._value = _value;
		return getResourceNode;
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
			throw new NodeOutputNotFoundException(output, "EE_GetResourceNode");
		}
		return CastValue<T>(_value);
	}
}
