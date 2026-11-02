using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Event Nodes/Is Deck Active Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(Goal),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class IsDeckActiveNode : FunctionNode
{
	public const string ID = "EE_IsDeckActiveNode";

	[SerializeField]
	private Deck _deck;

	private const string INPUT_DECK_NAME = "Deck";

	private const string OUTPUT_IS_ACTIVE_NAME = "Is Active";

	private const string NODE_NAME = "Is Deck Active";

	private const int INPUT_DECK_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string DECKS_NULL_OR_NOT_CONNECTED_ERROR = "Input Deck is null or not connected in {0}";

	public override string GetID => "EE_IsDeckActiveNode";

	public override Node Create(Vector2 pos)
	{
		IsDeckActiveNode isDeckActiveNode = ScriptableObject.CreateInstance<IsDeckActiveNode>();
		isDeckActiveNode.rect = new Rect(pos.x, pos.y, 150f, 95f);
		isDeckActiveNode.name = "Is Deck Active";
		isDeckActiveNode.CreateInput("Deck", "Deck");
		isDeckActiveNode.CreateOutput("Is Active", "Bool");
		return isDeckActiveNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsDeckActiveNode isDeckActiveNode = (IsDeckActiveNode)Create(rect.position + new Vector2(20f, 20f));
		isDeckActiveNode._deck = _deck;
		return isDeckActiveNode;
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
		GetInputValue(Inputs[0], ref _deck, canvas);
		return CastValue<T>(_deck.IsActive);
	}
}
