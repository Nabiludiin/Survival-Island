using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Player Input/Make Personality Choice Node", new Type[] { typeof(SurvivalEvent) })]
public class MakePersonalityChoiceNode : PlayerDecisionNode
{
	private const string ID = "EE_MakePersonalityChoiceNode";

	private const string INPUT_INCREASE_AMOUNT_NAME = "Increase";

	private const string INPUT_DECREASE_AMOUNT_NAME = "Decrease";

	private const int INPUT_INCREASE_AMOUNT_INDEX = 1;

	private const int INPUT_DECREASE_AMOUNT_INDEX = 2;

	private const string NODE_NAME = "Make Personality Choice";

	[SerializeField]
	private float _increaseAmount;

	[SerializeField]
	private float _decreaseAmount;

	[SerializeField]
	private PersonalityProfile _personality;

	[SerializeField]
	private CharacterNeed _need;

	public override string GetID => "EE_MakePersonalityChoiceNode";

	public override Node Create(Vector2 pos)
	{
		MakePersonalityChoiceNode makePersonalityChoiceNode = ScriptableObject.CreateInstance<MakePersonalityChoiceNode>();
		makePersonalityChoiceNode.rect = new Rect(pos.x, pos.y, 225f, 160f);
		makePersonalityChoiceNode.name = "Make Personality Choice";
		makePersonalityChoiceNode.CreateMutliInput("In", "Flow");
		makePersonalityChoiceNode.CreateInput("Increase", "Float");
		makePersonalityChoiceNode.CreateInput("Decrease", "Float");
		makePersonalityChoiceNode.CreateOutput("Out", "Flow");
		return makePersonalityChoiceNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
	}

	protected override void OnNodeValidate()
	{
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _increaseAmount, canvas);
		GetInputValue(Inputs[2], ref _decreaseAmount, canvas);
		List<Character> liveCharacters = CharacterManager.Instance.GetCharacterList().GetLiveCharacters();
		for (int i = 0; i < liveCharacters.Count; i++)
		{
			UpdateNeed(liveCharacters[i]);
		}
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}

	private void UpdateNeed(Character character)
	{
		float num = 0f;
		num = character.StaticData.GetAttitudeTowardsPersonalityProfile(_personality) switch
		{
			AttitudeTowardsPersonalityProfile.POSITIVE => _increaseAmount, 
			AttitudeTowardsPersonalityProfile.NEGATIVE => 0f - _decreaseAmount, 
			_ => 0f, 
		};
		character.RuntimeData.ChangeNeedValue(_need.Id, num);
	}
}
