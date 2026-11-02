using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Get Need Value Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(ConditionEvent)
})]
public class GetNeedValueNode : CharacterNode
{
	private const string OUTPUT_NAME = "Value";

	private const string INPUT_NAME = "Character";

	private const int CHARACTER_INPUT_INDEX = 0;

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Get Need Value";

	public const string ID = "getNeedValueNode";

	[SerializeField]
	private Character _character;

	[SerializeField]
	private CharacterNeed _need;

	public override string GetID => "getNeedValueNode";

	public override Node Create(Vector2 pos)
	{
		GetNeedValueNode getNeedValueNode = ScriptableObject.CreateInstance<GetNeedValueNode>();
		getNeedValueNode.rect = new Rect(pos.x, pos.y, 175f, 50f);
		getNeedValueNode.name = "Get Need Value";
		getNeedValueNode.CreateInput("Character", "Character");
		getNeedValueNode.CreateOutput("Value", "Float");
		return getNeedValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetNeedValueNode getNeedValueNode = (GetNeedValueNode)Create(rect.position + new Vector2(20f, 20f));
		getNeedValueNode._character = _character;
		getNeedValueNode._need = _need;
		return getNeedValueNode;
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
			throw new NodeOutputNotFoundException(output, "getNeedValueNode");
		}
		GetInputValue(Inputs[0], ref _character, canvas);
		float value = _character.RuntimeData.GetNeedById(_need.Id).Value;
		return CastValue<T>(value);
	}
}
