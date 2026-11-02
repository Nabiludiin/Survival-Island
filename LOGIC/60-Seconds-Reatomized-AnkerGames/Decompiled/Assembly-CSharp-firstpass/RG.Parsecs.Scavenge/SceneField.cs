using System;
using UnityEngine;

namespace RG.Parsecs.Scavenge;

[Serializable]
public class SceneField
{
	[SerializeField]
	private UnityEngine.Object m_SceneAsset;

	[SerializeField]
	private string m_SceneName = "";

	public string SceneName => m_SceneName;

	public static implicit operator string(SceneField sceneField)
	{
		return sceneField.SceneName;
	}

	public bool Validate()
	{
		if (m_SceneName != m_SceneAsset.name)
		{
			Debug.LogWarning("Scena z inna nazwa " + m_SceneName + " vs " + m_SceneAsset.name);
			m_SceneName = m_SceneAsset.name;
			return true;
		}
		return false;
	}
}
