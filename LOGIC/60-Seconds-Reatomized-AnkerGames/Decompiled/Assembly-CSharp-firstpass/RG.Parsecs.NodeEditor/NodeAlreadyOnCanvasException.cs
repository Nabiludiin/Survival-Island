namespace RG.Parsecs.NodeEditor;

public class NodeAlreadyOnCanvasException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "There can be only one instance of {0} on this canvas.";

	public NodeAlreadyOnCanvasException(string nodeId)
		: base($"There can be only one instance of {nodeId} on this canvas.")
	{
	}
}
