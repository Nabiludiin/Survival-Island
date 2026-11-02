using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Utility Nodes/Get Sound Slot For Character Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class GetSoundSlotForCharacterNode : EventNode
{
	public const string ID = "EE_getSoundSlotForCharacterNode";

	private const int SOUND_SLOT_OUTPUT_INDEX = 0;

	private const int CHARACTER_INPUT_INDEX = 0;

	private const string NODE_NAME = "Get sound slot for char";

	private const string SOUND_SLOT_OUTPUT_NAME = "Sound slot";

	private const string CHARACTER_INPUT_NAME = "Character";

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_getSoundSlotForCharacterNode";

	public override Node Create(Vector2 pos)
	{
		GetSoundSlotForCharacterNode getSoundSlotForCharacterNode = ScriptableObject.CreateInstance<GetSoundSlotForCharacterNode>();
		getSoundSlotForCharacterNode.rect = new Rect(pos.x, pos.y, 250f, 40f);
		getSoundSlotForCharacterNode.name = "Get sound slot for char";
		getSoundSlotForCharacterNode.CreateInput("Character", "Character");
		getSoundSlotForCharacterNode.CreateOutput("Sound slot", "SoundSlot");
		return getSoundSlotForCharacterNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetSoundSlotForCharacterNode getSoundSlotForCharacterNode = (GetSoundSlotForCharacterNode)Create(rect.position + new Vector2(20f, 20f));
		getSoundSlotForCharacterNode._character = _character;
		return getSoundSlotForCharacterNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		if (_character == null)
		{
			throw new UnityException("No character provided for Get Sound Slot For Character Node!");
		}
		return CastValue<T>(_character.StaticData.VoiceOver);
	}

	public override string ToString()
	{
		return _character.ToString();
	}
}
