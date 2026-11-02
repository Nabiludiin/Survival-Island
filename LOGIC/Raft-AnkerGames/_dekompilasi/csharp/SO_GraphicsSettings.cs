using UnityEngine;

[CreateAssetMenu(fileName = "GraphicsSettings", menuName = "Raft/Camera/GraphicsSettings", order = 1)]
public class SO_GraphicsSettings : ScriptableObject
{
	public TextureQuality textureQuality;

	public ShadowQuality shadowQuality;

	public ShadowCascadeType shadowCascadeType;

	public float shadowDistance;

	public void ApplyGraphicsSettingsToEngine()
	{
		SetTextureQuality(textureQuality);
		SetShadowQuality(shadowQuality);
		SetShadowCascade(shadowCascadeType);
		SetShadowDistance(shadowDistance);
	}

	public void CopyToCurrentQuality()
	{
		textureQuality = (TextureQuality)QualitySettings.masterTextureLimit;
		shadowQuality = QualitySettings.shadows;
		shadowCascadeType = (ShadowCascadeType)QualitySettings.shadowCascades;
		shadowDistance = QualitySettings.shadowDistance;
	}

	public void SetTextureQuality(TextureQuality textureQuality)
	{
		this.textureQuality = textureQuality;
		QualitySettings.masterTextureLimit = (int)(3 - textureQuality);
	}

	public void SetShadowQuality(ShadowQuality quality)
	{
		shadowQuality = quality;
		QualitySettings.shadows = quality;
	}

	public void SetShadowCascade(ShadowCascadeType type)
	{
		shadowCascadeType = type;
		QualitySettings.shadowCascades = (int)type;
	}

	public void SetShadowDistance(float distance)
	{
		shadowDistance = distance;
		QualitySettings.shadowDistance = distance;
	}
}
