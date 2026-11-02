using System;
using NodeEditorFramework;
using UnityEngine;

namespace RG.Parsecs.NodeEditor;

public abstract class ParsecsNode : Node
{
	private const string NODE_ALREADY_ON_CANVAS_WARNING_MESSAGE = "There can be only one instance of {0} on this canvas.";

	protected virtual bool IsSingularNode => false;

	protected override void NodeGUI()
	{
	}

	public void ExecuteWithErrorHandling(NodeCanvas canvas)
	{
		ParentCanvas = canvas;
		try
		{
			Execute(canvas);
		}
		catch (Exception ex)
		{
			if (ex is ParsecsNodeExecutionException)
			{
				throw;
			}
			throw new ParsecsNodeExecutionException($"Node \"{name}\", of index \"{GetNodeNumber(force: true, canvas)}\", from event \"{parentCanvas.name}\", has thrown an exception: \"{ex.Message}\".\nStack Trace:\n{ex.StackTrace}\n\n", ex);
		}
	}

	public virtual void Execute(NodeCanvas canvas)
	{
		throw new FunctionNotImplementedException("Execute", GetID);
	}

	public virtual T GetValue<T>(int output, NodeCanvas canvas)
	{
		throw new FunctionNotImplementedException("GetValue<T>", GetID);
	}

	public T GetValueWithErrorHandling<T>(int output, NodeCanvas canvas)
	{
		ParentCanvas = canvas;
		try
		{
			return GetValue<T>(output, canvas);
		}
		catch (Exception ex)
		{
			if (ex is ParsecsNodeExecutionException)
			{
				throw;
			}
			throw new ParsecsNodeExecutionException($"Node \"{name}\", of index \"{GetNodeNumber(force: true, canvas)}\", from event \"{parentCanvas.name}\", has thrown an exception: \"{ex.Message}\".\nStack Trace:\n{ex.StackTrace}\n\n", ex);
		}
	}

	public virtual Rect GetNodeRect()
	{
		return rect;
	}

	public void GetInputValue<T>(NodeInput input, ref T currentValue, NodeCanvas canvas)
	{
		if (input.isConnected)
		{
			currentValue = input.GetCustomNodeAcrossConnection<ParsecsNode>().GetValueWithErrorHandling<T>(GetConnectedoutput(input), canvas);
		}
	}

	public T GetInputValue<T>(NodeInput input, NodeCanvas canvas)
	{
		if (input.isConnected)
		{
			return input.GetCustomNodeAcrossConnection<ParsecsNode>().GetValueWithErrorHandling<T>(GetConnectedoutput(input), canvas);
		}
		return default;
	}

	public T GetInputValue<T>(NodeInput input, T currentValue, NodeCanvas canvas)
	{
		if (input.isConnected)
		{
			return input.GetCustomNodeAcrossConnection<ParsecsNode>().GetValueWithErrorHandling<T>(GetConnectedoutput(input), canvas);
		}
		return currentValue;
	}

	protected T CastValue<T>(object obj)
	{
		if (obj == null)
		{
			return default;
		}
		try
		{
			return (T)obj;
		}
		catch (InvalidCastException)
		{
			throw new MismatchedOutputTypeException(obj.GetType(), typeof(T));
		}
	}

	protected int GetConnectedoutput(NodeInput input)
	{
		return input.GetCustomNodeAcrossConnection<ParsecsNode>().Outputs.IndexOf(input.connection);
	}

	protected override void InitBase()
	{
		if (IsSingularNode && IsNodeAlreadyOnCanvas())
		{
			Debug.LogWarning($"There can be only one instance of {GetID} on this canvas.");
		}
		else
		{
			base.InitBase();
		}
	}

	protected void CheckAreAllFlowOutputsConnected()
	{
		for (int i = 0; i < Outputs.Count; i++)
		{
			if (!Outputs[i].isConnected && Outputs[i].typeID == "Flow")
			{
				throw new NodeOutputNotFoundException(i, GetID);
			}
		}
	}

	private bool IsNodeAlreadyOnCanvas()
	{
		for (int i = 0; i < NodeEditorFramework.NodeEditor.curEditorState.canvas.nodes.Count; i++)
		{
			if (NodeEditorFramework.NodeEditor.curEditorState.canvas.nodes[i].GetID.Equals(GetID))
			{
				return true;
			}
		}
		return false;
	}
}
