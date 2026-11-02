using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Resources/Set Resource Value Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent)
})]
public class SetResourceValueNode : ResourceNode
{
	public const string ID = "EE_SetResourceValueNode";

	private const string INPUT_IN_NAME = "In";

	private const string INPUT_RESOURCE_NAME = "Resource";

	private const string INPUT_VALUE_NAME = "Value";

	private const string OUTPUT_OUT_NAME = "Out";

	private const string NODE_NAME = "Set Resource Value";

	private const int INPUT_IN_INDEX = 0;

	private const int INPUT_RESOURCE_INDEX = 1;

	private const int INPUT_VALUE_INDEX = 2;

	private const int OUTPUT_OUT_INDEX = 0;

	[SerializeField]
	private Resource _resource;

	[SerializeField]
	private int _value;

	public override string GetID => "EE_SetResourceValueNode";

	public override Node Create(Vector2 pos)
	{
		SetResourceValueNode setResourceValueNode = ScriptableObject.CreateInstance<SetResourceValueNode>();
		setResourceValueNode.rect = new Rect(pos.x, pos.y, 180f, 130f);
		setResourceValueNode.name = "Set Resource Value";
		setResourceValueNode.CreateMutliInput("In", "Flow");
		setResourceValueNode.CreateInput("Resource", "Resources");
		setResourceValueNode.CreateInput("Value", "Int");
		setResourceValueNode.CreateOutput("Out", "Flow");
		return setResourceValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetResourceValueNode setResourceValueNode = (SetResourceValueNode)Create(rect.position + new Vector2(20f, 20f));
		setResourceValueNode._value = _value;
		setResourceValueNode._resource = _resource;
		return setResourceValueNode;
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
		GetInputValue(Inputs[1], ref _resource, canvas);
		GetInputValue(Inputs[2], ref _value, canvas);
		Singleton<ItemManager>.Instance.GetPlayerResources().SetResource(_resource, _value);
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
