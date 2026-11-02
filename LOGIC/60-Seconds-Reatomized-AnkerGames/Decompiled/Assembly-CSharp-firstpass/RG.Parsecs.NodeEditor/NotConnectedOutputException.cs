namespace RG.Parsecs.NodeEditor;

public class NotConnectedOutputException : ParsecsNodeEditorException
{
	private const string MESSAGE = "Node Output \"{0}\"was not connected in {1} node.";

	public NotConnectedOutputException(string nodeId, int output)
		: base($"Node Output \"{output}\"was not connected in {nodeId} node.")
	{
	}
}
