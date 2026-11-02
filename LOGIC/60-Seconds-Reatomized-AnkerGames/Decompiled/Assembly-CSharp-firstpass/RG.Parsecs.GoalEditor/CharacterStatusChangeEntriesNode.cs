using System;
using System.Collections.Generic;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Game Stats/Character Status Change/Character Status Change Node", new Type[]
{
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class CharacterStatusChangeEntriesNode : GoalNode
{
	private const string ID = "GE_CharacterStatusChangeEntriesNode";

	private List<CharacterStatusChangeStatsEntry> _result;

	private const int OUTPUT_INDEX = 0;

	private const string OUTPUT_NAME = "Output";

	private const string NODE_NAME = "Status Change Entries";

	public override string GetID => "GE_CharacterStatusChangeEntriesNode";

	public override Node Create(Vector2 pos)
	{
		CharacterStatusChangeEntriesNode characterStatusChangeEntriesNode = ScriptableObject.CreateInstance<CharacterStatusChangeEntriesNode>();
		characterStatusChangeEntriesNode.rect = new Rect(pos.x, pos.y, 200f, 75f);
		characterStatusChangeEntriesNode.name = "Status Change Entries";
		characterStatusChangeEntriesNode.CreateOutput("Output", ListConnection.ID);
		return characterStatusChangeEntriesNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		CharacterStatusChangeEntriesNode characterStatusChangeEntriesNode = (CharacterStatusChangeEntriesNode)Create(rect.position + new Vector2(20f, 20f));
		characterStatusChangeEntriesNode._result = _result;
		return characterStatusChangeEntriesNode;
	}

	protected override void NodeGUI()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		_result = StatsManager.Instance.CharacterStatusChangeStatsEntries;
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		return CastValue<T>(_result);
	}
}
