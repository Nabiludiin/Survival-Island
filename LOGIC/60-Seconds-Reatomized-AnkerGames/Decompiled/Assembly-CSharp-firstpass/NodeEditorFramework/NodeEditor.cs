using System;
using System.Collections.Generic;
using NodeEditorFramework.Utilities;
using UnityEngine;

namespace NodeEditorFramework;

public static class NodeEditor
{
	public static string editorPath;

	public static NodeCanvas curNodeCanvas;

	public static NodeEditorState curEditorState;

	internal static Action NEUpdate;

	public static Action ClientRepaints;

	private static bool initiatedBase;

	private static bool initiatedGUI;

	public static bool InitiationError;

	public static void Update()
	{
		if (NEUpdate != null)
		{
			NEUpdate();
		}
	}

	public static void RepaintClients()
	{
		if (ClientRepaints != null)
		{
			ClientRepaints();
		}
	}

	public static void checkInit(bool GUIFunction)
	{
		if (!InitiationError)
		{
			if (!initiatedBase)
			{
				setupBaseFramework();
			}
			if (GUIFunction && !initiatedGUI)
			{
				setupGUI();
			}
		}
	}

	public static void ReInit(bool GUIFunction)
	{
		InitiationError = (initiatedBase = (initiatedGUI = false));
		setupBaseFramework();
		if (GUIFunction)
		{
			setupGUI();
		}
	}

	private static void setupBaseFramework()
	{
		CheckEditorPath();
		ResourceManager.SetDefaultResourcePath(editorPath + "Resources/");
		ConnectionTypes.FetchTypes();
		NodeTypes.FetchNodes();
		NodeCanvasManager.GetAllCanvasTypes();
		NodeEditorCallbacks.SetupReceivers();
		NodeEditorCallbacks.IssueOnEditorStartUp();
		NodeEditorInputSystem.SetupInput();
		initiatedBase = true;
	}

	private static void setupGUI()
	{
		if (!initiatedBase)
		{
			setupBaseFramework();
		}
		initiatedGUI = false;
		GUIScaleUtility.CheckInit();
		if (!NodeEditorGUI.Init())
		{
			InitiationError = true;
		}
		else
		{
			initiatedGUI = true;
		}
	}

	public static void CheckEditorPath()
	{
		editorPath = "Assets/Standard Assets/RG NodeEditor/NodeEditor/";
	}

	public static void DrawCanvas(NodeCanvas nodeCanvas, NodeEditorState editorState)
	{
		if (editorState.drawing)
		{
			checkInit(GUIFunction: true);
			DrawSubCanvas(nodeCanvas, editorState);
		}
	}

	private static void DrawSubCanvas(NodeCanvas nodeCanvas, NodeEditorState editorState)
	{
		if (!editorState.drawing)
		{
			return;
		}
		if (curNodeCanvas == null || curNodeCanvas != nodeCanvas)
		{
			curNodeCanvas = nodeCanvas;
		}
		curEditorState = editorState;
		if (Event.current.type == EventType.Repaint)
		{
			float num = curEditorState.zoom / (float)NodeEditorGUI.Background.width;
			float num2 = curEditorState.zoom / (float)NodeEditorGUI.Background.height;
			Vector2 vector = curEditorState.zoomPos + curEditorState.panOffset / curEditorState.zoom;
			GUI.DrawTextureWithTexCoords(texCoords: new Rect((0f - vector.x) * num, (vector.y - curEditorState.canvasRect.height) * num2, curEditorState.canvasRect.width * num, curEditorState.canvasRect.height * num2), position: curEditorState.canvasRect, image: NodeEditorGUI.Background);
		}
		NodeEditorInputSystem.HandleInputEvents(curEditorState);
		if (Event.current.type != EventType.Layout)
		{
			curEditorState.ignoreInput = new List<Rect>();
		}
		Rect rect = curEditorState.canvasRect;
		curEditorState.zoomPanAdjust = GUIScaleUtility.BeginScale(ref rect, curEditorState.zoomPos, curEditorState.zoom, adjustGUILayout: false);
		if (curEditorState.navigate)
		{
			Vector2 startPos = ((curEditorState.SelectedNode != null) ? curEditorState.SelectedNode.rect.center : curEditorState.panOffset) + curEditorState.zoomPanAdjust;
			Vector2 mousePosition = Event.current.mousePosition;
			RTEditorGUI.DrawLine(startPos, mousePosition, Color.green, null, 3f);
			RepaintClients();
		}
		if (curEditorState.connectOutput != null)
		{
			NodeOutput connectOutput = curEditorState.connectOutput;
			Vector2 center = connectOutput.GetGUIKnob().center;
			Vector2 direction = connectOutput.GetDirection();
			Vector2 mousePosition2 = Event.current.mousePosition;
			Vector2 secondConnectionVector = NodeEditorGUI.GetSecondConnectionVector(center, mousePosition2, direction);
			NodeEditorGUI.DrawConnection(center, direction, mousePosition2, secondConnectionVector, connectOutput.typeData.Color);
			RepaintClients();
		}
		for (int i = 0; i < curNodeCanvas.nodes.Count; i++)
		{
			curNodeCanvas.nodes[i].DrawConnections();
		}
		for (int j = 0; j < curNodeCanvas.nodes.Count; j++)
		{
			Node node = curNodeCanvas.nodes[j];
			node.DrawNode();
			if (Event.current.type == EventType.Repaint)
			{
				node.DrawKnobs();
			}
		}
		GUIScaleUtility.EndScale();
		NodeEditorInputSystem.HandleLateInputEvents(curEditorState);
	}

	public static Node NodeAtPosition(Vector2 canvasPos)
	{
		NodeKnob focusedKnob;
		return NodeAtPosition(curEditorState, canvasPos, out focusedKnob);
	}

	public static Node NodeAtPosition(Vector2 canvasPos, out NodeKnob focusedKnob)
	{
		return NodeAtPosition(curEditorState, canvasPos, out focusedKnob);
	}

	public static Node NodeAtPosition(NodeEditorState editorState, Vector2 canvasPos, out NodeKnob focusedKnob)
	{
		focusedKnob = null;
		if (NodeEditorInputSystem.shouldIgnoreInput(editorState))
		{
			return null;
		}
		NodeCanvas canvas = editorState.canvas;
		for (int num = canvas.nodes.Count - 1; num >= 0; num--)
		{
			Node node = canvas.nodes[num];
			for (int i = 0; i < node.nodeKnobs.Count; i++)
			{
				if (node.nodeKnobs[i].GetCanvasSpaceKnob().Contains(canvasPos))
				{
					focusedKnob = node.nodeKnobs[i];
					return node;
				}
			}
			if (node.rect.Contains(canvasPos))
			{
				return node;
			}
		}
		return null;
	}

	public static Vector2 ScreenToCanvasSpace(Vector2 screenPos)
	{
		return ScreenToCanvasSpace(curEditorState, screenPos);
	}

	public static Vector2 ScreenToCanvasSpace(NodeEditorState editorState, Vector2 screenPos)
	{
		return (screenPos - editorState.canvasRect.position - editorState.zoomPos) * editorState.zoom - editorState.panOffset;
	}
}
