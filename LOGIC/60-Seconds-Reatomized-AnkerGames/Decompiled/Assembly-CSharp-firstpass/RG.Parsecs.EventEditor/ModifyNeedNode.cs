using System;
using NodeEditorFramework;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Modify Need Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class ModifyNeedNode : CharacterNode
{
	public const string ID = "EE_modifyNeedNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTERS_INPUT_INDEX = 1;

	private const int CHANGE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Modify need";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string CHARACTERS_INPUT_NAME = "Character";

	private const string CHANGE_INPUT_NAME = "Change amount";

	[SerializeField]
	private CharacterNeed _need;

	[SerializeField]
	private float _changeAmount;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_modifyNeedNode";

	public override Node Create(Vector2 pos)
	{
		ModifyNeedNode modifyNeedNode = ScriptableObject.CreateInstance<ModifyNeedNode>();
		modifyNeedNode.rect = new Rect(pos.x, pos.y, 220f, 40f);
		modifyNeedNode.name = "Modify need";
		modifyNeedNode.CreateMutliInput("In", "Flow");
		modifyNeedNode.CreateInput("Character", "Character");
		modifyNeedNode.CreateInput("Change amount", "Float");
		modifyNeedNode.CreateOutput("Out", "Flow");
		return modifyNeedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ModifyNeedNode modifyNeedNode = (ModifyNeedNode)Create(rect.position + new Vector2(20f, 20f));
		modifyNeedNode._need = _need;
		modifyNeedNode._changeAmount = _changeAmount;
		modifyNeedNode._character = _character;
		return modifyNeedNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
		if (!Inputs[0].isConnected)
		{
			LogMessage("No flow input connected to Modify need", EMessageType.WARNING);
		}
		if (!Inputs[1].isConnected && _character == null)
		{
			LogMessage("No Character set nor connected in Modify need", EMessageType.ERROR);
		}
		if (_need == null)
		{
			LogMessage("No Character Need connected in Modify need", EMessageType.ERROR);
		}
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _character, canvas);
		GetInputValue(Inputs[2], ref _changeAmount, canvas);
		if (!(ParentCanvas is Goal))
		{
			ExpeditionEvent expeditionEvent = ParentEvent as ExpeditionEvent;
			if (expeditionEvent != null && expeditionEvent.ReturnEvaluated && expeditionEvent.CharacterReturned)
			{
				_character.RuntimeData.ChangeNeedValueWithLimit(_need.Id, _changeAmount);
			}
			else
			{
				_character.RuntimeData.ChangeNeedValue(_need.Id, _changeAmount);
			}
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
