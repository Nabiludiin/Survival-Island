using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NodeEditorFramework;

public static class NodeEditorSaveManager
{
	private static GameObject sceneSaveHolder;

	private static void FetchSceneSaveHolder()
	{
		if (sceneSaveHolder == null)
		{
			sceneSaveHolder = GameObject.Find("NodeEditor_SceneSaveHolder");
			if (sceneSaveHolder == null)
			{
				sceneSaveHolder = new GameObject("NodeEditor_SceneSaveHolder");
			}
			sceneSaveHolder.hideFlags = HideFlags.None;
		}
	}

	public static string[] GetSceneSaves()
	{
		FetchSceneSaveHolder();
		return (from save in sceneSaveHolder.GetComponents<NodeCanvasSceneSave>()
			select save.savedNodeCanvas.name).ToArray();
	}

	private static NodeCanvasSceneSave FindSceneSave(string saveName)
	{
		FetchSceneSaveHolder();
		return sceneSaveHolder.GetComponents<NodeCanvasSceneSave>().ToList().Find((NodeCanvasSceneSave save) => save.savedNodeCanvas.name == saveName);
	}

	public static void SaveSceneNodeCanvas(string saveName, ref NodeCanvas nodeCanvas, bool createWorkingCopy)
	{
		if (string.IsNullOrEmpty(saveName))
		{
			Debug.LogError("Cannot save Canvas to scene: No save name specified!");
			return;
		}
		if (!nodeCanvas.livesInScene)
		{
			nodeCanvas = CreateWorkingCopy(nodeCanvas, editorStates: true);
		}
		else
		{
			nodeCanvas.Validate();
		}
		nodeCanvas.livesInScene = true;
		nodeCanvas.name = saveName;
		NodeCanvas canvas = nodeCanvas;
		ProcessCanvas(ref canvas, createWorkingCopy);
		NodeCanvasSceneSave nodeCanvasSceneSave = FindSceneSave(saveName);
		if (nodeCanvasSceneSave == null)
		{
			nodeCanvasSceneSave = sceneSaveHolder.AddComponent<NodeCanvasSceneSave>();
		}
		nodeCanvasSceneSave.savedNodeCanvas = canvas;
	}

	public static NodeCanvas LoadSceneNodeCanvas(string saveName, bool createWorkingCopy)
	{
		if (string.IsNullOrEmpty(saveName))
		{
			Debug.LogError("Cannot load Canvas from scene: No save name specified!");
			return null;
		}
		NodeCanvasSceneSave nodeCanvasSceneSave = FindSceneSave(saveName);
		if (nodeCanvasSceneSave == null)
		{
			return null;
		}
		NodeCanvas canvas = nodeCanvasSceneSave.savedNodeCanvas;
		canvas.livesInScene = true;
		ProcessCanvas(ref canvas, createWorkingCopy);
		return canvas;
	}

	public static void SaveNodeCanvas(string path, NodeCanvas nodeCanvas, bool createWorkingCopy)
	{
		throw new NotImplementedException();
	}

	public static NodeCanvas LoadNodeCanvas(string path, bool createWorkingCopy)
	{
		if (!File.Exists(path))
		{
			throw new UnityException("Cannot Load NodeCanvas: File '" + path + "' deos not exist!");
		}
		NodeEditorCallbacks.IssueOnLoadCanvas(null);
		return null;
	}

	private static void ProcessCanvas(ref NodeCanvas canvas, bool workingCopy)
	{
		if (workingCopy)
		{
			canvas = CreateWorkingCopy(canvas, editorStates: true);
		}
		else
		{
			canvas.Validate();
		}
	}

	public static void Uncompress(ref NodeCanvas nodeCanvas)
	{
		for (int i = 0; i < nodeCanvas.nodes.Count; i++)
		{
			Node node = nodeCanvas.nodes[i];
			node.Inputs = new List<NodeInput>();
			node.Outputs = new List<NodeOutput>();
			for (int j = 0; j < node.nodeKnobs.Count; j++)
			{
				NodeKnob nodeKnob = node.nodeKnobs[j];
				if (nodeKnob is NodeInput)
				{
					node.Inputs.Add(nodeKnob as NodeInput);
				}
				else if (nodeKnob is NodeOutput)
				{
					node.Outputs.Add(nodeKnob as NodeOutput);
				}
			}
		}
	}

