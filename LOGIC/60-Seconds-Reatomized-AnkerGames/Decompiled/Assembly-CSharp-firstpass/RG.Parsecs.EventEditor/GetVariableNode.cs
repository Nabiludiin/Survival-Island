using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Variable Nodes/Get Variable Node", new Type[]
{
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetVariableNode : EventNode
{
	public const string ID = "EE_GetVariableNode";

	private const string OUTPUT_DATA_NAME = "Data";

	private const int DATA_OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Get Variable Node";

	[SerializeField]
	private Variable _variable;

	[HideInInspector]
	[SerializeField]
	private string _previousVarName;

	[SerializeField]
	private string _eventName = string.Empty;

	public override string GetID => "EE_GetVariableNode";

	public Variable Variable => _variable;

	public override Node Create(Vector2 pos)
	{
		GetVariableNode getVariableNode = ScriptableObject.CreateInstance<GetVariableNode>();
		getVariableNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		getVariableNode.name = "Get Variable Node";
		return getVariableNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetVariableNode getVariableNode = (GetVariableNode)Create(rect.position + new Vector2(20f, 20f));
		getVariableNode._variable = _variable;
		getVariableNode._eventName = _eventName;
		return getVariableNode;
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

	private void CheckLocalVariable()
	{
	}

	private void DrawVariablesKnobes()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "EE_GetVariableNode");
		}
		return CastValue<T>(_variable.GetValue());
	}
}
