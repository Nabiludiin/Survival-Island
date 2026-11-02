using System.Collections.Generic;
using Cinemachine.PostFX;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace RG.Parsecs.Scavenge;

public class PostProcessSettingsSelector : MonoBehaviour
{
	public PostProcessingBehaviour PostProcessingBehaviour;

	public CinemachinePostFX CinemachinePostFX;

	public List<PostProcessSettingsLevel> Levels = new List<PostProcessSettingsLevel>();

	private void Awake()
	{
		PostProcessSettingsLevel postProcessSettingsLevel = Levels.Find((PostProcessSettingsLevel l) => l.Name == QualitySettings.names[QualitySettings.GetQualityLevel()]);
		if (postProcessSettingsLevel != null)
		{
			PostProcessingBehaviour.profile = postProcessSettingsLevel.Profile;
			CinemachinePostFX.m_Profile = postProcessSettingsLevel.Profile;
		}
	}
}
