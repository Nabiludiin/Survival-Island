using System;
using System.Collections.Generic;
using UnityEngine;

namespace NodeEditorFramework;

public class NodeMultiInput : NodeInput
{
	private static GUIStyle _defaultStyle;

	public List<NodeOutput> connections = new List<NodeOutput>();

	private TypeData _typeData;

	[NonSerialized]
	private object value;

	public bool calculationBlockade;

	protected override NodeSide defaultSide => NodeSide.Left;

	internal new TypeData typeData
	{
		get
		{
			CheckType();
			return _typeData;
		}
	}

	public new bool IsValueNull => value == null;

	public new static NodeMultiInput Create(Node nodeBody, string outputName, string outputType)
	{
		return Create(nodeBody, outputName, outputType, NodeSide.Left, 20f);
	}

	public new static NodeMultiInput Create(Node nodeBody, string outputName, string outputType, NodeSide nodeSide)
	{
		return Create(nodeBody, outputName, outputType, nodeSide, 20f);
	}

	public new static NodeMultiInput Create(Node nodeBody, string outputName, string outputType, NodeSide nodeSide, float sidePosition)
	{
		NodeMultiInput nodeMultiInput = ScriptableObject.CreateInstance<NodeMultiInput>();
		nodeMultiInput.typeID = outputType;
		nodeMultiInput.InitBase(nodeBody, nodeSide, sidePosition, outputName);
		nodeBody.Inputs.Add(nodeMultiInput);
		return nodeMultiInput;
	}

	public override void Delete()
	{
		RemoveConnection();
		body.Inputs.Remove(this);
		base.Delete();
	}

	public new bool TryApplyConnection(NodeOutput output)
	{
		if (CanApplyConnection(output))
		{
			ApplyConnection(output);
			return true;
		}
		return false;
	}

	public new bool CanApplyConnection(NodeOutput output)
	{
		if (output == null || body == output.body || connections.Contains(output) || !typeData.Type.IsAssignableFrom(output.typeData.Type))
		{
			return false;
		}
		if (output.body.isChildOf(body) && !output.body.allowsLoopRecursion(body))
		{
			Debug.LogWarning("Cannot apply output: Recursion detected!");
			return false;
		}
		return true;
	}

	public new void ApplyConnection(NodeOutput output)
	{
		if (output == null)
		{
			return;
		}
		if (output.connections.Count > 0 && typeof(void) == typeData.Type)
		{
			NodeEditorCallbacks.IssueOnRemoveConnection(output.connections[0]);
			if (output.connections[0] is NodeMultiInput)
			{
				((NodeMultiInput)output.connections[0]).RemoveSingleConnection(output);
			}
			else
			{
				output.connections[0].RemoveConnection();
			}
			output.connections.Clear();
			output.isConnected = false;
			output.ReloadKnobTexture();
		}
		connections.Add(output);
		output.isConnected = true;
		output.ReloadKnobTexture();
		isConnected = true;
		ReloadKnobTexture();
		output.connections.Add(this);
		output.body.OnAddOutputConnection(output);
		body.OnAddInputConnection(this);
		NodeEditorCallbacks.IssueOnAddConnection(this);
	}

	public void RemoveSingleConnection(NodeOutput output)
	{
		if (connections.Count > 0)
		{
			NodeEditorCallbacks.IssueOnRemoveConnection(this);
			output.connections.Remove(this);
			if (output.connections.Count == 0)
			{
				output.isConnected = false;
				output.ReloadKnobTexture();
			}
			connections.Remove(output);
			if (connections.Count <= 0)
			{
				isConnected = false;
				ReloadKnobTexture();
			}
		}
	}

	public new void RemoveConnection()
	{
		if (connections.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < connections.Count; i++)
		{
			NodeEditorCallbacks.IssueOnRemoveConnection(this);
			connections[i].connections.Remove(this);
			if (connections[i].connections.Count == 0)
			{
				connections[i].isConnected = false;
				connections[i].ReloadKnobTexture();
			}
		}
		connections.Clear();
		isConnected = false;
		ReloadKnobTexture();
	}

	protected internal override void CopyScriptableObjects(Func<ScriptableObject, ScriptableObject> replaceSerializableObject)
	{
		for (int i = 0; i < connections.Count; i++)
		{
			connections[i] = replaceSerializableObject(connections[i]) as NodeOutput;
		}
	}

	protected override void ReloadTexture()
	{
		CheckType();
		if (isConnected)
		{
			knobTexture = typeData.OutKnobFilledTex;
		}
		else
		{
			knobTexture = typeData.OutKnobTex;
		}
	}

	private void CheckType()
	{
		if (_typeData == null || !_typeData.isValid())
		{
			_typeData = ConnectionTypes.GetTypeData(typeID);
		}
		if (_typeData == null || !_typeData.isValid())
		{
			ConnectionTypes.FetchTypes();
			_typeData = ConnectionTypes.GetTypeData(typeID);
			if (_typeData == null || !_typeData.isValid())
			{
				throw new UnityException("Could not find type " + typeID + "!");
			}
		}
	}

	public new object GetValue()
	{
		return value;
	}

	public new object GetValue(Type type)
	{
		if (type == null)
		{
			throw new UnityException("Trying to get value of " + name + " with null type!");
		}
		CheckType();
		if (type.IsAssignableFrom(typeData.Type))
		{
			return value;
		}
		Debug.LogError("Trying to GetValue<" + type.FullName + "> for Output Type: " + typeData.Type.FullName);
		return null;
	}

	public new void SetValue(object Value)
	{
		CheckType();
		if (Value == null || typeData.Type.IsAssignableFrom(Value.GetType()))
		{
			value = Value;
		}
		else
		{
			Debug.LogError("Trying to SetValue of type " + Value.GetType().FullName + " for Output Type: " + typeData.Type.FullName);
		}
	}

	public new T GetValue<T>()
	{
		CheckType();
		if (typeof(T).IsAssignableFrom(typeData.Type))
		{
			return (T)(value ?? (value = GetDefault<T>()));
		}
		Debug.LogError("Trying to GetValue<" + typeof(T).FullName + "> for Output Type: " + typeData.Type.FullName);
		return GetDefault<T>();
	}

	public new void SetValue<T>(T Value)
	{
		CheckType();
		if (typeData.Type.IsAssignableFrom(typeof(T)))
		{
			value = Value;
		}
		else
		{
			Debug.LogError("Trying to SetValue<" + typeof(T).FullName + "> for Output Type: " + typeData.Type.FullName);
		}
	}

	public void ResetValue()
	{
		value = null;
	}

	public static T GetDefault<T>()
	{
		if (typeof(T).GetConstructor(Type.EmptyTypes) != null)
		{
			return Activator.CreateInstance<T>();
		}
		return default;
	}

	public static object GetDefault(Type type)
	{
		if (type.GetConstructor(Type.EmptyTypes) != null)
		{
			return Activator.CreateInstance(type);
		}
		return null;
	}

	public override Node GetNodeAcrossConnection()
	{
		if (connections.Count <= 0)
		{
			return null;
		}
		return connections[0].body;
	}

	public new T GetCustomNodeAcrossConnection<T>() where T : Node
	{
		return (T)((connections.Count > 0) ? connections[0].body : null);
	}
}
