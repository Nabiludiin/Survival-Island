using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Get Random Event From Deck Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class GetRandomEventFromDeckNode : FunctionNode
{
	public const string ID = "EE_GetRandomEventFromDeckNode";

	[SerializeField]
	private Deck _deck;

	private const string INPUT_DECK_NAME = "Deck";

	private const string OUTPUT_OUT_NAME = "Event";

	private const string NODE_NAME = "Get Random Event From Deck Node";

	private const int INPUT_DECK_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NO_EVENTS_AVAILABLE_ERROR = "Deck does not have any available events in {0}";

	private const string DECKS_NULL_OR_NOT_CONNECTED_ERROR = "Input Deck is null or not connected in {0}";

	public override string GetID => "EE_GetRandomEventFromDeckNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomEventFromDeckNode getRandomEventFromDeckNode = ScriptableObject.CreateInstance<GetRandomEventFromDeckNode>();
		getRandomEventFromDeckNode.rect = new Rect(pos.x, pos.y, 250f, 95f);
		getRandomEventFromDeckNode.name = "Get Random Event From Deck Node";
		getRandomEventFromDeckNode.CreateInput("Deck", "Deck");
		getRandomEventFromDeckNode.CreateOutput("Event", "ParsecsEvent");
		return getRandomEventFromDeckNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetRandomEventFromDeckNode getRandomEventFromDeckNode = (GetRandomEventFromDeckNode)Create(rect.position + new Vector2(20f, 20f));
		getRandomEventFromDeckNode._deck = _deck;
		return getRandomEventFromDeckNode;
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
		if (!Inputs[0].isConnected && _deck == null)
		{
			LogMessage($"Input Deck is null or not connected in {GetID}", EMessageType.ERROR);
		}
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		if (_deck == null)
		{
			throw new UnityException($"Input Deck is null or not connected in {GetID}");
		}
		GetInputValue(Inputs[0], ref _deck, canvas);
		bool isActive = _deck.IsActive;
		if (!isActive)
		{
			_deck.SetActive(activate: true);
		}
		SurvivalEvent randomEvent = _deck.GetRandomEvent();
		if (!isActive)
		{
			_deck.SetActive(activate: false);
		}
		if (randomEvent == null)
		{
			throw new UnityException($"Deck does not have any available events in {GetID}");
		}
		return CastValue<T>(randomEvent);
	}
}
