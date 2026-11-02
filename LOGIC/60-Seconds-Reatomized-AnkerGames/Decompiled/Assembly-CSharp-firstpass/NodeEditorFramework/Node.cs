using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework;

public abstract class Node : ScriptableObject
{
	public enum EMessageType
	{
		INFO,
		WARNING,
		ERROR
	}

	private const int INDEX_UNDEFINED = -1;

	public Rect rect;

	internal Vector2 contentOffset = new Vector2(10f, 0f);

	[SerializeField]
	public List<NodeKnob> nodeKnobs = new List<NodeKnob>();

	[NonSerialized]
	public Vector2 drag;

	[SerializeField]
	public List<NodeInput> Inputs = new List<NodeInput>();

	[SerializeField]
	public List<NodeOutput> Outputs = new List<NodeOutput>();

	[SerializeField]
	protected int NodeVersion = 1;

	[SerializeField]
	private Dictionary<int, string> _infoMessages = new Dictionary<int, string>();

	[SerializeField]
	private Dictionary<int, string> _warningMessages = new Dictionary<int, string>();

	[SerializeField]
	private Dictionary<int, string> _errorMessages = new Dictionary<int, string>();

	[SerializeField]
	private List<string> _recreationLog = new List<string>();

	[NonSerialized]
	protected NodeCanvas parentCanvas;

	[NonSerialized]
	protected int index = -1;

	private int _availableMessageId;

	private int _lastAmountOfMessages;

	private bool _updatedLog;

	private bool _onValidate;

	private List<NodeInput> _recreatedInputs;

	private List<NodeOutput> _recreatedOutputs;

	[NonSerialized]
	private List<Node> recursiveSearchSurpassed;

	[NonSerialized]
	private Node startRecursiveSearchNode;

	public NodeCanvas ParentCanvas
	{
		get
		{
			return parentCanvas;
		}
		set
		{
			parentCanvas = value;
		}
	}

	protected virtual GUIStyle HeaderBox => NodeEditorGUI.nodeBox;

	protected virtual GUIStyle HeaderBoxBold => NodeEditorGUI.nodeBoxBold;

	public abstract string GetID { get; }

	public virtual bool AllowRecursion => false;

	public virtual bool ContinueCalculation => true;

	protected virtual void InitBase()
	{
		if (!NodeEditor.curNodeCanvas.nodes.Contains(this))
		{
			NodeEditor.curNodeCanvas.nodes.Add(this);
		}
		NodeEditor.RepaintClients();
		AssignNodeCanvas(NodeEditor.curNodeCanvas);
	}

	public void AssignNodeCanvas(NodeCanvas canvas)
	{
		parentCanvas = canvas;
	}

	public abstract Node Duplicate(Vector2 pos);

	public void Delete()
	{
		if (!NodeEditor.curNodeCanvas.nodes.Contains(this))
		{
			throw new UnityException("The Node " + name + " does not exist on the Canvas " + NodeEditor.curNodeCanvas.name + "!");
		}
		NodeEditorCallbacks.IssueOnDeleteNode(this);
		NodeEditor.curNodeCanvas.nodes.Remove(this);
		for (int i = 0; i < Outputs.Count; i++)
		{
			NodeOutput nodeOutput = Outputs[i];
			while (nodeOutput.connections.Count != 0)
			{
				if (nodeOutput.connections[0] is NodeMultiInput)
				{
					(nodeOutput.connections[0] as NodeMultiInput).RemoveSingleConnection(nodeOutput);
				}
				else
				{
					nodeOutput.connections[0].RemoveConnection();
				}
			}
			UnityEngine.Object.DestroyImmediate(nodeOutput, allowDestroyingAssets: true);
		}
		int num = 0;
		while (num < Inputs.Count)
		{
			NodeInput nodeInput = Inputs[num];
			if (nodeInput is NodeMultiInput)
			{
				((NodeMultiInput)nodeInput).Delete();
			}
			else
			{
				num++;
			}
			if (nodeInput.connection != null)
			{
				nodeInput.connection.connections.Remove(nodeInput);
				if (nodeInput.connection.connections.Count <= 0)
				{
					nodeInput.connection.isConnected = false;
					nodeInput.connection.ReloadKnobTexture();
				}
			}
			UnityEngine.Object.DestroyImmediate(nodeInput, allowDestroyingAssets: true);
		}
		for (int j = 0; j < nodeKnobs.Count; j++)
		{
			if (nodeKnobs[j] != null)
			{
				UnityEngine.Object.DestroyImmediate(nodeKnobs[j], allowDestroyingAssets: true);
			}
		}
		UnityEngine.Object.DestroyImmediate(this, allowDestroyingAssets: true);
	}

