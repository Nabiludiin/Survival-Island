using System;
using System.Collections.Generic;
using System.Linq;
using NodeEditorFramework.Utilities;
using UnityEngine;

namespace NodeEditorFramework;

public static class NodeEditorInputControls
{
	private static NodeEditorInputInfo info;

	[ContextFiller(ContextType.Canvas)]
	private static void FillAddNodes(NodeEditorInputInfo inputInfo, GenericMenu canvasContextMenu)
	{
		NodeEditorState editorState = inputInfo.editorState;
		foreach (Node item in (editorState.connectOutput != null) ? NodeTypes.getCompatibleNodes(editorState.connectOutput) : NodeTypes.nodes.Keys.ToList())
		{
			if (NodeCanvasManager.CheckCanvasCompability(item, inputInfo.editorState.canvas.GetType()))
			{
				canvasContextMenu.AddItem(new GUIContent("Add " + NodeTypes.nodes[item].adress), on: false, CreateNodeCallback, new NodeEditorInputInfo(item.GetID, editorState));
			}
		}
	}

	private static void CreateNodeCallback(object infoObj)
	{
		if (!(infoObj is NodeEditorInputInfo nodeEditorInputInfo))
		{
			throw new UnityException("Callback Object passed by context is not of type NodeEditorInputInfo!");
		}
		nodeEditorInputInfo.SetAsCurrentEnvironment();
		Node.Create(nodeEditorInputInfo.message, NodeEditor.ScreenToCanvasSpace(nodeEditorInputInfo.inputPos), nodeEditorInputInfo.editorState.connectOutput);
		nodeEditorInputInfo.editorState.connectOutput = null;
		NodeEditor.RepaintClients();
	}

	[ContextEntry(ContextType.Node, "Delete Node")]
	private static void DeleteNode(NodeEditorInputInfo inputInfo)
	{
		inputInfo.SetAsCurrentEnvironment();
		if (inputInfo.editorState.selectedNodes.Count > 1)
		{
			for (int i = 0; i < inputInfo.editorState.selectedNodes.Count; i++)
			{
				inputInfo.editorState.selectedNodes[i].Delete();
			}
		}
		else if (inputInfo.editorState.focusedNode != null)
		{
			inputInfo.editorState.focusedNode.Delete();
		}
		inputInfo.editorState.selectedNodes.Clear();
		inputInfo.inputEvent.Use();
	}

