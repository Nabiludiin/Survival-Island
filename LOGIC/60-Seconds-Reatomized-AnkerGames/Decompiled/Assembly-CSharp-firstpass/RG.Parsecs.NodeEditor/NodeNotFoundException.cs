namespace RG.Parsecs.NodeEditor;

public class NodeNotFoundException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "Node of id: {0}, was not found on canvas: {1}.";

	public NodeNotFoundException(string nodeId, string canvasName)
		: base($"Node of id: {nodeId}, was not found on canvas: {canvasName}.")
	{
	}
}
