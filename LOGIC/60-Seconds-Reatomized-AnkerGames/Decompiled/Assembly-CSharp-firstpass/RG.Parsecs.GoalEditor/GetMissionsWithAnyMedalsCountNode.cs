using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Global Stats/GetMissionCountAnyMedalsNode", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class GetMissionsWithAnyMedalsCountNode : GoalNode
{
	public const string ID = "GE_GetMissionsWithAnyMedalsCountNode";

	private const string OUTOUP_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Mission Count Any Medals";

	public override string GetID => "GE_GetMissionsWithAnyMedalsCountNode";

	public override Node Create(Vector2 pos)
	{
		GetMissionsWithAnyMedalsCountNode getMissionsWithAnyMedalsCountNode = ScriptableObject.CreateInstance<GetMissionsWithAnyMedalsCountNode>();
		getMissionsWithAnyMedalsCountNode.name = "Mission Count Any Medals";
		getMissionsWithAnyMedalsCountNode.rect = new Rect(pos.x, pos.y, 300f, 100f);
		getMissionsWithAnyMedalsCountNode.CreateOutput("Value", "Int");
		return getMissionsWithAnyMedalsCountNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (GetMissionsWithAnyMedalsCountNode)Create(pos + new Vector2(20f, 20f));
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(MissionManager.Instance.GetMissionWithAnyMedalsCount());
	}
}
