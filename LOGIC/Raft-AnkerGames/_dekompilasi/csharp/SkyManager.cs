using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;

public class SkyManager : MonoBehaviour
{
	public static float normalizedDistanceToZone;

	public static bool UsingCustomEnvironmentLight = false;

	[SerializeField]
	private float lerpAmbientSpeed = 3f;

	public static ReflectionType ReflectionMode = ReflectionType.Resolution8;

	private static int[] ReflectionResolutions = new int[4] { 0, 8, 16, 32 };

	[Range(0f, 1f)]
	[SerializeField]
	private float defaultReflectionIntensity = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	private float maxNightReflectionIntensity = 1f;

	public AzureSkyController skyController;

	private EnvironmentProbe_ColorGroup currentSkyColorGroup = new EnvironmentProbe_ColorGroup();

	private EnvironmentProbe_ColorGroup lastCalculatedEnvironmentColorGroup;

	private SubregionComponent_EnvironmentProbe lastCalculatedEnvironmentProbeRegion;

	[SerializeField]
	private Cubemap cubemapSimple;

	public PostProcessingProfile postProcessingProfile;

	private Settings settings;

	public static int DesiredReflectionResolution => ReflectionResolutions[(int)ReflectionMode];

	private void Awake()
	{
		RegionManager.OnRegionUpdate = (Action)Delegate.Combine(RegionManager.OnRegionUpdate, new Action(OnRegionUpdate));
		settings = ComponentManager<Settings>.Value;
	}

	private void LateUpdate()
	{
		HandleEnvironmentLight();
		CalculateRegionShaderDarkness();
	}

