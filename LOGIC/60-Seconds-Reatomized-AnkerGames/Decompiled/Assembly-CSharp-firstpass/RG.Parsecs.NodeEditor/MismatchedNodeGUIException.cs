namespace RG.Parsecs.NodeEditor;

public class MismatchedNodeGUIException : ParsecsNodeEditorException
{
	private const string ERROR_MESSAGE = "Node GUI has already started or hasn't finished successufly.";

	public MismatchedNodeGUIException()
		: base("Node GUI has already started or hasn't finished successufly.")
	{
	}
}
