using System;
using System.IO;
using UnityEngine;

namespace NodeEditorFramework;

public class NodeEditorUserCache
{
	public NodeCanvas nodeCanvas;

	public NodeEditorState editorState;

	private string cachePath;

	private bool useCache;

	private const string MainEditorStateIdentifier = "MainEditorState";

	public DateTime MainLastSaveTime;

	public DateTime[] LastSaveTime;

	public string openedCanvasPath = "";

	private string lastSessionPath => Path.Combine(cachePath, "LastSession.asset");

	public void AssureCanvas()
	{
		if (nodeCanvas == null)
		{
			NewNodeCanvas();
		}
		if (editorState == null)
		{
			NewEditorState();
		}
	}

	public NodeEditorUserCache(NodeCanvas loadedCanvas)
	{
		useCache = false;
		SetCanvas(loadedCanvas);
	}

	public NodeEditorUserCache()
	{
		useCache = false;
	}

	public void SetCanvas(NodeCanvas canvas)
	{
		nodeCanvas = canvas;
		editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, "MainEditorState");
		NodeEditor.RepaintClients();
	}

	public void SaveSceneNodeCanvas(string path)
	{
		nodeCanvas.editorStates = new NodeEditorState[1] { editorState };
		NodeEditorSaveManager.SaveSceneNodeCanvas(path, ref nodeCanvas, createWorkingCopy: true);
		editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, "MainEditorState");
		editorState.Path = path;
		NodeEditor.RepaintClients();
	}

	public void LoadSceneNodeCanvas(string path)
	{
		if ((nodeCanvas = NodeEditorSaveManager.LoadSceneNodeCanvas(path, createWorkingCopy: true)) == null)
		{
			NewNodeCanvas();
			return;
		}
		editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, "MainEditorState");
		editorState.Path = path;
		NodeEditor.RepaintClients();
	}

	public void SaveNodeCanvas(string path)
	{
		nodeCanvas.editorStates = new NodeEditorState[1] { editorState };
		NodeEditorSaveManager.SaveNodeCanvas(path, nodeCanvas, createWorkingCopy: true);
		editorState.Path = path;
		NodeEditor.RepaintClients();
	}

	public void AutosaveNodeCanvas(string path)
	{
		nodeCanvas.editorStates = new NodeEditorState[1] { editorState };
		NodeEditorSaveManager.SaveNodeCanvas(path, nodeCanvas, createWorkingCopy: true);
	}

	public void LoadNodeCanvas(string path)
	{
		if (!File.Exists(path) || (nodeCanvas = NodeEditorSaveManager.LoadNodeCanvas(path, createWorkingCopy: true)) == null)
		{
			NewNodeCanvas();
			return;
		}
		editorState = NodeEditorSaveManager.ExtractEditorState(nodeCanvas, "MainEditorState");
		editorState.Path = path;
		NodeEditor.RepaintClients();
	}

	public void NewNodeCanvas(Type canvasType = null)
	{
		if (canvasType != null && canvasType.IsSubclassOf(typeof(NodeCanvas)))
		{
			nodeCanvas = ScriptableObject.CreateInstance(canvasType) as NodeCanvas;
		}
		else
		{
			nodeCanvas = ScriptableObject.CreateInstance<NodeCanvas>();
		}
		nodeCanvas.name = "New " + nodeCanvas.canvasName;
	}

	public void NewEditorState()
	{
		editorState = ScriptableObject.CreateInstance<NodeEditorState>();
		editorState.canvas = nodeCanvas;
		editorState.name = "MainEditorState";
		editorState.Path = "";
		nodeCanvas.editorStates = new NodeEditorState[1] { editorState };
	}
}
