using System;

namespace NodeEditorFramework;

public class NodeCanvasTypeAttribute : Attribute
{
	public string Name;

	public string WindowTitle = "Create new Node Cavnas";

	public string DefaultFileName = "New Node Canvas";

	public string SaveFileMessage = "Create new Node Canvas";

	public NodeCanvasTypeAttribute(string displayName)
	{
		Name = displayName;
	}

	public NodeCanvasTypeAttribute(string displayName, string windowTitle, string defaultFileName, string saveFileMessage)
	{
		Name = displayName;
		WindowTitle = windowTitle;
		DefaultFileName = defaultFileName;
		SaveFileMessage = saveFileMessage;
	}
}
