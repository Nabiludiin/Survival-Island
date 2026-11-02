using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Attribute Value Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetAttributeValueNode : CharacterNode
{
	private const string OUTPUT_NAME = "Attribute";

	private const string INPUT_NAME = "Character";

	private const int CHARACTER_INPUT_INDEX = 0;

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Get Attribute Value";

	public const string ID = "getAttributeValueNode";

	[SerializeField]
	private Character _character;

	[SerializeField]
	private CharacterAttribute _attribute;

	public override string GetID => "getAttributeValueNode";

	public override Node Create(Vector2 pos)
	{
		GetAttributeValueNode getAttributeValueNode = ScriptableObject.CreateInstance<GetAttributeValueNode>();
		getAttributeValueNode.rect = new Rect(pos.x, pos.y, 175f, 50f);
		getAttributeValueNode.name = "Get Attribute Value";
		getAttributeValueNode.CreateInput("Character", "Character");
		getAttributeValueNode.CreateOutput("Attribute", "Float");
		return getAttributeValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetAttributeValueNode getAttributeValueNode = (GetAttributeValueNode)Create(rect.position + new Vector2(20f, 20f));
		getAttributeValueNode._character = _character;
		getAttributeValueNode._attribute = _attribute;
		return getAttributeValueNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NodeOutputNotFoundException(output, "getAttributeValueNode");
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		float value = _character.RuntimeData.GetAttributeById(_attribute.Id).Value;
		return CastValue<T>(value);
	}
}
