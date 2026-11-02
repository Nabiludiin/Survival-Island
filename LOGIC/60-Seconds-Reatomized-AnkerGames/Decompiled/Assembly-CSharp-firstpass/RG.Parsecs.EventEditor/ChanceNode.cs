using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Flow Nodes/Chance Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class ChanceNode : FlowNode
{
	public const string ID = "GE_ChanceNode";

	[SerializeField]
	private List<int> _values;

	private const string NODE_NAME = "Chance";

	public override string GetID => "GE_ChanceNode";

	public override Node Create(Vector2 pos)
	{
		ChanceNode chanceNode = ScriptableObject.CreateInstance<ChanceNode>();
		chanceNode.rect = new Rect(pos.x, pos.y, 200f, 105f);
		chanceNode.name = "Chance";
		chanceNode._values = new List<int>();
		chanceNode._values.Add(50);
		chanceNode._values.Add(50);
		chanceNode.CreateMutliInput("In", "Flow");
		chanceNode.CreateOutputs();
		return chanceNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		ChanceNode chanceNode = ScriptableObject.CreateInstance<ChanceNode>();
		chanceNode.rect = new Rect(pos.x, pos.y, 200f, 105f);
		chanceNode.name = "Chance";
		chanceNode._values = new List<int>(_values);
		chanceNode.CreateMutliInput("In", "Flow");
		chanceNode.CreateOutputs();
		return chanceNode;
	}

	private void CreateOutputs()
	{
	}

	private void UpdateOutputs()
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
		float num = UnityEngine.Random.Range(0f, _values.Sum());
		int num2 = 0;
		for (int i = 0; i < _values.Count; i++)
		{
			num2 += _values[i];
			if ((float)num2 >= num)
			{
				if (!Outputs[i].isConnected)
				{
					throw new NotExistingOutputException(GetID, i);
				}
				Outputs[i].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
				return;
			}
		}
		Outputs[Outputs.Count - 1].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
