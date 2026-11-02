using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Global Stats/Add Stat Node", new Type[]
{
	typeof(Goal),
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(ExpeditionEvent),
	typeof(SystemStatusEvent),
	typeof(EndGameCanvas)
})]
public class AddStatsNode : EventNode
{
	public const string ID = "GE_AddStatsNode";

	public const string NODE_NAME = "Add stat (List)";

	private const int INPUT_FLOW_INDEX = 0;

	private const int INPUT_STAT_NAME_INDEX = 1;

	private const int OUTPUT_FLOW_INDEX = 0;

	private const string INPUT_FLOW_NAME = "In";

	private const string OUTPUT_FLOW_NAME = "Out";

	private const string INPUT_STAT_VALUE = "Value";

	[SerializeField]
	private int _key;

	[SerializeField]
	private string _value;

	private readonly GUIContent[] _options = new GUIContent[3]
	{
		new GUIContent("Planet1Endings"),
		new GUIContent("Planet2Endings"),
		new GUIContent("Planet3Endings")
	};

	private readonly int[] _optionsIndex = new int[3] { 0, 1, 2 };

	public override string GetID => "GE_AddStatsNode";

	public override Node Create(Vector2 pos)
	{
		AddStatsNode addStatsNode = ScriptableObject.CreateInstance<AddStatsNode>();
		addStatsNode.rect = new Rect(pos.x, pos.y, 300f, 200f);
		addStatsNode.name = "Add stat (List)";
		addStatsNode.CreateInput("In", "Flow");
		addStatsNode.CreateInput("Value", "String");
		addStatsNode.CreateOutput("Out", "Flow");
		return addStatsNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		AddStatsNode addStatsNode = (AddStatsNode)Create(pos + new Vector2(20f, 20f));
		addStatsNode._key = _key;
		addStatsNode._value = _value;
		return addStatsNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override void Execute(NodeCanvas canvas)
	{
		GetInputValue(Inputs[1], ref _value, canvas);
		StatsManager.Instance.AddGlobalDataToList(_options[_key].text, _value);
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