	public static NodeCanvas CreateWorkingCopy(NodeCanvas nodeCanvas, bool editorStates)
	{
		nodeCanvas.Validate();
		nodeCanvas = Clone(nodeCanvas);
		List<ScriptableObject> allSOs = new List<ScriptableObject>();
		List<ScriptableObject> clonedSOs = new List<ScriptableObject>();
		for (int i = 0; i < nodeCanvas.nodes.Count; i++)
		{
			Node node = nodeCanvas.nodes[i];
			node.CheckNodeKnobMigration();
			Node node2 = AddClonedSO(allSOs, clonedSOs, node);
			AddClonedSOs(allSOs, clonedSOs, node2.GetScriptableObjects());
			foreach (NodeKnob nodeKnob4 in node2.nodeKnobs)
			{
				AddClonedSO(allSOs, clonedSOs, nodeKnob4);
				AddClonedSOs(allSOs, clonedSOs, nodeKnob4.GetScriptableObjects());
			}
		}
		for (int j = 0; j < nodeCanvas.nodes.Count; j++)
		{
			Node initialSO = nodeCanvas.nodes[j];
			Node node3 = (nodeCanvas.nodes[j] = ReplaceSO(allSOs, clonedSOs, initialSO));
			Node node5 = node3;
			node5.CopyScriptableObjects((ScriptableObject so) => ReplaceSO(allSOs, clonedSOs, so));
			node5.Inputs = new List<NodeInput>();
			node5.Outputs = new List<NodeOutput>();
			for (int num = 0; num < node5.nodeKnobs.Count; num++)
			{
				NodeKnob nodeKnob = (node5.nodeKnobs[num] = ReplaceSO(allSOs, clonedSOs, node5.nodeKnobs[num]));
				NodeKnob nodeKnob3 = nodeKnob;
				nodeKnob3.body = node5;
				nodeKnob3.CopyScriptableObjects((ScriptableObject so) => ReplaceSO(allSOs, clonedSOs, so));
				if (nodeKnob3 is NodeInput)
				{
					node5.Inputs.Add(nodeKnob3 as NodeInput);
				}
				else if (nodeKnob3 is NodeOutput)
				{
					node5.Outputs.Add(nodeKnob3 as NodeOutput);
				}
			}
		}
		if (editorStates)
		{
			nodeCanvas.editorStates = CreateWorkingCopy(nodeCanvas.editorStates, nodeCanvas);
			NodeEditorState[] editorStates2 = nodeCanvas.editorStates;
			foreach (NodeEditorState nodeEditorState in editorStates2)
			{
				nodeEditorState.SelectedNode = ReplaceSO(allSOs, clonedSOs, nodeEditorState.SelectedNode);
			}
		}
		else
		{
			NodeEditorState[] editorStates2 = nodeCanvas.editorStates;
			for (int num2 = 0; num2 < editorStates2.Length; num2++)
			{
				editorStates2[num2].SelectedNode = null;
			}
		}
		return nodeCanvas;
	}

	private static NodeEditorState[] CreateWorkingCopy(NodeEditorState[] editorStates, NodeCanvas associatedNodeCanvas)
	{
		if (editorStates == null)
		{
			return new NodeEditorState[0];
		}
		editorStates = (NodeEditorState[])editorStates.Clone();
		for (int i = 0; i < editorStates.Length; i++)
		{
			if (!(editorStates[i] == null))
			{
				NodeEditorState nodeEditorState = (editorStates[i] = Clone(editorStates[i]));
				if (nodeEditorState == null)
				{
					Debug.LogError("Failed to create a working copy for an NodeEditorState during the loading process of " + associatedNodeCanvas.name + "!");
				}
				else
				{
					nodeEditorState.canvas = associatedNodeCanvas;
				}
			}
		}
		associatedNodeCanvas.editorStates = editorStates;
		return editorStates;
	}

	private static T Clone<T>(T SO) where T : ScriptableObject
	{
		string name = SO.name;
		SO = UnityEngine.Object.Instantiate(SO);
		SO.name = name;
		return SO;
	}

	private static void AddClonedSOs(List<ScriptableObject> scriptableObjects, List<ScriptableObject> clonedScriptableObjects, ScriptableObject[] initialSOs)
	{
		scriptableObjects.AddRange(initialSOs);
		clonedScriptableObjects.AddRange(initialSOs.Select((ScriptableObject so) => Clone(so)));
	}

	private static T AddClonedSO<T>(List<ScriptableObject> scriptableObjects, List<ScriptableObject> clonedScriptableObjects, T initialSO) where T : ScriptableObject
	{
		if (initialSO == null)
		{
			return null;
		}
		scriptableObjects.Add(initialSO);
		T val = Clone(initialSO);
		clonedScriptableObjects.Add(val);
		return val;
	}

	private static T ReplaceSO<T>(List<ScriptableObject> scriptableObjects, List<ScriptableObject> clonedScriptableObjects, T initialSO) where T : ScriptableObject
	{
		if (initialSO == null)
		{
			return null;
		}
		int num = scriptableObjects.IndexOf(initialSO);
		if (num == -1)
		{
			Debug.LogError("GetWorkingCopy: ScriptableObject " + initialSO.name + " was not copied before! It will be null!");
		}
		if (num != -1)
		{
			return (T)clonedScriptableObjects[num];
		}
		return null;
	}

	public static NodeEditorState ExtractEditorState(NodeCanvas canvas, string stateName)
	{
		NodeEditorState nodeEditorState = null;
		if (canvas.editorStates.Length != 0)
		{
			nodeEditorState = canvas.editorStates.First((NodeEditorState s) => s.name == stateName);
			if (nodeEditorState == null)
			{
				nodeEditorState = canvas.editorStates[0];
			}
		}
		if (nodeEditorState == null)
		{
			nodeEditorState = ScriptableObject.CreateInstance<NodeEditorState>();
			nodeEditorState.canvas = canvas;
			canvas.editorStates = new NodeEditorState[1] { nodeEditorState };
		}
		nodeEditorState.name = stateName;
		return nodeEditorState;
	}
}
