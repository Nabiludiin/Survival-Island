using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Is Remedium Rationed Node", new Type[]
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
public class IsRemediumRationedNode : ResourceNode
{
	public const string ID = "EE_IsRemediumRationed";

	private const string IS_RATIONED_OUTPUT_NAME = "Is Rationed";

	private const string NODE_NAME = "Get Available Remedium";

	private const string INPUT_REMEDIUM_NAME = "Remedium";

	private const string INPUT_CHARACTER_NAME = "Character";

	private const int INPUT_REMEDIUM_INDEX = 1;

	private const int INPUT_CHARACTER_INDEX = 0;

	private const int IS_RATIONED_OUTPUT_INDEX = 0;

	[SerializeField]
	private Remedium _remedium;

	[SerializeField]
	private Character _character;

	public override string GetID => "EE_IsRemediumRationed";

	public override Node Create(Vector2 pos)
	{
		IsRemediumRationedNode isRemediumRationedNode = ScriptableObject.CreateInstance<IsRemediumRationedNode>();
		isRemediumRationedNode.rect = new Rect(pos.x, pos.y, 200f, 60f);
		isRemediumRationedNode.name = "Get Available Remedium";
		isRemediumRationedNode.CreateInput("Character", "Character");
		isRemediumRationedNode.CreateInput("Remedium", "Remedium");
		isRemediumRationedNode.CreateOutput("Is Rationed", "Bool");
		return isRemediumRationedNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		IsRemediumRationedNode isRemediumRationedNode = (IsRemediumRationedNode)Create(rect.position + new Vector2(20f, 20f));
		isRemediumRationedNode._remedium = _remedium;
		return isRemediumRationedNode;
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
		GetInputValue(Inputs[0], ref _character, canvas);
		GetInputValue(Inputs[1], ref _remedium, canvas);
		if (output == 0)
		{
			return CastValue<T>(RationingManager.Instance.IsRemediumRationedToCharacter(_character, _remedium));
		}
		throw new NotExistingOutputException(GetID, output);
	}
}
