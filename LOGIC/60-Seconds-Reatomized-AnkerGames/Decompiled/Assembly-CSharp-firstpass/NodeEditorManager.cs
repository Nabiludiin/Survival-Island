using UnityEngine;

public abstract class NodeEditorManager : MonoBehaviour
{
	[SerializeField]
	[Tooltip("Reference to the settings file.")]
	private NodeEditorSettings _settings;

	public NodeEditorSettings Settings => _settings;
}
