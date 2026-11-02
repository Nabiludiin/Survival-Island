using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Deactivate Deck Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DeactivateDeckNode : FunctionNode
{
	public const string ID = "EE_DeactivateDeckNode";

	[SerializeField]
	private Deck _deck;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_DECK_NAME = "Deck";

	private const string OUTPUT_OUT_NAME = "Out";

	private const string NODE_NAME = "Deactivate Deck";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_DECK_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string DECKS_NULL_OR_NOT_CONNECTED_ERROR = "Input Deck is null or not connected in {0}";

	public override string GetID => "EE_DeactivateDeckNode";

	public override Node Create(Vector2 pos)
	{
		DeactivateDeckNode deactivateDeckNode = ScriptableObject.CreateInstance<DeactivateDeckNode>();
		deactivateDeckNode.rect = new Rect(pos.x, pos.y, 150f, 95f);
		deactivateDeckNode.name = "Deactivate Deck";
		deactivateDeckNode.CreateMutliInput("In", "Flow");
		deactivateDeckNode.CreateInput("Deck", "Deck");
		deactivateDeckNode.CreateOutput("Out", "Flow");
		return deactivateDeckNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DeactivateDeckNode deactivateDeckNode = (DeactivateDeckNode)Create(rect.position + new Vector2(20f, 20f));
		deactivateDeckNode._deck = _deck;
		return deactivateDeckNode;
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
		if (!Inputs[1].isConnected && _deck == null)
		{
			LogMessage($"Input Deck is null or not connected in {GetID}", EMessageType.ERROR);
		}
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _deck, canvas);
		_deck.SetActive(activate: false);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
