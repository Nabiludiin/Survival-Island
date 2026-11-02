namespace RG.Parsecs.NodeEditor;

public class NotExistingOutputException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "{0} doesn't have output of id {1}";

	public NotExistingOutputException(string nodeId, int outputId)
		: base($"{nodeId} doesn't have output of id {outputId}")
	{
	}
}
