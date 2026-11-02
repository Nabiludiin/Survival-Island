using System;
using System.Collections.Generic;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[Serializable]
public class WaterRenderer
{
	[SerializeField]
	[FormerlySerializedAs("volumeFrontShader")]
	[HideInInspector]
	private Shader _VolumeFrontShader;

	[FormerlySerializedAs("volumeFrontFastShader")]
	[HideInInspector]
	[SerializeField]
	private Shader _VolumeFrontFastShader;

	[HideInInspector]
	[SerializeField]
	[FormerlySerializedAs("volumeBackShader")]
	private Shader _VolumeBackShader;

	[FormerlySerializedAs("reflectionProbeAnchor")]
	[SerializeField]
	private Transform _ReflectionProbeAnchor;

	[SerializeField]
	[FormerlySerializedAs("shadowCastingMode")]
	private ShadowCastingMode _ShadowCastingMode;

	[SerializeField]
	[FormerlySerializedAs("useSharedMask")]
	private bool _UseSharedMask = true;

	private Water _Water;

	private MaterialPropertyBlock _PropertyBlock;

	private RenderTexture _AdditiveMaskTexture;

	private RenderTexture _SubtractiveMaskTexture;

	private readonly List<WaterSimpleMask> _Masks = new List<WaterSimpleMask>();

	public int MaskCount => _Masks.Count;

	public MaterialPropertyBlock PropertyBlock
	{
		get
		{
			if (_PropertyBlock == null)
			{
				return _PropertyBlock = new MaterialPropertyBlock();
			}
			return _PropertyBlock;
		}
	}

	public Transform ReflectionProbeAnchor
	{
		get
		{
			return _ReflectionProbeAnchor;
		}
		set
		{
			_ReflectionProbeAnchor = value;
		}
	}

	public void AddMask(WaterSimpleMask mask)
	{
		mask.Renderer.enabled = false;
		int renderQueuePriority = mask.RenderQueuePriority;
		for (int num = _Masks.Count - 1; num >= 0; num--)
		{
			if (_Masks[num].RenderQueuePriority <= renderQueuePriority)
			{
				_Masks.Insert(num + 1, mask);
				return;
			}
		}
		_Masks.Insert(0, mask);
	}

	public void RemoveMask(WaterSimpleMask mask)
	{
		_Masks.Remove(mask);
	}

	public void RenderEffects(WaterCamera waterCamera)
	{
		Camera cameraComponent = waterCamera.CameraComponent;
		if (_Water.isActiveAndEnabled && (cameraComponent.cullingMask & (1 << _Water.gameObject.layer)) != 0 && (_Water.Volume.Boundless || !_Water.Volume.HasRenderableAdditiveVolumes || waterCamera.RenderVolumes))
		{
			_Water.OnWaterRender(waterCamera);
		}
	}

