using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Add Status Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(Goal),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class AddStatusNode : CharacterNode
{
	public const string ID = "addStatusNode";

	public const string STATUS_INPUT_NAME = "Status to add";

	public const string FLOW_INPUT_NAME = "In";

	public const string CHARACTER_INPUT_NAME = "Character";

	public const string NODE_NAME = "Add Status";

	public const string FLOW_OUTPUT_NAME = "Out";

	public const int FLOW_INPUT_INDEX = 0;

	public const int STATUS_INPUT_INDEX = 2;

	public const int CHARACTER_INPUT_INDEX = 1;

	public const int FLOW_OUTPUT_INDEX = 0;

	public const string RUN_INPUT_NAME = "Run on status enter event";

	public const int RUN_INPUT_INDEX = 3;

	[SerializeField]
	private bool _runOnStatusEnterEvent;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private CharacterStatus _status;

	public override string GetID => "addStatusNode";

	public override Node Create(Vector2 pos)
	{
		AddStatusNode addStatusNode = ScriptableObject.CreateInstance<AddStatusNode>();
		addStatusNode.rect = new Rect(pos.x, pos.y, 200f, 105f);
		addStatusNode.name = "Add Status";
		addStatusNode.CreateMutliInput("In", "Flow");
		addStatusNode.CreateInput("Character", "Character");
		addStatusNode.CreateInput("Status to add", "CharacterStatusConnection");
		addStatusNode.CreateInput("Run on status enter event", "Bool");
		addStatusNode.CreateOutput("Out", "Flow");
		return addStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddStatusNode addStatusNode = (AddStatusNode)Create(rect.position + new Vector2(20f, 20f));
		addStatusNode._character = _character;
		addStatusNode._status = _status;
		addStatusNode._runOnStatusEnterEvent = _runOnStatusEnterEvent;
		return addStatusNode;
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
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _status, canvas);
		GetInputValue(Inputs[3], ref _runOnStatusEnterEvent, canvas);
		if (!(ParentCanvas is Goal))
		{
			ExpeditionEvent expeditionEvent = ParentEvent as ExpeditionEvent;
			if (expeditionEvent != null && expeditionEvent.ReturnEvaluated && expeditionEvent.CharacterReturned && _status.PreventsReturnFromExpedition)
			{
				Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
				return;
			}
		}
		CheckAreAllFlowOutputsConnected();
		if (!_character.RuntimeData.CurrentStatuses.Contains(_status))
		{
			_character.RuntimeData.AddStatus(_status);
		}
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