	[ContextEntry(ContextType.Node, "Duplicate Node")]
	private static void DuplicateNode(NodeEditorInputInfo inputInfo)
	{
		inputInfo.SetAsCurrentEnvironment();
		NodeEditorState editorState = inputInfo.editorState;
		if (editorState.selectedNodes.Count > 1)
		{
			Dictionary<Node, Node> dictionary = new Dictionary<Node, Node>();
			for (int i = 0; i < editorState.selectedNodes.Count; i++)
			{
				Node value = Node.Duplicate(editorState.selectedNodes[i], NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos), editorState.connectOutput);
				dictionary.Add(editorState.selectedNodes[i], value);
			}
			for (int j = 0; j < editorState.selectedNodes.Count; j++)
			{
				for (int k = 0; k < editorState.selectedNodes[j].Inputs.Count; k++)
				{
					if (!editorState.selectedNodes[j].Inputs[k].isConnected)
					{
						continue;
					}
					Node nodeAcrossConnection = editorState.selectedNodes[j].Inputs[k].GetNodeAcrossConnection();
					if (!editorState.selectedNodes.Contains(nodeAcrossConnection))
					{
						continue;
					}
					if (editorState.selectedNodes[j].Inputs[k] is NodeMultiInput)
					{
						foreach (NodeOutput connection2 in ((NodeMultiInput)editorState.selectedNodes[j].Inputs[k]).connections)
						{
							int index = nodeAcrossConnection.Outputs.IndexOf(connection2);
							((NodeMultiInput)dictionary[editorState.selectedNodes[j]].Inputs[k]).TryApplyConnection(dictionary[nodeAcrossConnection].Outputs[index]);
						}
					}
					else
					{
						NodeOutput connection = editorState.selectedNodes[j].Inputs[k].connection;
						if (connection != null)
						{
							int index2 = nodeAcrossConnection.Outputs.IndexOf(connection);
							dictionary[editorState.selectedNodes[j]].Inputs[k].TryApplyConnection(dictionary[nodeAcrossConnection].Outputs[index2]);
						}
					}
				}
			}
			editorState.selectedNodes.Clear();
			foreach (KeyValuePair<Node, Node> item in dictionary)
			{
				editorState.selectedNodes.Add(item.Value);
			}
		}
		else if (editorState.focusedNode != null)
		{
			Node focusedNode = Node.Duplicate(editorState.focusedNode, NodeEditor.ScreenToCanvasSpace(inputInfo.inputPos), editorState.connectOutput);
			editorState.SelectedNode = (editorState.focusedNode = focusedNode);
		}
		editorState.connectOutput = null;
		inputInfo.inputEvent.Use();
	}

	private static void KB_MoveNode(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (editorState.SelectedNode != null)
		{
			Vector2 position = editorState.SelectedNode.rect.position;
			int num = (inputInfo.inputEvent.shift ? 50 : 10);
			if (inputInfo.inputEvent.keyCode == KeyCode.RightArrow)
			{
				position = new Vector2(position.x + (float)num, position.y);
			}
			else if (inputInfo.inputEvent.keyCode == KeyCode.LeftArrow)
			{
				position = new Vector2(position.x - (float)num, position.y);
			}
			else if (inputInfo.inputEvent.keyCode == KeyCode.DownArrow)
			{
				position = new Vector2(position.x, position.y + (float)num);
			}
			else if (inputInfo.inputEvent.keyCode == KeyCode.UpArrow)
			{
				position = new Vector2(position.x, position.y - (float)num);
			}
			editorState.SelectedNode.rect.position = position;
			inputInfo.inputEvent.Use();
		}
		NodeEditor.RepaintClients();
	}

	[EventHandler(EventType.MouseDown, 110)]
	private static void HandleNodeDraggingStart(NodeEditorInputInfo inputInfo)
	{
		if (GUIUtility.hotControl > 0)
		{
			return;
		}
		NodeEditorState editorState = inputInfo.editorState;
		if (inputInfo.inputEvent.button != 0 || !(editorState.focusedNode != null) || !editorState.selectedNodes.Contains(editorState.focusedNode) || !(editorState.focusedNodeKnob == null))
		{
			return;
		}
		editorState.dragNode = true;
		editorState.dragStart = inputInfo.inputPos;
		foreach (Node selectedNode in editorState.selectedNodes)
		{
			if ((bool)selectedNode)
			{
				selectedNode.drag = selectedNode.rect.position;
			}
		}
		editorState.dragOffset = Vector2.zero;
		inputInfo.inputEvent.delta = Vector2.zero;
	}

	[EventHandler(EventType.MouseDrag)]
	private static void HandleNodeDragging(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (!editorState.dragNode)
		{
			return;
		}
		if (editorState.selectedNodes.Count > 0 && GUIUtility.hotControl == 0)
		{
			editorState.dragOffset = inputInfo.inputPos - editorState.dragStart;
			foreach (Node selectedNode in editorState.selectedNodes)
			{
				if ((bool)selectedNode)
				{
					selectedNode.rect.position = selectedNode.drag + editorState.dragOffset * editorState.zoom;
					NodeEditorCallbacks.IssueOnMoveNode(selectedNode);
				}
			}
			NodeEditor.RepaintClients();
		}
		else
		{
			editorState.dragNode = false;
		}
	}

	[EventHandler(EventType.MouseDown)]
	[EventHandler(EventType.MouseUp)]
	private static void HandleNodeDraggingEnd(NodeEditorInputInfo inputInfo)
	{
		inputInfo.editorState.dragNode = false;
	}

	[EventHandler(EventType.MouseDown)]
	private static void UnselectNode(NodeEditorInputInfo inputInfo)
	{
		if (!NodeEditorInputSystem.CtrlPressed && !inputInfo.editorState.selectedNodes.Contains(inputInfo.editorState.focusedNode) && inputInfo.inputEvent.button == 0)
		{
			inputInfo.editorState.selectedNodes.Clear();
		}
	}

	[EventHandler(EventType.MouseDown, 100)]
	private static void HandleWindowPanningStart(NodeEditorInputInfo inputInfo)
	{
		if (GUIUtility.hotControl <= 0)
		{
			NodeEditorState editorState = inputInfo.editorState;
			if (inputInfo.inputEvent.button == 2 && editorState.focusedNode == null)
			{
				editorState.panWindow = true;
				editorState.dragStart = inputInfo.inputPos;
				editorState.dragOffset = Vector2.zero;
			}
		}
	}

	[EventHandler(EventType.MouseDrag)]
	private static void HandleWindowPanning(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (editorState.panWindow)
		{
			Vector2 dragOffset = editorState.dragOffset;
			editorState.dragOffset = inputInfo.inputPos - editorState.dragStart;
			dragOffset = (editorState.dragOffset - dragOffset) * editorState.zoom;
			editorState.panOffset += dragOffset;
			NodeEditor.RepaintClients();
		}
	}

	[EventHandler(EventType.MouseDown)]
	[EventHandler(EventType.MouseUp)]
	private static void HandleWindowPanningEnd(NodeEditorInputInfo inputInfo)
	{
		inputInfo.editorState.panWindow = false;
	}

	[EventHandler(EventType.MouseDown)]
	private static void HandleConnectionDrawing(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (inputInfo.inputEvent.button != 0 || !(editorState.focusedNodeKnob != null))
		{
			return;
		}
		if (editorState.focusedNodeKnob is NodeOutput)
		{
			editorState.connectOutput = (NodeOutput)editorState.focusedNodeKnob;
			inputInfo.inputEvent.Use();
		}
		else
		{
			if (!(editorState.focusedNodeKnob is NodeInput))
			{
				return;
			}
			NodeInput nodeInput = (NodeInput)editorState.focusedNodeKnob;
			if (nodeInput.connection != null)
			{
				editorState.connectOutput = nodeInput.connection;
				if (nodeInput is NodeMultiInput)
				{
					(nodeInput as NodeMultiInput).RemoveConnection();
				}
				else
				{
					nodeInput.RemoveConnection();
				}
				inputInfo.inputEvent.Use();
			}
		}
	}

	[EventHandler(EventType.MouseUp)]
	private static void HandleApplyConnection(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (inputInfo.inputEvent.button == 0 && editorState.connectOutput != null && editorState.focusedNode != null && editorState.focusedNodeKnob != null && editorState.focusedNodeKnob is NodeInput)
		{
			NodeInput nodeInput = editorState.focusedNodeKnob as NodeInput;
			if (nodeInput is NodeMultiInput)
			{
				((NodeMultiInput)nodeInput).TryApplyConnection(editorState.connectOutput);
			}
			else
			{
				nodeInput.TryApplyConnection(editorState.connectOutput);
			}
			inputInfo.inputEvent.Use();
		}
		editorState.connectOutput = null;
	}

	[EventHandler(EventType.ScrollWheel)]
	private static void HandleZooming(NodeEditorInputInfo inputInfo)
	{
		inputInfo.editorState.zoom = (float)Math.Round(Math.Min(NodeEditorSettings.MaxZoomDistance, Math.Max(0.6, inputInfo.editorState.zoom + inputInfo.inputEvent.delta.y / 15f)), 2);
		NodeEditor.RepaintClients();
	}

	private static void HandleStartNavigating(NodeEditorInputInfo inputInfo)
	{
		inputInfo.editorState.navigate = true;
	}

	private static void HandleEndNavigating(NodeEditorInputInfo inputInfo)
	{
		inputInfo.editorState.navigate = false;
	}

	[Hotkey(KeyCode.LeftAlt, EventType.KeyDown, 60)]
	[Hotkey(KeyCode.LeftAlt, EventType.KeyUp, 60)]
	private static void HandleNodeSnap(NodeEditorInputInfo inputInfo)
	{
		NodeEditorState editorState = inputInfo.editorState;
		if (editorState.SelectedNode != null)
		{
			Vector2 position = editorState.SelectedNode.rect.position;
			position = new Vector2(Mathf.RoundToInt(position.x / 10f) * 10, Mathf.RoundToInt(position.y / 10f) * 10);
			editorState.SelectedNode.rect.position = position;
			inputInfo.inputEvent.Use();
		}
		NodeEditor.RepaintClients();
	}

	[Hotkey(KeyCode.LeftControl, EventType.KeyDown)]
	private static void HandleMultiSelectOn(NodeEditorInputInfo inputInfo)
	{
		NodeEditorInputSystem.CtrlPressed = true;
		info = inputInfo;
	}

	[Hotkey(KeyCode.LeftControl, EventType.KeyUp)]
	private static void HandleMultiSelectOff(NodeEditorInputInfo inputInfo)
	{
		NodeEditorInputSystem.CtrlPressed = false;
		info = null;
	}
}
