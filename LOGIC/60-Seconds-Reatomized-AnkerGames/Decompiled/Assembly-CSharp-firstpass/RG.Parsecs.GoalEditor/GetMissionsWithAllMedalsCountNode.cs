using System;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Global Stats/GetMissionCountAllMedalsNode", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal)
})]
public class GetMissionsWithAllMedalsCountNode : GoalNode
{
	public const string ID = "GE_GetMissionsWithAllMedalsCountNode";

	private const string OUTOUP_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Mission Count All Medals";

	public override string GetID => "GE_GetMissionsWithAllMedalsCountNode";

	public override Node Create(Vector2 pos)
	{
		GetMissionsWithAllMedalsCountNode getMissionsWithAllMedalsCountNode = ScriptableObject.CreateInstance<GetMissionsWithAllMedalsCountNode>();
		getMissionsWithAllMedalsCountNode.name = "Mission Count All Medals";
		getMissionsWithAllMedalsCountNode.rect = new Rect(pos.x, pos.y, 300f, 100f);
		getMissionsWithAllMedalsCountNode.CreateOutput("Value", "Int");
		return getMissionsWithAllMedalsCountNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return (GetMissionsWithAllMedalsCountNode)Create(pos + new Vector2(20f, 20f));
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
		return CastValue<T>(MissionManager.Instance.GetMissionWithAllMedalsCount());
	}
}
