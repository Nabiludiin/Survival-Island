using System;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.EventEditor;
using RG.Parsecs.GoalEditor;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

[Node(false, "Utility Nodes/Debug Log Node", new Type[]
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
public class DebugLogNode : FlowNode
{
	public const string ID = "GE_DebugLogNode";

	public const string NODE_NAME = "Debug Log";

	private const int TEXT_INPUT_ID = 1;

	private const string TEXT_INPUT_NAME = "Text";

	private const string OBJECT_INPUT_NOT_IN_USE_WARNING_MESSAGE = "Input {0} is connected but it's not in use. Use {{{1}}} in your Text input.";

	private const string PARAMS_COUNT_DONT_MATCH = "There is to many parameters in string ({0}) than a actual parameters list ({1})";

	[SerializeField]
	[TextArea(5, 10)]
	private string _textInput;

	public override string GetID => "GE_DebugLogNode";

	public override Node Create(Vector2 pos)
	{
		DebugLogNode debugLogNode = ScriptableObject.CreateInstance<DebugLogNode>();
		debugLogNode.rect = new Rect(pos.x, pos.y, 150f, 100f);
		debugLogNode.name = "Debug Log";
		debugLogNode.CreateMutliInput("In", "Flow");
		debugLogNode.CreateInput("Text", "String");
		debugLogNode.CreateInput("Param 0", ObjectConnection.ID);
		debugLogNode.CreateOutput("Out", "Flow");
		return debugLogNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DebugLogNode debugLogNode = (DebugLogNode)Create(rect.position + new Vector2(20f, 20f));
		debugLogNode._textInput = _textInput;
		return debugLogNode;
	}

	private void UpdateInputsCount()
	{
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
		if (Inputs[1].isConnected)
		{
			_textInput = Inputs[1].GetCustomNodeAcrossConnection<ParsecsNode>().GetValueWithErrorHandling<string>(GetConnectedoutput(Inputs[1]), canvas);
		}
		object[] array = new object[Inputs.Count - 2];
		for (int i = 2; i < Inputs.Count; i++)
		{
			array[i - 2] = GetInputValue<object>(Inputs[i], canvas) ?? "NULL";
		}
		Debug.Log(string.Format(_textInput, array));
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
