using System;

namespace NodeEditorFramework;

public struct NodeData(string name, Type[] limitedCanvasTypes)
{
	public string adress = name;

	public Type[] limitToCanvasTypes = limitedCanvasTypes;
}
