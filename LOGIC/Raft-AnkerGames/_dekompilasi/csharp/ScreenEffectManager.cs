using EZCameraShake;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
	public SO_ScreenEffect screenEffect_Poison;

	public SO_ScreenEffect screenEffect_Radiation;

	public ScreenEffect_Image screenImage_Blood;

	public ScreenEffect_Image screenImage_Poison;

	public ScreenEffect_Image screenImage_Radiation;

	private void Awake()
	{
		ComponentManager<ScreenEffectManager>.Value = this;
	}

	private void OnDestroy()
	{
		screenImage_Blood.SetAlpha(0f);
		screenImage_Poison.SetAlpha(0f);
		screenImage_Radiation.SetAlpha(0f);
	}

	public void ShakeCamera(SO_CameraShaker cameraShakeAsset)
	{
		CameraShaker.Instance.Shake(cameraShakeAsset.ShakerInstance);
		if (cameraShakeAsset.hapticEvent > 0)
		{
			SimpleMonoBehaviourSingleton<HapticManager>.Instance?.PlayOneShot((HapticEvent)cameraShakeAsset.hapticEvent);
		}
	}

	public void ShakeCamera(SO_CameraShaker cameraShakeAsset, Vector3 fromPosition, float maxDistance)
	{
		if (Helper.LocalPlayerIsWithinDistance(fromPosition, maxDistance))
		{
			ShakeCamera(cameraShakeAsset);
		}
	}
}
