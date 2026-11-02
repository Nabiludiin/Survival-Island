using System.Collections.Generic;
using System.Linq;
using RG.Core.Base;
using UnityEngine;

namespace NodeEditorFramework;

[CreateAssetMenu(menuName = "RGTools/Node Editor/Node Canvas", fileName = "New Node Canvas")]
public class NodeCanvas : RGScriptableObject
{
	public List<Node> nodes = new List<Node>();

	public NodeEditorState[] editorStates = new NodeEditorState[0];

	public bool livesInScene;

	[SerializeField]
	public string path;

	public virtual string canvasName => "Event Canvas";

	public virtual void BeforeSavingCanvas()
	{
	}

	public void Validate()
	{
		if (nodes == null)
		{
			Debug.LogWarning("NodeCanvas '" + name + "' nodes were erased and set to null! Automatically fixed!");
			nodes = new List<Node>();
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			Node node = nodes[i];
			if (node == null)
			{
				Debug.LogWarning("NodeCanvas '" + name + "' contained broken (null) nodes! Automatically fixed!");
				nodes.RemoveAt(i);
				i--;
				continue;
			}
			for (int j = 0; j < node.nodeKnobs.Count; j++)
			{
				NodeKnob nodeKnob = node.nodeKnobs[j];
				if (nodeKnob == null)
				{
					Debug.LogWarning("NodeCanvas '" + name + "' Node '" + node.name + "' contained broken (null) NodeKnobs! Automatically fixed!");
					node.nodeKnobs.RemoveAt(j);
					j--;
				}
				else if (nodeKnob is NodeInput)
				{
					NodeInput nodeInput = nodeKnob as NodeInput;
					if (nodeInput.connection != null && nodeInput.connection.body == null)
					{
						nodeInput.connection = null;
					}
				}
				else
				{
					if (!(nodeKnob is NodeOutput))
					{
						continue;
					}
					NodeOutput nodeOutput = nodeKnob as NodeOutput;
					for (int k = 0; k < nodeOutput.connections.Count; k++)
					{
						NodeInput nodeInput2 = nodeOutput.connections[k];
						if (nodeInput2 == null || nodeInput2.body == null)
						{
							nodeOutput.connections.RemoveAt(k);
							k--;
						}
					}
				}
			}
		}
		if (editorStates == null)
		{
			Debug.LogWarning("NodeCanvas '" + name + "' editorStates were erased! Automatically fixed!");
			editorStates = new NodeEditorState[0];
		}
		editorStates = editorStates.Where((NodeEditorState state) => state != null).ToArray();
		NodeEditorState[] array = editorStates;
		for (int num = 0; num < array.Length; num++)
		{
			array[num].selectedNodes.Clear();
		}
	}

	protected T FindNodeOfId<T>(string nodeId) where T : Node
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (nodes[i].GetID.Equals(nodeId))
			{
				return nodes[i] as T;
			}
		}
		return null;
	}

	public Node GetNodeOfIndex(int nodeIndex)
	{
		if (nodeIndex >= nodes.Count || nodeIndex < 0)
		{
			Debug.LogWarning($"There is no node of id: {nodeIndex} on canvas: {name}");
			return null;
		}
		return nodes[nodeIndex];
	}
}
