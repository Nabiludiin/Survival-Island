using UnityEngine;

namespace UltimateWater;

[ExecuteInEditMode]
public sealed class WaterCameraIME : MonoBehaviour
{
	private WaterCamera _WaterCamera;

	private void Awake()
	{
		_WaterCamera = GetComponent<WaterCamera>();
	}

	[ImageEffectOpaque]
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_WaterCamera == null)
		{
			Graphics.Blit(source, destination);
			if (Application.isPlaying)
			{
				Object.Destroy(this);
			}
		}
		else
		{
			_WaterCamera.OnRenderImageCallback(source, destination);
		}
	}
}
