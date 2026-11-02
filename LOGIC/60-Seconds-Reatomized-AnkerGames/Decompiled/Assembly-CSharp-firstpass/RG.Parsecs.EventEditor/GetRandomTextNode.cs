using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Text Nodes/Get Random Text Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas)
})]
public class GetRandomTextNode : MessageNode
{
	public const string ID = "EE_GetRandomTextNode";

	[SerializeField]
	private int _inputsCounter;

	private const string INPUT_TERM_NAME = "Term ";

	private const string OUTPUT_RESULT_NAME = "Result";

	public override string GetID => "EE_GetRandomTextNode";

	public override Node Create(Vector2 pos)
	{
		GetRandomTextNode getRandomTextNode = ScriptableObject.CreateInstance<GetRandomTextNode>();
		getRandomTextNode.rect = new Rect(pos.x, pos.y, 150f, 200f);
		getRandomTextNode.name = "Get Random Text";
		getRandomTextNode.CreateInput("Term 1", "LocalizedString");
		getRandomTextNode.CreateInput("Term 2", "LocalizedString");
		getRandomTextNode.CreateOutput("Result", "LocalizedString");
		return getRandomTextNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		return Create(rect.position + new Vector2(20f, 20f));
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
		int num = UnityEngine.Random.Range(0, Inputs.Count - 1);
		LocalizedString currentValue = string.Empty;
		GetInputValue(Inputs[num], ref currentValue, canvas);
		return CastValue<T>(currentValue);
	}
}
