using UnityEngine;

public class NodeEditorSettings : ScriptableObject
{
	private const string NodeEditorSettingsPath = "/Settings/NodeEditorSettings.asset";

	private static NodeEditorSettings _settings;

	[SerializeField]
	[Header("Autosave")]
	[Tooltip("Main autosave interval. 0 - no autosave. (int seconds)")]
	private int _mainAutosaveInterval = 60;

	[SerializeField]
	[Tooltip("How often autosaves should be created (in seconds).")]
	private int[] _autosaveIntervals = new int[1] { 350 };

	[SerializeField]
	[Header("Canvas Settings")]
	[Range(1f, 100f)]
	private float _maxZoomDistance = 3f;

	public static NodeEditorSettings Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = LoadOrCreate();
			}
			return _settings;
		}
	}

	public static int[] AutosaveIntervals => Settings._autosaveIntervals;

	public static float MaxZoomDistance => Settings._maxZoomDistance;

	public static int MainAutosaveInterval => Settings._mainAutosaveInterval;

	public static NodeEditorSettings LoadOrCreate()
	{
		return null;
	}
}
