using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Get Icon Term For Action Condition Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class GetIconTermForActionConditionNode : CharacterNode
{
	public const string ID = "GetIconTermForActionConditionNode";

	public const string NODE_NAME = "Get Icon Term For Action Condition Node";

	public const string OUTPUT_NAME = "Icon Term";

	public const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private BaseActionCondition _actionCondition;

	public override string GetID => "GetIconTermForActionConditionNode";

	public override Node Create(Vector2 pos)
	{
		GetIconTermForActionConditionNode getIconTermForActionConditionNode = ScriptableObject.CreateInstance<GetIconTermForActionConditionNode>();
		getIconTermForActionConditionNode.rect = new Rect(pos.x, pos.y, 200f, 90f);
		getIconTermForActionConditionNode.name = "Get Icon Term For Action Condition Node";
		getIconTermForActionConditionNode.CreateOutput("Icon Term", "LocalizedString");
		return getIconTermForActionConditionNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetIconTermForActionConditionNode getIconTermForActionConditionNode = (GetIconTermForActionConditionNode)Create(rect.position + new Vector2(20f, 20f));
		getIconTermForActionConditionNode._actionCondition = _actionCondition;
		return getIconTermForActionConditionNode;
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
		if (_actionCondition == null)
		{
			throw new UnityException("Action Condition has not been set.");
		}
		return CastValue<T>(_actionCondition.IconTerm);
	}

	public override string ToString()
	{
		return _actionCondition.IconTerm.ToString();
	}
}
