using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Get Threat Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class GetThreatNode : ExpeditionNode
{
	public const string ID = "EE_GetThreatNode";

	public const string NODE_NAME = "Get Threat";

	public const string OUTPUT_THREAT_NAME = "Threat";

	public const int OUTPUT_THREAT_INDEX = 0;

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_GetThreatNode";

	public override Node Create(Vector2 pos)
	{
		GetThreatNode getThreatNode = ScriptableObject.CreateInstance<GetThreatNode>();
		getThreatNode.rect = new Rect(pos.x, pos.y, 150f, 90f);
		getThreatNode.name = "Get Threat";
		getThreatNode.CreateOutput("Threat", "ExpeditionThreat");
		return getThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetThreatNode getThreatNode = (GetThreatNode)Create(rect.position + new Vector2(20f, 20f));
		getThreatNode._threat = _threat;
		return getThreatNode;
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
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_threat);
	}
}