	public static Node Create(string nodeID, Vector2 position)
	{
		return Create(nodeID, position, null);
	}

	public static Node Create(string nodeID, Vector2 position, NodeOutput connectingOutput)
	{
		Node defaultNode = NodeTypes.getDefaultNode(nodeID);
		if (defaultNode == null)
		{
			throw new UnityException("Cannot create Node with id " + nodeID + " as no such Node type is registered!");
		}
		defaultNode = defaultNode.Create(position);
		defaultNode.InitBase();
		if (connectingOutput != null)
		{
			foreach (NodeInput input in defaultNode.Inputs)
			{
				if (input is NodeMultiInput)
				{
					if (((NodeMultiInput)input).TryApplyConnection(connectingOutput))
					{
						break;
					}
				}
				else if (input.TryApplyConnection(connectingOutput))
				{
					break;
				}
			}
		}
		NodeEditorCallbacks.IssueOnAddNode(defaultNode);
		return defaultNode;
	}

	public static Node Duplicate(Node baseNode, Vector2 position, NodeOutput connectingOutput)
	{
		if (baseNode == null)
		{
			throw new UnityException("Cannot create Node with id " + baseNode.GetID + " as no such Node type is registered!");
		}
		Node node = baseNode.Duplicate(position);
		node.InitBase();
		NodeEditorCallbacks.IssueOnAddNode(node);
		return node;
	}

	internal void CheckNodeKnobMigration()
	{
		if (nodeKnobs.Count == 0 && (Inputs.Count != 0 || Outputs.Count != 0))
		{
			nodeKnobs.AddRange(Inputs.Cast<NodeKnob>());
			nodeKnobs.AddRange(Outputs.Cast<NodeKnob>());
		}
	}

	public abstract Node Create(Vector2 pos);

	protected abstract void NodeGUI();

	public virtual void DrawNodePropertyEditor()
	{
	}

	protected internal virtual void OnDelete()
	{
	}

	protected internal virtual void OnAddInputConnection(NodeInput input)
	{
	}

	protected internal virtual void OnAddOutputConnection(NodeOutput output)
	{
	}

	protected internal virtual void OnDeleteInputConnection(NodeInput input)
	{
	}

	protected internal virtual void OnDeleteOutputConnection(NodeOutput output, NodeInput input)
	{
	}

	public virtual ScriptableObject[] GetScriptableObjects()
	{
		return new ScriptableObject[0];
	}

