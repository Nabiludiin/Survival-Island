using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Cast To Threat Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class CastToThreatNode : EventNode
{
	public const string ID = "PE_CastToThreatNode";

	private const int INPUT_OBJECT_INDEX = 0;

	private const int OUTPUT_THREAT_INDEX = 0;

	private const string INPUT_OBJECT_NAME = "Object";

	private const string OUTPUT_THREAT_NAME = "Threat";

	private const string NODE_NAME = "Cast To Threat";

	public override string GetID => "PE_CastToThreatNode";

	public override Node Create(Vector2 pos)
	{
		CastToThreatNode castToThreatNode = ScriptableObject.CreateInstance<CastToThreatNode>();
		castToThreatNode.rect = new Rect(pos.x, pos.y, 150f, 75f);
		castToThreatNode.name = "Cast To Threat";
		castToThreatNode.CreateInput("Object", ObjectConnection.ID);
		castToThreatNode.CreateOutput("Threat", "ExpeditionThreat");
		return castToThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(GetInputValue<object>(Inputs[0], canvas));
	}
}
