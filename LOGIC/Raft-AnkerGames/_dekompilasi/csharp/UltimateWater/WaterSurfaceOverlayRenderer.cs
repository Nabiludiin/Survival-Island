using System;
using UltimateWater.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UltimateWater;

[RequireComponent(typeof(Renderer))]
public class WaterSurfaceOverlayRenderer : MonoBehaviour, ILocalDisplacementRenderer, IDynamicWaterEffects, ILocalDisplacementMaskRenderer, ILocalFoamRenderer
{
	[SerializeField]
	[FormerlySerializedAs("displacementAndNormalMaterial")]
	private Material _DisplacementAndNormalMaterial;

	[SerializeField]
	[FormerlySerializedAs("displacementMaskMaterial")]
	private Material _DisplacementMaskMaterial;

	[SerializeField]
	[FormerlySerializedAs("foamMaterial")]
	private Material _FoamMaterial;

	private Renderer _RendererComponent;

	public Material DisplacementAndNormalMaterial => _DisplacementAndNormalMaterial;

	public Material DisplacementMaskMaterial => _DisplacementMaskMaterial;

	public Material FoamMaterial => _FoamMaterial;

	public Renderer Renderer => _RendererComponent;

	public void RenderLocalDisplacement(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		commandBuffer.DrawRenderer(_RendererComponent, _DisplacementAndNormalMaterial);
	}

	public void RenderLocalMask(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		commandBuffer.DrawRenderer(_RendererComponent, _DisplacementMaskMaterial);
	}

	public void RenderLocalFoam(CommandBuffer commandBuffer, DynamicWaterCameraData overlays)
	{
		commandBuffer.DrawRenderer(_RendererComponent, _FoamMaterial);
	}

	public void Enable()
	{
		throw new NotImplementedException();
	}

	public void Disable()
	{
		throw new NotImplementedException();
	}

	private void Awake()
	{
		_RendererComponent = GetComponent<Renderer>();
	}

	private void OnEnable()
	{
		if (_DisplacementAndNormalMaterial != null)
		{
			DynamicWater.AddRenderer((ILocalDisplacementRenderer)this);
		}
		if (_DisplacementMaskMaterial != null)
		{
			DynamicWater.AddRenderer((ILocalDisplacementMaskRenderer)this);
		}
		if (_FoamMaterial != null)
		{
			DynamicWater.AddRenderer((ILocalFoamRenderer)this);
		}
	}

	private void OnDisable()
	{
		DynamicWater.RemoveRenderer((ILocalDisplacementRenderer)this);
		DynamicWater.RemoveRenderer((ILocalDisplacementMaskRenderer)this);
		DynamicWater.RemoveRenderer((ILocalFoamRenderer)this);
	}
}
