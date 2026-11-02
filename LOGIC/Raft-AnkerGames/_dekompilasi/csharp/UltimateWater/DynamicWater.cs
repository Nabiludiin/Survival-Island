using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace UltimateWater;

public sealed class DynamicWater : WaterModule
{
	[Serializable]
	public class Data
	{
		public int Antialiasing = 1;

		public LayerMask CustomEffectsLayerMask = -1;
	}

	private class Renderers
	{
		public readonly List<ILocalDisplacementRenderer> LocalDisplacement = new List<ILocalDisplacementRenderer>();

		public readonly List<ILocalDisplacementMaskRenderer> LocalDisplacementMask = new List<ILocalDisplacementMaskRenderer>();

		public readonly List<ILocalFoamRenderer> LocalFoam = new List<ILocalFoamRenderer>();

		public readonly List<ILocalDiffuseRenderer> LocalDiffuse = new List<ILocalDiffuseRenderer>();

		public void Add<T>(T renderer) where T : IDynamicWaterEffects
		{
			ILocalDisplacementRenderer localDisplacementRenderer = renderer as ILocalDisplacementRenderer;
			ILocalDisplacementMaskRenderer localDisplacementMaskRenderer = renderer as ILocalDisplacementMaskRenderer;
			ILocalFoamRenderer localFoamRenderer = renderer as ILocalFoamRenderer;
			ILocalDiffuseRenderer localDiffuseRenderer = renderer as ILocalDiffuseRenderer;
			IWavesInteractive wavesInteractive = renderer as IWavesInteractive;
			if (localDisplacementRenderer != null)
			{
				LocalDisplacement.Add(localDisplacementRenderer);
			}
			if (localDisplacementMaskRenderer != null)
			{
				LocalDisplacementMask.Add(localDisplacementMaskRenderer);
			}
			if (localFoamRenderer != null)
			{
				LocalFoam.Add(localFoamRenderer);
			}
			if (wavesInteractive != null)
			{
				Interactions.Add(wavesInteractive);
			}
			if (localDiffuseRenderer != null)
			{
				LocalDiffuse.Add(localDiffuseRenderer);
			}
		}

		public void Remove<T>(T renderer) where T : IDynamicWaterEffects
		{
			ILocalDisplacementRenderer localDisplacementRenderer = renderer as ILocalDisplacementRenderer;
			ILocalDisplacementMaskRenderer localDisplacementMaskRenderer = renderer as ILocalDisplacementMaskRenderer;
			ILocalFoamRenderer localFoamRenderer = renderer as ILocalFoamRenderer;
			ILocalDiffuseRenderer localDiffuseRenderer = renderer as ILocalDiffuseRenderer;
			IWavesInteractive wavesInteractive = renderer as IWavesInteractive;
			if (localDisplacementRenderer != null)
			{
				LocalDisplacement.Remove(localDisplacementRenderer);
			}
			if (localDisplacementMaskRenderer != null)
			{
				LocalDisplacementMask.Remove(localDisplacementMaskRenderer);
			}
			if (localFoamRenderer != null)
			{
				LocalFoam.Remove(localFoamRenderer);
			}
			if (wavesInteractive != null)
			{
				Interactions.Remove(wavesInteractive);
			}
			if (localDiffuseRenderer != null)
			{
				LocalDiffuse.Remove(localDiffuseRenderer);
			}
		}
	}

	private IOverlaysRenderer[] _OverlayRenderers;

	private Material _MapLocalDisplacementsMaterial;

	private static readonly Renderers _Renderers = new Renderers();

	public static List<IWavesInteractive> Interactions = new List<IWavesInteractive>();

	private readonly Water _Water;

	private readonly Data _Data;

	private readonly bool _OnlyColor;

	private readonly Shader _MapLocalDisplacementsShader;

	private static CommandBuffer _RenderDisplacementBuffer;

	private static CommandBuffer _RenderDisplacementMaskBuffer;

	private static CommandBuffer _RenderFoamBuffer;

	private static CommandBuffer _RenderDiffuseBuffer;

	private readonly Dictionary<Camera, DynamicWaterCameraData> _Buffers = new Dictionary<Camera, DynamicWaterCameraData>();

	private readonly List<Camera> _LostCameras = new List<Camera>();

	private static readonly RenderTargetIdentifier[] _CustomEffectsBuffers = new RenderTargetIdentifier[2];

	public Water Water => _Water;

	public DynamicWater(Water water, Data data)
	{
		_Water = water;
		_Data = data;
		_OnlyColor = true;
		CreateCommandBuffers();
		Validate();
		_MapLocalDisplacementsShader = Shader.Find("UltimateWater/Utility/Map Local Displacements");
	}

