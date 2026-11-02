using RG.Parsecs.EventEditor;
using UnityEngine;

public class NodeUsageFinderWindowData : ScriptableObject
{
	private const string NodeUsageFinderWindowDataPath = "/Settings/NodeUsageFinder.asset";

	public string NodeId;

	public Variable Variable;

	public NodeFunction Function;
}
