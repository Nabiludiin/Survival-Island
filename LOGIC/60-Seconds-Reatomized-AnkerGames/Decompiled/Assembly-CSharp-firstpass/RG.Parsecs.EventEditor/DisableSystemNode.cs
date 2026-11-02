using System;
using I2.Loc;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Ship System Nodes/Disable System Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class DisableSystemNode : ResourceNode
{
	public const string ID = "EE_DisableSystemNode";

	[SerializeField]
	private ShipSystem _shipSystem;

	[SerializeField]
	private LocalizedString _disabledSystemDescription;

	[SerializeField]
	private LocalizedString _disabledSystemTitle;

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_SYSTEM_NAME = "Ship System";

	private const string INPUT_TITLE_TERM_NAME = "Title";

	private const string INPUT_DESCRIPTION_TERM_NAME = "Description";

	private const string OUTPUT_OUT_NAME = "Out";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_SYSTEM_INDEX = 1;

	private const int INPUT_TITLE_TERM_INDEX = 2;

	private const int INPUT_DESCRIPTION_TERM_INDEX = 3;

	private const int OUTPUT_OUT_INDEX = 0;

	private const string OUTPUT_NOT_CONNECTED_MESSAGE = "Output is not connected";

	private const string NOT_SET_SYSTEM_MESSAGE = "Node has not set any ship system!";

	private bool _showTermProperty;

	private string _termButtonMessage = "Show";

	public override string GetID => "EE_DisableSystemNode";

	public override Node Create(Vector2 pos)
	{
		DisableSystemNode disableSystemNode = ScriptableObject.CreateInstance<DisableSystemNode>();
		disableSystemNode.rect = new Rect(pos.x, pos.y, 250f, 130f);
		disableSystemNode.name = "Disable System";
		disableSystemNode.CreateMutliInput("In", "Flow");
		disableSystemNode.CreateInput("Ship System", "ShipSystem");
		disableSystemNode.CreateInput("Title", "LocalizedString");
		disableSystemNode.CreateInput("Description", "LocalizedString");
		disableSystemNode.CreateOutput("Out", "Flow");
		return disableSystemNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		DisableSystemNode disableSystemNode = (DisableSystemNode)Create(rect.position + new Vector2(20f, 20f));
		disableSystemNode._shipSystem = _shipSystem;
		return disableSystemNode;
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
		GetInputValue(Inputs[1], ref _shipSystem, canvas);
		GetInputValue(Inputs[2], ref _disabledSystemTitle, canvas);
		GetInputValue(Inputs[3], ref _disabledSystemDescription, canvas);
		_shipSystem.IsAvailable = false;
		_shipSystem.SetDisabledReason(_disabledSystemTitle, _disabledSystemDescription);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