	public void ValidateWaterComponents()
	{
		_OverlayRenderers = _Water.GetComponentsInChildren<IOverlaysRenderer>();
		int[] array = new int[_OverlayRenderers.Length];
		for (int i = 0; i < array.Length; i++)
		{
			object[] customAttributes = _OverlayRenderers[i].GetType().GetCustomAttributes(typeof(OverlayRendererOrderAttribute), inherit: true);
			if (customAttributes.Length != 0)
			{
				array[i] = ((OverlayRendererOrderAttribute)customAttributes[0]).Priority;
			}
		}
		Array.Sort(array, _OverlayRenderers);
	}

	public static void AddRenderer<T>(T renderer) where T : IDynamicWaterEffects
	{
		_Renderers.Add(renderer);
	}

	public static void RemoveRenderer<T>(T renderer) where T : IDynamicWaterEffects
	{
		_Renderers.Remove(renderer);
	}

	public void RenderTotalDisplacementMap(WaterCamera camera, RenderTexture renderTexture)
	{
		Rect localMapsRect = camera.LocalMapsRect;
		Camera effectsCamera = camera.EffectsCamera;
		effectsCamera.enabled = false;
		effectsCamera.stereoTargetEye = StereoTargetEyeMask.None;
		effectsCamera.depthTextureMode = DepthTextureMode.None;
		effectsCamera.renderingPath = RenderingPath.VertexLit;
		effectsCamera.orthographic = true;
		effectsCamera.orthographicSize = localMapsRect.width * 0.5f;
		effectsCamera.farClipPlane = 2000f;
		effectsCamera.ResetProjectionMatrix();
		effectsCamera.clearFlags = CameraClearFlags.Nothing;
		effectsCamera.allowHDR = true;
		effectsCamera.transform.position = new Vector3(localMapsRect.center.x, 1000f, localMapsRect.center.y);
		effectsCamera.transform.rotation = Quaternion.LookRotation(new Vector3(0f, -1f, 0f), new Vector3(0f, 0f, 1f));
		WaterCamera component = effectsCamera.GetComponent<WaterCamera>();
		component.enabled = true;
		component.GeometryType = WaterGeometryType.UniformGrid;
		component.ForcedVertexCount = renderTexture.width * renderTexture.height >> 2;
		component.RenderWaterWithShader("[Ultimate Water] Render Total Displacement Map", renderTexture, _MapLocalDisplacementsShader, _Water);
	}

	public DynamicWaterCameraData GetCameraOverlaysData(Camera camera, bool createIfNotExists = true)
	{
		if (!_Buffers.TryGetValue(camera, out var value) & createIfNotExists)
		{
			value = (_Buffers[camera] = new DynamicWaterCameraData(this, WaterCamera.GetWaterCamera(camera), _Data.Antialiasing, _OnlyColor));
			if (!_OnlyColor)
			{
				DrawDisplacementMaskRenderers(value);
			}
			value.SwapFoamMaps();
			for (int i = 0; i < _OverlayRenderers.Length; i++)
			{
				_OverlayRenderers[i].RenderOverlays(value);
			}
		}
		if (value != null)
		{
			value._LastFrameUsed = Time.frameCount;
		}
		return value;
	}