	private void HandleEnvironmentLight()
	{
		float timelineGradientTime = skyController.TimelineGradientTime;
		float num = 4f * Mathf.Pow(timelineGradientTime, 2f) - 4f * timelineGradientTime + 1f;
		ColorGradingModel.Settings settings = postProcessingProfile.colorGrading.settings;
		settings.basic.postExposure = num * (this.settings.consoleGraphicsBox.BrightnessOffset * 2f + 1f) + this.settings.consoleGraphicsBox.BrightnessOffset * 0.1f;
		postProcessingProfile.colorGrading.settings = settings;
		AzureSkyLightingComponent lighting = skyController.currentDayProfile.lighting;
		if (lighting != null)
		{
			currentSkyColorGroup.skyColor = skyController.GetGradientValue(lighting.ambientSkyGradientColor, lighting.ambientSkyGradientColorIndex);
			currentSkyColorGroup.equatorColor = skyController.GetGradientValue(lighting.equatorGradientColor, lighting.equatorGradientColorIndex);
			currentSkyColorGroup.groundColor = skyController.GetGradientValue(lighting.groundGradientColor, lighting.groundGradientColorIndex);
		}
		UsingCustomEnvironmentLight = lastCalculatedEnvironmentColorGroup != null;
		Color b = (UsingCustomEnvironmentLight ? lastCalculatedEnvironmentColorGroup.skyColor : currentSkyColorGroup.skyColor);
		Color b2 = (UsingCustomEnvironmentLight ? lastCalculatedEnvironmentColorGroup.equatorColor : currentSkyColorGroup.equatorColor);
		Color b3 = (UsingCustomEnvironmentLight ? lastCalculatedEnvironmentColorGroup.groundColor : currentSkyColorGroup.groundColor);
		RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor, b, Time.deltaTime * lerpAmbientSpeed);
		RenderSettings.ambientEquatorColor = Color.Lerp(RenderSettings.ambientEquatorColor, b2, Time.deltaTime * lerpAmbientSpeed);
		RenderSettings.ambientGroundColor = Color.Lerp(RenderSettings.ambientGroundColor, b3, Time.deltaTime * lerpAmbientSpeed);
		if (Cheat.ForceReflectionsOff)
		{
			if (RenderSettings.defaultReflectionMode != DefaultReflectionMode.Skybox)
			{
				RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
			}
			if (RenderSettings.customReflection != cubemapSimple)
			{
				RenderSettings.customReflection = cubemapSimple;
			}
			if (RenderSettings.reflectionIntensity != 1f)
			{
				RenderSettings.reflectionIntensity = 1f;
			}
			return;
		}
		if (RenderSettings.defaultReflectionMode != DefaultReflectionMode.Custom)
		{
			RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		}
		float num2 = ((ReflectionMode == ReflectionType.None) ? skyController.reflectionIntensity : defaultReflectionIntensity);
		if (UsingCustomEnvironmentLight)
		{
			num2 = ((ReflectionMode != ReflectionType.None) ? (num2 * lastCalculatedEnvironmentColorGroup.reflectionModifier) : (num2 * lastCalculatedEnvironmentColorGroup.reflectionModifierNone));
		}
		if (num2 > maxNightReflectionIntensity && BedManager.BedTime())
		{
			num2 = Mathf.Lerp(num2, maxNightReflectionIntensity, BedManager.GetNormalizedNightTime());
		}
		if (RenderSettings.reflectionIntensity != num2)
		{
			RenderSettings.reflectionIntensity = num2;
		}
	}

	private void CalculateRegionShaderDarkness()
	{
		float num = 0f;
		if (lastCalculatedEnvironmentProbeRegion == null)
		{
			float V = 0f;
			Color.RGBToHSV(RenderSettings.ambientSkyColor, out var _, out var _, out V);
			num = V;
		}
		else
		{
			num = lastCalculatedEnvironmentProbeRegion.probeSettings.particleIntensityModifier;
		}
		Shader.SetGlobalFloat("LightMultiply", num);
	}

	private void OnRegionUpdate()
	{
		Transform localPlayerCameraTransform = Network_Player.LocalPlayerCameraTransform;
		if (localPlayerCameraTransform != null)
		{
			lastCalculatedEnvironmentColorGroup = CalculateLightFromPosition(localPlayerCameraTransform.position, currentSkyColorGroup, skyController.TimelineGradientTime);
			UsingCustomEnvironmentLight = lastCalculatedEnvironmentColorGroup != null;
		}
	}

	public void EnterZone()
	{
	}

	public void ExitZone()
	{
	}

	private EnvironmentProbe_ColorGroup CalculateLightFromPosition(Vector3 position, EnvironmentProbe_ColorGroup defaultColorGroup, float currentTime)
	{
		List<EnvironmentProbe_ColorWeight> list = new List<EnvironmentProbe_ColorWeight>();
		list.Clear();
		float num = 0f;
		bool flag = false;
		foreach (Subregion currentSubregion in RegionManager.CurrentSubregions)
		{
			SubregionComponent_EnvironmentProbe subRegionComponent = currentSubregion.GetSubRegionComponent<SubregionComponent_EnvironmentProbe>();
			if (!flag && subRegionComponent == null)
			{
				lastCalculatedEnvironmentProbeRegion = null;
			}
			if (subRegionComponent != null)
			{
				flag = true;
				lastCalculatedEnvironmentProbeRegion = subRegionComponent;
				float normalizedDistanceToPoint = currentSubregion.regionArea.GetNormalizedDistanceToPoint(position);
				if (normalizedDistanceToPoint > 0f)
				{
					num += normalizedDistanceToPoint;
					EnvironmentProbe_ColorWeight item = new EnvironmentProbe_ColorWeight(subRegionComponent.probeSettings.customAmbientColors ? subRegionComponent.GetSkyColor(currentTime) : (defaultColorGroup.skyColor * subRegionComponent.probeSettings.ambientModifier), subRegionComponent.probeSettings.customAmbientColors ? subRegionComponent.GetEquatorColor(currentTime) : (defaultColorGroup.equatorColor * subRegionComponent.probeSettings.ambientModifier), subRegionComponent.probeSettings.customAmbientColors ? subRegionComponent.GetGroundColor(currentTime) : (defaultColorGroup.groundColor * subRegionComponent.probeSettings.ambientModifier), subRegionComponent.probeSettings.reflectionMultiplier, subRegionComponent.probeSettings.reflectionMultiplierNone, normalizedDistanceToPoint);
					list.Add(item);
				}
			}
		}
		if (num == 0f)
		{
			return null;
		}
		if (num < 1f)
		{
			list.Add(new EnvironmentProbe_ColorWeight(defaultColorGroup.skyColor, defaultColorGroup.equatorColor, defaultColorGroup.groundColor, 1f, 1f, 1f - num));
		}
		return AverageColorsWeightBased(list.ToArray());
	}

	private EnvironmentProbe_ColorGroup AverageColorsWeightBased(params EnvironmentProbe_ColorWeight[] colorWeights)
	{
		if (colorWeights.Length == 1)
		{
			return colorWeights[0].ColorGroupWeighted;
		}
		EnvironmentProbe_ColorGroup environmentProbe_ColorGroup = new EnvironmentProbe_ColorGroup(Color.black, Color.black, Color.black, 1f, 1f);
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < colorWeights.Length; i++)
		{
			EnvironmentProbe_ColorWeight environmentProbe_ColorWeight = colorWeights[i];
			num += environmentProbe_ColorWeight.ReflectionModifierWeighted;
			num2 += environmentProbe_ColorWeight.ReflectionModifierNoneWeighted;
			environmentProbe_ColorGroup.skyColor.r += Mathf.Pow(255f * environmentProbe_ColorWeight.SkyColorWeighted.r, 2f);
			environmentProbe_ColorGroup.skyColor.g += Mathf.Pow(255f * environmentProbe_ColorWeight.SkyColorWeighted.g, 2f);
			environmentProbe_ColorGroup.skyColor.b += Mathf.Pow(255f * environmentProbe_ColorWeight.SkyColorWeighted.b, 2f);
			environmentProbe_ColorGroup.equatorColor.r += Mathf.Pow(255f * environmentProbe_ColorWeight.EquatorColorWeighted.r, 2f);
			environmentProbe_ColorGroup.equatorColor.g += Mathf.Pow(255f * environmentProbe_ColorWeight.EquatorColorWeighted.g, 2f);
			environmentProbe_ColorGroup.equatorColor.b += Mathf.Pow(255f * environmentProbe_ColorWeight.EquatorColorWeighted.b, 2f);
			environmentProbe_ColorGroup.groundColor.r += Mathf.Pow(255f * environmentProbe_ColorWeight.GroundColorWeighted.r, 2f);
			environmentProbe_ColorGroup.groundColor.g += Mathf.Pow(255f * environmentProbe_ColorWeight.GroundColorWeighted.g, 2f);
			environmentProbe_ColorGroup.groundColor.b += Mathf.Pow(255f * environmentProbe_ColorWeight.GroundColorWeighted.b, 2f);
		}
		environmentProbe_ColorGroup.reflectionModifier = Mathf.Clamp01(num / (float)colorWeights.Length);
		environmentProbe_ColorGroup.reflectionModifierNone = Mathf.Clamp01(num2 / (float)colorWeights.Length);
		environmentProbe_ColorGroup.skyColor /= (float)colorWeights.Length;
		environmentProbe_ColorGroup.equatorColor /= (float)colorWeights.Length;
		environmentProbe_ColorGroup.groundColor /= (float)colorWeights.Length;
		environmentProbe_ColorGroup.skyColor.r = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.skyColor.r), 0f, 1f);
		environmentProbe_ColorGroup.skyColor.g = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.skyColor.g), 0f, 1f);
		environmentProbe_ColorGroup.skyColor.b = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.skyColor.b), 0f, 1f);
		environmentProbe_ColorGroup.skyColor.a = 1f;
		environmentProbe_ColorGroup.equatorColor.r = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.equatorColor.r), 0f, 1f);
		environmentProbe_ColorGroup.equatorColor.g = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.equatorColor.g), 0f, 1f);
		environmentProbe_ColorGroup.equatorColor.b = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.equatorColor.b), 0f, 1f);
		environmentProbe_ColorGroup.equatorColor.a = 1f;
		environmentProbe_ColorGroup.groundColor.r = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.groundColor.r), 0f, 1f);
		environmentProbe_ColorGroup.groundColor.g = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.groundColor.g), 0f, 1f);
		environmentProbe_ColorGroup.groundColor.b = Helper.MapValue(0f, 255f, Mathf.Sqrt(environmentProbe_ColorGroup.groundColor.b), 0f, 1f);
		environmentProbe_ColorGroup.groundColor.a = 1f;
		return environmentProbe_ColorGroup;
	}
}
