using System;
using RG.Parsecs.NodeEditor;

namespace RG.Parsecs.EventEditor;

public class WrongDecisionTypeException : ParsecsNodeEditorException
{
	private const string MESSAGE = "Wrong decision type!. Expected decision type was: {0}, found: {1}.";

	public WrongDecisionTypeException(Type expectedType, Type decisionType)
		: base($"Wrong decision type!. Expected decision type was: {expectedType}, found: {decisionType}.")
	{
	}
}
