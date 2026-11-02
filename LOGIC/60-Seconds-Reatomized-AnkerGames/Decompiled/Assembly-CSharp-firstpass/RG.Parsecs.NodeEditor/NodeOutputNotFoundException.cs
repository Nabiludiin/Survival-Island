namespace RG.Parsecs.NodeEditor;

public class NodeOutputNotFoundException : ParsecsNodeEditorException
{
	private int _outputId;

	private string _nodeId;

	private const string NODE_OUTPUT_NOT_FOUND_MESSAGE = "The {0} output is not connected in {1}";

	public int OutputId => _outputId;

	public string NodeId => _nodeId;

	public NodeOutputNotFoundException(int outputId, string nodeId)
		: base($"The {outputId} output is not connected in {nodeId}")
	{
		_outputId = outputId;
		_nodeId = nodeId;
	}
}
