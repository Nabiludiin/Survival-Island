using System;
using FMODUnity;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Set Sound Slot Event Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetSoundSlotEventNode : EventNode
{
	public const string ID = "EE_setSoundSlotEventNode";

	private const int FLOW_INPUT_INDEX = 0;

	private const int FLOW_OUTPUT_INDEX = 0;

	private const int SOUND_SLOT_INPUT_INDEX = 1;

	private const int VALUE_INPUT_INDEX = 2;

	private const string NODE_NAME = "Set sound slot";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string SOUND_SLOT_INPUT_NAME = "Sound slot";

	private const string VALUE_INPUT_NAME = "Event to set";

	[EventRef]
	[SerializeField]
	private string _eventToSet;

	[SerializeField]
	private SoundSlot _soundSlot;

	public override string GetID => "EE_setSoundSlotEventNode";

	public override Node Create(Vector2 pos)
	{
		SetSoundSlotEventNode setSoundSlotEventNode = ScriptableObject.CreateInstance<SetSoundSlotEventNode>();
		setSoundSlotEventNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		setSoundSlotEventNode.name = "Set sound slot";
		setSoundSlotEventNode.CreateMutliInput("In", "Flow");
		setSoundSlotEventNode.CreateInput("Sound slot", "SoundSlot");
		setSoundSlotEventNode.CreateInput("Event to set", "String");
		setSoundSlotEventNode.CreateOutput("Out", "Flow");
		return setSoundSlotEventNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetSoundSlotEventNode setSoundSlotEventNode = (SetSoundSlotEventNode)Create(rect.position + new Vector2(20f, 20f));
		setSoundSlotEventNode._eventToSet = _eventToSet;
		setSoundSlotEventNode._soundSlot = _soundSlot;
		return setSoundSlotEventNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _soundSlot, canvas);
		GetInputValue(Inputs[2], ref _eventToSet, canvas);
		_soundSlot.SoundEventName = _eventToSet;
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