	public void RenderWaterBeforeImageEffectsOpaque(Camera camera, WaterGeometryType geometryType, CommandBuffer commandBuffer)
	{
		if (!_Water.isActiveAndEnabled || (camera.cullingMask & (1 << _Water.gameObject.layer)) == 0)
		{
			return;
		}
		WaterCamera waterCamera = WaterCamera.GetWaterCamera(camera);
		if ((object)waterCamera == null || (!_Water.Volume.Boundless && _Water.Volume.HasRenderableAdditiveVolumes && !waterCamera.RenderVolumes))
		{
			return;
		}
		if (_Water.ShaderSet.ReceiveShadows)
		{
			Vector2 min = new Vector2(0f, 0f);
			Vector2 max = new Vector2(1f, 1f);
			waterCamera.ReportShadowedWaterMinMaxRect(min, max);
		}
		if (!_UseSharedMask)
		{
			RenderMasks(camera, waterCamera, _PropertyBlock);
		}
		Mesh[] transformedMeshes = _Water.Geometry.GetTransformedMeshes(camera, out var matrix, geometryType, volume: false, waterCamera.ForcedVertexCount);
		commandBuffer.SetRenderTarget(BuiltinRenderTextureType.CameraTarget, BuiltinRenderTextureType.ResolvedDepth);
		for (int i = 0; i < transformedMeshes.Length; i++)
		{
			commandBuffer.DrawMesh(transformedMeshes[i], matrix, _Water.Materials.SurfaceMaterial, 0, 0, _PropertyBlock);
			commandBuffer.DrawMesh(transformedMeshes[i], matrix, _Water.Materials.SurfaceMaterial, 0, 1, _PropertyBlock);
			if (waterCamera.ContainingWater != null && waterCamera.Type == WaterCamera.CameraType.Normal)
			{
				commandBuffer.DrawMesh(transformedMeshes[i], matrix, _Water.Materials.SurfaceBackMaterial, 0, 1, _PropertyBlock);
			}
		}
		waterCamera.CameraComponent.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _Water.WaterCommandBuffer);
		waterCamera.CameraComponent.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, _Water.WaterCommandBuffer);
	}

	public void Render(Camera camera, WaterGeometryType geometryType, CommandBuffer commandBuffer = null, Shader shader = null)
	{
		if (!_Water.isActiveAndEnabled || (camera.cullingMask & (1 << _Water.gameObject.layer)) == 0)
		{
			return;
		}
		WaterCamera waterCamera = WaterCamera.GetWaterCamera(camera);
		if ((object)waterCamera == null || (!_Water.Volume.Boundless && _Water.Volume.HasRenderableAdditiveVolumes && !waterCamera.RenderVolumes))
		{
			return;
		}
		if (_Water.ShaderSet.ReceiveShadows)
		{
			Vector2 min = new Vector2(0f, 0f);
			Vector2 max = new Vector2(1f, 1f);
			waterCamera.ReportShadowedWaterMinMaxRect(min, max);
		}
		if (!_UseSharedMask)
		{
			RenderMasks(camera, waterCamera, _PropertyBlock);
		}
		Mesh[] transformedMeshes = _Water.Geometry.GetTransformedMeshes(camera, out var matrix, geometryType, volume: false, waterCamera.ForcedVertexCount);
		if (commandBuffer == null)
		{
			Camera camera2 = ((waterCamera.RenderMode != WaterRenderMode.DefaultQueue) ? waterCamera._WaterRenderCamera : camera);
			for (int i = 0; i < transformedMeshes.Length; i++)
			{
				Graphics.DrawMesh(transformedMeshes[i], matrix, _Water.Materials.SurfaceMaterial, _Water.gameObject.layer, camera2, 0, _PropertyBlock, _ShadowCastingMode, receiveShadows: false, (_ReflectionProbeAnchor != null) ? _ReflectionProbeAnchor : _Water.transform, useLightProbes: false);
				if (waterCamera.ContainingWater != null && waterCamera.Type == WaterCamera.CameraType.Normal)
				{
					Graphics.DrawMesh(transformedMeshes[i], matrix, _Water.Materials.SurfaceBackMaterial, _Water.gameObject.layer, camera2, 0, _PropertyBlock, _ShadowCastingMode, receiveShadows: false, (_ReflectionProbeAnchor != null) ? _ReflectionProbeAnchor : _Water.transform, useLightProbes: false);
				}
			}
		}
		else
		{
			Material variant = UtilityShaderVariants.Instance.GetVariant(shader, _Water.Materials.UsedKeywords);
			for (int j = 0; j < transformedMeshes.Length; j++)
			{
				commandBuffer.DrawMesh(transformedMeshes[j], matrix, variant, 0, 0, _PropertyBlock);
			}
		}
	}

	public void RenderVolumes(CommandBuffer commandBuffer, Shader shader, bool twoPass)
	{
		if (_Water.enabled)
		{
			Material variant = UtilityShaderVariants.Instance.GetVariant(shader, _Water.Materials.UsedKeywords);
			List<WaterVolumeAdd> volumesDirect = _Water.Volume.GetVolumesDirect();
			RenderVolumes(commandBuffer, variant, volumesDirect, twoPass);
			List<WaterVolumeSubtract> subtractiveVolumesDirect = _Water.Volume.GetSubtractiveVolumesDirect();
			RenderVolumes(commandBuffer, variant, subtractiveVolumesDirect, twoPass);
		}
	}

	public void RenderMasks(CommandBuffer commandBuffer)
	{
		for (int num = _Masks.Count - 1; num >= 0; num--)
		{
			commandBuffer.DrawMesh(_Masks[num].GetComponent<MeshFilter>().sharedMesh, _Masks[num].transform.localToWorldMatrix, _Masks[num].Renderer.sharedMaterial, 0, 0);
		}
	}

	public void PostRender(WaterCamera waterCamera)
	{
		if (_Water != null)
		{
			_Water.OnWaterPostRender(waterCamera);
		}
		ReleaseTemporaryBuffers();
	}

	public void OnSharedSubtractiveMaskRender(ref bool hasSubtractiveVolumes, ref bool hasAdditiveVolumes, ref bool hasFlatMasks)
	{
		if (!_Water.enabled)
		{
			return;
		}
		List<WaterVolumeAdd> volumesDirect = _Water.Volume.GetVolumesDirect();
		int count = volumesDirect.Count;
		for (int i = 0; i < count; i++)
		{
			volumesDirect[i].DisableRenderers();
		}
		List<WaterVolumeSubtract> subtractiveVolumesDirect = _Water.Volume.GetSubtractiveVolumesDirect();
		int count2 = subtractiveVolumesDirect.Count;
		if (_UseSharedMask)
		{
			for (int j = 0; j < count2; j++)
			{
				subtractiveVolumesDirect[j].EnableRenderers(forBorderRendering: false);
			}
			hasSubtractiveVolumes = hasSubtractiveVolumes || _Water.Volume.GetSubtractiveVolumesDirect().Count != 0;
			hasAdditiveVolumes = hasAdditiveVolumes || count != 0;
			hasFlatMasks = hasFlatMasks || _Masks.Count != 0;
		}
		else
		{
			for (int k = 0; k < count2; k++)
			{
				subtractiveVolumesDirect[k].DisableRenderers();
			}
		}
	}

	public void OnSharedMaskAdditiveRender()
	{
		if (_Water.enabled && _UseSharedMask)
		{
			List<WaterVolumeAdd> volumesDirect = _Water.Volume.GetVolumesDirect();
			int count = volumesDirect.Count;
			for (int i = 0; i < count; i++)
			{
				volumesDirect[i].EnableRenderers(forBorderRendering: false);
			}
			List<WaterVolumeSubtract> subtractiveVolumesDirect = _Water.Volume.GetSubtractiveVolumesDirect();
			int count2 = subtractiveVolumesDirect.Count;
			for (int j = 0; j < count2; j++)
			{
				subtractiveVolumesDirect[j].DisableRenderers();
			}
		}
	}

	public void OnSharedMaskPostRender()
	{
		if (_Water.enabled)
		{
			List<WaterVolumeAdd> volumesDirect = _Water.Volume.GetVolumesDirect();
			int count = volumesDirect.Count;
			for (int i = 0; i < count; i++)
			{
				volumesDirect[i].EnableRenderers(forBorderRendering: true);
			}
			List<WaterVolumeSubtract> subtractiveVolumesDirect = _Water.Volume.GetSubtractiveVolumesDirect();
			int count2 = subtractiveVolumesDirect.Count;
			for (int j = 0; j < count2; j++)
			{
				subtractiveVolumesDirect[j].EnableRenderers(forBorderRendering: true);
			}
		}
	}

	internal void Awake(Water water)
	{
		_Water = water;
	}

	internal void OnValidate()
	{
		if (_VolumeFrontShader == null)
		{
			_VolumeFrontShader = Shader.Find("UltimateWater/Volumes/Front");
		}
		if (_VolumeFrontFastShader == null)
		{
			_VolumeFrontFastShader = Shader.Find("UltimateWater/Volumes/Front Simple");
		}
		if (_VolumeBackShader == null)
		{
			_VolumeBackShader = Shader.Find("UltimateWater/Volumes/Back");
		}
	}

	private static void RenderVolumes<T>(CommandBuffer commandBuffer, Material material, List<T> boundingVolumes, bool twoPass) where T : WaterVolumeBase
	{
		for (int num = boundingVolumes.Count - 1; num >= 0; num--)
		{
			MeshRenderer[] volumeRenderers = boundingVolumes[num].VolumeRenderers;
			MeshFilter[] volumeFilters = boundingVolumes[num].VolumeFilters;
			if (volumeRenderers != null && volumeRenderers.Length != 0 && volumeRenderers[0].enabled)
			{
				if (!twoPass)
				{
					int shaderPass = ((material.passCount != 1) ? 1 : 0);
					for (int i = 0; i < volumeRenderers.Length; i++)
					{
						commandBuffer.DrawMesh(volumeFilters[i].sharedMesh, volumeRenderers[i].transform.localToWorldMatrix, material, 0, shaderPass);
					}
				}
				else
				{
					for (int j = 0; j < volumeRenderers.Length; j++)
					{
						Mesh sharedMesh = volumeFilters[j].sharedMesh;
						commandBuffer.DrawMesh(sharedMesh, volumeRenderers[j].transform.localToWorldMatrix, material, 0, 0);
						commandBuffer.DrawMesh(sharedMesh, volumeRenderers[j].transform.localToWorldMatrix, material, 0, 1);
					}
				}
			}
		}
	}

	private void ReleaseTemporaryBuffers()
	{
		if (_AdditiveMaskTexture != null)
		{
			RenderTexture.ReleaseTemporary(_AdditiveMaskTexture);
			_AdditiveMaskTexture = null;
		}
		if (_SubtractiveMaskTexture != null)
		{
			RenderTexture.ReleaseTemporary(_SubtractiveMaskTexture);
			_SubtractiveMaskTexture = null;
		}
	}

	private void RenderMasks(Camera camera, WaterCamera waterCamera, MaterialPropertyBlock propertyBlock)
	{
		List<WaterVolumeSubtract> subtractiveVolumesDirect = _Water.Volume.GetSubtractiveVolumesDirect();
		List<WaterVolumeAdd> volumesDirect = _Water.Volume.GetVolumesDirect();
		if ((object)waterCamera == null || !waterCamera.RenderVolumes || (subtractiveVolumesDirect.Count == 0 && volumesDirect.Count == 0 && _Masks.Count == 0))
		{
			ReleaseTemporaryBuffers();
			return;
		}
		int waterTempLayer = WaterProjectSettings.Instance.WaterTempLayer;
		int waterCollidersLayer = WaterProjectSettings.Instance.WaterCollidersLayer;
		Camera effectsCamera = waterCamera.EffectsCamera;
		if (effectsCamera == null)
		{
			ReleaseTemporaryBuffers();
			return;
		}
		bool hasSubtractiveVolumes = false;
		bool hasAdditiveVolumes = false;
		bool hasFlatMasks = false;
		OnSharedSubtractiveMaskRender(ref hasSubtractiveVolumes, ref hasAdditiveVolumes, ref hasFlatMasks);
		effectsCamera.CopyFrom(camera);
		effectsCamera.enabled = false;
		effectsCamera.GetComponent<WaterCamera>().enabled = false;
		effectsCamera.renderingPath = RenderingPath.Forward;
		effectsCamera.depthTextureMode = DepthTextureMode.None;
		effectsCamera.cullingMask = 1 << waterTempLayer;
		if (subtractiveVolumesDirect.Count != 0)
		{
			if (_SubtractiveMaskTexture == null)
			{
				_SubtractiveMaskTexture = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 24, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, 1, RenderTextureMemoryless.None, VRTextureUsage.None, useDynamicScale: false);
			}
			Graphics.SetRenderTarget(_SubtractiveMaskTexture);
			int count = subtractiveVolumesDirect.Count;
			for (int i = 0; i < count; i++)
			{
				subtractiveVolumesDirect[i].SetLayer(waterTempLayer);
			}
			RenderTexture temporary = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 24, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, 1, RenderTextureMemoryless.None, VRTextureUsage.None, useDynamicScale: false);
			effectsCamera.clearFlags = CameraClearFlags.Color;
			effectsCamera.backgroundColor = new Color(0f, 0f, 0.5f, 0f);
			effectsCamera.targetTexture = temporary;
			effectsCamera.RenderWithShader(_VolumeFrontShader, "CustomType");
			GL.Clear(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f), 0f);
			Shader.SetGlobalTexture("_VolumesFrontDepth", temporary);
			effectsCamera.clearFlags = CameraClearFlags.Nothing;
			effectsCamera.targetTexture = _SubtractiveMaskTexture;
			effectsCamera.RenderWithShader(_VolumeBackShader, "CustomType");
			RenderTexture.ReleaseTemporary(temporary);
			for (int j = 0; j < count; j++)
			{
				subtractiveVolumesDirect[j].SetLayer(waterCollidersLayer);
			}
			propertyBlock.SetTexture(ShaderVariables.SubtractiveMask, _SubtractiveMaskTexture);
		}
		if (volumesDirect.Count != 0)
		{
			OnSharedMaskAdditiveRender();
			if (_AdditiveMaskTexture == null)
			{
				_AdditiveMaskTexture = RenderTexture.GetTemporary(camera.pixelWidth, camera.pixelHeight, 16, SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat) ? RenderTextureFormat.ARGBFloat : RenderTextureFormat.ARGBHalf, RenderTextureReadWrite.Linear, 1, RenderTextureMemoryless.None, VRTextureUsage.None, useDynamicScale: false);
			}
			Graphics.SetRenderTarget(_AdditiveMaskTexture);
			GL.Clear(clearDepth: true, clearColor: true, new Color(0f, 0f, 0f, 0f));
			int count2 = volumesDirect.Count;
			for (int k = 0; k < count2; k++)
			{
				volumesDirect[k].SetLayer(waterTempLayer);
				volumesDirect[k].EnableRenderers(forBorderRendering: false);
			}
			effectsCamera.clearFlags = CameraClearFlags.Nothing;
			effectsCamera.targetTexture = _AdditiveMaskTexture;
			effectsCamera.RenderWithShader(waterCamera.IsInsideAdditiveVolume ? _VolumeFrontShader : _VolumeFrontFastShader, "CustomType");
			effectsCamera.clearFlags = CameraClearFlags.Nothing;
			effectsCamera.targetTexture = _AdditiveMaskTexture;
			effectsCamera.RenderWithShader(_VolumeBackShader, "CustomType");
			for (int l = 0; l < count2; l++)
			{
				volumesDirect[l].SetLayer(waterCollidersLayer);
			}
			propertyBlock.SetTexture(ShaderVariables.AdditiveMask, _AdditiveMaskTexture);
		}
		OnSharedMaskPostRender();
		effectsCamera.targetTexture = null;
	}
}
