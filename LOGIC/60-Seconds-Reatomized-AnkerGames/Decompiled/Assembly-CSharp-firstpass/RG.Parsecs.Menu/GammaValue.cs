using System;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace RG.Parsecs.Menu;

public class GammaValue : MonoBehaviour
{
	private PostProcessingBehaviour postProcessingBeh;

	[SerializeField]
	private SettingsSO settingsSO;

	public bool updateOnStart = true;

	public bool useMainCamera;

	private void Start()
	{
		if (updateOnStart)
		{
			UpdateGamma();
		}
	}

	private PostProcessingBehaviour GetPostProcessingBeh()
	{
		GameObject gameObject = null;
		gameObject = ((!useMainCamera) ? base.gameObject : Camera.main.gameObject);
		return gameObject.GetComponent<PostProcessingBehaviour>();
	}

	public void UpdateGamma()
	{
		try
		{
			if (postProcessingBeh == null)
			{
				postProcessingBeh = GetPostProcessingBeh();
			}
			ColorGradingModel.Settings settings = postProcessingBeh.profile.colorGrading.settings;
			settings.basic.postExposure = settingsSO.runtimeData.gammaVal;
			postProcessingBeh.profile.colorGrading.settings = settings;
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
		}
	}
}
