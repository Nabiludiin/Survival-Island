using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Function Nodes/Get Input Value Node", new Type[] { typeof(NodeFunction) })]
public class GetInputValueNode : EventNode
{
	public const string ID = "EE_GetInputValueNode";

	private const string NODE_NAME_PARTIAL = "Get Input: ";

	private const string NODE_NAME = "Get Input Value";

	private const string OUTPUT_VALUE_NAME = "Value";

	[SerializeField]
	private NodeFunctionConnectionDefinition _inputDefinition;

	[SerializeField]
	private bool _outputCreated;

	[SerializeField]
	private string _lastConnectionTypeId = string.Empty;

	public override string GetID => "EE_GetInputValueNode";

	protected override GUIStyle HeaderBox
	{
		get
		{
			GUIStyle headerBox = base.HeaderBox;
			headerBox.normal.textColor = ConnectionTypes.GetTypeData(_inputDefinition.ConnectionTypeId).Color;
			return headerBox;
		}
	}

	protected override GUIStyle HeaderBoxBold
	{
		get
		{
			GUIStyle headerBoxBold = base.HeaderBoxBold;
			headerBoxBold.normal.textColor = ConnectionTypes.GetTypeData(_inputDefinition.ConnectionTypeId).Color;
			return headerBoxBold;
		}
	}

	public override Node Create(Vector2 pos)
	{
		GetInputValueNode getInputValueNode = ScriptableObject.CreateInstance<GetInputValueNode>();
		getInputValueNode.rect = new Rect(pos.x, pos.y, 300f, 250f);
		getInputValueNode.name = "Get Input Value";
		return getInputValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		GetInputValueNode getInputValueNode = (GetInputValueNode)Create(rect.position + new Vector2(20f, 20f));
		getInputValueNode._inputDefinition = new NodeFunctionConnectionDefinition();
		getInputValueNode._inputDefinition.ConnectionName = _inputDefinition.ConnectionName;
		getInputValueNode._inputDefinition.ConnectionTypeId = _inputDefinition.ConnectionTypeId;
		getInputValueNode._outputCreated = false;
		getInputValueNode._lastConnectionTypeId = string.Empty;
		return getInputValueNode;
	}

	protected override void NodeEnable()
	{
	}

	protected override void NodeGUI()
	{
	}

	private void UpdateNodeTitle()
	{
	}

	private void RecreateOutputIfNecessary()
	{
	}

	private void CreateValueOutput()
	{
	}

	private void RemoveValueOutput()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public override T GetValue<T>(int output, NodeCanvas canvas)
	{
		NodeFunction nodeFunction = ParentCanvas as NodeFunction;
		if (nodeFunction == null)
		{
			throw new UnityException("Variable nodeFunction is null. Is this node somehow not added to Node Function type of canvas?");
		}
		return CastValue<T>(nodeFunction.GetInputValue(_inputDefinition.ConnectionName));
	}
}