	internal override void OnWaterRender(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		if (waterCamera.Type != WaterCamera.CameraType.Normal || !Application.isPlaying || (!WaterProjectSettings.Instance.RenderInSceneView && WaterUtilities.IsSceneViewCamera(cameraComponent)))
		{
			return;
		}
		Shader.SetGlobalVector("_TileSizesInv", _Water.WindWaves.TileSizesInv);
		DynamicWaterCameraData cameraOverlaysData = GetCameraOverlaysData(cameraComponent);
		cameraOverlaysData.SwapFoamMaps();
		cameraOverlaysData.ClearOverlays();
		if (!_OnlyColor)
		{
			DrawDisplacementMaskRenderers(cameraOverlaysData);
			DrawDisplacementRenderers(cameraOverlaysData);
		}
		for (int i = 0; i < _OverlayRenderers.Length; i++)
		{
			_OverlayRenderers[i].RenderOverlays(cameraOverlaysData);
		}
		if (_Water.Foam != null)
		{
			if (cameraOverlaysData.Camera.SubmersionState != SubmersionState.Full)
			{
				cameraOverlaysData.ClearFoam();
				_Water.Foam.RenderOverlays(cameraOverlaysData);
				cameraOverlaysData._FoamContentWritten = true;
			}
			else if (cameraOverlaysData._FoamContentWritten)
			{
				cameraOverlaysData.ClearFoam();
				cameraOverlaysData._FoamContentWritten = false;
			}
		}
		for (int j = 0; j < _OverlayRenderers.Length; j++)
		{
			_OverlayRenderers[j].RenderFoam(cameraOverlaysData);
		}
		int localDiffuseMap = ShaderVariables.LocalDiffuseMap;
		MaterialPropertyBlock propertyBlock = _Water.Renderer.PropertyBlock;
		if (!_OnlyColor)
		{
			propertyBlock.SetTexture(ShaderVariables.LocalDisplacementMap, cameraOverlaysData.DynamicDisplacementMap);
			propertyBlock.SetTexture(ShaderVariables.LocalNormalMap, cameraOverlaysData.NormalMap);
			propertyBlock.SetTexture(ShaderVariables.TotalDisplacementMap, cameraOverlaysData.TotalDisplacementMap);
			propertyBlock.SetTexture(ShaderVariables.DisplacementsMask, cameraOverlaysData.DisplacementsMask);
		}
		propertyBlock.SetTexture(localDiffuseMap, cameraOverlaysData.DiffuseMap);
		Shader.SetGlobalTexture("DIFFUSE", cameraOverlaysData.DiffuseMap);
		if (waterCamera.MainWater == _Water)
		{
			Shader.SetGlobalTexture(ShaderVariables.TotalDisplacementMap, cameraOverlaysData.TotalDisplacementMap);
		}
		DrawFoamRenderers(cameraOverlaysData);
		DrawDiffuseRenderers(cameraOverlaysData);
	}

	internal override void Enable()
	{
		ValidateWaterComponents();
		if (_MapLocalDisplacementsMaterial == null)
		{
			_MapLocalDisplacementsMaterial = new Material(_MapLocalDisplacementsShader)
			{
				hideFlags = HideFlags.DontSave
			};
		}
	}

	internal override void Disable()
	{
		foreach (KeyValuePair<Camera, DynamicWaterCameraData> buffer in _Buffers)
		{
			buffer.Value.Dispose();
		}
		_Buffers.Clear();
	}

	internal override void Destroy()
	{
		foreach (KeyValuePair<Camera, DynamicWaterCameraData> buffer in _Buffers)
		{
			buffer.Value.Dispose();
		}
		_Buffers.Clear();
	}

	internal override void Validate()
	{
	}

