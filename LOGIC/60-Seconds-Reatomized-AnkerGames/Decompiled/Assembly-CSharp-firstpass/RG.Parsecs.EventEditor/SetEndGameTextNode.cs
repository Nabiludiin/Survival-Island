using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Functions/Set Dynamic End Game Text", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetEndGameTextNode : ParsecsNode
{
	public const string ID = "EGC_SetEndGameText";

	private const int FLOW_INPUT_INDEX = 0;

	private const int TEXT_INPUT_INDEX = 1;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string NODE_NAME = "Set dynamic end game text";

	private const string OUTPUT_NAME = "Out";

	private const string FLOW_INPUT_NAME = "In";

	private const string TEXT_INPUT_NAME = "Term";

	private const int OFFSET = 4;

	[SerializeField]
	private LocalizedString _text;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private IItem _item;

	private bool _showTermProperty;

	private string _termButtonMessage = "Show";

	[SerializeField]
	private DynamicEndGameText _endgameText;

	public override string GetID => "EGC_SetEndGameText";

	public override Node Create(Vector2 pos)
	{
		SetEndGameTextNode setEndGameTextNode = ScriptableObject.CreateInstance<SetEndGameTextNode>();
		setEndGameTextNode.rect = new Rect(pos.x, pos.y, 400f, 105f);
		setEndGameTextNode.name = "Set dynamic end game text";
		setEndGameTextNode.CreateMutliInput("In", "Flow");
		setEndGameTextNode.CreateInput("Term", "LocalizedString");
		setEndGameTextNode.CreateOutput("Out", "Flow");
		return setEndGameTextNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetEndGameTextNode setEndGameTextNode = (SetEndGameTextNode)Create(rect.position + new Vector2(20f, 20f));
		setEndGameTextNode._text = _text;
		setEndGameTextNode._character = _character;
		setEndGameTextNode._item = _item;
		return setEndGameTextNode;
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
		CheckAreAllFlowOutputsConnected();
		GetInputValue(Inputs[1], ref _text, canvas);
		_endgameText.RuntimeData.RuntimeValue = _text;
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
