using System;

namespace NodeEditorFramework;

public class NodeAttribute : Attribute
{
	public bool hide { get; private set; }

	public string contextText { get; private set; }

	public Type[] limitToCanvasTypes { get; private set; }

	public NodeAttribute(bool HideNode, string ReplacedContextText)
	{
		hide = HideNode;
		contextText = ReplacedContextText;
		limitToCanvasTypes = null;
	}

	public NodeAttribute(bool HideNode, string ReplacedContextText, Type[] limitedCanvasTypes)
	{
		hide = HideNode;
		contextText = ReplacedContextText;
		limitToCanvasTypes = limitedCanvasTypes;
	}
}
