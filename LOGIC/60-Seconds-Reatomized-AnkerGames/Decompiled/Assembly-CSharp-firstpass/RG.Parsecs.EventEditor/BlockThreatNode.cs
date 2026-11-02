using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Block Threat Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class BlockThreatNode : ExpeditionNode
{
	public const string ID = "EE_BlockThreatNode";

	private const string INPUT_THREAT_NAME = "Threat";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_THREAT_INDEX = 1;

	private const int INPUT_IN_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Block Threat";

	[SerializeField]
	private ExpeditionThreat _threat;

	public override string GetID => "EE_BlockThreatNode";

	public override Node Create(Vector2 pos)
	{
		BlockThreatNode blockThreatNode = ScriptableObject.CreateInstance<BlockThreatNode>();
		blockThreatNode.rect = new Rect(pos.x, pos.y, 170f, 50f);
		blockThreatNode.name = "Block Threat";
		blockThreatNode.CreateMutliInput("In", "Flow");
		blockThreatNode.CreateInput("Threat", "ExpeditionThreat");
		blockThreatNode.CreateOutput("Out", "Flow");
		return blockThreatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		BlockThreatNode blockThreatNode = (BlockThreatNode)Create(rect.position + new Vector2(20f, 20f));
		blockThreatNode._threat = _threat;
		return blockThreatNode;
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

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _threat, canvas);
		ExpeditionManager.Instance.BlockThreat(_threat);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
