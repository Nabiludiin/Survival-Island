using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public class MaskModule : IRenderModule
{
	private CommandBuffer _Commands;

	private int _SubtractiveMaskId;

	private int _AdditiveMaskId;

	private Shader _VolumeFrontFastShader;

	private Shader _VolumeFrontShader;

	private Shader _VolumeBackShader;

	public void OnEnable(WaterCamera waterCamera)
	{
		_Commands = new CommandBuffer
		{
			name = "[Water] : Render Volumes and Masks"
		};
		_SubtractiveMaskId = ShaderVariables.SubtractiveMask;
		_AdditiveMaskId = ShaderVariables.AdditiveMask;
		_VolumeFrontFastShader = ShaderUtility.Instance.Get(ShaderList.VolumesFrontSimple);
		_VolumeFrontShader = ShaderUtility.Instance.Get(ShaderList.VolumesFront);
		_VolumeBackShader = ShaderUtility.Instance.Get(ShaderList.VolumesBack);
	}

	public void OnDisable(WaterCamera waterCamera)
	{
		if (_Commands != null)
		{
			Camera cameraComponent = waterCamera.CameraComponent;
			CameraEvent evt = GetEvent(waterCamera);
			cameraComponent.RemoveCommandBuffer(evt, _Commands);
			_Commands.Release();
			_Commands = null;
		}
	}

	public void OnValidate(WaterCamera waterCamera)
	{
		ShaderUtility instance = ShaderUtility.Instance;
		instance.Use(ShaderList.VolumesFrontSimple);
		instance.Use(ShaderList.VolumesFront);
		instance.Use(ShaderList.VolumesBack);
	}

	public void Process(WaterCamera waterCamera)
	{
		if (waterCamera.RenderVolumes)
		{
			bool hasSubtractiveVolumes = false;
			bool hasAdditiveVolumes = false;
			bool hasFlatMasks = false;
			List<Water> waters = ApplicationSingleton<WaterSystem>.Instance.Waters;
			for (int num = waters.Count - 1; num >= 0; num--)
			{
				waters[num].Renderer.OnSharedSubtractiveMaskRender(ref hasSubtractiveVolumes, ref hasAdditiveVolumes, ref hasFlatMasks);
			}
			SetupCamera(waterCamera);
			_Commands.Clear();
			SubtractiveMask(waterCamera, hasSubtractiveVolumes, hasFlatMasks);
			AdditiveMask(waterCamera, hasAdditiveVolumes);
			if (_Commands.sizeInBytes != 0)
			{
				Camera cameraComponent = waterCamera.CameraComponent;
				CameraEvent evt = GetEvent(waterCamera);
				cameraComponent.RemoveCommandBuffer(evt, _Commands);
				cameraComponent.AddCommandBuffer(evt, _Commands);
			}
			for (int num2 = waters.Count - 1; num2 >= 0; num2--)
			{
				waters[num2].Renderer.OnSharedMaskPostRender();
			}
		}
	}

	public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
	{
	}

	private void SubtractiveMask(WaterCamera waterCamera, bool hasSubtractiveVolumes, bool hasFlatMasks)
	{
		if (hasSubtractiveVolumes | hasFlatMasks)
		{
			RenderSubtractivePass(waterCamera, hasSubtractiveVolumes, hasFlatMasks);
		}
		else
		{
			Shader.SetGlobalTexture(_SubtractiveMaskId, DefaultTextures.Get(Color.clear));
		}
	}

	private void AdditiveMask(WaterCamera waterCamera, bool hasAdditiveVolumes)
	{
		if (hasAdditiveVolumes)
		{
			RenderAdditivePass(waterCamera);
		}
		else
		{
			Shader.SetGlobalTexture(_AdditiveMaskId, DefaultTextures.Get(Color.clear));
		}
	}

	private void RenderSubtractivePass(WaterCamera waterCamera, bool hasSubtractiveVolumes, bool hasFlatMasks)
	{
		int baseEffectWidth = waterCamera.BaseEffectWidth;
		int baseEffectHeight = waterCamera.BaseEffectHeight;
		FilterMode filter = ((!(waterCamera.BaseEffectsQuality > 0.98f)) ? FilterMode.Bilinear : FilterMode.Point);
		Camera effectsCamera = waterCamera.EffectsCamera;
		WaterCamera component = effectsCamera.GetComponent<WaterCamera>();
		_Commands.GetTemporaryRT(_SubtractiveMaskId, baseEffectWidth, baseEffectHeight, 24, filter, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear, 1, enableRandomWrite: false, RenderTextureMemoryless.None, useDynamicScale: false);
		_Commands.SetRenderTarget(_SubtractiveMaskId);
		_Commands.ClearRenderTarget(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f));
		if (hasSubtractiveVolumes)
		{
			Shader shader = (waterCamera.IsInsideSubtractiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader);
			effectsCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
			component.AddWaterRenderCommands(_Commands, shader, surfaces: false, volumes: true, waterCamera.IsInsideSubtractiveVolume);
			component.AddWaterRenderCommands(_Commands, _VolumeBackShader, surfaces: false, volumes: true, volumesTwoPass: false);
		}
		if (hasFlatMasks && waterCamera.RenderFlatMasks)
		{
			effectsCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterTempLayer;
			component.AddWaterMasksRenderCommands(_Commands);
		}
	}

	private void RenderAdditivePass(WaterCamera waterCamera)
	{
		int baseEffectWidth = waterCamera.BaseEffectWidth;
		int baseEffectHeight = waterCamera.BaseEffectHeight;
		List<Water> waters = ApplicationSingleton<WaterSystem>.Instance.Waters;
		Camera effectsCamera = waterCamera.EffectsCamera;
		WaterCamera component = effectsCamera.GetComponent<WaterCamera>();
		Shader shader = (waterCamera.IsInsideAdditiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader);
		FilterMode filter = ((!(waterCamera.BaseEffectsQuality > 0.98f)) ? FilterMode.Bilinear : FilterMode.Point);
		for (int num = waters.Count - 1; num >= 0; num--)
		{
			waters[num].Renderer.OnSharedMaskAdditiveRender();
		}
		effectsCamera.cullingMask = 1 << WaterProjectSettings.Instance.WaterLayer;
		_Commands.GetTemporaryRT(_AdditiveMaskId, baseEffectWidth, baseEffectHeight, 24, filter, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear, 1, enableRandomWrite: false, RenderTextureMemoryless.None, useDynamicScale: false);
		_Commands.SetRenderTarget(_AdditiveMaskId);
		_Commands.ClearRenderTarget(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f));
		component.AddWaterRenderCommands(_Commands, shader, surfaces: false, volumes: true, waterCamera.IsInsideAdditiveVolume);
		component.AddWaterRenderCommands(_Commands, _VolumeBackShader, surfaces: false, volumes: true, volumesTwoPass: false);
	}

	private static void SetupCamera(WaterCamera waterCamera)
	{
		Camera effectsCamera = waterCamera.EffectsCamera;
		effectsCamera.transform.position = waterCamera.transform.position;
		effectsCamera.transform.rotation = waterCamera.transform.rotation;
		effectsCamera.projectionMatrix = waterCamera.CameraComponent.projectionMatrix;
	}

	private static CameraEvent GetEvent(WaterCamera waterCamera)
	{
		if (waterCamera.CameraComponent.actualRenderingPath != RenderingPath.Forward)
		{
			return CameraEvent.BeforeGBuffer;
		}
		return CameraEvent.BeforeForwardOpaque;
	}
}
