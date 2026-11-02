using System;
using NodeEditorFramework;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EventEditor;

[Node(false, "Function Nodes/Set Output Value Node", new Type[] { typeof(NodeFunction) })]
public class SetOutputValueNode : EventNode
{
	public const string ID = "EE_SetOutputValueNode";

	private const string NODE_NAME_PARTIAL = "Set Output: ";

	private const string NODE_NAME = "Set Output Value";

	private const string INPUT_VALUE_NAME = "Value";

	private const string INPUT_IN_NAME = "In";

	private const string OUTPUT_OUT_NAME = "Out";

	[SerializeField]
	private NodeFunctionConnectionDefinition _outputDefinition;

	[SerializeField]
	private bool _inputCreated;

	[SerializeField]
	private string _lastConnectionTypeId = string.Empty;

	public override string GetID => "EE_SetOutputValueNode";

	protected override GUIStyle HeaderBox
	{
		get
		{
			GUIStyle headerBox = base.HeaderBox;
			headerBox.normal.textColor = ConnectionTypes.GetTypeData(_outputDefinition.ConnectionTypeId).Color;
			return headerBox;
		}
	}

	protected override GUIStyle HeaderBoxBold
	{
		get
		{
			GUIStyle headerBoxBold = base.HeaderBoxBold;
			headerBoxBold.normal.textColor = ConnectionTypes.GetTypeData(_outputDefinition.ConnectionTypeId).Color;
			return headerBoxBold;
		}
	}

	public override Node Create(Vector2 pos)
	{
		SetOutputValueNode setOutputValueNode = ScriptableObject.CreateInstance<SetOutputValueNode>();
		setOutputValueNode.rect = new Rect(pos.x, pos.y, 300f, 250f);
		setOutputValueNode.name = "Set Output Value";
		setOutputValueNode.CreateMutliInput("In", "Flow");
		setOutputValueNode.CreateOutput("Out", "Flow");
		return setOutputValueNode;
	}

	public override Node Duplicate(Vector2 pos)
	{
		SetOutputValueNode setOutputValueNode = (SetOutputValueNode)Create(rect.position + new Vector2(20f, 20f));
		setOutputValueNode._outputDefinition = new NodeFunctionConnectionDefinition();
		setOutputValueNode._outputDefinition.ConnectionName = _outputDefinition.ConnectionName;
		setOutputValueNode._outputDefinition.ConnectionTypeId = _outputDefinition.ConnectionTypeId;
		setOutputValueNode._inputCreated = false;
		setOutputValueNode._lastConnectionTypeId = string.Empty;
		NodeEditorCallbacks.IssueOnAddNode(setOutputValueNode);
		return setOutputValueNode;
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

	private void RecreateInputIfNecessary()
	{
	}

	private void CreateValueInput()
	{
	}

	private void RemoveValueInput()
	{
	}

	protected override void OnNodeValidate()
	{
	}

	public bool HasOutputSet(string outputDefinitionName)
	{
		return true;
	}

	public override void Execute(NodeCanvas canvas)
	{
		NodeFunction obj = ParentCanvas as NodeFunction;
		if (obj == null)
		{
			throw new UnityException("Variable nodeFunction is null. Is this node somehow not added to Node Function type of canvas?");
		}
		obj.SetOutputValue(_outputDefinition.ConnectionName, GetInputValue<object>(Inputs[1], canvas));
		CheckAreAllFlowOutputsConnected();
		Outputs[0].GetCustomNodeAcrossConnection<ParsecsNode>().ExecuteWithErrorHandling(canvas);
	}
}
