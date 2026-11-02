using System;

namespace RG.Parsecs.NodeEditor;

public class FunctionNotImplementedException : ParsecsNodeEditorException
{
	private const string FUNCTION_NOT_IMPLEMENTED_WARNING = "{0} function was not implemented in node {1} but it was invoked.";

	public FunctionNotImplementedException(Delegate method, string nodeId)
		: base($"{method.Method.Name} function was not implemented in node {nodeId} but it was invoked.")
	{
	}

	public FunctionNotImplementedException(string method, string nodeId)
		: base($"{method} function was not implemented in node {nodeId} but it was invoked.")
	{
	}
}
