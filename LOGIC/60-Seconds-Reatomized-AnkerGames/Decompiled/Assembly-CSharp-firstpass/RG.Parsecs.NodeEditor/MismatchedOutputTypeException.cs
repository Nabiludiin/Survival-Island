using System;

namespace RG.Parsecs.NodeEditor;

public class MismatchedOutputTypeException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "This output is of the type {0}, not {1}";

	public MismatchedOutputTypeException(Type outputType, Type expectedType)
		: base($"This output is of the type {outputType}, not {expectedType}")
	{
	}
}
