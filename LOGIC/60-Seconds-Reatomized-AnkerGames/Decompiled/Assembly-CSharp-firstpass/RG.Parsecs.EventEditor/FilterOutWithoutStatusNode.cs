using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Filter Out Without Status Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal)
})]
public class FilterOutWithoutStatusNode : CharacterNode
{
	public const string ID = "filterOutWithoutStatusNode";

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTERS_INPUT_INDEX = 0;

	private const string NODE_NAME = "Filter Out Without Status";

	private const string OUTPUT_NAME = "Filtered";

	private const string CHARACTERS_INPUT_NAME = "Characters";

	private const string STATUS_INPUT_NAME = "Required status";

	private const int STATUS_INPUT_INDEX = 1;

	[SerializeField]
	private CharacterStatus _status;

	public override string GetID => "filterOutWithoutStatusNode";

	public override Node Create(Vector2 pos)
	{
		FilterOutWithoutStatusNode filterOutWithoutStatusNode = ScriptableObject.CreateInstance<FilterOutWithoutStatusNode>();
		filterOutWithoutStatusNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		filterOutWithoutStatusNode.name = "Filter Out Without Status";
		filterOutWithoutStatusNode.CreateInput("Characters", "CharacterList");
		filterOutWithoutStatusNode.CreateInput("Required status", "CharacterStatusConnection");
		filterOutWithoutStatusNode.CreateOutput("Filtered", "CharacterList");
		return filterOutWithoutStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterOutWithoutStatusNode filterOutWithoutStatusNode = (FilterOutWithoutStatusNode)Create(rect.position + new Vector2(20f, 20f));
		filterOutWithoutStatusNode._status = _status;
		return filterOutWithoutStatusNode;
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
			throw new NodeOutputNotFoundException(output, "filterOutWithoutStatusNode");
		}
		List<Character> list = new List<Character>();
		List<Character> inputValue = GetInputValue<List<Character>>(Inputs[0], canvas);
		GetInputValue(Inputs[1], ref _status, canvas);
		for (int i = 0; i < inputValue.Count; i++)
		{
			if (inputValue[i].RuntimeData.HasStatus(_status.Id))
			{
				list.Add(inputValue[i]);
			}
		}
		return CastValue<T>(list);
	}
}
