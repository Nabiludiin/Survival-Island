using RG.Parsecs.NodeEditor;

namespace RG.Parsecs.EventEditor;

public class ParsecsEventParameterNotFoundException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "There is no parameter named {0} in ParsecsEvent {1}";

	public ParsecsEventParameterNotFoundException(ParsecsEvent parsecsEvent, string parameterName)
		: base($"There is no parameter named {parameterName} in ParsecsEvent {parsecsEvent.name}")
	{
	}
}
