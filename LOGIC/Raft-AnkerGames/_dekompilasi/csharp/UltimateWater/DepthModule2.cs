using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public class DepthModule2 : IRenderModule
{
	private CommandBuffer _CopyGbufferDepthToWaterlessDepthCommands;

	private CommandBuffer _CopyGbufferDepthToForwardDepth;

	private Material _DepthBlitCache;

	private int _WaterlessDepthId;

	private Material _DepthBlit
	{
		get
		{
			if (!(_DepthBlitCache != null))
			{
				return _DepthBlitCache = ShaderUtility.Instance.CreateMaterial(ShaderList.DepthCopy, HideFlags.DontSave);
			}
			return _DepthBlitCache;
		}
	}

	public void OnEnable(WaterCamera waterCamera)
	{
		if (Application.isPlaying)
		{
			_CopyGbufferDepthToWaterlessDepthCommands = new CommandBuffer
			{
				name = "[UWS] DepthModule - Render GBuffer Depth to Waterless Depth"
			};
			_CopyGbufferDepthToForwardDepth = new CommandBuffer
			{
				name = "[UWS] DepthModule - Copy GBuffer Depth to Forward Depth"
			};
			_WaterlessDepthId = ShaderVariables.WaterlessDepthTexture;
		}
	}

	public void OnDisable(WaterCamera waterCamera)
	{
		if (Application.isPlaying)
		{
			Unbind(waterCamera);
			if (_DepthBlitCache != null)
			{
				_DepthBlitCache.Destroy();
				_DepthBlitCache = null;
			}
			if (_CopyGbufferDepthToWaterlessDepthCommands != null)
			{
				_CopyGbufferDepthToWaterlessDepthCommands.Release();
				_CopyGbufferDepthToWaterlessDepthCommands = null;
			}
			if (_CopyGbufferDepthToForwardDepth != null)
			{
				_CopyGbufferDepthToForwardDepth.Release();
				_CopyGbufferDepthToForwardDepth = null;
			}
		}
	}

	public void OnValidate(WaterCamera waterCamera)
	{
		ShaderUtility.Instance.Use(ShaderList.WaterDepth);
	}

	public void Process(WaterCamera waterCamera)
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (!waterCamera.RenderWaterDepth && waterCamera.RenderMode != WaterRenderMode.ImageEffectDeferred)
		{
			Unbind(waterCamera);
			return;
		}
		int baseEffectWidth = waterCamera.BaseEffectWidth;
		int baseEffectHeight = waterCamera.BaseEffectHeight;
		if (_CopyGbufferDepthToWaterlessDepthCommands == null)
		{
			_CopyGbufferDepthToWaterlessDepthCommands = new CommandBuffer
			{
				name = "[UWS] DepthModule - Render GBuffer Depth to Waterless Depth"
			};
		}
		_CopyGbufferDepthToWaterlessDepthCommands.Clear();
		_CopyGbufferDepthToWaterlessDepthCommands.GetTemporaryRT(_WaterlessDepthId, baseEffectWidth, baseEffectHeight, 32, FilterMode.Point, RenderTextureFormat.Depth, RenderTextureReadWrite.Linear, 1, enableRandomWrite: false, RenderTextureMemoryless.None, useDynamicScale: false);
		_CopyGbufferDepthToWaterlessDepthCommands.SetRenderTarget(_WaterlessDepthId);
		_CopyGbufferDepthToWaterlessDepthCommands.DrawMesh(Quads.TriangleMeshInversedY, Matrix4x4.identity, _DepthBlit, 0, 3);
		if (_CopyGbufferDepthToForwardDepth == null)
		{
			_CopyGbufferDepthToForwardDepth = new CommandBuffer
			{
				name = "[UWS] DepthModule - Copy GBuffer Depth to Forward Depth"
			};
		}
		_CopyGbufferDepthToForwardDepth.Clear();
		_CopyGbufferDepthToForwardDepth.SetRenderTarget(BuiltinRenderTextureType.Depth);
		_CopyGbufferDepthToForwardDepth.DrawMesh(Quads.TriangleMeshInversedY, Matrix4x4.identity, _DepthBlit, 0, 3);
		Unbind(waterCamera);
		Bind(waterCamera);
	}

	public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
	{
	}

	private void Bind(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		cameraComponent.AddCommandBuffer(CameraEvent.BeforeSkybox, _CopyGbufferDepthToWaterlessDepthCommands);
		cameraComponent.AddCommandBuffer(CameraEvent.BeforeForwardAlpha, _CopyGbufferDepthToForwardDepth);
	}

	private void Unbind(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		if (_CopyGbufferDepthToWaterlessDepthCommands != null)
		{
			cameraComponent.RemoveCommandBuffer(CameraEvent.BeforeSkybox, _CopyGbufferDepthToWaterlessDepthCommands);
		}
		if (_CopyGbufferDepthToForwardDepth != null)
		{
			cameraComponent.RemoveCommandBuffer(CameraEvent.BeforeForwardAlpha, _CopyGbufferDepthToForwardDepth);
		}
	}
}
