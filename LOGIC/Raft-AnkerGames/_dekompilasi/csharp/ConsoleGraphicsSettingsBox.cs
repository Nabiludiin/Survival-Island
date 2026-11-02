using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class ConsoleGraphicsSettingsBox : OptionsMenuBox
{
	public Slider brightnessSlider;

	public PostProcessingProfile postEffects;

	private Network_Player localPlayer;

	public float BrightnessOffset => brightnessSlider.value;

	public override void SetFocus(bool gamepad)
	{
		brightnessSlider.Select();
	}

	public override BoxVisibility GetBoxVisibility()
	{
		return BoxVisibility.Console;
	}

	public override void SettingStart()
	{
	}

	public override void SettingsUpdate()
	{
		base.Update();
		_ = Raft_Network.InMenuScene;
	}

	public override void Save(RGD_Settings settings)
	{
		base.Save(settings);
		settings.graphics.brightness = brightnessSlider.value;
	}

	public override void Load(RGD_Settings settings)
	{
		base.Load(settings);
		brightnessSlider.value = settings.graphics.brightness;
		ColorGradingModel.Settings settings2 = postEffects.colorGrading.settings;
		settings2.basic.postExposure = settings.graphics.brightness;
		postEffects.colorGrading.settings = settings2;
	}

	public void OnQualityChange()
	{
	}

	public void OnFOVSliderChange()
	{
		ColorGradingModel.Settings settings = postEffects.colorGrading.settings;
		settings.basic.postExposure = brightnessSlider.value;
		postEffects.colorGrading.settings = settings;
	}
}
