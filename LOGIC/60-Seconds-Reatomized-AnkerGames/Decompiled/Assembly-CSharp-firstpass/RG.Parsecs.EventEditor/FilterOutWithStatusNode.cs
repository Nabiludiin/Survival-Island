using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Character Nodes/Filter Out With Status Node", new Type[]
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
public class FilterOutWithStatusNode : CharacterNode
{
	public const string ID = "filterOutWithStatusNode";

	private const int OUTPUT_OUT_INDEX = 0;

	private const int CHARACTERS_INPUT_INDEX = 0;

	private const int STATUS_INPUT_INDEX = 1;

	private const string NODE_NAME = "Filter Out With Status";

	private const string OUTPUT_NAME = "Filtered";

	private const string CHARACTERS_INPUT_NAME = "Characters";

	private const string STATUS_INPUT_NAME = "Filtered out status";

	[SerializeField]
	private CharacterStatus _status;

	public override string GetID => "filterOutWithStatusNode";

	public override Node Create(Vector2 pos)
	{
		FilterOutWithStatusNode filterOutWithStatusNode = ScriptableObject.CreateInstance<FilterOutWithStatusNode>();
		filterOutWithStatusNode.rect = new Rect(pos.x, pos.y, 200f, 40f);
		filterOutWithStatusNode.name = "Filter Out With Status";
		filterOutWithStatusNode.CreateInput("Characters", "CharacterList");
		filterOutWithStatusNode.CreateInput("Filtered out status", "CharacterStatusConnection");
		filterOutWithStatusNode.CreateOutput("Filtered", "CharacterList");
		return filterOutWithStatusNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		FilterOutWithStatusNode filterOutWithStatusNode = (FilterOutWithStatusNode)Create(rect.position + new Vector2(20f, 20f));
		filterOutWithStatusNode._status = _status;
		return filterOutWithStatusNode;
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
			throw new NodeOutputNotFoundException(output, "filterOutWithStatusNode");
		}
		List<Character> list = new List<Character>();
		List<Character> inputValue = GetInputValue<List<Character>>(Inputs[0], canvas);
		GetInputValue(Inputs[1], ref _status, canvas);
		for (int i = 0; i < inputValue.Count; i++)
		{
			if (!inputValue[i].RuntimeData.HasStatus(_status.Id))
			{
				list.Add(inputValue[i]);
			}
		}
		return CastValue<T>(list);
	}
}
