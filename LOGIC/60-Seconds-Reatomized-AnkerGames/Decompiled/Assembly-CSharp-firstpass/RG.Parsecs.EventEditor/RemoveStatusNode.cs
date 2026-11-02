using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Remove Status Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class RemoveStatusNode : CharacterNode
{
	public const string ID = "EE_removeStatusNode";

	public const string STATUS_INPUT_NAME = "Status";

	public const string FLOW_INPUT_NAME = "In";

	public const string CHARACTER_INPUT_NAME = "Character";

	public const string NODE_NAME = "Remove Status";

	public const string FLOW_OUTPUT_NAME = "Out";

	public const string RUN_INPUT_NAME = "Run on status exit event";

	public const int RUN_INPUT_INDEX = 3;

	public const int FLOW_INPUT_INDEX = 0;

	public const int STATUS_INPUT_INDEX = 2;

	public const int CHARACTER_INPUT_INDEX = 1;

	public const int FLOW_OUTPUT_INDEX = 0;

	[SerializeField]
	private bool _runOnStatusExitEvent;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private CharacterStatus _status;

	public override string GetID => "EE_removeStatusNode";

	public override Node Create(Vector2 pos)
	{
		RemoveStatusNode removeStatusNode = ScriptableObject.CreateInstance<RemoveStatusNode>();
		removeStatusNode.rect = new Rect(pos.x, pos.y, 220f, 105f);
		removeStatusNode.name = "Remove Status";
		removeStatusNode.CreateMutliInput("In", "Flow");
		removeStatusNode.CreateInput("Character", "Character");
		removeStatusNode.CreateInput("Status", "CharacterStatusConnection");
		removeStatusNode.CreateInput("Run on status exit event", "Bool");
		removeStatusNode.CreateOutput("Out", "Flow");
		return removeStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		RemoveStatusNode removeStatusNode = (RemoveStatusNode)Create(rect.position + new Vector2(20f, 20f));
		removeStatusNode._status = _status;
		removeStatusNode._character = _character;
		removeStatusNode._runOnStatusExitEvent = _runOnStatusExitEvent;
		return removeStatusNode;
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
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _status, canvas);
		GetInputValue(Inputs[3], ref _runOnStatusExitEvent, canvas);
		if (_character.RuntimeData.CurrentStatuses.Contains(_status))
		{
			_character.RuntimeData.RemoveStatus(_status);
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
