using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class RGD_Settings_Graphics : SerializableBehaviour
{
	public int overallQuality;

	public int waterQuality;

	public int resolutionIndex;

	public int resolutionWidth;

	public int resolutionHeight;

	public int resolutionRefreshRate;

	public int vsync;

	public bool ambientOcclusion;

	public bool antialiasing;

	public int textureQuality;

	public ShadowQuality shadowType;

	public ShadowResolution shadowResolution;

	public int fullscreenDropdownIndex;

	public int shadowCascades;

	public float shadowDistance;

	public int fpsIndex;

	public int FOVValue;

	public int reflectionDropDownValue;

	public float brightness;

	public RGD_Settings_Graphics()
	{
		SetDefaults(default);
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext sc)
	{
		base.GetObjectData(info, sc);
		info.AddValue("Quality", overallQuality);
		info.AddValue("WaterQuality", waterQuality);
		info.AddValue("ResolutionWidth", resolutionWidth);
		info.AddValue("ResolutionHeight", resolutionHeight);
		info.AddValue("ResolutionRefreshRate", resolutionRefreshRate);
		info.AddValue("VSync", vsync);
		info.AddValue("AmbientOcclusion", ambientOcclusion);
		info.AddValue("Antialiasing", antialiasing);
		info.AddValue("TextureQuality", textureQuality);
		info.AddValue("ShadowType", shadowType);
		info.AddValue("ShadowCascade", shadowCascades);
		info.AddValue("ShadowDistance", shadowDistance);
		info.AddValue("ShadowResolution", shadowResolution);
		info.AddValue("FPSIndex", fpsIndex);
		info.AddValue("ResolutionIndex", resolutionIndex);
		info.AddValue("FOVValue", FOVValue);
		info.AddValue("ReflectionType", reflectionDropDownValue);
		info.AddValue("FullscreenModeIndex", fullscreenDropdownIndex);
		info.AddValue("Brightness", brightness);
	}

	public RGD_Settings_Graphics(SerializationInfo info, StreamingContext sc)
		: base(info, sc)
	{
		foreach (SerializationEntry item in info)
		{
			switch (item.Name)
			{
			case "Quality":
				overallQuality = (int)item.Value;
				break;
			case "ResolutionWidth":
				resolutionWidth = (int)item.Value;
				break;
			case "ResolutionHeight":
				resolutionHeight = (int)item.Value;
				break;
			case "ResolutionRefreshRate":
				resolutionRefreshRate = (int)item.Value;
				break;
			case "VSync":
				vsync = (int)item.Value;
				break;
			case "AmbientOcclusion":
				ambientOcclusion = (bool)item.Value;
				break;
			case "Antialiasing":
				antialiasing = (bool)item.Value;
				break;
			case "WaterQuality":
				waterQuality = (int)item.Value;
				break;
			case "TextureQuality":
				textureQuality = (int)item.Value;
				break;
			case "ShadowType":
				shadowType = (ShadowQuality)item.Value;
				break;
			case "ShadowCascade":
				shadowCascades = (int)item.Value;
				break;
			case "ShadowDistance":
				shadowDistance = (float)item.Value;
				break;
			case "ShadowResolution":
				shadowResolution = (ShadowResolution)item.Value;
				break;
			case "FPSIndex":
				fpsIndex = (int)item.Value;
				break;
			case "ResolutionIndex":
				resolutionIndex = (int)item.Value;
				break;
			case "FOVValue":
				FOVValue = (int)item.Value;
				break;
			case "ReflectionType":
				reflectionDropDownValue = (int)item.Value;
				break;
			case "FullscreenModeIndex":
				fullscreenDropdownIndex = (int)item.Value;
				break;
			case "Brightness":
				brightness = (float)item.Value;
				break;
			}
		}
	}

	protected override void SetDefaults(StreamingContext sc)
	{
		base.SetDefaults(sc);
		overallQuality = 2;
		waterQuality = 2;
		vsync = 0;
		resolutionWidth = -1;
		resolutionHeight = -1;
		resolutionRefreshRate = -1;
		ambientOcclusion = true;
		antialiasing = true;
		textureQuality = 1;
		shadowType = ShadowQuality.All;
		shadowCascades = 4;
		shadowDistance = 100f;
		shadowResolution = ShadowResolution.High;
		fpsIndex = 1;
		resolutionIndex = -1;
		FOVValue = 50;
		reflectionDropDownValue = 1;
		fullscreenDropdownIndex = 1;
		brightness = 0f;
	}
}
