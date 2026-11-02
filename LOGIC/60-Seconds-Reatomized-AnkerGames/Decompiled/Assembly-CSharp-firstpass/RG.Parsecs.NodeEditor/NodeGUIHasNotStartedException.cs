namespace RG.Parsecs.NodeEditor;

public class NodeGUIHasNotStartedException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "NodeGUI drawing was not initialized. Invoke BeginDynamicNodeGUI() or BeginStaticNodeGUI() first.";

	public NodeGUIHasNotStartedException()
		: base("NodeGUI drawing was not initialized. Invoke BeginDynamicNodeGUI() or BeginStaticNodeGUI() first.")
	{
	}
}
