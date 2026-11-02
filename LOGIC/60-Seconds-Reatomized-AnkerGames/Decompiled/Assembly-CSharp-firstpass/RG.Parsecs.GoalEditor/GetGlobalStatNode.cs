using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.GoalEditor;

[Node(false, "Global Stats/GetGlobalStatNode", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetGlobalStatNode : GoalNode
{
	public const string ID = "GE_GetGlobaStatNode";

	private const string OUTOUP_NAME = "Value";

	private const int OUTPUT_INDEX = 0;

	private const string NODE_NAME = "Get Global Stats";

	private readonly GUIContent[] _options = new GUIContent[33]
	{
		new GUIContent("SurvivedDays"),
		new GUIContent("CraftedItems"),
		new GUIContent("RecycledItems"),
		new GUIContent("RepairedItems"),
		new GUIContent("EatenSoupRations"),
		new GUIContent("UsedSockPuppets"),
		new GUIContent("CraftedSockPuppets"),
		new GUIContent("ExpeditionsMade"),
		new GUIContent("ExpeditionsSuccessful"),
		new GUIContent("ItemsCollectedOnExpeditions"),
		new GUIContent("ItemsCollectedInScavengeMode"),
		new GUIContent("CharactersPerishedInSurvivalMode"),
		new GUIContent("TestsMade"),
		new GUIContent("TestsPassed"),
		new GUIContent("TestsCrewMade"),
		new GUIContent("TestsCrewPassed"),
		new GUIContent("TestsStrengthMade"),
		new GUIContent("TestsStrengthPassed"),
		new GUIContent("TestsAgilityMade"),
		new GUIContent("TestsAgilityPassed"),
		new GUIContent("TestsWitsMade"),
		new GUIContent("TestsWitsPassed"),
		new GUIContent("ScavengedSoups"),
		new GUIContent("ScavengedItems"),
		new GUIContent("SuccessfulEvacuations"),
		new GUIContent("TotalTryEvacuations"),
		new GUIContent("TotalCharactersDeadOnScavenge"),
		new GUIContent("Planet1_dst_discovered"),
		new GUIContent("Planet2_dst_discovered"),
		new GUIContent("Planet3_dst_discovered"),
		new GUIContent("ScavengedItemsIDs"),
		new GUIContent("UniqueUpgradedItemList"),
		new GUIContent("UniqueUpgradedSystemList")
	};

	private readonly int[] _optionsIndex = new int[33]
	{
		0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
		10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
		20, 21, 22, 23, 24, 25, 26, 27, 28, 29,
		30, 31, 32
	};

	[SerializeField]
	private int _key;

	public override string GetID => "GE_GetGlobaStatNode";

	public override Node Create(Vector2 pos)
	{
		GetGlobalStatNode getGlobalStatNode = ScriptableObject.CreateInstance<GetGlobalStatNode>();
		getGlobalStatNode.name = "Get Global Stats";
		getGlobalStatNode.rect = new Rect(pos.x, pos.y, 300f, 100f);
		getGlobalStatNode.CreateOutput("Value", "Int");
		return getGlobalStatNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetGlobalStatNode getGlobalStatNode = (GetGlobalStatNode)Create(pos + new Vector2(20f, 20f));
		getGlobalStatNode._key = _key;
		return getGlobalStatNode;
	}

	protected override void NodeEnable()
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
		return CastValue<T>(StatsManager.Instance.GetGlobalData(_options[_key].text));
	}
}