	protected internal virtual void CopyScriptableObjects(Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
	{
	}

	public void SerializeInputsAndOutputs(Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
	{
	}

	protected void OnEnable()
	{
		_infoMessages.Clear();
		_warningMessages.Clear();
		_errorMessages.Clear();
		_availableMessageId = 0;
		_lastAmountOfMessages = 0;
		NodeEnable();
	}

	public void SetNodeNumber(NodeCanvas canvas)
	{
		index = GetNodeNumber(force: false, canvas);
	}

	protected int GetNodeNumber(bool force = false, NodeCanvas canvas = null)
	{
		if (canvas == null)
		{
			return index;
		}
		if ((index == -1) | force)
		{
			for (int i = 0; i < canvas.nodes.Count; i++)
			{
				if (canvas.nodes[i] == this)
				{
					return i;
				}
			}
		}
		return index;
	}

	protected virtual void NodeEnable()
	{
	}

	protected internal virtual void DrawNode()
	{
		if (index == -1)
		{
			SetNodeNumber(NodeEditor.curNodeCanvas);
		}
		_availableMessageId = 0;
		_onValidate = true;
		OnNodeValidate();
		_onValidate = false;
		Rect rect = this.rect;
		rect.position += NodeEditor.curEditorState.zoomPanAdjust + NodeEditor.curEditorState.panOffset;
		contentOffset = new Vector2(0f, 20f);
		Rect position = new Rect(rect.x, rect.y, rect.width, contentOffset.y);
		Rect rect2 = new Rect(position.x - 7f, position.y - contentOffset.y - 7f, 50f, 50f);
		DisplayActionIcon(rect2);
		int num = 1 + index.ToString().Length;
		GUI.Label(new Rect(position.x + position.width - (float)(9 * num), position.y - 35f, 50f, 50f), $"#{index}");
		GUI.Label(position, name, NodeEditor.curEditorState.selectedNodes.Contains(this) ? HeaderBoxBold : HeaderBox);
		Rect rect3 = new Rect(rect.x, rect.y + contentOffset.y, rect.width, rect.height - contentOffset.y);
		GUI.BeginGroup(rect3, GUI.skin.box);
		rect3.position = Vector2.zero;
		GUILayout.BeginArea(rect3, GUI.skin.box);
		GUI.changed = false;
		try
		{
			if (!IsNodeValid())
			{
				Recreate();
			}
		}
		catch (Exception ex)
		{
			AddRecreationLogEntry($"Caught {ex.GetType()} during node recreation: {ex.Message}", EMessageType.ERROR);
			NodeVersion = GetCurrentNodeVersion();
		}
		try
		{
			NodeGUI();
		}
		catch (Exception ex2)
		{
			Debug.LogError($"Drawing exception: {ex2.Message}, in {GetID}");
		}
		GUILayout.EndArea();
		GUI.EndGroup();
		ClearUnusedMessages();
		LogAllMessagesToConsole();
		_lastAmountOfMessages = _availableMessageId;
	}

	protected virtual void ClearUnusedMessages()
	{
		if (_availableMessageId > _lastAmountOfMessages)
		{
			return;
		}
		for (int i = _availableMessageId; i < _lastAmountOfMessages; i++)
		{
			if (_infoMessages.ContainsKey(i))
			{
				_infoMessages.Remove(i);
			}
			if (_warningMessages.ContainsKey(i))
			{
				_warningMessages.Remove(i);
			}
			if (_errorMessages.ContainsKey(i))
			{
				_errorMessages.Remove(i);
			}
		}
	}

	protected virtual void OnNodeValidate()
	{
	}

	private void DisplayActionIcon(Rect rect)
	{
		if (_errorMessages.Count > 0 || _warningMessages.Count > 0 || _infoMessages.Count > 0)
		{
			Rect rect2 = rect;
			if (_errorMessages.Count > 0)
			{
				GUI.Label(new Rect(rect2.position.x, rect2.position.y, 50f, 50f), new GUIContent(NodeEditorGUI.errorIcon));
				GUI.Label(new Rect(rect2.position.x + 30f, rect2.position.y, 50f, 50f), new GUIContent(_errorMessages.Count.ToString()));
				rect2.position = new Vector2(rect2.position.x + 35f, rect2.position.y);
			}
			if (_warningMessages.Count > 0)
			{
				GUI.Label(new Rect(rect2.position.x, rect2.position.y, 50f, 50f), new GUIContent(NodeEditorGUI.warningIcon));
				GUI.Label(new Rect(rect2.position.x + 30f, rect2.position.y, 50f, 50f), new GUIContent(_warningMessages.Count.ToString()));
				rect2.position = new Vector2(rect2.position.x + 35f, rect2.position.y);
			}
			if (_infoMessages.Count > 0)
			{
				GUI.Label(new Rect(rect2.position.x, rect2.position.y, 50f, 50f), new GUIContent(NodeEditorGUI.infoIcon));
				GUI.Label(new Rect(rect2.position.x + 30f, rect2.position.y, 50f, 50f), new GUIContent(_infoMessages.Count.ToString()));
			}
		}
	}

	private void LogAllMessagesToConsole(bool force = false)
	{
		if (!_updatedLog && !force)
		{
			return;
		}
		foreach (KeyValuePair<int, string> errorMessage in _errorMessages)
		{
			Debug.LogError(errorMessage.Value);
		}
		foreach (KeyValuePair<int, string> warningMessage in _warningMessages)
		{
			Debug.LogWarning(warningMessage.Value);
		}
		foreach (KeyValuePair<int, string> infoMessage in _infoMessages)
		{
			Debug.Log(infoMessage.Value);
		}
		_updatedLog = false;
	}

	public void LogMessage(string message, EMessageType messageType)
	{
		if (!_onValidate)
		{
			Debug.LogError("This function needs to be invoked inside OnNodeValidate");
		}
		if (!CanAddNewMessage(message, messageType))
		{
			_availableMessageId++;
			return;
		}
		switch (messageType)
		{
		case EMessageType.INFO:
			_infoMessages.Add(_availableMessageId, message);
			break;
		case EMessageType.WARNING:
			_warningMessages.Add(_availableMessageId, message);
			break;
		case EMessageType.ERROR:
			_errorMessages.Add(_availableMessageId, message);
			break;
		default:
			throw new ArgumentOutOfRangeException("messageType", messageType, null);
		}
		_updatedLog = true;
		_availableMessageId++;
	}

	private bool CanAddNewMessage(string message, EMessageType messageType)
	{
		if (RemoveIfContainsDifferent(message, messageType, _infoMessages, EMessageType.INFO) && RemoveIfContainsDifferent(message, messageType, _warningMessages, EMessageType.WARNING))
		{
			return RemoveIfContainsDifferent(message, messageType, _errorMessages, EMessageType.ERROR);
		}
		return false;
	}

	private bool RemoveIfContainsDifferent(string message, EMessageType messageType, Dictionary<int, string> messageList, EMessageType expectedMessageType)
	{
		if (messageList.ContainsKey(_availableMessageId))
		{
			if (messageType != expectedMessageType)
			{
				messageList.Remove(_availableMessageId);
				return true;
			}
			if (messageType == expectedMessageType && !messageList[_availableMessageId].Equals(message))
			{
				messageList.Remove(_availableMessageId);
				return true;
			}
			return false;
		}
		return true;
	}

	protected virtual bool IsNodeValid()
	{
		return NodeVersion >= GetCurrentNodeVersion();
	}

	protected virtual void OnRecreate(Vector2 pos)
	{
	}

	public void RecreateInput(string knobName, string type, string previousName = null)
	{
		AddRecreationLogEntry($"Recreating input {knobName} in {GetID}", EMessageType.INFO);
		bool flag = false;
		string text = ((!string.IsNullOrEmpty(previousName)) ? previousName : knobName);
		for (int i = 0; i < Inputs.Count; i++)
		{
			if (!_recreatedInputs.Contains(Inputs[i]) && Inputs[i].name == text && Inputs[i].typeID == type)
			{
				Inputs[i].name = knobName;
				_recreatedInputs.Add(Inputs[i]);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AddRecreationLogEntry($"Input {knobName} in {GetID} was not found, creating new input.", EMessageType.INFO);
			NodeInput nodeInput = ScriptableObject.CreateInstance<NodeInput>();
			nodeInput.typeID = type;
			nodeInput.InitBase(this, NodeSide.Left, 20f, knobName);
			_recreatedInputs.Add(nodeInput);
		}
	}

	public void RecreateMultiInput(string knobName, string type, string previousName = null)
	{
		AddRecreationLogEntry($"Recreating multi input {knobName} in {GetID}", EMessageType.INFO);
		bool flag = false;
		string text = ((!string.IsNullOrEmpty(previousName)) ? previousName : knobName);
		for (int i = 0; i < Inputs.Count; i++)
		{
			if (!_recreatedInputs.Contains(Inputs[i]) && Inputs[i].name == text && Inputs[i].typeID == type && Inputs[i] is NodeMultiInput)
			{
				Inputs[i].name = knobName;
				_recreatedInputs.Add(Inputs[i]);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AddRecreationLogEntry($"Multi {knobName} in {GetID} was not found, creating new input.", EMessageType.INFO);
			NodeInput nodeInput = ScriptableObject.CreateInstance<NodeMultiInput>();
			nodeInput.typeID = type;
			nodeInput.InitBase(this, NodeSide.Left, 20f, knobName);
			_recreatedInputs.Add(nodeInput);
		}
	}

	public void RecreateOutput(string knobName, string type, string previousName = null)
	{
		AddRecreationLogEntry($"Recreating output {knobName} in {GetID}", EMessageType.INFO);
		bool flag = false;
		string text = ((!string.IsNullOrEmpty(previousName)) ? previousName : knobName);
		for (int i = 0; i < Outputs.Count; i++)
		{
			if (!_recreatedOutputs.Contains(Outputs[i]) && Outputs[i].name == text && Outputs[i].typeID == type)
			{
				Outputs[i].name = knobName;
				_recreatedOutputs.Add(Outputs[i]);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			AddRecreationLogEntry($"Output {knobName} in {GetID} was not found, creating new output.", EMessageType.INFO);
			NodeOutput nodeOutput = ScriptableObject.CreateInstance<NodeOutput>();
			nodeOutput.typeID = type;
			nodeOutput.InitBase(this, NodeSide.Right, 20f, knobName);
			_recreatedOutputs.Add(nodeOutput);
		}
	}

	private void RemoveUnusedInputs()
	{
		List<NodeInput> list = new List<NodeInput>();
		for (int i = 0; i < Inputs.Count; i++)
		{
			if (!_recreatedInputs.Contains(Inputs[i]))
			{
				list.Add(Inputs[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].isConnected)
			{
				AddRecreationLogEntry($"Removing knob with connection, {list[j].name} in {GetID}", EMessageType.WARNING);
			}
			else
			{
				AddRecreationLogEntry($"Removing knob {list[j].name} in {GetID}", EMessageType.INFO);
			}
			list[j].RemoveConnection();
			Inputs.Remove(list[j]);
			nodeKnobs.Remove(list[j]);
		}
	}

	private void RemoveUnusedOutputs()
	{
		List<NodeOutput> list = new List<NodeOutput>();
		for (int i = 0; i < Outputs.Count; i++)
		{
			if (!_recreatedOutputs.Contains(Outputs[i]))
			{
				list.Add(Outputs[i]);
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].isConnected)
			{
				AddRecreationLogEntry($"Removing knob with connection, {list[j].name} in {GetID}", EMessageType.WARNING);
			}
			else
			{
				AddRecreationLogEntry($"Removing knob {list[j].name} in {GetID}", EMessageType.INFO);
			}
			for (int k = 0; k < list[j].connections.Count; k++)
			{
				list[j].connections[k].RemoveConnection();
			}
			Outputs.Remove(list[j]);
			nodeKnobs.Remove(list[j]);
		}
	}

	private void Recreate()
	{
		_recreationLog = new List<string>();
		AddRecreationLogEntry($"Recreation of node {GetID} started", EMessageType.INFO);
		_recreatedInputs = new List<NodeInput>();
		_recreatedOutputs = new List<NodeOutput>();
		OnRecreate(rect.position);
		AddRecreationLogEntry($"Removing unused knobs", EMessageType.INFO);
		RemoveUnusedInputs();
		RemoveUnusedOutputs();
		Inputs = _recreatedInputs;
		Outputs = _recreatedOutputs;
		_recreatedInputs = null;
		_recreatedOutputs = null;
		AddRecreationLogEntry($"Recreation of node {GetID} finished", EMessageType.INFO);
		NodeVersion = GetCurrentNodeVersion();
	}

	private void AddRecreationLogEntry(string message, EMessageType messageType)
	{
		string text = "";
		switch (messageType)
		{
		case EMessageType.INFO:
			text = "INFO: ";
			break;
		case EMessageType.WARNING:
			text = "WARNING: ";
			Debug.LogWarning(message);
			break;
		case EMessageType.ERROR:
			text = "ERROR: ";
			Debug.LogError(message);
			break;
		default:
			throw new ArgumentOutOfRangeException("messageType", messageType, null);
		}
		_recreationLog.Add(text + message);
	}

	protected virtual int GetCurrentNodeVersion()
	{
		return 1;
	}

	protected internal virtual void DrawKnobs()
	{
		CheckNodeKnobMigration();
		for (int i = 0; i < nodeKnobs.Count; i++)
		{
			nodeKnobs[i].DrawKnob();
		}
	}

	protected internal virtual void DrawConnections()
	{
		CheckNodeKnobMigration();
		if (Event.current.type != EventType.Repaint)
		{
			return;
		}
		for (int i = 0; i < Outputs.Count; i++)
		{
			NodeOutput nodeOutput = Outputs[i];
			Vector2 center = nodeOutput.GetGUIKnob().center;
			Vector2 direction = nodeOutput.GetDirection();
			for (int j = 0; j < nodeOutput.connections.Count; j++)
			{
				NodeInput nodeInput = nodeOutput.connections[j];
				NodeEditorGUI.DrawConnection(center, direction, nodeInput.GetGUIKnob().center, nodeInput.GetDirection(), nodeOutput.typeData.Color);
			}
		}
	}

	public NodeOutput CreateOutput(string outputName, string outputType)
	{
		return NodeOutput.Create(this, outputName, outputType);
	}

	public NodeOutput CreateOutput(string outputName, string outputType, NodeSide nodeSide)
	{
		return NodeOutput.Create(this, outputName, outputType, nodeSide);
	}

	public NodeOutput CreateOutput(string outputName, string outputType, NodeSide nodeSide, float sidePosition)
	{
		return NodeOutput.Create(this, outputName, outputType, nodeSide, sidePosition);
	}

	protected void OutputKnob(int outputIdx)
	{
		if (Event.current.type == EventType.Repaint)
		{
			Outputs[outputIdx].SetPosition();
		}
	}

	public NodeInput CreateInput(string inputName, string inputType)
	{
		return NodeInput.Create(this, inputName, inputType);
	}

	public NodeInput CreateInput(string inputName, string inputType, NodeSide nodeSide)
	{
		return NodeInput.Create(this, inputName, inputType, nodeSide);
	}

	public NodeInput CreateInput(string inputName, string inputType, NodeSide nodeSide, float sidePosition)
	{
		return NodeInput.Create(this, inputName, inputType, nodeSide, sidePosition);
	}

	public NodeInput CreateMutliInput(string inputName, string inputType)
	{
		return NodeMultiInput.Create(this, inputName, inputType);
	}

	public NodeInput CreateMutliInput(string inputName, string inputType, NodeSide nodeSide)
	{
		return NodeMultiInput.Create(this, inputName, inputType, nodeSide);
	}

	public NodeInput CreateMutliInput(string inputName, string inputType, NodeSide nodeSide, float sidePosition)
	{
		return NodeMultiInput.Create(this, inputName, inputType, nodeSide, sidePosition);
	}

	protected void InputKnob(int inputIdx)
	{
		if (Event.current.type == EventType.Repaint)
		{
			Inputs[inputIdx].SetPosition();
		}
	}

	protected static void ReassignOutputType(ref NodeOutput output, Type newOutputType)
	{
		Node body = output.body;
		string outputName = output.name;
		IEnumerable<NodeInput> enumerable = output.connections.Where((NodeInput connection) => connection.typeData.Type.IsAssignableFrom(newOutputType));
		output.Delete();
		NodeEditorCallbacks.IssueOnAddNodeKnob(NodeOutput.Create(body, outputName, newOutputType.AssemblyQualifiedName));
		output = body.Outputs[body.Outputs.Count - 1];
		foreach (NodeInput item in enumerable)
		{
			if (item is NodeMultiInput)
			{
				((NodeMultiInput)item).ApplyConnection(output);
			}
			else
			{
				item.ApplyConnection(output);
			}
		}
	}

	protected static void ReassignInputType(ref NodeInput input, Type newInputType)
	{
		Node body = input.body;
		string text = input.name;
		NodeOutput output = null;
		if (input is NodeMultiInput)
		{
			NodeMultiInput nodeMultiInput = input as NodeMultiInput;
			{
				foreach (NodeOutput connection in nodeMultiInput.connections)
				{
					if (!(connection == null))
					{
						if (newInputType.IsAssignableFrom(input.connection.typeData.Type))
						{
							output = connection;
						}
						nodeMultiInput.Delete();
						NodeEditorCallbacks.IssueOnAddNodeKnob(NodeMultiInput.Create(body, text, newInputType.AssemblyQualifiedName));
						input = body.Inputs[body.Inputs.Count - 1];
						if (input is NodeMultiInput)
						{
							((NodeMultiInput)input).ApplyConnection(output);
						}
						else
						{
							input.ApplyConnection(output);
						}
					}
				}
				return;
			}
		}
		if (input.connection != null && newInputType.IsAssignableFrom(input.connection.typeData.Type))
		{
			output = input.connection;
		}
		input.Delete();
		NodeEditorCallbacks.IssueOnAddNodeKnob(NodeInput.Create(body, text, newInputType.AssemblyQualifiedName));
		input = body.Inputs[body.Inputs.Count - 1];
		input.ApplyConnection(output);
	}

	public bool isChildOf(Node otherNode)
	{
		if (otherNode == null || otherNode == this)
		{
			return false;
		}
		if (BeginRecursiveSearchLoop())
		{
			return false;
		}
		for (int i = 0; i < Inputs.Count; i++)
		{
			NodeOutput connection = Inputs[i].connection;
			if (connection != null && connection.body != startRecursiveSearchNode && (connection.body == otherNode || connection.body.isChildOf(otherNode)))
			{
				StopRecursiveSearchLoop();
				return true;
			}
		}
		EndRecursiveSearchLoop();
		return false;
	}

	internal bool isInLoop()
	{
		if (BeginRecursiveSearchLoop())
		{
			return this == startRecursiveSearchNode;
		}
		for (int i = 0; i < Inputs.Count; i++)
		{
			NodeOutput connection = Inputs[i].connection;
			if (connection != null && connection.body.isInLoop())
			{
				StopRecursiveSearchLoop();
				return true;
			}
		}
		EndRecursiveSearchLoop();
		return false;
	}

	internal bool allowsLoopRecursion(Node otherNode)
	{
		if (AllowRecursion)
		{
			return true;
		}
		if (otherNode == null)
		{
			return false;
		}
		if (BeginRecursiveSearchLoop())
		{
			return false;
		}
		for (int i = 0; i < Inputs.Count; i++)
		{
			NodeOutput connection = Inputs[i].connection;
			if (connection != null && connection.body.allowsLoopRecursion(otherNode))
			{
				StopRecursiveSearchLoop();
				return true;
			}
		}
		EndRecursiveSearchLoop();
		return false;
	}

	internal bool BeginRecursiveSearchLoop()
	{
		if (startRecursiveSearchNode == null || recursiveSearchSurpassed == null)
		{
			recursiveSearchSurpassed = new List<Node>();
			startRecursiveSearchNode = this;
		}
		if (recursiveSearchSurpassed.Contains(this))
		{
			return true;
		}
		recursiveSearchSurpassed.Add(this);
		return false;
	}

	internal void EndRecursiveSearchLoop()
	{
		if (startRecursiveSearchNode == this)
		{
			recursiveSearchSurpassed = null;
			startRecursiveSearchNode = null;
		}
	}

	internal void StopRecursiveSearchLoop()
	{
		recursiveSearchSurpassed = null;
		startRecursiveSearchNode = null;
	}
}
