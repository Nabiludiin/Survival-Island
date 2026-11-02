using System;
using NodeEditorFramework;
using RG.Parsecs.Common;
using RG.Parsecs.EndGameEditor;
using RG.Parsecs.GoalEditor;
using RG.Parsecs.NodeEditor;
using RG.Parsecs.Survival;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Supplies Nodes/Resources/Get Resource Value Node", new Type[]
{
	typeof(SurvivalEvent),
	typeof(ReportEvent),
	typeof(SystemEvent),
	typeof(NodeFunction),
	typeof(SystemStatusEvent),
	typeof(ExpeditionEvent),
	typeof(EndGameCanvas),
	typeof(Goal),
	typeof(ConditionEvent)
})]
public class GetResourceValueNode : ResourceNode
{
	public const string ID = "EE_GetResourceValueNode";

	private const string INPUT_RESOURCE_NAME = "Resource";

	private const string OUTPUT_VALUE_NAME = "Value";

	private const string NODE_NAME = "Get Resource Value";

	private const int OUTPUT_VALUE_INDEX = 0;

	private const int INPUT_RESOURCE_INDEX = 0;

	[SerializeField]
	private Resource _resource;

	public override string GetID => "EE_GetResourceValueNode";

	public override Node Create(Vector2 pos)
	{
		GetResourceValueNode getResourceValueNode = ScriptableObject.CreateInstance<GetResourceValueNode>();
		getResourceValueNode.rect = new Rect(pos.x, pos.y, 180f, 40f);
		getResourceValueNode.name = "Get Resource Value";
		getResourceValueNode.CreateInput("Resource", "Resources");
		getResourceValueNode.CreateOutput("Value", "Int");
		return getResourceValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetResourceValueNode getResourceValueNode = (GetResourceValueNode)Create(rect.position + new Vector2(20f, 20f));
		getResourceValueNode._resource = _resource;
		return getResourceValueNode;
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

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		if (output != 0)
		{
			throw new NotExistingOutputException(GetID, output);
		}
		GetInputValue(Inputs[0], ref _resource, canvas);
		PlayerResources playerResources = Singleton<ItemManager>.Instance.GetPlayerResources();
		return CastValue<T>(playerResources.GetResourceValue(_resource));
	}
}