	internal override void Update()
	{
		int num = Time.frameCount - 3;
		Dictionary<Camera, DynamicWaterCameraData>.Enumerator enumerator = _Buffers.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.Value._LastFrameUsed < num)
			{
				enumerator.Current.Value.Dispose();
				_LostCameras.Add(enumerator.Current.Key);
			}
		}
		enumerator.Dispose();
		for (int i = 0; i < _LostCameras.Count; i++)
		{
			_Buffers.Remove(_LostCameras[i]);
		}
		_LostCameras.Clear();
	}

	private static void DrawDisplacementRenderers(DynamicWaterCameraData overlays)
	{
		Camera cameraComponent = overlays.Camera.CameraComponent;
		_CustomEffectsBuffers[0] = overlays.DynamicDisplacementMap;
		_CustomEffectsBuffers[1] = overlays.NormalMap;
		GL.PushMatrix();
		GL.modelview = cameraComponent.worldToCameraMatrix;
		GL.LoadProjectionMatrix(cameraComponent.projectionMatrix);
		_RenderDisplacementBuffer.Clear();
		_RenderDisplacementBuffer.SetRenderTarget(_CustomEffectsBuffers, overlays.DynamicDisplacementMap);
		List<ILocalDisplacementRenderer> localDisplacement = _Renderers.LocalDisplacement;
		for (int num = localDisplacement.Count - 1; num >= 0; num--)
		{
			localDisplacement[num].RenderLocalDisplacement(_RenderDisplacementBuffer, overlays);
		}
		Graphics.ExecuteCommandBuffer(_RenderDisplacementBuffer);
		GL.PopMatrix();
	}

	private static void DrawDisplacementMaskRenderers(DynamicWaterCameraData overlays)
	{
		Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));
		Camera cameraComponent = overlays.Camera.CameraComponent;
		GL.PushMatrix();
		GL.modelview = cameraComponent.worldToCameraMatrix;
		GL.LoadProjectionMatrix(cameraComponent.projectionMatrix);
		_RenderDisplacementMaskBuffer.Clear();
		_RenderDisplacementMaskBuffer.SetRenderTarget(overlays.DisplacementsMask);
		List<ILocalDisplacementMaskRenderer> localDisplacementMask = _Renderers.LocalDisplacementMask;
		for (int num = localDisplacementMask.Count - 1; num >= 0; num--)
		{
			localDisplacementMask[num].RenderLocalMask(_RenderDisplacementMaskBuffer, overlays);
		}
		Graphics.ExecuteCommandBuffer(_RenderDisplacementMaskBuffer);
		GL.PopMatrix();
	}

	private static void DrawFoamRenderers(DynamicWaterCameraData overlays)
	{
		List<ILocalFoamRenderer> localFoam = _Renderers.LocalFoam;
		if (localFoam.Count != 0)
		{
			for (int num = localFoam.Count - 1; num >= 0; num--)
			{
				localFoam[num].Enable();
			}
			Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));
			Camera cameraComponent = overlays.Camera.CameraComponent;
			GL.PushMatrix();
			GL.modelview = cameraComponent.worldToCameraMatrix;
			GL.LoadProjectionMatrix(cameraComponent.projectionMatrix);
			_RenderFoamBuffer.Clear();
			_RenderFoamBuffer.SetRenderTarget(overlays.FoamMap);
			for (int num2 = localFoam.Count - 1; num2 >= 0; num2--)
			{
				localFoam[num2].RenderLocalFoam(_RenderFoamBuffer, overlays);
			}
			Graphics.ExecuteCommandBuffer(_RenderFoamBuffer);
			GL.PopMatrix();
			for (int num3 = localFoam.Count - 1; num3 >= 0; num3--)
			{
				localFoam[num3].Disable();
			}
		}
	}

	private static void DrawDiffuseRenderers(DynamicWaterCameraData overlays)
	{
		List<ILocalDiffuseRenderer> localDiffuse = _Renderers.LocalDiffuse;
		for (int num = localDiffuse.Count - 1; num >= 0; num--)
		{
			localDiffuse[num].Enable();
		}
		Camera cameraComponent = overlays.Camera.CameraComponent;
		List<ILocalDiffuseRenderer> list = new List<ILocalDiffuseRenderer>();
		for (int num2 = localDiffuse.Count - 1; num2 >= 0; num2--)
		{
			if (localDiffuse[num2].IsVisible(overlays.Camera))
			{
				list.Add(localDiffuse[num2]);
			}
		}
		if (list.Count > 0)
		{
			Shader.SetGlobalTexture("_CameraDepthTexture", DefaultTextures.Get(Color.white));
			GL.PushMatrix();
			GL.modelview = cameraComponent.worldToCameraMatrix;
			GL.LoadProjectionMatrix(cameraComponent.projectionMatrix);
			_RenderDiffuseBuffer.Clear();
			_RenderDiffuseBuffer.SetRenderTarget(overlays.DiffuseMap);
			_RenderDiffuseBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.clear);
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				list[num3].RenderLocalDiffuse(_RenderDiffuseBuffer, overlays);
			}
			Graphics.ExecuteCommandBuffer(_RenderDiffuseBuffer);
			GL.PopMatrix();
			overlays._DiffuseContentWritten = true;
		}
		else if (overlays._DiffuseContentWritten)
		{
			_RenderDiffuseBuffer.Clear();
			_RenderDiffuseBuffer.SetRenderTarget(overlays.DiffuseMap);
			_RenderDiffuseBuffer.ClearRenderTarget(clearDepth: false, clearColor: true, Color.clear);
			Graphics.ExecuteCommandBuffer(_RenderDiffuseBuffer);
			overlays._DiffuseContentWritten = false;
		}
		for (int num4 = localDiffuse.Count - 1; num4 >= 0; num4--)
		{
			localDiffuse[num4].Disable();
		}
	}

	private static void CreateCommandBuffers()
	{
		_RenderDisplacementBuffer = new CommandBuffer
		{
			name = "[Ultimate Water] Displacement Renderers"
		};
		_RenderDisplacementMaskBuffer = new CommandBuffer
		{
			name = "[Ultimate Water] Displacement Mask Renderers"
		};
		_RenderFoamBuffer = new CommandBuffer
		{
			name = "[Ultimate Water] Foam Renderers"
		};
		_RenderDiffuseBuffer = new CommandBuffer
		{
			name = "[Ultimate Water] Diffuse Renderers"
		};
	}

	private static void ReleaseCommandBuffers()
	{
		if (_RenderDisplacementBuffer != null)
		{
			_RenderDisplacementBuffer.Release();
		}
		if (_RenderDisplacementMaskBuffer != null)
		{
			_RenderDisplacementMaskBuffer.Release();
		}
		if (_RenderFoamBuffer != null)
		{
			_RenderFoamBuffer.Release();
		}
		if (_RenderDiffuseBuffer != null)
		{
			_RenderDiffuseBuffer.Release();
		}
	}
}
