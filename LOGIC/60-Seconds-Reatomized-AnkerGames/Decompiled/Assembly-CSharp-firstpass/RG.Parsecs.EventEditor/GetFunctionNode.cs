using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Function Nodes/Get Function Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetFunctionNode : EventNode
{
	public const string ID = "EE_GetFunctionNode";

	public const string NODE_NAME = "Get Function Node";

	public const string OUTPUT_NAME = "Function";

	public const int OUTPUT_INDEX = 0;

	[SerializeField]
	private NodeFunction _function;

	public override string GetID => "EE_GetFunctionNode";

	public NodeFunction Function => _function;

	public override Node Create(Vector2 pos)
	{
		GetFunctionNode getFunctionNode = ScriptableObject.CreateInstance<GetFunctionNode>();
		getFunctionNode.rect = new Rect(pos.x, pos.y, 200f, 120f);
		getFunctionNode.name = "Get Function Node";
		getFunctionNode.CreateOutput("Function", "NodeFunction");
		return getFunctionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetFunctionNode getFunctionNode = (GetFunctionNode)Create(rect.position + new Vector2(20f, 20f));
		getFunctionNode._function = _function;
		return getFunctionNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
		base.OnNodeValidate();
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (_function == null)
		{
			throw new UnityException("No function provided for Get Function Node!");
		}
		return CastValue<T>(_function);
	}

	public override string ToString()
	{
		return _function.ToString();
	}
}
