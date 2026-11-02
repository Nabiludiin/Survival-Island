using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Expedition Nodes/Is Expedition Destination Locked Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(ConditionEvent)
})]
public class IsExpeditionDestinationLockedNode : ExpeditionNode
{
	public const string ID = "EE_IsExpeditionDestinationLockedNode";

	private const string OUTPUT_LOCKED_NAME = "Is Locked";

	private const string INPUT_DESTINATION_NAME = "Destination";

	private const int OUTPUT_LOCKED_INDEX = 0;

	private const int INPUT_DESTINATION_INDEX = 0;

	private const string NODE_NAME = "Is Expedition Destination Locked";

	[SerializeField]
	private ExpeditionDestination _destination;

	public override string GetID => "EE_IsExpeditionDestinationLockedNode";

	public override Node Create(Vector2 pos)
	{
		IsExpeditionDestinationLockedNode isExpeditionDestinationLockedNode = ScriptableObject.CreateInstance<IsExpeditionDestinationLockedNode>();
		isExpeditionDestinationLockedNode.rect = new Rect(pos.x, pos.y, 220f, 50f);
		isExpeditionDestinationLockedNode.name = "Is Expedition Destination Locked";
		isExpeditionDestinationLockedNode.CreateInput("Destination", "ExpeditionDestination");
		isExpeditionDestinationLockedNode.CreateOutput("Is Locked", "Bool");
		return isExpeditionDestinationLockedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsExpeditionDestinationLockedNode isExpeditionDestinationLockedNode = (IsExpeditionDestinationLockedNode)Create(rect.position + new Vector2(20f, 20f));
		isExpeditionDestinationLockedNode._destination = _destination;
		return isExpeditionDestinationLockedNode;
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
			GetInputValue(Inputs[0], ref _destination, canvas);
			return CastValue<T>(ExpeditionManager.Instance.IsExpeditionDestinationLocked(_destination));
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
