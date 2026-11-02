using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Is Threat Blocked Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent),
	typeof(ConditionEvent)
})]
public class IsThreatBlockedNode : ExpeditionNode
{
	public const string ID = "EE_IsThreatBlockedNode";

	private const string OUTPUT_LOCKED_NAME = "Is Blocked";

	private const string INPUT_THREAT_NAME = "Threat";

	private const int OUTPUT_LOCKED_INDEX = 0;

	private const int INPUT_THREAT_INDEX = 0;

	private const string NODE_NAME = "Is Threat Blocked";

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_IsThreatBlockedNode";

	public override Node Create(Vector2 pos)
	{
		IsThreatBlockedNode isThreatBlockedNode = ScriptableObject.CreateInstance<IsThreatBlockedNode>();
		isThreatBlockedNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		isThreatBlockedNode.name = "Is Threat Blocked";
		isThreatBlockedNode.CreateInput("Threat", "ExpeditionThreat");
		isThreatBlockedNode.CreateOutput("Is Blocked", "Bool");
		return isThreatBlockedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsThreatBlockedNode isThreatBlockedNode = (IsThreatBlockedNode)Create(rect.position + new Vector2(20f, 20f));
		isThreatBlockedNode._threat = _threat;
		return isThreatBlockedNode;
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
		if (output == 0)
		{
			GetInputValue(Inputs[0], ref _threat, canvas);
			return CastValue<T>(ExpeditionManager.Instance.IsThreatBlocked(_threat));
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
