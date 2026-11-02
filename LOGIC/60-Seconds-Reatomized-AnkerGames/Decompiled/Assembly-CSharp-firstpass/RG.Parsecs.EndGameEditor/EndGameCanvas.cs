using System.Collections;
using NodeEditorFramework;
using RG.Parsecs.EventEditor;
using RG.Parsecs.NodeEditor;
using UnityEngine;

namespace RG.Parsecs.EndGameEditor;

[NodeCanvasType("End Game Editor/End Game Canvas", "Create new End Game Canvas", "New End Game Canvas", "Create new Survival Event")]
[CreateAssetMenu(menuName = "60 Parsecs!/End Game Editor/New End Game Canvas")]
public class EndGameCanvas : NodeCanvas
{
	private void EndGameCanvasGUI()
	{
	}

	public void DrawInfoGUI()
	{
	}

	protected virtual void InfoGUI()
	{
	}

	public virtual IEnumerator Run()
	{
		yield return ExecuteNode("GE_StartNode");
	}

	protected IEnumerator ExecuteNode(string nodeId)
	{
		EventNode eventNode = FindNodeOfId<EventNode>(nodeId);
		if (eventNode == null)
		{
			throw new NodeNotFoundException(nodeId, name);
		}
		eventNode.ExecuteWithErrorHandling(this);
		yield break;
	}
}
