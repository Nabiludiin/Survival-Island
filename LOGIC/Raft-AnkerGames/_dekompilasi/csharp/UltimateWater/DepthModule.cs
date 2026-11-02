using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public class DepthModule : IRenderModule
{
	private CommandBuffer _Commands;

	private RenderTextureFormat _DepthFormat;

	private RenderTextureFormat _BlendedDepthFormat;

	private Material _DepthBlitCache;

	private int _WaterDepthTextureId;

	private int _WaterlessDepthId;

	private int _CameraDepthTextureId;

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
		if (!Application.isPlaying)
		{
			return;
		}
		RenderTextureFormat? format = Compatibility.GetFormat(RenderTextureFormat.Depth, new RenderTextureFormat[3]
		{
			RenderTextureFormat.RFloat,
			RenderTextureFormat.RHalf,
			RenderTextureFormat.R8
		});
		RenderTextureFormat? format2 = Compatibility.GetFormat(RenderTextureFormat.RFloat, new RenderTextureFormat[1] { RenderTextureFormat.RHalf });
		if (!format.HasValue || !format2.HasValue)
		{
			return;
		}
		_Commands = new CommandBuffer
		{
			name = "[UWS] DepthModule - Render Depth"
		};
		_CameraDepthTextureId = ShaderVariables.CameraDepthTexture2;
		_WaterDepthTextureId = ShaderVariables.WaterDepthTexture;
		_WaterlessDepthId = ShaderVariables.WaterlessDepthTexture;
		_DepthFormat = format.Value;
		_BlendedDepthFormat = format.Value;
		if (SystemInfo.graphicsShaderLevel >= 50)
		{
			return;
		}
		_BlendedDepthFormat = format2.Value;
		if (_BlendedDepthFormat == RenderTextureFormat.RFloat && waterCamera.BaseEffectsQuality < 0.2f)
		{
			RenderTextureFormat? format3 = Compatibility.GetFormat(RenderTextureFormat.RHalf);
			if (format3.HasValue)
			{
				_BlendedDepthFormat = format3.Value;
			}
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
			if (_Commands != null)
			{
				_Commands.Release();
				_Commands = null;
			}
		}
	}

	public void OnValidate(WaterCamera waterCamera)
	{
		ShaderUtility.Instance.Use(ShaderList.WaterDepth);
	}

	public void Process(WaterCamera waterCamera)
	{
		if (Application.isPlaying && (waterCamera.RenderWaterDepth || waterCamera.RenderMode == WaterRenderMode.ImageEffectDeferred))
		{
			int baseEffectWidth = waterCamera.BaseEffectWidth;
			int baseEffectHeight = waterCamera.BaseEffectHeight;
			FilterMode filter = ((!(waterCamera.BaseEffectsQuality > 0.98f)) ? FilterMode.Bilinear : FilterMode.Point);
			Material depthBlit = _DepthBlit;
			if (_Commands == null)
			{
				_Commands = new CommandBuffer
				{
					name = "[Ultimate Water] Render Depth"
				};
			}
			_Commands.Clear();
			_Commands.GetTemporaryRT(_WaterDepthTextureId, baseEffectWidth, baseEffectHeight, (_DepthFormat == RenderTextureFormat.Depth) ? 32 : 16, filter, _DepthFormat, RenderTextureReadWrite.Linear);
			_Commands.SetRenderTarget(_WaterDepthTextureId);
			_Commands.ClearRenderTarget(clearDepth: true, clearColor: true, Color.white);
			waterCamera.AddWaterRenderCommands(_Commands, ShaderUtility.Instance.Get(ShaderList.WaterDepth), surfaces: true, volumes: true, volumesTwoPass: false);
			_Commands.GetTemporaryRT(_WaterlessDepthId, baseEffectWidth, baseEffectHeight, (_BlendedDepthFormat == RenderTextureFormat.Depth) ? 32 : 0, FilterMode.Point, _BlendedDepthFormat, RenderTextureReadWrite.Linear);
			_Commands.SetRenderTarget(_WaterlessDepthId);
			_Commands.DrawMesh(Quads.TriangleMeshInversedY, Matrix4x4.identity, depthBlit, 0, (_BlendedDepthFormat == RenderTextureFormat.Depth) ? 3 : 0);
			_Commands.GetTemporaryRT(_CameraDepthTextureId, baseEffectWidth, baseEffectHeight, (_BlendedDepthFormat == RenderTextureFormat.Depth) ? 32 : 0, FilterMode.Point, _BlendedDepthFormat, RenderTextureReadWrite.Linear);
			_Commands.SetRenderTarget(_CameraDepthTextureId);
			_Commands.ClearRenderTarget(clearDepth: true, clearColor: true, Color.white);
			_Commands.DrawMesh(Quads.TriangleMeshInversedY, Matrix4x4.identity, depthBlit, 0, (_BlendedDepthFormat != RenderTextureFormat.Depth) ? 1 : 4);
			_Commands.SetGlobalTexture("_CameraDepthTexture", _CameraDepthTextureId);
			Unbind(waterCamera);
			Bind(waterCamera);
		}
	}

	public void Render(WaterCamera waterCamera, RenderTexture source, RenderTexture destination)
	{
	}

	private void Bind(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		bool singlePassStereoRendering = WaterProjectSettings.Instance.SinglePassStereoRendering;
		cameraComponent.AddCommandBuffer((cameraComponent.actualRenderingPath != RenderingPath.Forward) ? CameraEvent.BeforeLighting : ((!singlePassStereoRendering) ? CameraEvent.AfterDepthTexture : CameraEvent.BeforeForwardOpaque), _Commands);
	}

	private void Unbind(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		bool singlePassStereoRendering = WaterProjectSettings.Instance.SinglePassStereoRendering;
		cameraComponent.RemoveCommandBuffer((!singlePassStereoRendering) ? CameraEvent.AfterDepthTexture : CameraEvent.BeforeForwardOpaque, _Commands);
		cameraComponent.RemoveCommandBuffer(CameraEvent.BeforeLighting, _Commands);
	}
}
