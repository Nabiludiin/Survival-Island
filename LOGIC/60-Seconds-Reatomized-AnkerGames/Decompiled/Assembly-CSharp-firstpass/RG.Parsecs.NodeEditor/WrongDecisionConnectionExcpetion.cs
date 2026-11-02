namespace RG.Parsecs.NodeEditor;

public class WrongDecisionConnectionExcpetion : ParsecsNodeEditorException
{
	private const string MESSAGE = "Node \"{0}\" has wrong input connection with output connection or there are inconsistent data.";

	public WrongDecisionConnectionExcpetion(string nodeId)
		: base($"Node \"{nodeId}\" has wrong input connection with output connection or there are inconsistent data.")
	{
	}
}
